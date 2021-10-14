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
    internal class TpcCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "tpc";

        public override string Description => "Teleports admin to specified position (~ can be used as relative position)";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "tpc";

        public override string[] Aliases => new string[] { };

        public string[] Usage => new string[] { "posX", "posY", "posZ", "%player%" };

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            if (args.Length < 3)
                return new string[] { this.GetUsage() };

            if (float.TryParse(args[0].Replace("~", string.Empty), out float x))
            {
                if (float.TryParse(args[1].Replace("~", string.Empty), out float y))
                {
                    if (float.TryParse(args[2].Replace("~", string.Empty), out float z))
                    {
                        return this.ForeachPlayer(
                            args.Length > 3 ? args[3] : sender.GetPlayer().Id.ToString(),
                            out success,
                            player =>
                            {
                                if (args[0].Contains("~"))
                                    x += player.Position.x;
                                if (args[1].Contains("~"))
                                    y += player.Position.y;
                                if (args[2].Contains("~"))
                                    z += player.Position.z;
                                player.Position = new Vector3(x, y, z);
                                return new string[] { "Done" };
                            });
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
