// -----------------------------------------------------------------------
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
        public override string Description => "CRT";

        public override string Command => "crt";

        public string GetUsage()
        {
            return "CRT";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = true;
            return new string[] { RoundStart.RoundLenght.ToString("hh\\:mm\\:ss\\.fff", CultureInfo.InvariantCulture) };
        }
    }
}
