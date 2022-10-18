// -----------------------------------------------------------------------
// <copyright file="BlackoutCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Mistaken.API.Commands;
using Mistaken.API.Utilities;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal sealed class BlackoutCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "blackout";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "blackout";

        public override string[] Aliases => new string[] { };

        public override string Description => "Enabled or disabled blackout in facility";

        public string[] Usage => new[] { "value (true/false)" };

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            if (args.Length == 0)
                return new[] { this.GetUsage() };
            else
            {
                if (bool.TryParse(args[0], out var value))
                {
                    Map.Blackout.Enabled = value;
                    success = true;
                    if (value)
                        return new[] { "Enabled" };
                    else
                        return new[] { "Disabled" };
                }
                else
                {
                    return new[] { this.GetUsage() };
                }
            }
        }

        public string GetUsage()
        {
            return "BLACKOUT true/false";
        }
    }
}
