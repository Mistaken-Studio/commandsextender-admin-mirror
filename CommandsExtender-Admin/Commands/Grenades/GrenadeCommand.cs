// -----------------------------------------------------------------------
// <copyright file="GrenadeCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using CommandSystem;
using Mistaken.API;
using Mistaken.API.Commands;
using Mistaken.API.Extensions;

namespace Mistaken.CommandsExtender.Admin.Commands.Grenades
{
    /*[CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class GrenadeCommand : IBetterCommand, IPermissionLocked
    {
        public string Permission => "grenade";

        public override string Description => "Detonates";

        public override string Command => "grenade";

        public override string[] Aliases => new string[] { };

        public string PluginName => PluginHandler.Instance.Name;

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
                    {
                        return new string[] { this.GetUsage() };
                    }
                }

                var pids = this.GetPlayers(args[0]).Select(p => p.Id).ToArray();
                if (pids.Length == 0)
                    return new string[] { "Player not found", this.GetUsage() };
                this.DropUnder(pids, amount);
                success = true;
                return new string[] { "Done" };
            }
        }

        public string GetUsage()
        {
            return "GRENADE [PLAYER ID] (AMOUNT)";
        }

        public void DropUnder(int[] pids, int times)
        {
            foreach (var item in pids)
            {
                RealPlayers.Get(item).DropGrenadeUnder(0, times);
            }
        }
    }*/
}
