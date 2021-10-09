// -----------------------------------------------------------------------
// <copyright file="OverchargeCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class OverchargeCommand : IBetterCommand, IPermissionLocked
    {
        public string Permission => "overcharge";

        public override string Description => "Teminate 079";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "KILL079";

        public override string[] Aliases => new string[] { "k079" };

        public string GetUsage()
        {
            return "KILL079";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            Recontainer079.FindObjectOfType<Recontainer079>().BeginOvercharge();
            success = true;
            return new string[] { "Starting" };
        }
    }
}
