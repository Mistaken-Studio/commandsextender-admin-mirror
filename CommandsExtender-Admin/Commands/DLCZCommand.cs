// -----------------------------------------------------------------------
// <copyright file="DLCZCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using CommandSystem;
using LightContainmentZoneDecontamination;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal sealed class DlczCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "dlcz";

        public override string Description => "SetTime -> Setts progress time, can be used to delay or speed up decontamination | SetStatus -> can disable decontamination";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "dlcz";

        public override string[] Aliases => new string[] { };

        public string[] Usage => new[] { "settime/setstatus", "value?" };

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            if (args.Length == 0)
                return new[] { GetUsage() };

            var dlcz = DecontaminationController.Singleton;
            if (dlcz == null)
                return new[] { "DecontaminationLCZ not found" };

            switch (args[0].ToLower())
            {
                case "st":
                case "settime":
                    {
                        if (args.Length == 1)
                            return new[] { GetUsage() };
                        if (float.TryParse(args[1], out var time))
                        {
                            var last = dlcz.DecontaminationPhases.First(i => i.Function == DecontaminationController.DecontaminationPhase.PhaseFunction.Final).TimeTrigger - DecontaminationController.GetServerTime;
                            var toSet = Mirror.NetworkTime.time -
                                        DecontaminationController.Singleton.DecontaminationPhases.First(i =>
                                            i.Function == DecontaminationController.DecontaminationPhase.PhaseFunction.Final).TimeTrigger +
                                        time;
                            if (toSet <= 0)
                            {
                                success = false;
                                return new[] { $"NetworkRoundStartTime can't be negative | it whould be {toSet}" };
                            }

                            dlcz.NetworkRoundStartTime = toSet;
                            success = true;
                            return new[] { $"Time set to {time}, it was {last}" };
                        }
                        else
                            return new[] { GetUsage() };
                    }

                case "ss":
                case "setstatus":
                    {
                        if (args.Length == 1)
                            return new[] { "LCZ Decontamination Status: " + dlcz.enabled };
                        if (!bool.TryParse(args[1], out var value))
                            return new[] { GetUsage() };
                        value = !value;
                        dlcz.disableDecontamination = value;
                        success = true;
                        if (value)
                            return new[] { $"Resumed Decontamination" };
                        else
                            return new[] { $"Paused Decontamination" };
                    }

                default:
                    return new[] { GetUsage() };
            }
        }

        private static string GetUsage()
        {
            return "dlcz get/settime/setstatus (value)";
        }
    }
}
