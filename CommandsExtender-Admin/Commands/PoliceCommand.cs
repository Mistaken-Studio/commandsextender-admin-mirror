// -----------------------------------------------------------------------
// <copyright file="PoliceCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using CommandSystem;
using Exiled.API.Features;
using MEC;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class PoliceCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public static readonly Dictionary<string, float> PoliceMode = new Dictionary<string, float>();

        public string Permission => "police";

        public override string Description =>
            "Signals :)";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "police";

        public override string[] Aliases => new string[] { };

        public string[] Usage => new string[] { "value (true/false, default: toggle)", "speed in seconds(default: 1s)" };

        public string GetUsage()
        {
            return "Police (True/False)";
        }

        public override string[] Execute(ICommandSender sender, string[] args, out bool s)
        {
            var player = Player.Get(sender);
            if (args.Length == 0 || !bool.TryParse(args[0], out bool value))
                value = !PoliceMode.ContainsKey(player.UserId);
            if (args.Length < 2 || !float.TryParse(args[1], out float time))
                time = 1;
            bool start = value;
            if (value && PoliceMode.ContainsKey(player.UserId))
            {
                PoliceMode.Remove(player.UserId);
                start = false;
            }

            if (value)
                PoliceMode.Add(player.UserId, time);
            else
                PoliceMode.Remove(player.UserId);
            if (start)
                API.Diagnostics.Module.RunSafeCoroutine(this.Execute(player), "Police.Execute");
            s = true;
            return new string[] { "Done" };
        }

        private IEnumerator<float> Execute(Player player)
        {
            while (PoliceMode.TryGetValue(player.UserId, out float time))
            {
                if (player.RankColor != "red")
                    player.RankColor = "red";
                else
                    player.RankColor = "cyan";

                yield return Timing.WaitForSeconds(time);
            }
        }
    }
}
