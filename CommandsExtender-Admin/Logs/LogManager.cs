// -----------------------------------------------------------------------
// <copyright file="LogManager.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;

namespace Mistaken.CommandsExtender.Admin.Logs
{
    internal static class LogManager
    {
        public static Dictionary<ElevatorType, List<ElevatorLog>> ElevatorLogs { get; } = new();

        public static Dictionary<Door, List<DoorLog>> DoorLogs { get; } = new();

        public static Dictionary<int, List<PlayerInfo>> PlayerLogs { get; } = new();

        public static Dictionary<int, DateTime> RoundStartTime { get; } = new();

        public static Dictionary<int, Player> FlashLog { get; } = new();
    }
}
