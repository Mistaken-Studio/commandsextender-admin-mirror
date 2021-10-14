﻿// -----------------------------------------------------------------------
// <copyright file="VanishCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using CommandSystem;
using Mistaken.API;
using Mistaken.API.Commands;
using Mistaken.API.Extensions;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class VanishCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "vanish";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "vanish";

        public override string[] Aliases => new string[] { "v" };

        public override string Description => "Enables vanish";

        public string[] Usage => new string[] { "value (true/false)", "-lx (where x is 1 or 2 or 3, when not defined level 1 will be used)" };

        public string GetUsage()
        {
            return "VANISH true/false";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = true;
            var player = sender.GetPlayer();
            bool value = !VanishHandler.Vanished.ContainsKey(player.Id);
            if (args.Any(str => str.ToLower() == "true"))
                value = true;
            else if (args.Any(str => str.ToLower() == "false"))
                value = false;
            if (args.Any(str => str == "-l3"))
            {
                VanishHandler.SetGhost(player, value, 3);
                return new string[] { $"GhostMode Status: {value} | Level: 3" };
            }
            else if (args.Any(str => str == "-l2"))
            {
                VanishHandler.SetGhost(player, value, 2);
                return new string[] { $"GhostMode Status: {value} | Level: 2" };
            }
            else if (args.Any(str => str == "-l1"))
            {
                VanishHandler.SetGhost(player, value, 1);
                return new string[] { $"GhostMode Status: {value} | Level: 1" };
            }
            else
            {
                VanishHandler.SetGhost(player, value);
                return new string[] { $"GhostMode Status: {value} | Level: 1" };
            }
        }
    }
}
