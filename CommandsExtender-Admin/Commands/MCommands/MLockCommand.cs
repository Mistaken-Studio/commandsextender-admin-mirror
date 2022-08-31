// -----------------------------------------------------------------------
// <copyright file="MLockCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using CommandSystem;
using Exiled.API.Features;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class MLockCommand : IBetterCommand, IPermissionLocked
    {
        public static readonly HashSet<int> Active = new HashSet<int>();

        public string Permission => "m.lock";

        public override string Description => "MLock";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "mlock";

        public override string[] Aliases => new string[] { };

        public string GetUsage()
        {
            return "MLock";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = true;
            var player = Player.Get(sender);
            if (!Active.Contains(player.Id))
                Active.Add(player.Id);
            else
                Active.Remove(player.Id);
            if (Active.Contains(player.Id))
                return new string[] { "Activated" };
            else
                return new string[] { "Deactivated" };
        }
    }
}
