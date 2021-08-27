// -----------------------------------------------------------------------
// <copyright file="AmmoCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Exiled.API.Enums;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    /*[CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class AmmoCommand : IBetterCommand, IPermissionLocked
    {
        public string Permission => "ammo";

        public override string Description => "Manipulates ammo";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "ammo";

        public override string[] Aliases => new string[] { };

        public string GetUsage()
        {
            return "AMMO [Id] add/remove/set/get [AMMO TYPE]/all [AMOUNT]\n types:\n 556\n762\n9";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool s)
        {
            s = false;
            if (args.Length < 4) return new string[] { this.GetUsage() };
            var output = this.ForeachPlayer(args[0], out bool success, (player) =>
            {
                if (!uint.TryParse(args[3], out uint amount)) return new string[] { this.GetUsage() };
                bool all = false;
                AmmoType ammotype = AmmoType.Nato556;
                switch (args[2].ToLower())
                {
                    case "5":
                    case "556":
                        {
                            ammotype = AmmoType.Nato556;
                            break;
                        }

                    case "7":
                    case "762":
                        {
                            ammotype = AmmoType.Nato762;
                            break;
                        }

                    case "9":
                        {
                            ammotype = AmmoType.Nato9;
                            break;
                        }

                    case "all":
                        {
                            all = true;
                            break;
                        }

                    default:
                        {
                            return new string[] { this.GetUsage() };
                        }
                }

                switch (args[1].ToLower())
                {
                    case "set":
                        {
                            player.Ammo[(int)AmmoType.Nato9] = player.Ammo[(int)AmmoType.Nato9];
                            if (all) player.Ammo[(int)ammotype] = amount;
                            else
                            {
                                player.Ammo[(int)AmmoType.Nato556] = amount;
                                player.Ammo[(int)AmmoType.Nato762] = amount;
                                player.Ammo[(int)AmmoType.Nato9] = amount;
                            }

                            return new string[] { "Done" };
                        }

                    case "add":
                        {
                            if (all) player.Ammo[(int)ammotype] += amount;
                            else
                            {
                                player.Ammo[(int)AmmoType.Nato556] += amount;
                                player.Ammo[(int)AmmoType.Nato762] += amount;
                                player.Ammo[(int)AmmoType.Nato9] += amount;
                            }

                            return new string[] { "Done" };
                        }

                    case "remove":
                        {
                            if (all) player.Ammo[(int)ammotype] -= amount;
                            else
                            {
                                player.Ammo[(int)AmmoType.Nato556] -= amount;
                                player.Ammo[(int)AmmoType.Nato762] -= amount;
                                player.Ammo[(int)AmmoType.Nato9] -= amount;
                            }

                            return new string[] { "Done" };
                        }

                    case "get":
                        {
                            if (all) return new string[] { $"{ammotype}: {player.Ammo[(int)ammotype]}" };
                            else
                            {
                                return new string[]
                                {
                                        $"{AmmoType.Nato556}: {player.Ammo[(int)AmmoType.Nato556]}",
                                        $"{AmmoType.Nato762}: {player.Ammo[(int)AmmoType.Nato762]}",
                                        $"{AmmoType.Nato9}: {player.Ammo[(int)AmmoType.Nato9]}",
                                };
                            }
                        }

                    default:
                        {
                            return new string[] { this.GetUsage() };
                        }
                }
            });
            if (!success)
                return new string[] { "Player not found", this.GetUsage() };
            s = true;
            return output;
        }
    }*/
}
