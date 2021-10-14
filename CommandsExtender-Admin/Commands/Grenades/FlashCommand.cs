// -----------------------------------------------------------------------
// <copyright file="FlashCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using CommandSystem;
using Exiled.API.Features.Items;
using Mistaken.API;
using Mistaken.API.Commands;
using Mistaken.API.Extensions;
using UnityEngine;

namespace Mistaken.CommandsExtender.Admin.Commands.Grenades
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class FlashCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "flash";

        public override string Description =>
        "Flashes";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "flash";

        public override string[] Aliases => new string[] { };

        public string[] Usage => new string[]
        {
            "%player%",
            "amount (default is 1)",
        };

        public string GetUsage()
        {
            return "FLASH [PLAYER ID] (AMOUNT)";
        }

        public void DropUnder(int[] pids, int times)
        {
            var nade = new Throwable(ItemType.GrenadeFlash);
            foreach (var item in pids)
            {
                var player = RealPlayers.Get(item);
                for (int i = 0; i < times; i++)
                    nade.Throw(player.Position, Vector3.down);
            }
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            if (args.Length == 0)
                return new string[] { this.GetUsage() };
            else
            {
                int amount = 1;
                if (args.Length > 1)
                {
                    if (!int.TryParse(args[1], out amount))
                        return new string[] { this.GetUsage() };
                }

                var pids = this.GetPlayers(args[0]).Select(p => p.Id).ToArray();
                if (pids.Length == 0)
                    return new string[] { "Player not found", this.GetUsage() };
                this.DropUnder(pids, amount);
                success = true;
                return new string[] { "Done" };
            }
        }
    }
}
