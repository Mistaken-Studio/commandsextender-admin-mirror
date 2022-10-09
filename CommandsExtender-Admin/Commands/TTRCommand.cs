// -----------------------------------------------------------------------
// <copyright file="TTRCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Mistaken.API.Commands;
using Respawning;
using UnityEngine;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class TTRCommand : IBetterCommand, IPermissionLocked
    {
        public string Permission => "ttr";

        public override string Description => "Time To Respawn";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "ttr";

        public override string[] Aliases => new string[] { };

        public string GetUsage()
        {
            return "TTR";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = true;

            var respawnManager = RespawnManager.Singleton;
            var ttr = Mathf.RoundToInt(RespawnManager.Singleton._timeForNextSequence - (float)RespawnManager.Singleton._stopwatch.Elapsed.TotalSeconds);
            return new[] { $"TTR: {(ttr - (ttr % 60)) / 60:00}m {ttr % 60:00}s | {respawnManager.NextKnownTeam}" };
        }
    }
}
