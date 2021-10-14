// -----------------------------------------------------------------------
// <copyright file="LogManager.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;

namespace Mistaken.CommandsExtender.Admin
{
    internal static class LogManager
    {
        public static Dictionary<ElevatorType, List<ElevatorLog>> ElevatorLogs { get; } = new Dictionary<ElevatorType, List<ElevatorLog>>();

        public static Dictionary<Interactables.Interobjects.DoorUtils.DoorVariant, List<DoorLog>> DoorLogs { get; } = new Dictionary<Interactables.Interobjects.DoorUtils.DoorVariant, List<DoorLog>>();

        public static Dictionary<int, List<PlayerInfo>> PlayerLogs { get; } = new Dictionary<int, List<PlayerInfo>>();

        public static Dictionary<int, List<SCP914Log>> SCP914Logs { get; } = new Dictionary<int, List<SCP914Log>>();

        public static Dictionary<int, DateTime> RoundStartTime { get; } = new Dictionary<int, DateTime>();

        public static Dictionary<int, Player> FlashLog { get; } = new Dictionary<int, Player>();
    }
}
