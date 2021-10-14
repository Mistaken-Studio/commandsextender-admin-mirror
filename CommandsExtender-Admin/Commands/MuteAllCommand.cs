﻿// -----------------------------------------------------------------------
// <copyright file="MuteAllCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Mistaken.API;
using Mistaken.API.Commands;
using Mistaken.API.Extensions;
using Mistaken.API.GUI;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class MuteAllCommand : IBetterCommand, IPermissionLocked
    {
        public static readonly List<string> Muted = new List<string>();

        public static bool GlobalMuteActive { get; internal set; } = false;

        public string Permission => "muteall";

        public override string Description => "Mutes everybody except admins";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "muteall";

        public override string[] Aliases => new string[] { "gmute", "mall" };

        public string GetUsage()
        {
            return "MUTEALL";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = true;
            GlobalMuteActive = true;
            foreach (var player in RealPlayers.List.Where(p => p.IsMuted == false && !p.CheckPermissions(PlayerPermissions.AdminChat)))
            {
                Muted.Add(player.UserId);
                player.ReferenceHub.dissonanceUserSetup.AdministrativelyMuted = true;
            }

            API.Diagnostics.Module.RunSafeCoroutine(this.InformGlobalMute(), "MuteAllCommand.InformGlobalMute");

            RoundLogger.RLogger.Log("GLOBAL MUTE", "ACTIVATED", "Activated GlobalMute");
            MapPlus.Broadcast("GLOBAL MUTE", 10, "Activated Global Mute", Broadcast.BroadcastFlags.AdminChat);
            return new string[] { "Done" };
        }

        private IEnumerator<float> InformGlobalMute()
        {
            while (GlobalMuteActive)
            {
                foreach (var player in RealPlayers.List)
                    player.SetGUI("globalMute", PseudoGUIPosition.TOP, "<color=green>[<color=orange>GLOBAL MUTE</color>]</color> Everyone except admins are muted");
                yield return MEC.Timing.WaitForSeconds(1);
            }

            foreach (var player in RealPlayers.List)
                player.SetGUI("globalMute", PseudoGUIPosition.TOP, null);
        }
    }
}