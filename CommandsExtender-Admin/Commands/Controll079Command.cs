// -----------------------------------------------------------------------
// <copyright file="Controll079Command.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Exiled.API.Features.Roles;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class Controll079Command : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "controll079";

        public override string Description => "This command can for example give you 5th level or force 1st";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "controll079";

        public override string[] Aliases => new[] { "c079" };

        public string[] Usage => new[] { "%player%", "lvl/xp/ap/max_ap", "value" };

        public string GetUsage()
        {
            return "CONTROLL079 [Id] LVL/XP/AP/MAX_AP [VALUE]";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool s)
        {
            s = false;
            if (args.Length < 3)
                return new[] { this.GetUsage() };
            if (!byte.TryParse(args[2], out var value))
                return new[] { this.GetUsage() };
            var output = this.ForeachPlayer(args[0], out var success, (player) =>
            {
                var scp = (Scp079Role)player.Role;
                switch (args[1].ToLower())
                {
                    case "lvl":
                        scp.Level = value;
                        return new[] { "Done" };
                    case "xp":
                        scp.Experience = value;
                        return new[] { "Done" };
                    case "ap":
                        scp.Energy = value;
                        return new[] { "Done" };
                    case "max_ap":
                        scp.MaxEnergy = value;
                        return new[] { "Done" };
                    default:
                        return new[] { this.GetUsage() };
                }
            });
            if (!success)
                return new[] { "Plyaer not found", this.GetUsage() };
            s = true;
            return output;
        }
    }
}
