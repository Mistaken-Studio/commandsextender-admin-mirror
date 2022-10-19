// -----------------------------------------------------------------------
// <copyright file="ShakeCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Exiled.API.Features;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal sealed class ShakeCommand : IBetterCommand, IPermissionLocked
    {
        public string Permission => "shake";

        public override string Description => "Shakes screen";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "shake";

        public override string[] Aliases => new string[] { };

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = true;

            foreach (var player in Player.List)
                Warhead.Controller.TargetRpcShake(player.Connection, false, true);

            return new[] { "Done" };
        }
    }
}
