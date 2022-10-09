// -----------------------------------------------------------------------
// <copyright file="TpcCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Exiled.API.Features;
using Mistaken.API.Commands;
using UnityEngine;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class TpcCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "tpc";

        public override string Description => "Teleports admin to specified position (~ can be used as relative position)";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "tpc";

        public override string[] Aliases => new string[] { };

        public string[] Usage => new[] { "posX", "posY", "posZ", "%player%" };

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            if (args.Length < 3)
                return new[] { GetUsage() };

            if (!float.TryParse(args[0].Replace("~", string.Empty), out var x))
                return new[] { "Wrong Args" };

            if (!float.TryParse(args[1].Replace("~", string.Empty), out var y))
                return new[] { "Wrong Args" };

            if (!float.TryParse(args[2].Replace("~", string.Empty), out var z))
                return new[] { "Wrong Args" };

            return this.ForeachPlayer(
                args.Length > 3 ? args[3] : Player.Get(sender).Id.ToString(),
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
                    return new[] { "Done" };
                });
        }

        private static string GetUsage()
        {
            return "TPC [X] [Y] [Z]";
        }
    }
}
