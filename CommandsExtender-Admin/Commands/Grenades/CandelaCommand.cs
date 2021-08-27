﻿// -----------------------------------------------------------------------
// <copyright file="CandelaCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using MEC;
using Mirror;
using Mistaken.API;
using Mistaken.API.Commands;
using UnityEngine;

namespace Mistaken.CommandsExtender.Admin.Commands.Grenades
{
    /*[CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class CandelaCommand : IBetterCommand, IPermissionLocked
    {
        public string Permission => "canadel";

        public override string Description => "Ying's toy";

        public override string Command => "canadel";

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

        public string GetUsage()
        {
            return "CANADEL [PLAYER ID/'ALL'] (AMOUNT)";
        }

        public void DropUnder(int[] pids, int times)
        {
            foreach (var item in pids)
                API.Diagnostics.Module.RunSafeCoroutine(this.Execute(RealPlayers.Get(item).GrenadeManager, times), "CanadelCommand.Execute");
        }

        public void Throw(int pid, int times)
        {
            API.Diagnostics.Module.RunSafeCoroutine(this.Execute(RealPlayers.Get(pid).GrenadeManager, times), "CanadelCommand.Execute");
        }

        public IEnumerator<float> Execute(GrenadeManager grenadeManager, int amount = 5, bool throwIt = false)
        {
            GrenadeSettings settings = grenadeManager.availableGrenades[1];

            Grenade originalComponent = UnityEngine.Object.Instantiate<GameObject>(settings.grenadeInstance).GetComponent<Grenade>();
            if (throwIt)
                originalComponent.InitData(grenadeManager, Vector3.zero, Player.Get(grenadeManager.gameObject).ReferenceHub.PlayerCameraReference.forward);
            else
                originalComponent.InitData(grenadeManager, Vector3.zero, Vector3.down);
            NetworkServer.Spawn(originalComponent.gameObject);
            yield return Timing.WaitForSeconds(2);
            Vector3 originaPosition = originalComponent.transform.position;
            for (int i = 0; i < amount; i++)
            {
                Grenade component = UnityEngine.Object.Instantiate<GameObject>(settings.grenadeInstance).GetComponent<Grenade>();
                Vector3 dir = new Vector3
                {
                    x = UnityEngine.Random.Range(-2f, 2f),
                    y = 2f,
                    z = UnityEngine.Random.Range(-2f, 2f),
                };
                grenadeManager.transform.position = originaPosition;
                component.InitData(grenadeManager, Vector3.one, dir, UnityEngine.Random.Range(0.5f, 5f));
                NetworkServer.Spawn(component.gameObject);
                yield return Timing.WaitForSeconds(0.1f);
            }

            NetworkServer.Destroy(originalComponent.gameObject);
            GameObject.Destroy(originalComponent.gameObject);
        }
    }*/
}
