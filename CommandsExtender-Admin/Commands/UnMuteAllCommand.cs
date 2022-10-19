// -----------------------------------------------------------------------
// <copyright file="UnMuteAllCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using CommandSystem;
using Mistaken.API;
using Mistaken.API.Commands;
using Mistaken.API.Extensions;
using Mistaken.API.GUI;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal sealed class UnMuteAllCommand : IBetterCommand, IPermissionLocked
    {
        public string Permission => "muteall";

        public override string Description => "Disable muteall";

        public override string Command => "unmuteall";

        public override string[] Aliases => new[] { "gunmute", "unmall" };

        public string PluginName => PluginHandler.Instance.Name;

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            if (!MuteAllCommand.GlobalMuteActive)
                return new[] { "Global Mute is not active" };
            success = true;
            MuteAllCommand.GlobalMuteActive = false;
            foreach (var uId in MuteAllCommand.Muted.ToArray())
            {
                if (RealPlayers.List.Any(p => p.UserId == uId))
                {
                    var player = RealPlayers.List.First(p => p.UserId == uId);
                    player.ReferenceHub.dissonanceUserSetup.AdministrativelyMuted = false;
                    player.SetGUI("globalMute", PseudoGUIPosition.TOP, "<color=green>[<color=orange>GLOBAL MUTE</color>]</color> Everyone was unmuted", 5);
                    MuteAllCommand.Muted.Remove(uId);

                    // player.Broadcast("GLOBAL MUTE", 10, "Everyone was unmuted");
                }
                else
                    MapPlus.Broadcast("GLOBAL MUTE", 10, $"Failed to unmute {uId} because plugin failed to find him, report this incident", Broadcast.BroadcastFlags.AdminChat);
            }

            RoundLogger.RLogger.Log("GLOBAL MUTE", "DEACTIVATED", "Deactivated GlobalMute");
            MapPlus.Broadcast("GLOBAL MUTE", 10, "Deactivated Global Mute", Broadcast.BroadcastFlags.AdminChat);
            return new[] { "Done" };
        }

        public string GetUsage()
        {
            return "UNMUTEALL";
        }
    }
}
