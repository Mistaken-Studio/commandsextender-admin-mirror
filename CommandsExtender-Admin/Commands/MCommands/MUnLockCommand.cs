﻿// -----------------------------------------------------------------------
// <copyright file="MUnLockCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using CommandSystem;
using Mistaken.API.Commands;
using Mistaken.API.Extensions;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class MUnlockCommand : IBetterCommand, IPermissionLocked
    {
        public static readonly HashSet<int> Active = new HashSet<int>();

        public string Permission => "m.unlock";

        public override string Description => "MUnlock";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "munlock";

        public override string[] Aliases => new string[] { };

        public string GetUsage()
        {
            return "MUnlock";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = true;
            var player = sender.GetPlayer();
            if (!Active.Contains(player.Id))
                Active.Add(player.Id);
            else
                Active.Remove(player.Id);
            if (Active.Contains(player.Id))
                return new string[] { "Activated" };
            else
                return new string[] { "Deactivated" };
        }
    }
}