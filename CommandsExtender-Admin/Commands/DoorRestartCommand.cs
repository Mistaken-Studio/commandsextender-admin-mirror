// -----------------------------------------------------------------------
// <copyright file="DoorRestartCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Mistaken.API.Commands;
using Mistaken.API.Utilities;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal sealed class DoorRestartCommand : IBetterCommand, IPermissionLocked
    {
        public string Permission => "doorrestart";

        public override string Description => "Restart Facility Door System, Closes all doors in facility with CASSIE message";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "doorrestart";

        public override string[] Aliases => new[] { "drestart" };

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            Map.RestartDoors();
            success = true;
            return new[] { "Done" };
        }
    }
}
