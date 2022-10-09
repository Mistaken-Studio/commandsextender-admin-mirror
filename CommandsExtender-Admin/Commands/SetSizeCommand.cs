// -----------------------------------------------------------------------
// <copyright file="SetSizeCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Mistaken.API.Commands;
using UnityEngine;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class SetSizeCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "setsize";

        public override string Description => "Sets targets size";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "setsize";

        public override string[] Aliases => new[] { "ssize" };

        public string[] Usage => new[] { "%player%", "sizeX", "sizeY", "sizeZ" };

        public override string[] Execute(ICommandSender sender, string[] args, out bool s)
        {
            s = false;
            if (args.Length < 4)
                return new[] { this.GetUsage() };
            else
            {
                if (float.TryParse(args[1], out var x))
                {
                    if (float.TryParse(args[2], out var y))
                    {
                        if (float.TryParse(args[3], out var z))
                        {
                            var output = this.ForeachPlayer(
                                args[0],
                                out var success,
                                (player) =>
                                {
                                    player.Scale = new Vector3(x, y, z);
                                    return new[] { "Done" };
                                },
                                true);
                            if (!success)
                                return new[] { "Player not found", this.GetUsage() };
                            s = true;
                            return output;
                        }
                        else
                            return new[] { this.GetUsage() };
                    }
                    else
                        return new[] { this.GetUsage() };
                }
                else
                    return new[] { this.GetUsage() };
            }
        }

        public string GetUsage()
        {
            return "SETSIZE [PLAYER ID] [X] [Y] [Z]";
        }
    }
}
