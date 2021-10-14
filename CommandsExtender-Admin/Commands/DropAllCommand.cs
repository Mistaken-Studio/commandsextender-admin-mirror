// -----------------------------------------------------------------------
// <copyright file="DropAllCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class DropAllCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "dropall";

        public override string Description => "Drops everything from targets inventory";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "dropall";

        public override string[] Aliases => new string[] { "dall" };

        public string[] Usage => new string[] { "%player%" };

        public string GetUsage()
        {
            return "DROPALL [Id]";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool s)
        {
            s = false;
            if (args.Length == 0)
                return new string[] { this.GetUsage() };
            var output = this.ForeachPlayer(args[0], out bool success, (player) =>
            {
                player.DropItems();
                return new string[] { "Done" };
            });
            if (!success)
                return new string[] { "Player not found", this.GetUsage() };
            s = true;
            return output;
        }
    }
}
