// -----------------------------------------------------------------------
// <copyright file="DoorLog.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Exiled.API.Features;

namespace Mistaken.CommandsExtender.Admin.Logs
{
    internal struct DoorLog
    {
        public Player Player;
        public DateTime Time;
        public bool Open;

        public DoorLog(Exiled.Events.EventArgs.InteractingDoorEventArgs ev)
        {
            this.Player = ev.Player;
            this.Time = DateTime.Now;
            this.Open = !ev.Door.IsOpen;
        }
    }
}
