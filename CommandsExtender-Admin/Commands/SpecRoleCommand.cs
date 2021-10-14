// -----------------------------------------------------------------------
// <copyright file="SpecRoleCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Mistaken.API;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class SpecRoleCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public static RoleType SpecRole { get; set; } = RoleType.Spectator;

        public string Permission => "specrole";

        public override string Description => "Every spectator will be forceclased to specified class";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "specrole";

        public override string[] Aliases => new string[] { };

        public string[] Usage => new string[] { "class" };

        public string GetUsage()
        {
            return "SPECROLE [class]";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            if (args.Length == 0) return new string[] { this.GetUsage() };
            success = true;
            switch (args[0].ToLower())
            {
                case "spectator":
                case "rip":
                case "none":
                case "dead":
                    SpecRole = RoleType.Spectator;
                    break;
                case "cd":
                case "d":
                case "classd":
                    SpecRole = RoleType.ClassD;
                    break;
                case "14":
                case "tutorial":
                case "tut":
                    SpecRole = RoleType.Tutorial;
                    break;
                case "sci":
                case "scientist":
                    SpecRole = RoleType.Scientist;
                    break;
                case "ci":
                case "chaos":
                    SpecRole = RoleType.ChaosConscript;
                    break;
                case "guard":
                    SpecRole = RoleType.FacilityGuard;
                    break;
                case "ntfcommander":
                case "comm":
                case "commander":
                    SpecRole = RoleType.NtfCaptain;
                    break;
                case "ntfcadet":
                case "cadet":
                    SpecRole = RoleType.NtfPrivate;
                    break;
                case "ntflieutenant":
                case "lieutenant":
                    SpecRole = RoleType.NtfSergeant;
                    break;
                case "ntfsci":
                case "ntfscientist":
                    SpecRole = RoleType.NtfSpecialist;
                    break;
                case "173":
                    SpecRole = RoleType.Scp173;
                    break;
                case "096":
                    SpecRole = RoleType.Scp096;
                    break;
                case "106":
                    SpecRole = RoleType.Scp106;
                    break;
                case "049":
                    SpecRole = RoleType.Scp049;
                    break;
                case "079":
                    SpecRole = RoleType.Scp079;
                    break;
                case "049-2":
                case "zombie":
                case "0492":
                    SpecRole = RoleType.Scp0492;
                    break;
                case "939":
                case "93953":
                    SpecRole = RoleType.Scp93953;
                    break;
                case "93989":
                    SpecRole = RoleType.Scp93989;
                    break;
                default:
                    success = false;
                    return new string[] { this.GetUsage() };
            }

            foreach (var item in RealPlayers.Get(RoleType.Spectator))
                item.Role = SpecRole;
            return new string[] { "SpecRole is now " + SpecRole };
        }
    }
}
