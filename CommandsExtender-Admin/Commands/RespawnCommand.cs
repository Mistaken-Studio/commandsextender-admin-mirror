// -----------------------------------------------------------------------
// <copyright file="RespawnCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class RespawnCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "respawn";

        public override string Description => "Edits time to respawn";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "respawn";

        public override string[] Aliases => new string[] { };

        public string[] Usage => new string[] { "time to respawn" };

        public string GetUsage()
        {
            return "respawn [time]";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool s)
        {
            s = false;
            if (args.Length == 0 || !int.TryParse(args[0], out int value))
                return new string[] { this.GetUsage() };
            Respawning.RespawnManager.Singleton._timeForNextSequence = (float)Respawning.RespawnManager.Singleton._stopwatch.Elapsed.TotalSeconds + value;
            s = true;
            return new string[] { "Done" };
        }
    }
}
