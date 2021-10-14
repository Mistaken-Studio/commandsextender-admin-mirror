// -----------------------------------------------------------------------
// <copyright file="ConfirmCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin
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
            var commandSender = sender as CommandSender;

            foreach (Ban2Command.BanData item in Ban2Command.AwaitingBans.ToArray())
            {
                if (item.Admin?.SenderId == commandSender.SenderId)
                {
                    item.Execute();
                    success = true;
                    return item.StyleBan();
                }
            }

            return new string[] { "Ban not found" };
        }
    }
}
