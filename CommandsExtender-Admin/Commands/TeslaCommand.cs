﻿// -----------------------------------------------------------------------
// <copyright file="TeslaCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Mistaken.API.Commands;
using Mistaken.API.Utilities;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class TeslaCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "tesla";

        public override string Description => "Manipulate Facility Tesla System";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "tesla";

        public override string[] Aliases => new string[] { };

        public string[] Usage => new string[] { "action (enable/disable/restart)" };

        public string GetUsage()
        {
            return "TESLA ENABLE/DISABLE/RESTART";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            if (args.Length == 0) return new string[] { this.GetUsage() };
            success = true;
            switch (args[0].ToLower())
            {
                case "enable":
                    {
                        Map.TeslaMode = TeslaMode.ENABLED;
                        return new string[] { "Tesla gates Enabled" };
                    }

                case "disable":
                    {
                        Map.TeslaMode = TeslaMode.DISABLED;
                        return new string[] { "Tesla gates Disabled for players" };
                    }

                case "disableall":
                    {
                        Map.TeslaMode = TeslaMode.DISABLED_FOR_ALL;
                        return new string[] { "Tesla gates Disabled for all" };
                    }

                case "disable079":
                    {
                        Map.TeslaMode = TeslaMode.DISABLED_FOR_079;
                        return new string[] { "Tesla gates Disabled for 079" };
                    }

                case "restart":
                    {
                        Map.RestartTeslaGates(true);
                        return new string[] { "Tesla gates Restarted" };
                    }

                default:
                    success = false;
                    return new string[] { this.GetUsage() };
            }
        }
    }
}