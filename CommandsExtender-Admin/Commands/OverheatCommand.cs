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
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal sealed class OverheatCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "overheat";

        public override string Description
            => "When triggered will overheat facility reactor (Kill everyone) after x amount of time where x is defined by level (-1 will cancel, 16 will be instant, 0 will be T-30minutes)";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "overheat";

        public override string[] Aliases => new[] { "oheat" };

        public string[] Usage => new[] { "level (0-7, 16, -1)" };

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            if (args.Length == 0 || !int.TryParse(args[0], out var proggressLevel))
                return new[] { "Proggress level has to be an int" };
            Map.Overheat.OverheatLevel = proggressLevel;
            success = true;
            return proggressLevel switch
            {
                -1 => new[] { "Deactivated overheat" },
                0 => new[] { "Overheat in T-30m" },
                1 => new[] { "Overheat in T-25m" },
                2 => new[] { "Overheat in T-20m" },
                3 => new[] { "Overheat in T-15m" },
                4 => new[] { "Overheat in T-10m" },
                5 => new[] { "Overheat in T-05m" },
                6 => new[] { "Overheat in T-03m" },
                7 => new[] { "Overheat in T-90s" },
                8 => new[] { "You can't use that, use 7 (T-90s) or 16 (T-00s)" },
                9 => new[] { "You can't use that, use 7 (T-90s) or 16 (T-00s)" },
                10 => new[] { "You can't use that, use 7 (T-90s) or 16 (T-00s)" },
                11 => new[] { "You can't use that, use 7 (T-90s) or 16 (T-00s)" },
                12 => new[] { "You can't use that, use 7 (T-90s) or 16 (T-00s)" },
                13 => new[] { "You can't use that, use 7 (T-90s) or 16 (T-00s)" },
                14 => new[] { "You can't use that, use 7 (T-90s) or 16 (T-00s)" },
                15 => new[] { "You can't use that, use 7 (T-90s) or 16 (T-00s)" },
                16 => new[] { "Overheat in T-00s" },
                _ => new[] { "Out of range" }
            };
        }
    }
}
