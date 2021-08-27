/*// -----------------------------------------------------------------------
// <copyright file="PBCCmd.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using CommandSystem;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class PBCCmd : IBetterCommand, IPermissionLocked
    {
        public string Permission => "pbc";

        public override string Description => "PBC";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "pbc";

        public override string[] Aliases => new string[] { };

        public string GetUsage()
        {
            return "PBC [Id] [DURATION] [MESSAGE]";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool s)
        {
            s = false;
            if (args.Length < 3) return new string[] { this.GetUsage() };
            if (ushort.TryParse(args[1], out ushort duration))
            {
                var msg = string.Join(" ", args.Skip(2));
                var output = this.ForeachPlayer(args[0], out bool success, (player) =>
                {
                    player.Broadcast(duration, msg);
                    return new string[] { "Done" };
                });
                if (!success)
                    return new string[] { "Player not found", this.GetUsage() };
                s = true;
                return output;
            }
            else return new string[] { "Duration has to be a uint", this.GetUsage() };
        }
    }
}
*/