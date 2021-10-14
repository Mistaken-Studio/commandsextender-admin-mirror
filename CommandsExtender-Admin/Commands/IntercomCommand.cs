﻿// -----------------------------------------------------------------------
// <copyright file="IntercomCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Assets._Scripts.Dissonance;
using CommandSystem;
using Exiled.API.Features;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class IntercomCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "intercom";

        public override string Description => "Enables intercom speach for target players";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "intercom";

        public override string[] Aliases => new string[] { "int" };

        public string[] Usage => new string[] { "%player%", "value? (true/false)" };

        public override string[] Execute(ICommandSender sender, string[] args, out bool s)
        {
            s = false;
            if (args.Length < 1)
                return new string[] { this.GetUsage() };
            var output = this.ForeachPlayer(args[0], out bool success, (Player player) =>
            {
                if (player.Team == Team.SCP || player.Team == Team.RIP)
                    return new string[] { "You have to be human" };
                DissonanceUserSetup dus = player.ReferenceHub.GetComponent<DissonanceUserSetup>();
                dus.TargetUpdateForTeam(player.Team);
                dus.SpectatorChat = false;
                dus.SCPChat = false;
                dus.RadioAsHuman = false;
                dus.MimicAs939 = false;
                dus.IntercomAsHuman = false;
                dus.ResetToDefault();

                if (bool.TryParse(args[1], out bool result))
                {
                    dus.IntercomAsHuman = result;
                    return new string[] { "Done: " + result };
                }

                return new string[] { "Error ?" };
            });
            s = true;
            return output;
        }

        public string GetUsage()
        {
            return "Intercom (Id) true/false";
        }
    }
}