// -----------------------------------------------------------------------
// <copyright file="ShakeCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Exiled.API.Features;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class ShakeCommand : IBetterCommand, IPermissionLocked
    {
        public string Permission => "shake";

        public override string Description => "SHAKE";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "shake";

        public override string[] Aliases => new string[] { };

        public string GetUsage()
        {
            return "SHAKE";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = true;
            Warhead.Shake();
            return new string[] { "Done" };
        }
    }
}
