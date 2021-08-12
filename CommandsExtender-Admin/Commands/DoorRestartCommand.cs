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
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class DoorRestartCommand : IBetterCommand, IPermissionLocked
    {
        public string Permission => "doorrestart";

        public override string Description => "Restart Facility Door System";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "doorrestart";

        public override string[] Aliases => new string[] { "drestart" };

        public string GetUsage()
        {
            return "DoorRestart";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            Map.RestartDoors();
            success = true;
            return new string[] { "Done" };
        }
    }
}
