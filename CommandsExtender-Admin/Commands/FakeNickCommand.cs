// -----------------------------------------------------------------------
// <copyright file="FakeNickCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Mistaken.API.Commands;
using RoundRestarting;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class FakeNickCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public static readonly Dictionary<string, string> FakeNicknames = new();

        public string Permission => "fakenick";

        public override string Description
            => "Changes nickname of target, if -full argument is used then target will be reconnected to server and his nickname will be fully changed (It won't be posible to check original nickname by report form)";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "fakenick";

        public override string[] Aliases => new[] { "fname", "fnick" };

        public string[] Usage => new[] { "%player%", "new nickname" };

        public override string[] Execute(ICommandSender sender, string[] args, out bool s)
        {
            s = false;
            if (args.Length < 2)
                return new[] { GetUsage() };

            var players = this.GetPlayers(args[0]);

            switch (players.Count)
            {
                case > 1:
                    return new[] { "<b><size=200%>1 PLAYER</size></b>" };
                case 0:
                    return new[] { "Player not found" };
            }

            var player = players[0];
            if (args.Contains("-full"))
            {
                FakeNicknames[player.UserId] = string.Join(" ", args.Skip(1).Where(i => i != "-full"));
                if (string.IsNullOrWhiteSpace(FakeNicknames[player.UserId]))
                    FakeNicknames.Remove(player.UserId);
                player.Connection.Send(new RoundRestartMessage(RoundRestartType.FullRestart, 0f, 0, true, false));
                return new[] { "Reconnecting" };
            }

            player.DisplayNickname = string.Join(" ", args.Skip(1));
            s = true;
            return new[] { "Done" };
        }

        private static string GetUsage()
        {
            return "fakenick [Id] [nick]";
        }
    }
}
