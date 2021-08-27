/*// -----------------------------------------------------------------------
// <copyright file="PBCClearCmd.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class PBCClearCmd : IBetterCommand, IPermissionLocked
    {
        public string Permission => "pbcc";

        public override string Description => "PBCCLEAR";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "pbcclear";

        public override string[] Aliases => new string[] { "pbcc" };

        public string GetUsage()
        {
            return "PBCCLEAR [Id]";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool s)
        {
            s = false;
            if (args.Length == 0) return new string[] { this.GetUsage() };
            var output = this.ForeachPlayer(args[0], out bool success, (player) =>
            {
                player.ClearBroadcasts();
                return new string[] { "Done" };
            });
            if (!success)
                return new string[] { "Player not found", this.GetUsage() };
            s = true;
            return output;
        }
    }
}
*/