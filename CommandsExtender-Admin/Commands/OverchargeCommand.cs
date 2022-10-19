// -----------------------------------------------------------------------
// <copyright file="OverchargeCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Mistaken.API.Commands;
using UnityEngine;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal sealed class OverchargeCommand : IBetterCommand, IPermissionLocked
    {
        public string Permission => "overcharge";

        public override string Description => "Forces SCP079 overcharge";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "KILL079";

        public override string[] Aliases => new[] { "k079" };

        public string GetUsage()
        {
            return "KILL079";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            Object.FindObjectOfType<Recontainer079>().BeginOvercharge();
            success = true;
            return new[] { "Starting" };
        }
    }
}
