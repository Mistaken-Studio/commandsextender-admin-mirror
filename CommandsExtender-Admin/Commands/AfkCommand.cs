// -----------------------------------------------------------------------
// <copyright file="AfkCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal sealed class AfkCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "afk";

        public override string Description => "Disconnect for AFK";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "afk";

        public override string[] Aliases => new string[] { };

        public string[] Usage => new[] { "%player%" };

        public override string[] Execute(ICommandSender sender, string[] args, out bool s)
        {
            s = false;
            if (args.Length == 0)
                return new[] { GetUsage() };

            var players = this.GetPlayers(args[0]);

            switch (players.Count)
            {
                case > 1:
                    return new[] { "<b><size=200%>1 PLAYER</size></b>" };
                case 0:
                    return new[] { "Player not found" };
            }

            players[0].Disconnect("You were kicked for being AFK");
            s = true;
            return new[] { "Done" };
        }

        private static string GetUsage()
        {
            return "Afk [Id]";
        }
    }
}
