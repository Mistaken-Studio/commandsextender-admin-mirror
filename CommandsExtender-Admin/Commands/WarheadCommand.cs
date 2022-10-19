// -----------------------------------------------------------------------
// <copyright file="WarheadCommand.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using Mistaken.API.Commands;

namespace Mistaken.CommandsExtender.Admin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal sealed class WarheadCommand : IBetterCommand, IPermissionLocked, IUsageProvider
    {
        public string Permission => "basic";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "warhead_controll";

        public override string[] Aliases => new[] { "wc" };

        public override string Description => "Controll Alpha Warhead";

        public string[] Usage => new[] { "start/stop/on/off/open/close/lockstart/lockstop/lockbutton/locklever/getlast/stats" };

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            if (args.Length == 0)
                return new[] { this.GetUsage() };
            var player = Player.Get(sender);

            switch (args[0])
            {
                case "start":
                    {
                        if (!Permissions.CheckPermission(player, $"{this.PluginName}.lock"))
                            return new[] { "No Permissions." };
                        Warhead.Start();
                        API.Handlers.BetterWarheadHandler.Warhead.CountingDown = true;
                        success = true;
                        return new[] { "Alpha Warhead engaged" };
                    }

                case "stop":
                    {
                        Warhead.Stop();
                        API.Handlers.BetterWarheadHandler.Warhead.CountingDown = false;
                        success = true;
                        return new[] { "Alpha Warhead cancled" };
                    }

                case "on":
                    {
                        if (!Permissions.CheckPermission(player, $"{this.PluginName}.lock"))
                            return new[] { "No Permissions." };
                        Warhead.LeverStatus = true;
                        API.Handlers.BetterWarheadHandler.Warhead.Enabled = true;
                        success = true;
                        return new[] { "Alpha Warhead turned on" };
                    }

                case "off":
                    {
                        if (!Permissions.CheckPermission(player, $"{this.PluginName}.lock"))
                            return new[] { "No Permissions." };
                        Warhead.LeverStatus = false;
                        API.Handlers.BetterWarheadHandler.Warhead.Enabled = false;
                        success = true;
                        return new[] { "Alpha Warhead turned off" };
                    }

                case "open":
                    {
                        if (!Permissions.CheckPermission(player, $"{this.PluginName}.lock"))
                            return new[] { "No Permissions." };
                        Warhead.IsKeycardActivated = true;
                        API.Handlers.BetterWarheadHandler.Warhead.ButtonOpen = true;
                        success = true;
                        return new[] { "Alpha Warhead button opened" };
                    }

                case "close":
                    {
                        if (!Permissions.CheckPermission(player, $"{this.PluginName}.lock"))
                            return new[] { "No Permissions." };
                        Warhead.IsKeycardActivated = false;
                        API.Handlers.BetterWarheadHandler.Warhead.ButtonOpen = false;
                        success = true;
                        return new[] { "Alpha Warhead button closed" };
                    }

                case "lockstart":
                    {
                        if (!Permissions.CheckPermission(player, $"{this.PluginName}.lock"))
                            return new[] { "No Permissions." };
                        if (args.Length == 1)
                            return new[] { "Wrong arguments", "warhead lockstart true/false" };
                        if (args[1] == "true")
                        {
                            API.Handlers.BetterWarheadHandler.Warhead.StartLock = true;
                            success = true;
                            return new[] { "Alpha Warhead start lock turned on" };
                        }
                        else if (args[1] == "false")
                        {
                            API.Handlers.BetterWarheadHandler.Warhead.StartLock = false;
                            success = true;
                            return new[] { "Alpha Warhead start lock turned off" };
                        }
                        else
                        {
                            return new[] { "Wrong arguments", "warhead lockstart true/false" };
                        }
                    }

                case "lockstop":
                    {
                        if (!Permissions.CheckPermission(player, $"{this.PluginName}.lock"))
                            return new[] { "No Permissions." };
                        if (args.Length == 1)
                            return new[] { "Wrong arguments", "warhead lockstop true/false" };
                        if (args[1] == "true")
                        {
                            API.Handlers.BetterWarheadHandler.Warhead.StopLock = true;
                            success = true;

                            // plugin.CommandManager.CallCommand(sender, "nuke", new string[] { "lock" });
                            return new[] { "Alpha Warhead stop lock turned on" };
                        }
                        else if (args[1] == "false")
                        {
                            API.Handlers.BetterWarheadHandler.Warhead.StopLock = false;
                            success = true;

                            // plugin.CommandManager.CallCommand(sender, "nuke", new string[] { "unlock" });
                            return new[] { "Alpha Warhead stop lock turned off" };
                        }
                        else
                        {
                            return new[] { "Wrong arguments", "warhead lockstop true/false" };
                        }
                    }

                case "lockbutton":
                    {
                        if (!Permissions.CheckPermission(player, $"{this.PluginName}.lock"))
                            return new[] { "No Permissions." };
                        if (args.Length == 1)
                            return new[] { "Wrong arguments", "warhead lockbutton true/false" };
                        if (args[1] == "true")
                        {
                            API.Handlers.BetterWarheadHandler.Warhead.ButtonLock = true;
                            success = true;
                            return new[] { "Alpha Warhead button lock turned on" };
                        }
                        else if (args[2] == "false")
                        {
                            API.Handlers.BetterWarheadHandler.Warhead.ButtonLock = false;
                            success = true;
                            return new[] { "Alpha Warhead button lock turned off" };
                        }
                        else
                        {
                            return new[] { "Wrong arguments", "warhead lockbutton true/false" };
                        }
                    }

                case "locklever":
                    {
                        if (!Permissions.CheckPermission(player, $"{this.PluginName}.lock"))
                            return new[] { "No Permissions." };
                        if (args.Length == 1)
                            return new[] { "Wrong arguments", "warhead locklever true/false" };
                        if (args[1] == "true")
                        {
                            API.Handlers.BetterWarheadHandler.Warhead.LeverLock = true;
                            success = true;
                            return new[] { "Alpha Warhead level locked" };
                        }
                        else if (args[1] == "false")
                        {
                            API.Handlers.BetterWarheadHandler.Warhead.LeverLock = false;
                            success = true;
                            return new[] { "Alpha Warhead level locked" };
                        }
                        else
                        {
                            return new[] { "Wrong arguments", "warhead locklever true/false" };
                        }
                    }

                case "getlast":
                    {
                        if (!Permissions.CheckPermission(player, $"{this.PluginName}.data"))
                            return new[] { "No Permissions." };
                        success = true;
                        return new[]
                        {
                            "Last Warhead Start User: " + (API.Handlers.BetterWarheadHandler.Warhead.LastStartUser == null ? "Unknown" : $"{API.Handlers.BetterWarheadHandler.Warhead.LastStartUser.Nickname} ({API.Handlers.BetterWarheadHandler.Warhead.LastStartUser.Id}) | {API.Handlers.BetterWarheadHandler.Warhead.LastStartUser.UserId}"),
                            "Last Warhead Stop User: " + (API.Handlers.BetterWarheadHandler.Warhead.LastStopUser == null ? "Unknown" : $"{API.Handlers.BetterWarheadHandler.Warhead.LastStopUser.Nickname} ({API.Handlers.BetterWarheadHandler.Warhead.LastStopUser.Id}) | {API.Handlers.BetterWarheadHandler.Warhead.LastStopUser.UserId}"),
                        };
                    }

                case "stats":
                    {
                        if (!Permissions.CheckPermission(player, $"{this.PluginName}.data"))
                            return new[] { "No Permissions." };
                        success = true;
                        return new[]
                        {
                            "Alpha Warhead stats:",
                            "Detonated: " + Warhead.IsDetonated,
                            "Counting Down: " + API.Handlers.BetterWarheadHandler.Warhead.CountingDown,
                            "Enabled: " + Warhead.LeverStatus,
                            "Time Left: " + API.Handlers.BetterWarheadHandler.Warhead.TimeLeft,
                            "StartLock: " + API.Handlers.BetterWarheadHandler.Warhead.StartLock,
                            "StopLock: " + API.Handlers.BetterWarheadHandler.Warhead.StopLock,
                            "LeverLock: " + API.Handlers.BetterWarheadHandler.Warhead.LeverLock,
                            "ButtonLock: " + API.Handlers.BetterWarheadHandler.Warhead.ButtonLock,
                            "Button Open: " + Warhead.IsLocked,
                            "Last Start User: " + (API.Handlers.BetterWarheadHandler.Warhead.LastStartUser == null ? "Unknown" : $"{API.Handlers.BetterWarheadHandler.Warhead.LastStartUser.Nickname} ({API.Handlers.BetterWarheadHandler.Warhead.LastStartUser.Id}) | {API.Handlers.BetterWarheadHandler.Warhead.LastStartUser.UserId}"),
                            "Last Stop User: " + (API.Handlers.BetterWarheadHandler.Warhead.LastStopUser == null ? "Unknown" : $"{API.Handlers.BetterWarheadHandler.Warhead.LastStopUser.Nickname} ({API.Handlers.BetterWarheadHandler.Warhead.LastStopUser.Id}) | {API.Handlers.BetterWarheadHandler.Warhead.LastStopUser.UserId}"),
                        };
                    }

                default:
                    {
                        return new[] { "Wrong arguments", this.GetUsage() };
                    }
            }
        }

        public string GetUsage()
        {
            return "warhead start/stop/on/off/lockstart/lockstop/lockbutton/locklever/getlast/stats";
        }
    }
}
