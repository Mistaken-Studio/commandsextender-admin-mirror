// -----------------------------------------------------------------------
// <copyright file="CassiePrivateCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using CommandSystem;
using Exiled.API.Extensions;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class CassiePrivateCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "cassie_p";

        public override string Description => "Sends CASSIE broadcast that only target players can hear";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "cassie_p";

        public override string[] Aliases => new string[] { "cassie_private" };

        public string[] Usage => new string[] { "%player%", "cassie announcment" };

        public string GetUsage()
        {
            return "CASSIE_P [Target] [MESSAGE]";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool s)
        {
            s = false;
            if (args.Length == 0)
                return new string[] { this.GetUsage() };
            var pids = args[0];
            args = args.Skip(1).ToArray();
            var tor = this.ForeachPlayer(pids, out bool success, (player) =>
            {
                player.PlayCassieAnnouncement(string.Join(" ", args), false, false);
                return new string[] { "Done" };
            });
            if (!success)
                return new string[] { "No players found" };
            s = true;
            return tor;
        }
    }
}
