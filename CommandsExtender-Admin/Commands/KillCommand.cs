// -----------------------------------------------------------------------
// <copyright file="KillCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using CommandSystem;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    /*[CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class KillCommand : IBetterCommand, IPermissionLocked
    {
        public string Permission => "kill";

        public override string Description => "Kill";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "kill";

        public override string[] Aliases => new string[] { "slay" };

        public override string[] Execute(ICommandSender sender, string[] args, out bool s)
        {
            s = false;
            if (args.Length == 0)
                return new string[] { this.GetUsage() };
            string reason = string.Empty;
            if (args.Length > 1)
                reason = string.Join(" ", args.Skip(1)).Trim();
            var output = this.ForeachPlayer(args[0], out bool success, (player) =>
            {
                player.Kill(new DamageTypes.DamageType("*Can not define what killed him"));
                player.Broadcast(5, $"<color=red>You have been killed by admin " + (reason != string.Empty ? $"with reason {reason}" : string.Empty) + "</color>");
                return new string[] { "Done" };
            });
            if (!success)
                return new string[] { "Player not found", this.GetUsage() };
            s = true;
            return output;
        }

        public string GetUsage()
        {
            return "Kill [Id] [Reason]";
        }
    }*/
}
