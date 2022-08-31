// -----------------------------------------------------------------------
// <copyright file="ElevatorLogCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using Mistaken.API.Commands;
using Mistaken.API.Extensions;

namespace Mistaken.CommandsExtender.Admin.Commands.Logs
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class ElevatorLogCommand : IBetterCommand
    {
        public static readonly HashSet<int> Active = new HashSet<int>();

        public static void Execute(Player p, ElevatorType d, List<ElevatorLog> data)
        {
            Active.Remove(p.Id);
            string toWrite = $"ElevatorLog for elevator {d}";
            if (data != null)
            {
                foreach (var item in data)
                    toWrite += $"\n[{item.Time:HH:mm:ss)}] {item.Status} ({item.Player.Id}) {item.Player.Nickname} | {item.Player.UserId}";
            }

            p.Broadcast("ElevatorLog", 5, "Printed in console (~)", Broadcast.BroadcastFlags.AdminChat);
            p.SendConsoleMessage(toWrite, "green");
        }

        public override string Command => "elevatorlog";

        public override string[] Aliases => new string[] { "elog" };

        public override string Description => "Access elevator logs";

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = true;
            var player = Player.Get(sender);
            if (!Active.Contains(player.Id))
                Active.Add(player.Id);
            else
                Active.Remove(player.Id);
            if (Active.Contains(player.Id))
                return new string[] { "Activated" };
            else
                return new string[] { "Deactivated" };
        }
    }
}
