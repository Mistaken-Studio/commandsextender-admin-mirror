// -----------------------------------------------------------------------
// <copyright file="BigItemsCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Mistaken.API;
using Mistaken.API.Commands;
using Mistaken.API.Extensions;
using UnityEngine;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    /*[CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class BigItemsCommand : IBetterCommand, IPermissionLocked
    {
        public string Permission => "big_items";

        public override string Description => "Spawns Big Items";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "bigitem";

        public override string[] Aliases => new string[] { "bitem" };

        public string GetUsage()
        {
            return "BigItem [type] [rot x] [rot y] [rot z] [size x] [size y] [size z]";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool s)
        {
            s = false;
            if (args.Length < 7) return new string[] { this.GetUsage() };

            try
            {
                var info = new Inventory.SyncItemInfo
                {
                    id = (ItemType)int.Parse(args[0]),
                };

                if (float.TryParse(args[1], out float pos_x))
                {
                    if (float.TryParse(args[2], out float pos_y))
                    {
                        if (float.TryParse(args[3], out float pos_z))
                        {
                            if (float.TryParse(args[4], out float size_x))
                            {
                                if (float.TryParse(args[5], out float size_y))
                                {
                                    if (float.TryParse(args[6], out float size_z))
                                    {
                                        MapPlus.Spawn(info, sender.GetPlayer().Position, Quaternion.Euler(pos_x, pos_y, pos_z), new Vector3(size_x, size_y, size_z));
                                        s = true;
                                        return new string[] { "Done" };
                                    }
                                }
                            }
                        }
                    }
                }

                return new string[] { this.GetUsage() };
            }
            catch
            {
                return new string[] { this.GetUsage() };
            }
        }
    }*/
}
