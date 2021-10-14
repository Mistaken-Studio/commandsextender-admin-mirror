// -----------------------------------------------------------------------
// <copyright file="AfkCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class AfkCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "afk";

        public override string Description => "Disconnect for AFK";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "afk";

        public override string[] Aliases => new string[] { };

        public string[] Usage => new string[] { "%player%" };

        public string GetUsage()
        {
            return "Afk [Id]";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool s)
        {
            s = false;
            if (args.Length == 0) return new string[] { this.GetUsage() };
            var players = this.GetPlayers(args[0]);
            if (players.Count > 1)
                return new string[] { "<b><size=200%>1 PLAYER</size></b>" };
            if (players.Count == 0)
                return new string[] { "Player not found" };
            players[0].Disconnect("You were kicked for being AFK");
            s = true;
            return new string[] { "Done" };
        }
    }
}
