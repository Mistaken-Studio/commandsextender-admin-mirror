// -----------------------------------------------------------------------
// <copyright file="TutorialMeCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using CommandSystem;
using Mistaken.API;
using Mistaken.API.Commands;
using Mistaken.API.Extensions;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class TutorialMeCommand : IBetterCommand, IPermissionLocked
    {
        public string Permission => "tutorial";

        public override string Description => "Tutorial";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "tutorial";

        public override string[] Aliases => new string[] { "tut" };

        public string GetUsage()
        {
            return "Tut [args]";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            var player = sender.GetPlayer();
            if (player.Role != RoleType.Tutorial)
            {
                player.IsOverwatchEnabled = false;
                player.Role = RoleType.Tutorial;
                player.IsGodModeEnabled = true;
                player.IsBypassModeEnabled = true;

                foreach (var item in args.Select(i => i.ToLower()))
                {
                    switch (item)
                    {
                        case "-n":
                        case "-noclip":
                            {
                                if (player.CheckPermission("Global.Noclip"))
                                    player.NoClipEnabled = true;
                                break;
                            }

                        case "-v":
                        case "--vanish":
                            {
                                if (player.CheckPermission(PluginHandler.Instance.Name + ".vanish"))
                                    VanishHandler.SetGhost(player, true);
                                break;
                            }
                    }
                }
            }
            else
            {
                player.Role = RoleType.Spectator;
                VanishHandler.SetGhost(player, false);
                player.IsGodModeEnabled = false;
                player.IsBypassModeEnabled = false;
                player.NoClipEnabled = false;
            }

            success = true;
            return new string[] { "Done" };
        }
    }
}
