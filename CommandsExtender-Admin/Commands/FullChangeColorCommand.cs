// -----------------------------------------------------------------------
// <copyright file="FullChangeColorCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Mistaken.API.Commands;
using UnityEngine;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class FullChangeColorCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "fullchangecolor";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "fullchangecolor";

        public override string[] Aliases => new string[] { "fullchangecolor", "fccolor", "fullccolor" };

        public override string Description => "Change colors everywere";

        public string[] Usage => new string[]
        {
            "r",
            "g",
            "b",
        };

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            if (args.Length < 3)
                return new string[] { this.GetUsage() };
            else
            {
                if (!float.TryParse(args[0], out float r))
                    return new string[] { this.GetUsage() };
                if (!float.TryParse(args[1], out float g))
                    return new string[] { this.GetUsage() };
                if (!float.TryParse(args[2], out float b))
                    return new string[] { this.GetUsage() };

                var color = new Color(r / 255f, g / 255f, b / 255f);

                foreach (var room in Exiled.API.Features.Room.List)
                {
                    var controller = room.GetComponentInChildren<FlickerableLightController>();
                    if (controller == null)
                        continue;
                    if (color == Color.white)
                    {
                        controller.WarheadLightColor = Color.red;
                        controller.WarheadLightOverride = false;
                    }
                    else
                    {
                        controller.WarheadLightColor = color;
                        controller.WarheadLightOverride = true;
                    }
                }

                return new string[] { "Done" };
            }
        }

        public string GetUsage()
        {
            return "FULLCHANGECOLOR [R] [G] [B]";
        }
    }
}
