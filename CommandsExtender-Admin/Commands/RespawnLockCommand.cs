// -----------------------------------------------------------------------
// <copyright file="RespawnLockCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class RespawnLockCommand : IBetterCommand, IUsageProvider
    {
        public override string Description => "Blocks respawns";

        public override string Command => "respawnlock";

        public override string[] Aliases => new[] { "disresp" };

        public string[] Usage => new[] { "true/false" };

        public string GetUsage()
        {
            return "RESPAWNLOCK true/false";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            if (args.Length == 0)
                return new[] { this.GetUsage() };
            if (bool.TryParse(args[0], out var value))
            {
                API.Utilities.Map.RespawnLock = value;
                success = true;
                return new[] { "RespawnLock:" + value };
            }
            else
                return new[] { this.GetUsage() };
        }
    }
}
