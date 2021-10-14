// -----------------------------------------------------------------------
// <copyright file="SCP914LogCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands.Logs
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class SCP914LogCommand : IBetterCommand, IUsageProvider
    {
        public override string Command => "scp914log";

        public override string[] Aliases => new string[] { "s914log" };

        public string[] Usage => new string[] { "round id ?" };

        public string GetUsage()
        {
            return "SCP914Log [Round Id]";
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
                if (int.TryParse(args[0], out int id))
                {
                    if (!LogManager.SCP914Logs.ContainsKey(id))
                        return new string[] { "SCP914Log from that round not found" };
                    List<string> tor = NorthwoodLib.Pools.ListPool<string>.Shared.Rent();
                    tor.Add("SCP914Log for round with id " + id);
                    foreach (var item in LogManager.SCP914Logs[id].Where(i => args.Any(arg => arg == "-change") || (i.Action != SCP914Action.CHANGE_ROUGH && i.Action != SCP914Action.CHANGE_COARSE && i.Action != SCP914Action.CHANGE_1TO1 && i.Action != SCP914Action.CHANGE_FINE && i.Action != SCP914Action.CHANGE_VERY_FINE)))
                        tor.Add($"[{item.Time:HH:mm:ss}] ({item.ID}) {item.Name} | {item.Action}");
                    success = true;
                    var torArray = tor.ToArray();
                    NorthwoodLib.Pools.ListPool<string>.Shared.Return(tor);
                    return torArray;
                }
                else
                    return new string[] { "Id has to be int" };
            }
        }
    }
}
