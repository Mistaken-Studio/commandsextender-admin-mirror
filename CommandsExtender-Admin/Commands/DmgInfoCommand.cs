// -----------------------------------------------------------------------
// <copyright file="DmgInfoCommand.cs" company="Mistaken">
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
    internal class DmgInfoCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public static readonly HashSet<int> Active = new HashSet<int>();

        public string Permission => "dmginfo";

        public override string Description => "Damage Info, When enabled you will recive broadcasts saying who and how much damage done to you";

        public override string Command => "dmginfo";

        public override string[] Aliases => new string[] { };

        public string PluginName => PluginHandler.Instance.Name;

        public string[] Usage => new string[] { "value (true/false)" };

        public string GetUsage()
        {
            return "DmgInfo (true/false)";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            var player = sender.GetPlayer();
            bool value;
            if (args.Length == 0)
            {
                value = !Active.Contains(player.Id);
            }
            else if (!bool.TryParse(args[0], out value))
                return new string[] { this.GetUsage() };
            success = true;
            if (value)
            {
                if (!Active.Contains(player.Id)) Active.Add(player.Id);
                return new string[] { "Enabled" };
            }
            else
            {
                Active.Remove(player.Id);
                return new string[] { "Disabled" };
            }
        }
    }
}
