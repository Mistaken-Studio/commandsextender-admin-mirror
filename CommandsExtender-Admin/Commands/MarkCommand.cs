// -----------------------------------------------------------------------
// <copyright file="MarkCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using CommandSystem;
using Exiled.API.Features;
using Mistaken.API;
using Mistaken.API.Commands;
using Mistaken.API.Extensions;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class MarkCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "mark";

        public override string Description => "When player is marked admin who marked him will see him as tutorial";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "mark";

        public string[] Usage => new string[] { "%player%" };

        public override string[] Execute(ICommandSender sender, string[] args, out bool s)
        {
            s = false;
            if (args.Length == 0)
                return new string[] { this.GetUsage() };
            var admin = sender.GetPlayer();
            var output = this.ForeachPlayer(args[0], out bool success, (player) =>
            {
                var set = player.GetSessionVariable(SessionVarType.ADMIN_MARK, new HashSet<Player>());
                if (set.Contains(admin))
                {
                    set.Remove(admin);
                    player.ChangeAppearance(admin, player.Role);
                    return new string[] { "Disabled" };
                }
                else
                {
                    if (admin.Role != RoleType.Tutorial)
                        return new string[] { "You have to be tutorial" };
                    if (!player.IsHuman)
                        return new string[] { "Has to be Human" };

                    player.ChangeAppearance(admin, RoleType.Tutorial);
                    set.Add(admin);
                    return new string[] { "Enabled" };
                }
            });
            if (!success)
                return new string[] { "Player not found", this.GetUsage() };
            s = true;
            return output;
        }

        public string GetUsage()
        {
            return "Mark (Id)";
        }
    }
}
