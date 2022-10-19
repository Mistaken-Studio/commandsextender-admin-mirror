// -----------------------------------------------------------------------
// <copyright file="VanishCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal sealed class VanishCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "vanish";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "vanish";

        public override string[] Aliases => new[] { "v" };

        public override string Description => "Enables vanish";

        public string[] Usage => new[] { "value (true/false)", "-lx (where x is 1 or 2 or 3, when not defined level 1 will be used)" };

        public string GetUsage()
        {
            return "VANISH true/false";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = true;
            var player = Player.Get(sender);
            var value = !API.Handlers.VanishHandler.Vanished.ContainsKey(player.Id);
            if (args.Any(str => str.ToLower() == "true"))
                value = true;
            else if (args.Any(str => str.ToLower() == "false"))
                value = false;
            if (args.Any(str => str == "-l3"))
            {
                API.Handlers.VanishHandler.SetGhost(player, value, 3);
                return new[] { $"GhostMode Status: {value} | Level: 3" };
            }
            else if (args.Any(str => str == "-l2"))
            {
                API.Handlers.VanishHandler.SetGhost(player, value, 2);
                return new[] { $"GhostMode Status: {value} | Level: 2" };
            }
            else if (args.Any(str => str == "-l1"))
            {
                API.Handlers.VanishHandler.SetGhost(player, value);
                return new[] { $"GhostMode Status: {value} | Level: 1" };
            }
            else
            {
                API.Handlers.VanishHandler.SetGhost(player, value);
                return new[] { $"GhostMode Status: {value} | Level: 1" };
            }
        }
    }
}
