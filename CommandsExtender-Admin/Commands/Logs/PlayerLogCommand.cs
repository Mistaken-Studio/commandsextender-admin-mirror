// -----------------------------------------------------------------------
// <copyright file="PlayerLogCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Mistaken.API.Commands;
using Mistaken.CommandsExtender.Admin.Logs;

namespace Mistaken.CommandsExtender.Admin.Commands.Logs
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class PlayerLogCommand : IBetterCommand, IUsageProvider
    {
        public override string Description => "PlayerLog";

        public override string Command => "playerlog";

        public override string[] Aliases => new[] { "plog" };

        public string[] Usage => new[] { "round id?" };

        public string GetUsage()
        {
            return "PlayerLog [Round Id]";
        }

        public string[] GetList()
        {
            List<string> tor = NorthwoodLib.Pools.ListPool<string>.Shared.Rent();
            tor.Add(this.GetUsage());
            foreach (var item in LogManager.RoundStartTime.Reverse())
                tor.Add($"RoundId: {item.Key} | Time: {item.Value}");
            var torArray = tor.ToArray();
            NorthwoodLib.Pools.ListPool<string>.Shared.Return(tor);
            return torArray;
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            if (args.Length == 0)
                return this.GetList();
            else
            {
                if (int.TryParse(args[0], out var id))
                {
                    if (!LogManager.PlayerLogs.ContainsKey(id))
                        return new[] { "PlayerLog from that round not found" };
                    List<string> tor = NorthwoodLib.Pools.ListPool<string>.Shared.Rent();
                    tor.Add("PlayerLog for round with id " + id);
                    foreach (var item in LogManager.PlayerLogs[id])
                        tor.Add($"({item.Id}) {item.Name}\n{item.UserId} | {item.Ip}\n IntercomMute: {item.IntercomMute} | Mute: {item.Mute}\n");
                    success = true;
                    var torArray = tor.ToArray();
                    NorthwoodLib.Pools.ListPool<string>.Shared.Return(tor);
                    return torArray;
                }
                else
                    return new[] { "Id has to be int" };
            }
        }
    }
}
