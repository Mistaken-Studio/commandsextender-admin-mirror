// -----------------------------------------------------------------------
// <copyright file="GiveRankCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using CommandSystem;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class GiveRankCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "giverank";

        public override string Description => "Gives Rank to target players";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "giverank";

        public override string[] Aliases => new string[] { };

        public string[] Usage => new[] { "%player%", "rank color", "rank name" };

        public string GetUsage()
        {
            return "GIVERANK [Id] [COLOR] [NAME]";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool s)
        {
            s = false;
            if (args.Length < 3)
                return new[] { this.GetUsage() };
            var color = args[1].ToLower();
            var txt = string.Join(" ", args.Skip(2));
            var output = this.ForeachPlayer(
                args[0],
                out var success,
                (player) =>
                {
                    player.RankColor = color;
                    player.RankName = txt;

                    return new[] { "Done" };
                },
                true);
            if (!success)
                return new[] { "Player not found", this.GetUsage() };
            s = true;
            return output;
        }
    }
}
