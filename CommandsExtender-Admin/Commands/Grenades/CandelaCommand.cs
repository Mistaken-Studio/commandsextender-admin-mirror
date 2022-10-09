// -----------------------------------------------------------------------
// <copyright file="CandelaCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using InventorySystem.Items.ThrowableProjectiles;
using MEC;
using Mirror;
using Mistaken.API;
using Mistaken.API.Commands;
using Mistaken.API.Extensions;
using UnityEngine;

namespace Mistaken.CommandsExtender.Admin.Commands.Grenades
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class CandelaCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "canadel";

        public override string Description => "Ying's toy";

        public override string Command => "canadel";

        public override string[] Aliases => new string[] { };

        public string PluginName => PluginHandler.Instance.Name;

        public string[] Usage => new[]
        {
            "%player%",
            "amount (default is 1)",
        };

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            if (args.Length == 0)
                return new[] { this.GetUsage() };
            else
            {
                var amount = 1;
                if (args.Length > 1)
                {
                    if (!int.TryParse(args[1], out amount))
                        return new[] { this.GetUsage() };
                }

                var pids = this.GetPlayers(args[0]).Select(p => p.Id).ToArray();
                if (pids.Length == 0)
                    return new[] { "Player not found", this.GetUsage() };
                this.DropUnder(pids, amount);
                success = true;
                return new[] { "Done" };
            }
        }

        public string GetUsage()
        {
            return "CANADEL [PLAYER ID] (AMOUNT)";
        }

        public void DropUnder(int[] pids, int times)
        {
            foreach (var item in pids)
                API.Diagnostics.Module.RunSafeCoroutine(this.Execute(RealPlayers.Get(item), times), "CanadelCommand.Execute");
        }

        public void Throw(int pid, int times)
        {
            API.Diagnostics.Module.RunSafeCoroutine(this.Execute(RealPlayers.Get(pid), times), "CanadelCommand.Execute");
        }

        public IEnumerator<float> Execute(Player player, int amount = 5, bool throwIt = false)
        {
            var nade = (ThrowableItem)Item.Create(ItemType.GrenadeFlash).Base;

            ThrownProjectile projectile;
            if (throwIt)
                projectile = nade.Throw(player.Position, player.CameraTransform.forward);
            else
                projectile = nade.Throw(player.Position, Vector3.down);
            yield return Timing.WaitForSeconds(2);
            var originaPosition = projectile.transform.position;
            for (var i = 0; i < amount; i++)
            {
                var dir = new Vector3
                {
                    x = Random.Range(-2f, 2f),
                    y = 2f,
                    z = Random.Range(-2f, 2f),
                };
                projectile = nade.Throw(originaPosition, dir, Random.Range(0.5f, 5f));
                yield return Timing.WaitForSeconds(0.1f);
            }

            NetworkServer.Destroy(projectile.gameObject);
        }
    }
}
