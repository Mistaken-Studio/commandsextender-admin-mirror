// -----------------------------------------------------------------------
// <copyright file="WarpCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using CommandSystem;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Mistaken.API.Commands;
using UnityEngine;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class WarpCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "warp";

        public override string Description => "Warp";

        public override string Command => "warp";

        public string PluginName => PluginHandler.Instance.Name;

        public string[] Usage => new string[] { "warp name (or \"list\" to get warp list)", "%player%" };

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            if (args.Length == 0)
                return new string[] { this.GetUsage() };
            if (args[0] == "list")
                return new string[] { "Warp list:", "heli", "car", "jail", "jail2", "jail3", "jail4", "jail5", "gatea_up", "gatea_bottom", "gateb_up", "escape_up", "079", "checkpoint_lcz_a", "checkpoint_lcz_b", "checkpoint_ez", "shelter" };
            var res = this.ForeachPlayer(args.Length > 1 ? args[1] : Player.Get(sender).Id.ToString(), out bool s, (player) =>
            {
                return ExecuteWarp(player, args[0].ToLower());
            });
            if (s)
                return res;
            else
                return new string[] { "Player not found" };
        }

        public string GetUsage()
        {
            return "WARP [Type] (Id)";
        }

        internal static string[] ExecuteWarp(Player player, string warp)
        {
            switch (warp.ToLower())
            {
                case "heli":
                    {
                        player.Position = new Vector3(295, 980, -62);
                        return new string[] { "Done" };
                    }

                case "car":
                    {
                        player.Position = new Vector3(-96, 988, -60);
                        return new string[] { "Done" };
                    }

                case "jail":
                case "jail1":
                    {
                        player.Position = new Vector3(55, 1020, -43);
                        return new string[] { "Done" };
                    }

                case "jail2":
                    {
                        player.Position = new Vector3(61, 1020, -70);
                        return new string[] { "Done" };
                    }

                case "jail3":
                    {
                        player.Position = new Vector3(-23, 1020, -43);
                        return new string[] { "Done" };
                    }

                case "jail4":
                    {
                        player.Position = new Vector3(148, 1020, -19);
                        return new string[] { "Done" };
                    }

                case "jail5":
                    {
                        player.Position = new Vector3(222, 1027, -18);
                        return new string[] { "Done" };
                    }

                case "gatea_up":
                    {
                        player.Position = new Vector3(10, 1010, -7);
                        return new string[] { "Done" };
                    }

                case "gatea_bottom":
                    {
                        player.Position = new Vector3(5, 995, -10);
                        return new string[] { "Done" };
                    }

                case "079":
                    {
                        Vector3 pos = RoleType.Scp079.GetRandomSpawnProperties().Item1;
                        player.Position = new Vector3(pos.x, pos.y + 2, pos.z);
                        return new string[] { "Done" };
                    }

                case "gateb_up":
                    {
                        player.Position = new Vector3(147, 1010, -45);
                        return new string[] { "Done" };
                    }

                case "escape_up":
                    {
                        player.Position = new Vector3(210, 1008, -10);
                        return new string[] { "Done" };
                    }

                case "checkpoint_lcz_a":
                    {
                        player.Position = Room.List.FirstOrDefault(r => r.Type == Exiled.API.Enums.RoomType.LczChkpA).transform.position + Vector3.up;
                        return new string[] { "Done" };
                    }

                case "checkpoint_lcz_b":
                    {
                        player.Position = Room.List.FirstOrDefault(r => r.Type == Exiled.API.Enums.RoomType.LczChkpB).transform.position + Vector3.up;
                        return new string[] { "Done" };
                    }

                case "checkpoint_ez":
                    {
                        player.Position = Room.List.FirstOrDefault(r => r.Type == Exiled.API.Enums.RoomType.HczEzCheckpoint).transform.position + Vector3.up;
                        return new string[] { "Done" };
                    }

                case "shelter":
                    {
                        player.Position = Room.List.FirstOrDefault(r => r.Type == Exiled.API.Enums.RoomType.EzShelter).transform.position + Vector3.up;
                        return new string[] { "Done" };
                    }

                default:
                    {
                        return new string[] { "Unknown warp. Warp list:", "heli", "car", "jail", "jail2", "jail3", "jail4", "jail5", "gatea_up", "gatea_bottom", "gateb_up", "escape_up", "079", "checkpoint_lcz_a", "checkpoint_lcz_b", "checkpoint_ez", "shelter" };
                    }
            }
        }
    }
}
