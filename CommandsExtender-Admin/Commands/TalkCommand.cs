// -----------------------------------------------------------------------
// <copyright file="TalkCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using JetBrains.Annotations;
using MEC;
using Mistaken.API;
using Mistaken.API.Commands;
using Mistaken.API.Extensions;
using Mistaken.API.GUI;
using Respawning;
using UnityEngine;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [UsedImplicitly]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal sealed class TalkCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public static readonly Dictionary<string, int[]> Active = new();

        public static readonly Dictionary<int, TalkPlayerInfo> SavedInfo = new();

        public string Permission => "talk";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "talk";

        public override string Description => "Teleports targets and admin to one of jails to do interogation";

        public string[] Usage => new[]
        {
            "%player%",
        };

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            var player = Player.Get(sender);
            if (Active.TryGetValue(player.UserId, out var players))
            {
                foreach (var playerId in players)
                {
                    if (!SavedInfo.TryGetValue(playerId, out var data))
                        continue;

                    SavedInfo.Remove(playerId);
                    var p = RealPlayers.Get(playerId);
                    if (p is null)
                        continue;

                    p.DisableAllEffects();
                    p.SetSessionVariable(SessionVarType.NO_SPAWN_PROTECT, true);

                    // p.SetSessionVar(SessionVarType.CC_IGNORE_CHANGE_ROLE, true);
                    p.SetSessionVariable(SessionVarType.ITEM_LESS_CLSSS_CHANGE, true);
                    var old = RespawnManager.CurrentSequence();
                    RespawnManager.Singleton._curSequence = RespawnManager.RespawnSequencePhase.SpawningSelectedTeam;
                    p.Role.Type = data.RoleType;
                    p.ReferenceHub.characterClassManager.NetworkCurSpawnableTeamType = data.UnitType;
                    if (RespawnManager.Singleton.NamingManager.TryGetAllNamesFromGroup(data.UnitType, out var array))
                        p.UnitName = array[data.UnitIndex];

                    RespawnManager.Singleton._curSequence = old;
                    p.SetSessionVariable(SessionVarType.ITEM_LESS_CLSSS_CHANGE, false);

                    // p.SetSessionVar(SessionVarType.CC_IGNORE_CHANGE_ROLE, false);
                    p.SetSessionVariable(SessionVarType.NO_SPAWN_PROTECT, false);
                    API.Diagnostics.Module.CallSafeDelayed(
                        0.5f,
                        () =>
                        {
                            if (!p.IsConnected())
                                return;

                            if (!Warhead.IsDetonated)
                            {
                                if (MapPlus.IsLCZDecontaminated(30))
                                {
                                    if (data.Position.y is not (> -100 and < 100))
                                        p.Position = data.Position;
                                    else
                                        p.Position = AfterDecontRooms[UnityEngine.Random.Range(0, AfterDecontRooms.Count)];
                                }
                                else
                                    p.Position = data.Position;
                            }
                            else
                            {
                                if (data.Position.y > 900)
                                    p.Position = data.Position;
                                else
                                    p.Position = AfterWarHeadRooms[UnityEngine.Random.Range(0, AfterWarHeadRooms.Count)];
                            }

                            p.Health = data.HP;
                            p.ArtificialHealth = data.AHP;
                            p.ClearInventory();
                            foreach (var item in data.Inventory)
                                p.AddItem(item);

                            p.Ammo[ItemType.Ammo12gauge] = data.Ammo12gauge;
                            p.Ammo[ItemType.Ammo44cal] = data.Ammo44cal;
                            p.Ammo[ItemType.Ammo556x45] = data.Ammo556x45;
                            p.Ammo[ItemType.Ammo762x39] = data.Ammo762x39;
                            p.Ammo[ItemType.Ammo9x19] = data.Ammo9x19;

                            foreach (var e in data.PlayerEffects)
                            {
                                e.Effect.Intensity = e.Intensity;
                                e.Effect.ServerChangeDuration(e.Duration);
                            }

                            p.SetSessionVariable(SessionVarType.TALK, false);
                            p.SetSessionVariable(SessionVarType.POST_TALK, true);
                            API.Handlers.CustomInfoHandler.Set(
                                p,
                                "post_talk",
                                "<color=red>Post TALK protection: <color=yellow>Active</color></color>");

                            API.Diagnostics.Module.CallSafeDelayed(
                                5f,
                                () =>
                                {
                                    p.SetSessionVariable(SessionVarType.POST_TALK, false);
                                    API.Handlers.CustomInfoHandler.Set(p, "post_talk", null);
                                },
                                "DisablePostTalkFlag");
                        },
                        "TalkRestore");
                }

                if (TalkRooms.TryGetValue(player, out var room))
                {
                    foreach (var nid in room.GetComponentsInChildren<Mirror.NetworkIdentity>())
                        Mirror.NetworkServer.Destroy(nid.gameObject);

                    UnityEngine.Object.Destroy(room);
                    TalkRooms.Remove(player);
                }

                Active.Remove(player.UserId);
            }
            else
            {
                var targets = (args[0] + $".{player.Id}").Split('.').Select(int.Parse).ToHashSet().ToArray();

                if (SavedInfo.Any(x => targets.Any(y => y == x.Key)))
                {
                    success = false;
                    return new[]
                    {
                        "At least one of specified players is already on talk!",
                        "<b><color=red>You foking monkey!</color></b>",
                    };
                }

                var pos = Warps.Dequeue();
                Warps.Enqueue(pos);
                List<Player> talkPlayers = targets
                    .Select(RealPlayers.Get)
                    .Where(target => target is not null && target.IsConnected())
                    .ToList();

                if (talkPlayers.Any(x => x.Role.Side == Side.Scp) && Round.ElapsedTime.TotalSeconds < 60)
                {
                    success = true;
                    return new[]
                    {
                        "You cannot use this command for the first 60 seconds of the round if one player is an SCP",
                    };
                }

                if (CustomStructuresIntegration.Asset is not null)
                {
                    if (!TalkRooms.ContainsKey(player))
                        TalkRooms.Add(player, null);

                    TalkRooms[player] = CustomStructuresIntegration.SpawnAsset(new(1000f + (100f * (TalkRooms.Count - 1f)), 1000f, 1000f));
                }

                for (int i = 0; i < talkPlayers.Count; i++)
                {
                    var p = talkPlayers[i];
                    p.SetSessionVariable(SessionVarType.TALK, true);
                    TalkPlayerInfo info = new()
                    {
                        Position = p.Position,
                        RoleType = p.Role.Type,
                        HP = p.Health,
                        AHP = p.ArtificialHealth,
                        Inventory = p.Items.ToArray(),
                        Ammo12gauge = p.Ammo.TryGetValue(ItemType.Ammo12gauge, out var ammo12Gauge) ? ammo12Gauge : (ushort)0,
                        Ammo44cal = p.Ammo.TryGetValue(ItemType.Ammo44cal, out var ammo44Cal) ? ammo44Cal : (ushort)0,
                        Ammo556x45 = p.Ammo.TryGetValue(ItemType.Ammo556x45, out var ammo556X45) ? ammo556X45 : (ushort)0,
                        Ammo762x39 = p.Ammo.TryGetValue(ItemType.Ammo762x39, out var ammo762X39) ? ammo762X39 : (ushort)0,
                        Ammo9x19 = p.Ammo.TryGetValue(ItemType.Ammo9x19, out var ammo9X19) ? ammo9X19 : (ushort)0,
                        UnitIndex = RespawnManager.Singleton.NamingManager.AllUnitNames.FindIndex(x => x.UnitName == p.ReferenceHub.characterClassManager.NetworkCurUnitName),
                        UnitType = p.ReferenceHub.characterClassManager.NetworkCurSpawnableTeamType,
                        PlayerEffects = p.ReferenceHub.playerEffectsController.AllEffects.Values.Select(i => new TalkPlayerInfo.TalkPlayerEffects(i, i.Duration, i.Intensity)).ToArray(),
                    };

                    // p.SetSessionVar(SessionVarType.CC_IGNORE_CHANGE_ROLE, true);
                    SavedInfo.Add(p.Id, info);

                    foreach (var item in p.Items.ToArray())
                        p.RemoveItem(item, false);

                    if (p.IsOverwatchEnabled)
                    {
                        p.IsOverwatchEnabled = false;
                        p.Broadcast("Talk", 10, "Overwatch mode disabled", flags: Broadcast.BroadcastFlags.AdminChat);
                    }

                    var old = RespawnManager.CurrentSequence();
                    RespawnManager.Singleton._curSequence = RespawnManager.RespawnSequencePhase.SpawningSelectedTeam;
                    p.Role.Type = RoleType.Tutorial;
                    RespawnManager.Singleton._curSequence = old;

                    // p.SetSessionVar(SessionVarType.CC_IGNORE_CHANGE_ROLE, false);
                    p.DisableAllEffects();
                    API.Diagnostics.Module.CallSafeDelayed(
                        0.5f,
                        () =>
                        {
                            float angle = 2f * (float)System.Math.PI / (i + 1); // calculate angle for each player in the circle

                            if (TalkRooms.TryGetValue(player, out var room))
                            {
                                var center = room.transform.Find("SpawnPoint").position;

                                if (p.CheckPermissions(PlayerPermissions.AdminChat))
                                    p.Position = center;
                                else
                                {
                                    Vector3 pos = new(
                                        (Mathf.Sin(angle) * 4f) + center.x,
                                        center.y,
                                        (Mathf.Cos(angle) * 4f) + center.z
                                        ); // 4f is for radius of the circle

                                    p.EnableEffect<CustomPlayerEffects.Ensnared>();
                                    p.Position = pos;
                                }
                            }
                            else
                            {
                                WarpCommand.ExecuteWarp(p, pos);

                                if (p.CheckPermissions(PlayerPermissions.AdminChat))
                                    return;

                                p.EnableEffect<CustomPlayerEffects.Ensnared>();
                                API.Diagnostics.Module.CallSafeDelayed(
                                    0.5f,
                                    () =>
                                    {
                                        if (!p.IsConnected())
                                            return;

                                        Vector3 pos = new(
                                            (Mathf.Sin(angle) * 2f) + p.Position.x,
                                            p.Position.y,
                                            (Mathf.Cos(angle) * 2f) + p.Position.z
                                            ); // 2f is for radius of the circle

                                        p.Position = pos;
                                    },
                                    "TalkTeleport");
                            }
                        },
                        "TalkEnable");
                }

                Active.Add(player.UserId, targets);
                API.Diagnostics.Module.RunSafeCoroutine(ShowHint(player), "Talk.ShowHint");
            }

            success = true;
            return new[] { "Done" };
        }

        public struct TalkPlayerInfo
        {
            public Vector3 Position { get; set; }

            public RoleType RoleType { get; set; }

            public float HP { get; set; }

            public float AHP { get; set; }

            public Exiled.API.Features.Items.Item[] Inventory { get; set; }

            public ushort Ammo12gauge { get; set; }

            public ushort Ammo44cal { get; set; }

            public ushort Ammo556x45 { get; set; }

            public ushort Ammo762x39 { get; set; }

            public ushort Ammo9x19 { get; set; }

            public int UnitIndex { get; set; }

            public byte UnitType { get; set; }

            public TalkPlayerEffects[] PlayerEffects { get; set; }

            public struct TalkPlayerEffects
            {
                public TalkPlayerEffects(CustomPlayerEffects.PlayerEffect effect, float duration, byte intensity)
                {
                    this.Effect = effect;
                    this.Duration = duration;
                    this.Intensity = intensity;
                }

                public CustomPlayerEffects.PlayerEffect Effect { get; set; }

                public float Duration { get; set; }

                public byte Intensity { get; set; }
            }
        }

        internal static readonly List<Vector3> AfterDecontRooms = new();
        internal static readonly List<Vector3> AfterWarHeadRooms = new();

        private static readonly Dictionary<Player, GameObject> TalkRooms = new();
        private static readonly Queue<string> Warps = new(
            new[]
            {
                "jail1",

                // "jail2",
                // "jail3",
                "jail4",
                "jail5",
            });

        private static IEnumerator<float> ShowHint(Player p)
        {
            if (!Active.TryGetValue(p.UserId, out var playerIds))
                yield break;

            Player[] players = playerIds.Select(RealPlayers.Get).Where(x => x is not null).ToArray();
            foreach (var player in players)
            {
                player.SetGUI(
                    "talk",
                    PseudoGUIPosition.TOP,
                    $"<size=150%><color=#F00><b>Trwa przesłuchanie</b></color></size>");
            }

            while (Active.ContainsKey(p.UserId))
                yield return Timing.WaitForSeconds(1f);

            foreach (var player in players)
            {
                player.SetGUI(
                    "talk",
                    PseudoGUIPosition.TOP,
                    "<size=150%><color=#F00><b>Przesłuchanie zakończone</b></color></size>",
                    5);
            }
        }
    }
}
