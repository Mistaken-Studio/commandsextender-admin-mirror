﻿// -----------------------------------------------------------------------
// <copyright file="TutorialMeCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal sealed class TutorialMeCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "tutorial";

        public override string Description => "Tutorial";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "tutorial";

        public override string[] Aliases => new[] { "tut" };

        public string[] Usage => new[] { "--noclip (automaticly enables noclip)", "--vanish (enables vanish with level 1)", "--vanish-level [level] (enables vanish with specified level)" };

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            var player = Player.Get(sender);
            if (player.Role.Type != RoleType.Tutorial)
            {
                player.IsOverwatchEnabled = false;
                player.Role.Type = RoleType.Tutorial;
                player.IsGodModeEnabled = true;
                player.IsBypassModeEnabled = true;

                var nextVanishLevel = false;

                foreach (var item in args.Select(i => i.ToLower()))
                {
                    if (nextVanishLevel)
                    {
                        if (byte.TryParse(item, out var lvl))
                            API.Handlers.VanishHandler.SetGhost(player, true, lvl);
                        nextVanishLevel = false;
                    }

                    switch (item)
                    {
                        case "-n":
                        case "--noclip":
                            {
                                if (Permissions.CheckPermission(player, "Global.Noclip"))
                                    player.NoClipEnabled = true;
                                break;
                            }

                        case "-v":
                        case "--vanish":
                            {
                                if (Permissions.CheckPermission(player, PluginHandler.Instance.Name + ".vanish"))
                                    API.Handlers.VanishHandler.SetGhost(player, true);
                                break;
                            }

                        case "-v-l":
                        case "--vanish-level":
                            {
                                if (Permissions.CheckPermission(player, PluginHandler.Instance.Name + ".vanish"))
                                    nextVanishLevel = true;
                                break;
                            }
                    }
                }
            }
            else
            {
                player.IsGodModeEnabled = false;
                player.IsBypassModeEnabled = false;
                player.NoClipEnabled = false;
                player.Role.Type = RoleType.Spectator;
                API.Handlers.VanishHandler.SetGhost(player, false);
            }

            success = true;
            return new[] { "Done" };
        }
    }
}
