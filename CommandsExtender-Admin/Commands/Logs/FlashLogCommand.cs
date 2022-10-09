// -----------------------------------------------------------------------
// <copyright file="FlashLogCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Mistaken.API.Commands;
using Mistaken.CommandsExtender.Admin.Logs;

namespace Mistaken.CommandsExtender.Admin.Commands.Logs
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class FlashLogCommand : IBetterCommand, IUsageProvider
    {
        public override string Command => "flashlog";

        public override string[] Aliases => new[] { "flog" };

        public string[] Usage => new[] { "%player%" };

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            if (args.Length == 0)
                return new[] { GetUsage() };

            if (!int.TryParse(args[0], out var id))
                return new[] { "Id has to be int" };

            if (!LogManager.FlashLog.TryGetValue(id, out var player))
                return new[] { "Flashlog not found" };

            return new[] { $"Player: ({player.Id}) {player.Nickname}", $"UserId: {player.UserId}" };
        }

        private static string GetUsage()
        {
            return "FlashLog [Player Id]";
        }
    }
}
