// -----------------------------------------------------------------------
// <copyright file="CommandsHandler.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Mistaken.API;
using Mistaken.API.Diagnostics;
using Mistaken.API.Extensions;
using Mistaken.CommandsExtender.Admin.Commands;
using Respawning;
using UnityEngine;
using VanishHandler = Mistaken.API.Handlers.VanishHandler;

namespace Mistaken.CommandsExtender.Admin
{
    internal class CommandsHandler : Module
    {
        public static readonly Dictionary<string, (Player, Player)> LastAttackers = new Dictionary<string, (Player, Player)>();
        public static readonly Dictionary<string, (Player, Player)> LastVictims = new Dictionary<string, (Player, Player)>();

        public CommandsHandler(PluginHandler plugin)
            : base(plugin)
        {
        }

        public override string Name => "CommandsExtender";

        public override void OnEnable()
        {
            Exiled.Events.Handlers.Server.RestartingRound += this.Server_RestartingRound;
            Exiled.Events.Handlers.Player.InteractingDoor += this.Player_InteractingDoor;
            Exiled.Events.Handlers.Player.Hurting += this.Player_Hurting;
            Exiled.Events.Handlers.Player.Destroying += this.Player_Destroying;
            Exiled.Events.Handlers.Player.Verified += this.Player_Verified;
            Exiled.Events.Handlers.Player.ChangingRole += this.Player_ChangingRole;
            Exiled.Events.Handlers.Server.RoundStarted += this.Server_RoundStarted;
            Exiled.Events.Handlers.Player.Dying += this.Player_Dying;
            Exiled.Events.Handlers.Player.Shooting += this.Player_Shooting;
            Exiled.Events.Handlers.Scp096.AddingTarget += this.Scp096_AddingTarget;
            Exiled.Events.Handlers.Player.UsingItem += this.Player_UsingItem;
        }

        public override void OnDisable()
        {
            Exiled.Events.Handlers.Server.RestartingRound -= this.Server_RestartingRound;
            Exiled.Events.Handlers.Player.InteractingDoor -= this.Player_InteractingDoor;
            Exiled.Events.Handlers.Player.Hurting -= this.Player_Hurting;
            Exiled.Events.Handlers.Player.Destroying -= this.Player_Destroying;
            Exiled.Events.Handlers.Player.Verified -= this.Player_Verified;
            Exiled.Events.Handlers.Player.ChangingRole -= this.Player_ChangingRole;
            Exiled.Events.Handlers.Server.RoundStarted -= this.Server_RoundStarted;
            Exiled.Events.Handlers.Player.Dying -= this.Player_Dying;
            Exiled.Events.Handlers.Player.Shooting -= this.Player_Shooting;
            Exiled.Events.Handlers.Scp096.AddingTarget -= this.Scp096_AddingTarget;
            Exiled.Events.Handlers.Player.UsingItem -= this.Player_UsingItem;
        }

        private void Server_RoundStarted()
        {
            TalkCommand.AfterWarHeadRooms.Add(RoleType.ChaosConscript.GetRandomSpawnProperties().Item1);
            TalkCommand.AfterWarHeadRooms.Add(RoleType.NtfCaptain.GetRandomSpawnProperties().Item1);
            TalkCommand.AfterWarHeadRooms.Add(new Vector3(87f, 996f, -48f)); // Elevator near Gate B
            TalkCommand.AfterWarHeadRooms.Add(new Vector3(0f, 1003f, -58f)); // Bridge
            TalkCommand.AfterWarHeadRooms.Add(new Vector3(0f, 1003f, 1f)); // Crossroads near Gate A elevator

            TalkCommand.AfterDecontRooms.Add(Room.List.First(x => x.Type == RoomType.HczChkpA).Position + Vector3.up);
            TalkCommand.AfterDecontRooms.Add(Room.List.First(x => x.Type == RoomType.HczChkpB).Position + Vector3.up);
        }

        private void Player_UsingItem(Exiled.Events.EventArgs.UsingItemEventArgs ev)
        {
            if (ev.Player.GetSessionVariable<bool>(SessionVarType.POST_TALK))
                ev.IsAllowed = false;
        }

        private void Scp096_AddingTarget(Exiled.Events.EventArgs.AddingTargetEventArgs ev)
        {
            if (ev.Target.GetSessionVariable<bool>(SessionVarType.POST_TALK))
                ev.IsAllowed = false;
        }

        private void Player_Shooting(Exiled.Events.EventArgs.ShootingEventArgs ev)
        {
            if (ev.Shooter.GetSessionVariable<bool>(SessionVarType.POST_TALK))
                ev.IsAllowed = false;
        }

        private void Player_ChangingRole(Exiled.Events.EventArgs.ChangingRoleEventArgs ev)
        {
            if (ev.NewRole == RoleType.Spectator && SpecRoleCommand.SpecRole != RoleType.Spectator && SpecRoleCommand.SpecRole != RoleType.None)
                ev.NewRole = SpecRoleCommand.SpecRole;

            if (VanishHandler.Vanished.ContainsKey(ev.Player.Id))
            {
                VanishHandler.SetGhost(ev.Player, false);
                ev.Player.Broadcast(5, "Vanish deactivated due to role change", Broadcast.BroadcastFlags.AdminChat);
            }

            MEC.Timing.CallDelayed(1, () =>
            {
                if (ev.Player.IsHuman)
                {
                    var set = ev.Player.GetSessionVariable(SessionVarType.ADMIN_MARK, new HashSet<Player>());
                    if (set.Count != 0)
                    {
                        foreach (var admin in set)
                        {
                            if (admin.Role == RoleType.Tutorial)
                                ev.Player.ChangeAppearance(admin, RoleType.Tutorial);
                        }
                    }
                }

                foreach (var item in RealPlayers.List)
                {
                    var set2 = item.GetSessionVariable(SessionVarType.ADMIN_MARK, new HashSet<Player>());
                    foreach (var admin in set2)
                    {
                        if (admin.Role != RoleType.Tutorial)
                        {
                            set2.Remove(admin);
                            item.ChangeAppearance(admin, item.Role);
                        }
                    }
                }
            });
        }

        private void Player_Verified(Exiled.Events.EventArgs.VerifiedEventArgs ev)
        {
            if (MuteAllCommand.GlobalMuteActive && !ev.Player.IsMuted)
            {
                ev.Player.ReferenceHub.dissonanceUserSetup.AdministrativelyMuted = true;
                MuteAllCommand.Muted.Add(ev.Player.UserId);
            }

            ev.Player.SetSessionVariable(SessionVarType.ADMIN_MARK, new HashSet<Player>());

            if (FakeNickCommand.FakeNicknames.ContainsKey(ev.Player.UserId) && Patches.NicknamePatch.RealNicknames.TryGetValue(ev.Player.UserId, out string realNickname))
                ev.Player.SetSessionVariable("REAL_NICKNAME", realNickname);
        }

        private void Player_Destroying(Exiled.Events.EventArgs.DestroyingEventArgs ev)
        {
            if (!ev.Player.IsReadyPlayer())
                return;
            if (MuteAllCommand.Muted.Contains(ev.Player.UserId))
                MuteAllCommand.Muted.Remove(ev.Player.UserId);

            if (TalkCommand.Active.TryGetValue(ev.Player.UserId, out int[] players))
            {
                foreach (var playerId in players)
                {
                    if (TalkCommand.SavedInfo.TryGetValue(playerId, out (Vector3 Pos, RoleType Role, float HP, float AP, Exiled.API.Features.Items.Item[] Inventory, ushort Ammo12gauge, ushort Ammo44cal, ushort Ammo556x45, ushort Ammo762x39, ushort Ammo9x19, int UnitIndex, byte UnitType, (CustomPlayerEffects.PlayerEffect effect, float dur, byte intensity)[] effects) data))
                    {
                        TalkCommand.SavedInfo.Remove(playerId);
                        Player p = RealPlayers.Get(playerId);
                        if (p == null)
                            continue;
                        var old = Respawning.RespawnManager.CurrentSequence();
                        Respawning.RespawnManager.Singleton._curSequence = RespawnManager.RespawnSequencePhase.SpawningSelectedTeam;
                        p.Role.Type = data.Role;
                        p.ReferenceHub.characterClassManager.NetworkCurSpawnableTeamType = data.UnitType;
                        if (Respawning.RespawnManager.Singleton.NamingManager.TryGetAllNamesFromGroup(data.UnitType, out var array))
                            p.UnitName = array[data.UnitIndex];
                        Respawning.RespawnManager.Singleton._curSequence = old;
                        this.CallDelayed(
                            0.5f,
                            () =>
                            {
                                if (!p.IsConnected)
                                    return;
                                p.Position = data.Pos;
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
                                p.ReferenceHub.characterClassManager.NetworkCurUnitName = RespawnManager.Singleton.NamingManager.AllUnitNames[data.UnitIndex].UnitName.Trim();
                                p.ReferenceHub.characterClassManager.NetworkCurSpawnableTeamType = data.UnitType;

                                foreach (var (effect, dur, intensity) in data.effects)
                                {
                                    effect.Intensity = intensity;
                                    effect.ServerChangeDuration(dur);
                                }
                            },
                            "PlayerLeft");
                    }
                }

                TalkCommand.Active.Remove(ev.Player.UserId);
            }
        }

        private void Player_Hurting(Exiled.Events.EventArgs.HurtingEventArgs ev)
        {
            if (!ev.Target.IsReadyPlayer())
                return;

            if (ev.Target.GetSessionVariable<bool>(SessionVarType.POST_TALK))
                ev.IsAllowed = false;

            if (DmgInfoCommand.Active.Contains(ev.Target.Id))
                ev.Target.Broadcast("DMG INFO", 10, $"({ev.Attacker.Id}) {ev.Attacker.Nickname} | {ev.Attacker.UserId}\n{ev.Handler.Type} | {ev.Amount}");
            if (!LastAttackers.TryGetValue(ev.Target.UserId, out (Player, Player) attackers))
                LastAttackers[ev.Target.UserId] = (null, ev.Attacker);
            else
                LastAttackers[ev.Target.UserId] = (attackers.Item1, ev.Attacker);
            if (ev.Attacker?.UserId == null)
                return;
            if (!LastVictims.TryGetValue(ev.Attacker.UserId, out (Player, Player) victims))
                LastVictims[ev.Attacker.UserId] = (null, ev.Target);
            else
                LastVictims[ev.Attacker.UserId] = (victims.Item1, ev.Target);
        }

        private void Player_Dying(Exiled.Events.EventArgs.DyingEventArgs ev)
        {
            if (!ev.Target.IsReadyPlayer())
                return;
            if (!LastAttackers.TryGetValue(ev.Target.UserId, out (Player, Player) attackers))
                LastAttackers[ev.Target.UserId] = (ev.Killer, null);
            else
                LastAttackers[ev.Target.UserId] = (ev.Killer, attackers.Item2);
            if (ev.Killer?.UserId == null)
                return;
            if (!LastVictims.TryGetValue(ev.Killer.UserId, out (Player, Player) victims))
                LastVictims[ev.Killer.UserId] = (ev.Target, null);
            else
                LastVictims[ev.Killer.UserId] = (ev.Target, victims.Item2);
        }

        private void Player_InteractingDoor(Exiled.Events.EventArgs.InteractingDoorEventArgs ev)
        {
            if (ev.Player == null)
            {
                this.Log.Debug("Player was null in Player_InteractingDoor", PluginHandler.Instance.Config.VerbouseOutput);
                return;
            }

            if (MDestroyCommand.Active.Contains(ev.Player.Id))
                ev.Door.BreakDoor();
            MDestroyCommand.Active.Remove(ev.Player.Id);
            if (MOpenCommand.Active.Contains(ev.Player.Id))
                ev.Door.IsOpen = true;
            MOpenCommand.Active.Remove(ev.Player.Id);
            if (MCloseCommand.Active.Contains(ev.Player.Id))
                ev.Door.IsOpen = false;
            MCloseCommand.Active.Remove(ev.Player.Id);
            if (MLockCommand.Active.Contains(ev.Player.Id))
                ev.Door.ChangeLock(ev.Door.DoorLockType | DoorLockType.AdminCommand);
            MLockCommand.Active.Remove(ev.Player.Id);
            if (MUnlockCommand.Active.Contains(ev.Player.Id))
                ev.Door.ChangeLock(ev.Door.DoorLockType & ~DoorLockType.AdminCommand);
            MUnlockCommand.Active.Remove(ev.Player.Id);
        }

        private void Server_RestartingRound()
        {
            MDestroyCommand.Active.Clear();
            MOpenCommand.Active.Clear();
            MCloseCommand.Active.Clear();
            MLockCommand.Active.Clear();
            MUnlockCommand.Active.Clear();
            SpecRoleCommand.SpecRole = RoleType.Spectator;
            MuteAllCommand.Muted.Clear();
            TalkCommand.SavedInfo.Clear();
            TalkCommand.Active.Clear();
        }
    }
}