// -----------------------------------------------------------------------
// <copyright file="RespawnLockCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class RespawnLockCommand : IBetterCommand
    {
        public override string Description => "RESPAWNLOCK";

        public override string Command => "respawnlock";

        public override string[] Aliases => new string[] { "disresp" };

        public string GetUsage()
        {
            return "RESPAWNLOCK true/false";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            if (args.Length == 0)
                return new string[] { this.GetUsage() };
            if (bool.TryParse(args[0], out bool value))
            {
                API.Utilities.Map.RespawnLock = value;
                success = true;
                return new string[] { "RespawnLock:" + value };
            }
            else
                return new string[] { this.GetUsage() };
        }
    }
}
