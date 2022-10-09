// -----------------------------------------------------------------------
// <copyright file="PlayerInfo.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Exiled.API.Features;

namespace Mistaken.CommandsExtender.Admin.Logs
{
    internal struct PlayerInfo
    {
        public readonly string Name;
        public readonly string UserId;
        public readonly string Ip;
        public readonly int Id;
        public readonly bool IntercomMute;
        public readonly bool Mute;

        public PlayerInfo(Player p)
        {
            this.Id = p.Id;
            this.UserId = p.UserId;
            this.Ip = p.IPAddress;
            this.IntercomMute = p.IsIntercomMuted;
            this.Mute = p.IsMuted;
            this.Name = p.Nickname;
        }
    }
}
