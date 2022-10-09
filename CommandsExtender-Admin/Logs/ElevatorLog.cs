// -----------------------------------------------------------------------
// <copyright file="ElevatorLog.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Exiled.API.Features;

namespace Mistaken.CommandsExtender.Admin.Logs
{
    internal struct ElevatorLog
    {
        public Player Player;
        public DateTime Time;
        public Lift.Status Status;

        public ElevatorLog(Exiled.Events.EventArgs.InteractingElevatorEventArgs ev)
        {
            this.Player = ev.Player;
            this.Time = DateTime.Now;
            this.Status = ev.Lift.Status;
        }
    }
}
