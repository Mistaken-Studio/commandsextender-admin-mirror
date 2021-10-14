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
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class SetSizeCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "setsize";

        public override string Description => "Sets targets size";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "setsize";

        public override string[] Aliases => new string[] { "ssize" };

        public string[] Usage => new string[] { "%player%", "sizeX", "sizeY", "sizeZ" };

        public override string[] Execute(ICommandSender sender, string[] args, out bool s)
        {
            s = false;
            if (args.Length < 4)
                return new string[] { this.GetUsage() };
            else
            {
                if (float.TryParse(args[1], out float x))
                {
                    if (float.TryParse(args[2], out float y))
                    {
                        if (float.TryParse(args[3], out float z))
                        {
                            var output = this.ForeachPlayer(
                                args[0],
                                out bool success,
                                (player) =>
                                {
                                    player.Scale = new Vector3(x, y, z);
                                    return new string[] { "Done" };
                                },
                                true);
                            if (!success)
                                return new string[] { "Player not found", this.GetUsage() };
                            s = true;
                            return output;
                        }
                        else
                            return new string[] { this.GetUsage() };
                    }
                    else
                        return new string[] { this.GetUsage() };
                }
                else
                    return new string[] { this.GetUsage() };
            }
        }

        public string GetUsage()
        {
            return "SETSIZE [PLAYER ID] [X] [Y] [Z]";
        }
    }
}
