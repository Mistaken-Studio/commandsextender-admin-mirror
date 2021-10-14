// -----------------------------------------------------------------------
// <copyright file="OverheatCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Mistaken.API.Commands;
using Mistaken.API.Utilities;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class OverheatCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "overheat";

        public override string Description
            => "When triggered will overheat facility reactor (Kill everyone) after x amount of time where x is defined by level (-1 will cancel, 16 will be instant, 0 will be T-30minutes)";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "overheat";

        public override string[] Aliases => new string[] { "oheat" };

        public string[] Usage => new string[] { "level (0-7, 16, -1)" };

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            if (args.Length == 0 || !int.TryParse(args[0], out int proggressLevel))
                return new string[] { "Proggress level has to be an int" };
            Map.Overheat.OverheatLevel = proggressLevel;
            success = true;
            switch (proggressLevel)
            {
                case -1:
                    return new string[] { "Deactivated overheat" };
                case 0:
                    return new string[] { "Overheat in T-30m" };
                case 1:
                    return new string[] { "Overheat in T-25m" };
                case 2:
                    return new string[] { "Overheat in T-20m" };
                case 3:
                    return new string[] { "Overheat in T-15m" };
                case 4:
                    return new string[] { "Overheat in T-10m" };
                case 5:
                    return new string[] { "Overheat in T-05m" };
                case 6:
                    return new string[] { "Overheat in T-03m" };
                case 7:
                    return new string[] { "Overheat in T-90s" };
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                    return new string[] { "You can't use that, use 7 (T-90s) or 16 (T-00s)" };
                case 16:
                    return new string[] { "Overheat in T-00s" };
                default:
                    return new string[] { "Out of range" };
            }
        }
    }
}
