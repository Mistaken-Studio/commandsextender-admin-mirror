// -----------------------------------------------------------------------
// <copyright file="FlashLogCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Exiled.API.Features;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands.Logs
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class FlashLogCommand : IBetterCommand, IUsageProvider
    {
        public override string Command => "flashlog";

        public override string[] Aliases => new string[] { "flog" };

        public string[] Usage => new string[] { "%player%" };

        public string GetUsage()
        {
            return "FlashLog [Player Id]";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            if (args.Length == 0)
                return new string[] { this.GetUsage() };
            if (!int.TryParse(args[0], out int id))
                return new string[] { "Id has to be int" };
            if (!LogManager.FlashLog.TryGetValue(id, out Player player))
                return new string[] { "Flashlog not found" };
            return new string[] { $"Player: ({player.Id}) {player.Nickname}", $"UserId: {player.UserId}" };
        }
    }
}
