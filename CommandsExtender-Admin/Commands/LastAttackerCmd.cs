// -----------------------------------------------------------------------
// <copyright file="LastAttackerCmd.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Exiled.API.Features;
using Mistaken.API.Commands;
using Mistaken.API.Extensions;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class LastAttackerCmd : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "last_attacker";

        public override string Description => "Get Last Attacker";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "lastattacker";

        public override string[] Aliases => new string[] { "la" };

        public string[] Usage => new string[] { "%player%" };

        public override string[] Execute(ICommandSender sender, string[] args, out bool s)
        {
            s = false;
            if (args.Length == 0) return new string[] { this.GetUsage() };
            var output = this.ForeachPlayer(args[0], out bool success, (player) =>
            {
                CommandsHandler.LastAttackers.TryGetValue(player.UserId, out (Player, Player) info);
                string[] tor = new string[3];
                tor[0] = $"Data for {player.UserId}";
                tor[1] = $"Attacker: {info.Item1?.ToString(true) ?? "Not found"}";
                tor[2] = $"Killer: {info.Item2?.ToString(true) ?? "Not found"}";
                return tor;
            });
            if (!success) return new string[] { "Player not found", this.GetUsage() };
            s = true;
            return output;
        }

        public string GetUsage()
        {
            return "LA (Id)";
        }
    }
}
