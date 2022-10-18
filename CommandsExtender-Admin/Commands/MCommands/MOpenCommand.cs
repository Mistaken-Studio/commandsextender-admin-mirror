// -----------------------------------------------------------------------
// <copyright file="MOpenCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using CommandSystem;
using Exiled.API.Features;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands.MCommands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal sealed class MOpenCommand : IBetterCommand, IPermissionLocked
    {
        public static readonly HashSet<int> Active = new();

        public string Permission => "m.open";

        public override string Description => "MOpen";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "mopen";

        public override string[] Aliases => new string[] { };

        public string GetUsage()
        {
            return "MOpen";
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
                return new[] { "Activated" };
            else
                return new[] { "Deactivated" };
        }
    }
}
