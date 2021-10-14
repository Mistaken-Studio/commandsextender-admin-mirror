// -----------------------------------------------------------------------
// <copyright file="PlayerInfo.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Exiled.API.Features;

namespace Mistaken.CommandsExtender.Admin
{
    internal struct PlayerInfo
    {
        public string Name;
        public string UserId;
        public string IP;
        public int ID;
        public bool IMute;
        public bool Mute;

        public PlayerInfo(Player p)
        {
            this.ID = p.Id;
            this.UserId = p.UserId;
            this.IP = p.IPAddress;
            this.IMute = p.IsIntercomMuted;
            this.Mute = p.IsMuted;
            this.Name = p.Nickname;
        }
    }
}
