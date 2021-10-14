// -----------------------------------------------------------------------
// <copyright file="ClearMapCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Mirror;
using Mistaken.API.Commands;
using UnityEngine;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class ClearMapCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "clearmap";

        public override string Description => "Removes all pickups from map";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "clearmap";

        public override string[] Aliases => new string[] { "cmap" };

        public string[] Usage => new string[] { "clear ragdolls (default false)" };

        public string GetUsage()
        {
            return "CLEARMAP [RAGDOLL = FALSE]";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool s)
        {
            bool ragdoll = false;
            if (args.Length > 0)
                bool.TryParse(args[0], out ragdoll);
            foreach (var item in Map.Pickups.ToArray())
                item.Destroy();
            if (ragdoll)
            {
                foreach (var item in GameObject.FindObjectsOfType<Ragdoll>().ToArray())
                    NetworkServer.Destroy(item.gameObject);
            }

            s = true;
            return new string[] { "Done" };
        }
    }
}
