// -----------------------------------------------------------------------
// <copyright file="FakeNickCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Exiled.API.Extensions;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class FakeNickCommand : IBetterCommand, IPermissionLocked
    {
        public static readonly Dictionary<string, string> FullNicknames = new Dictionary<string, string>();

        public string Permission => "fakenick";

        public override string Description =>
            "Fakenick";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "fakenick";

        public override string[] Aliases => new string[] { "fname", "fnick" };

        public string GetUsage()
        {
            return "fakenick [Id] [nick]";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool s)
        {
            s = false;
            if (args.Length < 2) return new string[] { this.GetUsage() };
            var players = this.GetPlayers(args[0]);
            if (players.Count > 1)
                return new string[] { "<b><size=200%>1 PLAYER</size></b>" };
            if (players.Count == 0)
                return new string[] { "Player not found" };
            var player = players[0];
            if (args.Contains("-full"))
            {
                FullNicknames[player.UserId] = string.Join(" ", args.Skip(1).Where(i => i != "-full"));
                if (string.IsNullOrWhiteSpace(FullNicknames[player.UserId]))
                    FullNicknames.Remove(player.UserId);
                MirrorExtensions.SendFakeTargetRpc(player, player.Connection.identity, typeof(PlayerStats), nameof(PlayerStats.RpcRoundrestart), 0.1f, true);
                return new string[] { "Reconnecting" };
            }

            player.DisplayNickname = string.Join(" ", args.Skip(1));
            s = true;
            return new string[] { "Done" };
        }
    }
}
