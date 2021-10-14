// -----------------------------------------------------------------------
// <copyright file="SCP914Log.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Exiled.API.Features;

namespace Mistaken.CommandsExtender.Admin
{
    internal struct SCP914Log
    {
        public string Name;
        public string UserId;
        public int ID;
        public SCP914Action Action;
        public DateTime Time;

        public SCP914Log(Player p, SCP914Action action)
        {
            this.ID = p.Id;
            this.UserId = p.UserId;
            this.Name = p.Nickname;
            this.Action = action;
            this.Time = DateTime.Now;
        }
    }
}
