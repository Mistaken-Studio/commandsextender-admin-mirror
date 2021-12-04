﻿// -----------------------------------------------------------------------
// <copyright file="C106Command.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Exiled.API.Features;
using Mistaken.API.Commands;
using UnityEngine;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class C106Command : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "c106";

        public override string Description => "Manipulate SCP 106 conntainment (force -> button press)";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "c106";

        public override string[] Aliases => new string[] { };

        public string[] Usage => new string[] { "action (true/false/force)" };

        public string GetUsage()
        {
            return "C106 True/False/Force";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            if (args.Length == 0) return new string[] { this.GetUsage() };
            if (args[0].ToLower() == "force")
            {
                var rh = ReferenceHub.GetHub(PlayerManager.localPlayer);
                foreach (var player in Player.List)
                {
                    if (player.Role == RoleType.Scp106)
                    {
                        player.ReferenceHub.scp106PlayerScript.Contain(new Footprinting.Footprint(rh));
                    }
                }

                rh.playerInteract.RpcContain106(rh.gameObject);
                OneOhSixContainer.used = true;
                success = true;
                return new string[] { "Done" };
            }
            else if (bool.TryParse(args[0], out bool value))
            {
                GameObject.FindObjectOfType<LureSubjectContainer>().SetState(value, value);
                success = true;
                return new string[] { "Changed" };
            }
            else return new string[] { this.GetUsage() };
        }
    }
}
