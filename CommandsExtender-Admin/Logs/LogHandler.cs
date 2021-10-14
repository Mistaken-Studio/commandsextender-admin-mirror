// -----------------------------------------------------------------------
// <copyright file="LogHandler.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Exiled.API.Extensions;
using Exiled.API.Interfaces;
using Mistaken.API;
using Mistaken.API.Diagnostics;
using Mistaken.API.Extensions;

namespace Mistaken.CommandsExtender.Admin
{
    internal class LogHandler : Module
    {
        public LogHandler(IPlugin<IConfig> plugin)
            : base(plugin)
        {
        }

        public override string Name => "LogHandler";

        public override void OnDisable()
        {
            throw new NotImplementedException();
        }

        public override void OnEnable()
        {
            Exiled.Events.Handlers.Server.RestartingRound += this.Handle(() => this.Server_RestartingRound(), "RoundRestart");
            Exiled.Events.Handlers.Player.InteractingElevator += this.Handle<Exiled.Events.EventArgs.InteractingElevatorEventArgs>((ev) => this.Player_InteractingElevator(ev));
            Exiled.Events.Handlers.Player.InteractingDoor += this.Handle<Exiled.Events.EventArgs.InteractingDoorEventArgs>((ev) => this.Player_InteractingDoor(ev));
            Exiled.Events.Handlers.Player.Verified += this.Handle<Exiled.Events.EventArgs.VerifiedEventArgs>((ev) => this.Player_Verified(ev));
            Exiled.Events.Handlers.Server.WaitingForPlayers += this.Handle(() => this.Server_WaitingForPlayers(), "WaitingForPlayers");
        }

        private void Server_WaitingForPlayers()
        {
            Commands.Logs.DoorLogCommand.Active.Clear();
            Commands.Logs.ElevatorLogCommand.Active.Clear();
        }

        private void Player_Verified(Exiled.Events.EventArgs.VerifiedEventArgs ev)
        {
            if (!LogManager.PlayerLogs.ContainsKey(RoundPlus.RoundId))
                LogManager.PlayerLogs[RoundPlus.RoundId] = NorthwoodLib.Pools.ListPool<PlayerInfo>.Shared.Rent();
            LogManager.PlayerLogs[RoundPlus.RoundId].Add(new PlayerInfo(ev.Player));
        }

        private void Player_InteractingElevator(Exiled.Events.EventArgs.InteractingElevatorEventArgs ev)
        {
            if (Commands.Logs.ElevatorLogCommand.Active.Contains(ev.Player.Id))
            {
                if (LogManager.ElevatorLogs.TryGetValue(ev.Lift.Type(), out List<ElevatorLog> value))
                    Commands.Logs.ElevatorLogCommand.Execute(ev.Player, ev.Lift.Type(), value);
                else
                    ev.Player.Broadcast("ELEVATOR LOG", 10, "Elevator data not found", Broadcast.BroadcastFlags.AdminChat);
                ev.IsAllowed = false;
            }

            if (!ev.IsAllowed)
                return;

            if (!LogManager.ElevatorLogs.ContainsKey(ev.Lift.Type()))
                LogManager.ElevatorLogs.Add(ev.Lift.Type(), NorthwoodLib.Pools.ListPool<ElevatorLog>.Shared.Rent());
            LogManager.ElevatorLogs[ev.Lift.Type()].Add(new ElevatorLog(ev));
        }

        private void Player_InteractingDoor(Exiled.Events.EventArgs.InteractingDoorEventArgs ev)
        {
            if (Commands.Logs.DoorLogCommand.Active.Contains(ev.Player.Id))
            {
                if (LogManager.DoorLogs.TryGetValue(ev.Door, out List<DoorLog> value))
                    Commands.Logs.DoorLogCommand.Execute(ev.Player, ev.Door, value);
                else
                    ev.Player.Broadcast("DOOR LOG", 10, "Door data not found");
                ev.IsAllowed = false;
            }

            if (!ev.IsAllowed)
                return;
            if (!LogManager.DoorLogs.ContainsKey(ev.Door))
                LogManager.DoorLogs[ev.Door] = NorthwoodLib.Pools.ListPool<DoorLog>.Shared.Rent();
            LogManager.DoorLogs[ev.Door].Add(new DoorLog(ev));
        }

        private void Server_RestartingRound()
        {
            LogManager.RoundStartTime[RoundPlus.RoundId] = DateTime.Now;
            foreach (var item in LogManager.DoorLogs)
                NorthwoodLib.Pools.ListPool<DoorLog>.Shared.Return(item.Value);
            LogManager.DoorLogs.Clear();
            foreach (var item in LogManager.ElevatorLogs)
                NorthwoodLib.Pools.ListPool<ElevatorLog>.Shared.Return(item.Value);
            LogManager.ElevatorLogs.Clear();
            LogManager.FlashLog.Clear();
        }
    }
}
