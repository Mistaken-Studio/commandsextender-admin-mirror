﻿// -----------------------------------------------------------------------
// <copyright file="CustomRoundTimeCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using CommandSystem;
using GameCore;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class CustomRoundTimeCommand : IBetterCommand
    {
        public override string Description => "Round Time but formated";

        public override string Command => "crt";

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = true;
            return new string[] { RoundStart.RoundLength.ToString("hh\\:mm\\:ss\\.fff", CultureInfo.InvariantCulture) };
        }
    }
}
