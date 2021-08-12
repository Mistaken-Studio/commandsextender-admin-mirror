// -----------------------------------------------------------------------
// <copyright file="WorkstationCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Exiled.API.Features;
using Mirror;
using Mistaken.API.Commands;
using Mistaken.API.Extensions;
using UnityEngine;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class WorkstationCommand : IBetterCommand, IPermissionLocked
    {
        public string Permission => "workstation";

        public override string Description => "WORKSTATION";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "workstation";

        public override string[] Aliases => new string[] { "ws" };

        public string GetUsage()
        {
            return "WORKSTATION SPAWN/REMOVE";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            var player = sender.GetPlayer();
            if (args.Length > 0)
            {
                switch (args[0].ToLower())
                {
                    case "spawn":
                        {
                            if (this.prefab == null)
                            {
                                foreach (var item in NetworkManager.singleton.spawnPrefabs)
                                {
                                    var ws = item.GetComponent<WorkStation>();
                                    if (ws)
                                    {
                                        Log.Debug(item.name);
                                        this.prefab = ws;
                                    }
                                }
                            }

                            var rh = player.ReferenceHub;
                            Transform cam = rh.PlayerCameraReference;

                            Vector3 pos = cam.position + (cam.forward * 5f);
                            pos += Vector3.down * 1f;
                            this.SpawnWorkStation(pos, new Vector3(0, cam.eulerAngles.y * -1f, 0), Vector3.one);
                            success = true;
                            return new string[] { "Spawned at " + pos };
                        }

                    case "remove":
                        {
                            var rh = player.ReferenceHub;
                            Transform cam = rh.PlayerCameraReference;
                            if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, 10f))
                            {
                                var comp = hit.collider.GetComponent<WorkStation>();
                                if (comp == null)
                                {
                                    var tmp = hit.collider.gameObject;
                                    while (tmp.transform.parent != null)
                                    {
                                        tmp = tmp.transform.parent.gameObject;
                                        comp = tmp.GetComponent<WorkStation>();
                                        if (comp != null) break;
                                        else Log.Debug("Not found on " + tmp.name);
                                    }
                                }

                                if (comp == null) return new string[] { "Not found workstation" };
                                else
                                {
                                    NetworkServer.Destroy(comp.gameObject);
                                    success = true;
                                    return new string[] { "Removed" };
                                }
                            }
                            else return new string[] { "Not found" };
                        }

                    default:
                        return new string[] { this.GetUsage() };
                }
            }
            else
            {
                return new string[] { this.GetUsage() };
            }
        }

        private WorkStation prefab;

        private void SpawnWorkStation(Vector3 pos, Vector3 rot, Vector3 size)
        {
            var spawned = GameObject.Instantiate(this.prefab.gameObject, pos, Quaternion.Euler(rot));
            spawned.transform.localScale = size;
            NetworkServer.Spawn(spawned);
        }
    }
}
