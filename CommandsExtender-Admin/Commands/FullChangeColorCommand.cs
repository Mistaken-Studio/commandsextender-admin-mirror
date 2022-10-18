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
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal sealed class FullChangeColorCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "fullchangecolor";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "fullchangecolor";

        public override string[] Aliases => new[] { "fullchangecolor", "fccolor", "fullccolor" };

        public override string Description => "Change colors everywere";

        public string[] Usage => new[]
        {
            "r",
            "g",
            "b",
        };

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            if (args.Length < 3)
                return new[] { this.GetUsage() };
            else
            {
                if (!float.TryParse(args[0], out var r))
                    return new[] { this.GetUsage() };
                if (!float.TryParse(args[1], out var g))
                    return new[] { this.GetUsage() };
                if (!float.TryParse(args[2], out var b))
                    return new[] { this.GetUsage() };

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

                return new[] { "Done" };
            }
        }

        public string GetUsage()
        {
            return "FULLCHANGECOLOR [R] [G] [B]";
        }
    }
}
