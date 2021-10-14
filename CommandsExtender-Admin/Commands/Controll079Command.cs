// -----------------------------------------------------------------------
// <copyright file="Controll079Command.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class Controll079Command : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "controll079";

        public override string Description => "This command can for example give you 5th level or force 1st";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "controll079";

        public override string[] Aliases => new string[] { "c079" };

        public string[] Usage => new string[] { "%player%", "lvl/xp/ap/max_ap", "value" };

        public string GetUsage()
        {
            return "CONTROLL079 [Id] LVL/XP/AP/MAX_AP [VALUE]";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool s)
        {
            s = false;
            if (args.Length < 3)
                return new string[] { this.GetUsage() };
            if (!byte.TryParse(args[2], out byte value))
                return new string[] { this.GetUsage() };
            var output = this.ForeachPlayer(args[0], out bool success, (player) =>
            {
                switch (args[1].ToLower())
                {
                    case "lvl":
                        player.Level = value;
                        return new string[] { "Done" };
                    case "xp":
                        player.Experience = value;
                        return new string[] { "Done" };
                    case "ap":
                        player.Energy = value;
                        return new string[] { "Done" };
                    case "max_ap":
                        player.MaxEnergy = value;
                        return new string[] { "Done" };
                    default:
                        return new string[] { this.GetUsage() };
                }
            });
            if (!success)
                return new string[] { "Plyaer not found", this.GetUsage() };
            s = true;
            return output;
        }
    }
}
