// -----------------------------------------------------------------------
// <copyright file="DoorLogCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using CommandSystem;
using Exiled.API.Features;
using Mistaken.API.Commands;
using Mistaken.API.Extensions;
using Mistaken.CommandsExtender.Admin.Logs;

namespace Mistaken.CommandsExtender.Admin.Commands.Logs
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class DoorLogCommand : IBetterCommand
    {
        public static readonly HashSet<int> Active = new();

        public static void Execute(Player player, Door door, List<DoorLog> data)
        {
            Active.Remove(player.Id);
            var toWrite = $"DoorLog for door {door.Type}";
            if (data != null)
            {
                foreach (var item in data)
                    toWrite += $"\n[{item.Time:HH:mm:ss}] {(item.Open ? "OPEN" : "CLOSE")} ({item.Player.Id}) {item.Player.Nickname} | {item.Player.UserId}";
            }

            player.Broadcast("DoorLog", 5, "Printed in console (~)", Broadcast.BroadcastFlags.AdminChat);
            player.SendConsoleMessage(toWrite, "green");
        }

        public override string Command => "doorlog";

        public override string[] Aliases => new[] { "dlog" };

        public override string Description => "Access door logs";

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = true;
            var player = Player.Get(sender);
            if (!Active.Contains(player.Id))
                Active.Add(player.Id);
            else
                Active.Remove(player.Id);

            if (Active.Contains(player.Id))
                return new[] { "Activated" };
            else
                return new[] { "Deactivated" };
        }
    }
}
