// -----------------------------------------------------------------------
// <copyright file="SpawnDummyCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using CommandSystem;
using Exiled.API.Features;
using Mirror;
using Mistaken.API;
using Mistaken.API.Commands;
using Mistaken.API.Extensions;
using UnityEngine;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class SpawnDummyCommand : IBetterCommand, IPermissionLocked
    {
        public string Permission => "spawndummy";

        public override string Description => "Spawns Dummy";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "spawndummy";

        public override string[] Aliases => new string[] { "sdummy" };

        public string GetUsage()
        {
            return "SpawnDummy RAW/TARGET/DELETE (argumets) (autoDestroyInterval (seconds))";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool s)
        {
            s = false;
            if (args.Length == 0)
                return new string[] { this.GetUsage() };

            GameObject dummy = null;

            var dummyId = this.nextId;
            this.nextId++;

            switch (args[0].ToUpper())
            {
                case "RAW":
                    {
                        RoleType role;
                        string name;
                        bool success = false;

                        if (float.TryParse(args[1], out float pos_x))
                        {
                            if (float.TryParse(args[2], out float pos_y))
                            {
                                if (float.TryParse(args[3], out float pos_z))
                                {
                                    if (float.TryParse(args[4], out float rot_x))
                                    {
                                        if (float.TryParse(args[5], out float rot_y))
                                        {
                                            if (float.TryParse(args[6], out float rot_z))
                                            {
                                                if (float.TryParse(args[7], out float size_x))
                                                {
                                                    if (float.TryParse(args[8], out float size_y))
                                                    {
                                                        if (float.TryParse(args[9], out float size_z))
                                                        {
                                                            if (int.TryParse(args[10], out int roleId))
                                                            {
                                                                role = (RoleType)roleId;
                                                                name = args[11];
                                                                dummy = MapPlus.SpawnDummy(role, new Vector3(pos_x, pos_y, pos_z), Quaternion.Euler(rot_x, rot_y, rot_z), new Vector3(size_x, size_y, size_z), name);
                                                                success = true;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (!success) return new string[] { "SpawnDummy RAW [pos_x] [pos_y] [pos_z] [rot_x] [rot_y] [rot_z] [size_x] [size_y] [size_z] [role] [name]" };
                        break;
                    }

                case "TARGET":
                    {
                        this.ForeachPlayer(args[1], out bool success, (Player player) =>
                        {
                            string name = player.Nickname;
                            if (args.Length > 2 && args[2] != "default")
                                name = args[2];
                            dummy = MapPlus.SpawnDummy(player.Role, player.Position, Quaternion.Euler(player.Rotation), Vector3.one, name);
                            return new string[] { "Done" };
                        });
                        if (!success)
                        {
                            var player = sender.GetPlayer();
                            string name = player.Nickname;
                            if (args.Length > 1 && args[1] != "default")
                                name = args[1];
                            dummy = MapPlus.SpawnDummy(player.Role, player.Position, Quaternion.Euler(player.Rotation), Vector3.one, name);
                        }

                        break;
                    }

                case "DELETE":
                    {
                        if (args.Length > 1 && uint.TryParse(args[1], out uint id) && this.dummys.TryGetValue(id, out GameObject obj))
                        {
                            NetworkServer.Destroy(obj);
                            GameObject.Destroy(obj);
                            this.dummys.Remove(id);
                        }

                        break;
                    }

                default:
                    return new string[] { this.GetUsage() };
            }

            if (dummy == null) return new string[] { "Dummy failed to spawn" };
            this.dummys.Add(dummyId, dummy);
            if (args.Length > 2 && float.TryParse(args[args.Length - 1], out float time))
            {
                API.Diagnostics.Module.CallSafeDelayed(
                    time,
                    () =>
                    {
                        NetworkServer.Destroy(dummy);
                        GameObject.Destroy(dummy);
                        this.dummys.Remove(dummyId);
                    },
                    "SpawnDummy");
                return new string[] { $"Dummy ({dummyId}) spawnned and will be removed in {time} seconds" };
            }
            else return new string[] { $"Dummy ({dummyId}) spawnned" };
        }

        private readonly Dictionary<uint, GameObject> dummys = new Dictionary<uint, GameObject>();
        private uint nextId = 0;
    }
}
