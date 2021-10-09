// -----------------------------------------------------------------------
// <copyright file="TpcCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Mistaken.API.Commands;
using Mistaken.API.Extensions;
using UnityEngine;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class TpcCommand : IBetterCommand, IPermissionLocked
    {
        public string Permission => "tpc";

        public override string Description => "TPC";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "tpc";

        public override string[] Aliases => new string[] { };

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            if (args.Length < 3) return new string[] { this.GetUsage() };

            var player = sender.GetPlayer();
            if (float.TryParse(args[0].Replace("~", string.Empty), out float x))
            {
                if (float.TryParse(args[1].Replace("~", string.Empty), out float y))
                {
                    if (float.TryParse(args[2].Replace("~", string.Empty), out float z))
                    {
                        if (args[0].Contains("~"))
                            x += player.Position.x;
                        if (args[1].Contains("~"))
                            y += player.Position.y;
                        if (args[2].Contains("~"))
                            z += player.Position.z;
                        player.Position = new Vector3(x, y, z);
                        success = true;
                        return new string[] { "Done" };
                    }
                }
            }

            return new string[] { "Wrong Args" };
        }

        public string GetUsage()
        {
            return "TPC [X] [Y] [Z]";
        }
    }
}
