// -----------------------------------------------------------------------
// <copyright file="ConfirmCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using CommandSystem;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands.Ban2
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class ConfirmCommand : IBetterCommand
    {
        public override string Command => "confirm";

        public override string[] Aliases => new string[] { };

        public override string Description => "Let's confirm some bans";

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;

            if (sender is not CommandSender commandSender)
                throw new ArgumentException("Expected CommandSender", nameof(sender));

            foreach (var item in Ban2Command.AwaitingBans.ToArray())
            {
                if (item.Admin?.SenderId != commandSender.SenderId)
                    continue;
                item.Execute();
                success = true;
                return item.StyleBan();
            }

            return new[] { "Ban not found" };
        }
    }
}
