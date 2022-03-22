// -----------------------------------------------------------------------
// <copyright file="LockElevatorCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using CommandSystem;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class LockElevatorCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "lockelevator";

        public override string Description => "Allows to lock elevators";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "lockelevator";

        public override string[] Aliases => new string[] { "lelevator" };

        public string[] Usage => new string[] { "elevator (049/Nuke/GateA/GateB/LczA/LczB)", "value (true/false)" };

        public string GetUsage()
        {
            return "LOCKELEVATOR [ELEVATOR] TRUE/FALSE \n Elevators:\n049\nNuke\nGateA\nGateB\nLczA\nLczB";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            if (args.Length < 2)
                return new string[] { this.GetUsage() };
            if (!bool.TryParse(args[1], out bool value))
                return new string[] { this.GetUsage() };
            success = true;
            var elevators = Exiled.API.Features.Lift.List;
            switch (args[0].ToLower())
            {
                case "049":
                    {
                        var elev = elevators.FirstOrDefault(e => e.Name == "SCP-049");
                        if (elev != null)
                            elev.IsLocked = value;
                        else
                            return new string[] { "Server Error, elevator not found" };
                        return new string[] { "Done" };
                    }

                case "nuke":
                    {
                        var elev = elevators.FirstOrDefault(e => e.Name == string.Empty);
                        if (elev != null)
                            elev.IsLocked = value;
                        else
                            return new string[] { "Server Error, elevator not found" };
                        return new string[] { "Done" };
                    }

                case "gatea":
                    {
                        var elev = elevators.FirstOrDefault(e => e.Name == "GateA");
                        if (elev != null)
                            elev.IsLocked = value;
                        else
                            return new string[] { "Server Error, elevator not found" };
                        return new string[] { "Done" };
                    }

                case "gateb":
                    {
                        var elev = elevators.FirstOrDefault(e => e.Name == "GateB");
                        if (elev != null)
                            elev.IsLocked = value;
                        else
                            return new string[] { "Server Error, elevator not found" };
                        return new string[] { "Done" };
                    }

                case "lcza":
                    {
                        var elev = elevators.FirstOrDefault(e => e.Name == "ElA");
                        if (elev != null)
                            elev.IsLocked = value;
                        else
                            return new string[] { "Server Error, elevator not found" };
                        elevators.First(e => e.Name == "ElA2").IsLocked = value;

                        return new string[] { "Done" };
                    }

                case "lczb":
                    {
                        var elev = elevators.FirstOrDefault(e => e.Name == "ElB");
                        if (elev != null)
                            elev.IsLocked = value;
                        else
                            return new string[] { "Server Error, elevator not found" };
                        elevators.First(e => e.Name == "ElB2").IsLocked = value;
                        if (elev != null)
                            elev.IsLocked = value;
                        else
                            return new string[] { "Server Error, elevator not found" };
                        return new string[] { "Done" };
                    }

                default:
                    {
                        success = false;
                        return new string[] { this.GetUsage() };
                    }
            }
        }
    }
}
