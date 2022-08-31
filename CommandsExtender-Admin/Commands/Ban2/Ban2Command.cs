// -----------------------------------------------------------------------
// <copyright file="Ban2Command.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Mistaken.API;
using Mistaken.API.Commands;
using Mistaken.API.Diagnostics;
using Mistaken.API.Extensions;

namespace Mistaken.CommandsExtender.Admin
{
    /// <inheritdoc/>
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Ban2Command : IBetterCommand, IUsageProvider
    {
        /// <summary>
        /// Event used to guess ban duration.
        /// </summary>
        public static event Action<GuessBanDurationEventArgs> BanDurationGuesser;

        /// <inheritdoc/>
        public override string Command => "ban2";

        /// <inheritdoc/>
        public override string[] Aliases => new string[] { "b2", "ban" };

        /// <inheritdoc/>
        public override string Description => "Good old ban2";

        /// <inheritdoc/>
        public string[] Usage => new string[]
        {
            "%player%",
            "Duration (ex 5m, 1h, 2d, 1w, 1mo, 50y)",
            "Reason",
        };

        /// <inheritdoc/>
        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            success = false;
            if (args.Length > 2)
            {
                try
                {
                    if (!int.TryParse(args[0].Split('.')[0], out int pid))
                        return new string[] { "Failed to parse playerId to int32" };
                    var target = RealPlayers.Get(pid);
                    if (target == null)
                        return new string[] { "Player not found" };

                    if (sender.IsPlayer())
                    {
                        var admin = Player.Get(sender);

                        if (admin.ReferenceHub.serverRoles.KickPower <= target.ReferenceHub.serverRoles.Group?.RequiredKickPower)
                            return new string[] { $"Access denied | Too low kickpower | {admin.ReferenceHub.serverRoles.KickPower} <= {target.ReferenceHub.serverRoles.Group?.RequiredKickPower}" };
                    }

                    if (!int.TryParse(args[1].ToLower().Replace("mo", string.Empty).Trim(new char[] { 'm', 'h', 'd', 'w', 'y' }), out int duration))
                        return new string[] { "Detected error with number convertion", "Failed to convert |" + args[1].ToLower().Replace("mo", string.Empty).Trim(new char[] { 'm', 'h', 'd', 'w', 'y' }) + "| to Int32" };

                    var baseDur = duration;
                    GetDurationAndReason(duration, args, out duration, out string type, out string reason, out string textTime, out bool loud);

                    bool requireConfirmation = false;
                    string confirmationReason = "Error";
                    if (((float)duration / 1440) >= 1)
                    {
                        requireConfirmation = true;
                        confirmationReason = "Ban is longer than 1 day";
                    }

                    if (GuessBanDuration(target, duration, reason, out string tmp))
                    {
                        requireConfirmation = true;
                        confirmationReason = tmp;
                    }

                    success = true;
                    if (!requireConfirmation)
                    {
                        CompleteBan(sender as CommandSender, reason, target, duration, textTime, loud);
                        return StyleBan(pid, target, baseDur, duration, type, reason);
                    }
                    else
                    {
                        BanData data = new BanData(sender as CommandSender, reason, target, duration, baseDur, type, textTime, loud);
                        AwaitingBans.Add(data);
                        Module.CallSafeDelayed(15, () => AwaitingBans.Remove(data), "Ban2.RemoveAwaitingBan");

                        return StyleBanConfirm(pid, target, baseDur, duration, type, reason, confirmationReason);
                    }
                }
                catch (Exception e)
                {
                    return new string[] { e.Message, e.StackTrace, e.InnerException.ToString(), e.Source };
                }
            }
            else
                return new string[] { "BAN2 [ID] [DURATION] [REASON]" };
        }

        internal static readonly List<BanData> AwaitingBans = new List<BanData>();

        internal static void CompleteBan(CommandSender senderPlayer, string reason, Player target, int duration, string textTime, bool loud)
        {
            target.Ban(duration * 60, $"[{textTime}] {reason}", senderPlayer?.LogName ?? "UNKNOWN");
            if (loud)
                Map.Broadcast(5, target?.Nickname + " has been <color=red>banned</color> from this server", Broadcast.BroadcastFlags.Normal);
        }

        internal struct BanData
        {
            public CommandSender Admin;
            public string Reason;
            public Player Target;
            public int Duration;
            public int BaseDur;
            public string DurType;
            public string TextTime;
            public bool IsLoud;

            public BanData(CommandSender admin, string reason, Player target, int duration, int baseDur, string durType, string textTime, bool loud = false)
            {
                this.Admin = admin;
                this.Reason = reason;
                this.Target = target;
                this.Duration = duration;
                this.BaseDur = baseDur;
                this.DurType = durType;
                this.TextTime = textTime;
                this.IsLoud = loud;
            }

            public void Execute()
            {
                CompleteBan(this.Admin, this.Reason, this.Target, this.Duration, this.TextTime, this.IsLoud);
                AwaitingBans.Remove(this);
            }

            public string[] StyleBan()
                => Ban2Command.StyleBan(this.Target.Id, this.Target, this.BaseDur, this.Duration, this.DurType, this.Reason);
        }

        private static void PrepareStyleBan(Player target, int baseDur, ref string type, out string nickname, out string userId, out string ip)
        {
            if (baseDur != 1)
                type += "s";
            if (target == null)
            {
                nickname = "ERROR";
                userId = "ERROR";
                ip = "ERROR";
            }
            else
            {
                nickname = target.Nickname;
                userId = target.UserId;
                ip = target.IPAddress;
            }
        }

        private static string[] StyleBanConfirm(int playerID, Player target, int baseDur, int duration, string type, string reason, string confirmReason)
        {
            PrepareStyleBan(target, baseDur, ref type, out string nick, out string userId, out string ip);

            return new string[]
            {
                string.Empty,
                "<color=blue>======= BANNING FINAL STEP =======</color>",
                $"ID: {playerID}",
                $"Nickname: {nick}",
                $"UserId: {userId}",
                $"IpAddress: {ip}",
                $"Time : {baseDur} {type} (RAW: {duration})",
                $"Reason: {reason}",
                string.Empty,
                $"<color=red>Confirmation Request Reason: {confirmReason}</color>",
                string.Empty,
                "Type \"CONFIRM\" to ban player or wait 15 seconds to cancel",
                "<color=blue>======= BANNING FINAL STEP =======</color>",
            };
        }

        private static string[] StyleBan(int playerID, Player target, int baseDur, int duration, string type, string reason)
        {
            PrepareStyleBan(target, baseDur, ref type, out string nick, out string userId, out string ip);

            return new string[]
            {
                string.Empty,
                "<color=green>======= BANNING CONFIRMATION =======</color>",
                $"ID: {playerID}",
                $"Nickname: {nick}",
                $"UserId: {userId}",
                $"IpAddress: {ip}",
                $"Time : {baseDur} {type} (RAW: {duration})",
                $"Reason: {reason}",
                "<color=green>======= BANNING CONFIRMATION =======</color>",
            };
        }

        private static void GetDurationAndReason(int dur, string[] args, out int duration, out string type, out string reason, out string textTime, out bool loud)
        {
            type = string.Empty;
            duration = dur;
            if (args[1].Contains("m") && !args[1].Contains("mo"))
            {
                args[1] = args[1].Replace("m", null);
                duration *= 1;
                type = "minute";
            }

            if (args[1].Contains("h"))
            {
                args[1].Replace("h", null);
                duration *= 60;
                type = "hour";
            }

            if (args[1].Contains("d"))
            {
                args[1].Replace("d", null);
                duration *= 1440;
                type = "day";
            }

            if (args[1].Contains("w"))
            {
                args[1].Replace("w", null);
                duration *= 10080;
                type = "week";
            }

            if (args[1].Contains("mo"))
            {
                args[1].Replace("mo", null);
                duration *= 43200;
                type = "month";
            }

            if (args[1].Contains("y"))
            {
                args[1].Replace("y", null);
                duration *= 518400;
                type = "year";
            }

            if (type == string.Empty)
                type = "minute";
            reason = string.Empty;
            loud = false;
            foreach (string s in args.Skip(2))
            {
                if (s.ToLower() == "-loud")
                    loud = true;
                if (s.Trim().StartsWith("-") == false)
                    reason += " " + s;
            }

            textTime = $"BAN: {dur} {type}";
            if (dur == 0)
                textTime = "KICK";
            else if (dur != 1)
                textTime += "s";
        }

        private static bool GuessBanDuration(Player player, int rawDuration, string reason, out string message)
        {
            var ev = new GuessBanDurationEventArgs(player, rawDuration, reason);
            BanDurationGuesser?.Invoke(ev);
            message = ev.Response;
            if (string.IsNullOrWhiteSpace(message))
                return false;
            return true;

            /*var suggestedLength = Systems.Bans.BansAnalizer.GuessBanDuration(Systems.Bans.BansManager.GetBans(player.UserId), Systems.Bans.BansAnalizer.GetBanCategory(reason));
            var durrationCategory = Systems.Bans.BansAnalizer.GetDurationCategory(rawDuration);
            var suggestedLengthCategory = Systems.Bans.BansAnalizer.GetDurationCategory(suggestedLength);
            if (suggestedLengthCategory != durrationCategory)
            {
                message = $"Guessed ban duration is diffrent, {durrationCategory} => {suggestedLengthCategory}";
                return true;
            }

            message = null;
            return false;*/
        }
    }
}
