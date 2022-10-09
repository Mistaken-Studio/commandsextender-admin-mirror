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
    internal class TalkCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public static readonly Dictionary<string, int[]> Active = new();

        public static readonly Dictionary<int, (Vector3 Pos, RoleType Role, float HP, float AP,
            Exiled.API.Features.Items.Item[] Inventory, ushort Ammo12gauge, ushort Ammo44cal, ushort Ammo556x45, ushort
            Ammo762x39, ushort Ammo9x19, int UnitIndex, byte UnitType, (CustomPlayerEffects.PlayerEffect effect, float
            dur, byte intensity)[] effects)> SavedInfo
            = new();

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
                    if (p == null)
                    {
                        // if (data.Role.GetTeam() == Team.SCP)
                        //     NineTailedFoxAnnouncer.CheckForZombies(Server.Host.GameObject);
                        continue;
                    }

                    p.DisableAllEffects();
                    p.SetSessionVariable(SessionVarType.NO_SPAWN_PROTECT, true);

                    // p.SetSessionVar(SessionVarType.CC_IGNORE_CHANGE_ROLE, true);
                    p.SetSessionVariable(SessionVarType.ITEM_LESS_CLSSS_CHANGE, true);
                    var old = RespawnManager.CurrentSequence();
                    RespawnManager.Singleton._curSequence =
                        RespawnManager.RespawnSequencePhase.SpawningSelectedTeam;
                    p.Role.Type = data.Role;
                    p.ReferenceHub.characterClassManager.NetworkCurSpawnableTeamType = data.UnitType;
                    if (RespawnManager.Singleton.NamingManager.TryGetAllNamesFromGroup(
                            data.UnitType,
                            out var array))
                        p.UnitName = array[data.UnitIndex];
                    RespawnManager.Singleton._curSequence = old;
                    p.SetSessionVariable(SessionVarType.ITEM_LESS_CLSSS_CHANGE, false);

                    // p.SetSessionVar(SessionVarType.CC_IGNORE_CHANGE_ROLE, false);
                    p.SetSessionVariable(SessionVarType.NO_SPAWN_PROTECT, false);
                    API.Diagnostics.Module.CallSafeDelayed(
                        0.5f,
                        () =>
                        {
                            if (!p.IsConnected)
                                return;

                            if (!Warhead.IsDetonated)
                            {
                                if (MapPlus.IsLCZDecontaminated(30))
                                {
                                    if (data.Pos.y is not (> -100 and < 100))
                                        p.Position = data.Pos;
                                    else
                                        p.Position = AfterDecontRooms[Random.Range(0, AfterDecontRooms.Count)];
                                }
                                else
                                    p.Position = data.Pos;
                            }
                            else
                            {
                                if (data.Pos.y > 900)
                                    p.Position = data.Pos;
                                else
                                    p.Position = AfterWarHeadRooms[Random.Range(0, AfterWarHeadRooms.Count)];
                            }

                            p.Health = data.HP;
                            p.ArtificialHealth = data.AP;
                            p.ClearInventory();
                            foreach (var item in data.Inventory)
                                p.AddItem(item);
                            p.Ammo[ItemType.Ammo12gauge] = data.Ammo12gauge;
                            p.Ammo[ItemType.Ammo44cal] = data.Ammo44cal;
                            p.Ammo[ItemType.Ammo556x45] = data.Ammo556x45;
                            p.Ammo[ItemType.Ammo762x39] = data.Ammo762x39;
                            p.Ammo[ItemType.Ammo9x19] = data.Ammo9x19;

                            foreach (var (effect, duration, intensity) in data.effects)
                            {
                                effect.Intensity = intensity;
                                effect.ServerChangeDuration(duration);
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
                    Object.Destroy(room);
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
                var counter = 0;
                Warps.Enqueue(pos);
                List<Player> talkPlayers = targets
                    .Select(RealPlayers.Get)
                    .Where(target => target is not null)
                    .ToList();

                if (talkPlayers.Any(x => x.Role.Side == Side.Scp) && Round.ElapsedTime.TotalSeconds < 35)
                {
                    success = true;
                    return new[]
                    {
                        "You cannot use this command for the first 35 seconds of the round if one player is an SCP",
                    };
                }

                if (CustomStructuresIntegration.Asset is not null)
                {
                    if (!TalkRooms.ContainsKey(player))
                        TalkRooms.Add(player, null);
                    TalkRooms[player] = CustomStructuresIntegration.SpawnAsset(
                        new Vector3(1000f + (100f * (TalkRooms.Count - 1f)), 1000f, 1000f));
                }

                var lastPos = Vector3.zero;
                var offset = Vector3.zero;
                foreach (var p in talkPlayers.Where(p => p?.IsConnected() ?? false))
                {
                    p.SetSessionVariable(SessionVarType.TALK, true);

                    // p.SetSessionVar(SessionVarType.CC_IGNORE_CHANGE_ROLE, true);
                    SavedInfo.Add(
                        p.Id,
#pragma warning disable SA1118
                        (
                            p.Position,
                            p.Role,
                            p.Health,
                            p.ArtificialHealth,
                            p.Items.ToArray(),
                            p.Ammo.TryGetValue(ItemType.Ammo12gauge, out var ammo12Gauge) ? ammo12Gauge : (ushort)0,
                            p.Ammo.TryGetValue(ItemType.Ammo44cal, out var ammo44Cal) ? ammo44Cal : (ushort)0,
                            p.Ammo.TryGetValue(ItemType.Ammo556x45, out var ammo556X45) ? ammo556X45 : (ushort)0,
                            p.Ammo.TryGetValue(ItemType.Ammo762x39, out var ammo762X39) ? ammo762X39 : (ushort)0,
                            p.Ammo.TryGetValue(ItemType.Ammo9x19, out var ammo9X19) ? ammo9X19 : (ushort)0,
                            RespawnManager.Singleton.NamingManager.AllUnitNames.FindIndex(
                                x => x.UnitName == p.ReferenceHub.characterClassManager.NetworkCurUnitName),
                            p.ReferenceHub.characterClassManager.NetworkCurSpawnableTeamType,
                            p.ReferenceHub.playerEffectsController.AllEffects.Values
                                .Select(i => (i, i.Duration, i.Intensity)).ToArray()));
#pragma warning restore SA1118
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
                            if (TalkRooms.TryGetValue(player, out var room))
                            {
                                if (p.CheckPermissions(PlayerPermissions.AdminChat))
                                    p.Position = room.transform.Find("Admin_SpawnPoint").position;
                                else
                                {
                                    if (counter == 0)
                                    {
                                        lastPos = room.transform.Find("Player_SpawnPoint").position;
                                        p.Position = lastPos;
                                    }
                                    else if (counter % 2 != 0)
                                    {
                                        offset += Vector3.right * 0.4f;
                                        p.Position = lastPos + offset;
                                    }
                                    else
                                        p.Position = lastPos - offset;

                                    counter++;

                                    p.EnableEffect<CustomPlayerEffects.Ensnared>();
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
                                        if (!p.IsConnected || TalkRooms.ContainsKey(player))
                                            return;
                                        p.Position += GetPosByCounter(counter++);
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

        private static Vector3 GetPosByCounter(int counter)
        {
            return counter switch
            {
                0 => new Vector3(0.5f, -0.2f, 0),
                1 => new Vector3(0, -0.2f, 0.5f),
                2 => new Vector3(-0.5f, -0.2f, 0),
                3 => new Vector3(0, -0.2f, -.5f),
                4 => new Vector3(0.5f, -0.2f, 0.5f),
                5 => new Vector3(0.5f, -0.2f, -0.5f),
                6 => new Vector3(-0.5f, -0.2f, 0.5f),
                7 => new Vector3(-0.5f, -0.2f, -0.5f),
                _ => new Vector3(0, -0.2f, 0)
            };
        }

        private static IEnumerator<float> ShowHint(Player p)
        {
            if (!Active.TryGetValue(p.UserId, out var playerIds))
                yield break;
            Player[] players = playerIds.Select(RealPlayers.Get).Where(x => x != null).ToArray();
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
