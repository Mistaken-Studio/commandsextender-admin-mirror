﻿// -----------------------------------------------------------------------
// <copyright file="NicknamePatch.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using HarmonyLib;
using Mistaken.CommandsExtender.Admin.Commands;

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter

namespace Mistaken.CommandsExtender.Admin.Patches
{
    /// <summary>
    /// Nickname patch.
    /// </summary>
    [HarmonyPatch(typeof(NicknameSync), nameof(NicknameSync.UpdateNickname), typeof(string))]
    [HarmonyPatch(typeof(NicknameSync), nameof(NicknameSync.UserCode_CmdSetNick), typeof(string))]
    public static class NicknamePatch
    {
        /// <summary>
        /// List of real nicknames (Orginal).
        /// </summary>
        public static readonly Dictionary<string, string> RealNicknames = new Dictionary<string, string>();

        /// <summary>
        /// Prefix Patch.
        /// </summary>
        public static bool Prefix(NicknameSync __instance, ref string n)
        {
            if (string.IsNullOrWhiteSpace(__instance._hub.characterClassManager.UserId))
                return true;
            RealNicknames[__instance._hub.characterClassManager.UserId] = n;
            if (FakeNickCommand.FullNicknames.TryGetValue(__instance._hub.characterClassManager.UserId, out string newNick))
                n = newNick;
            return true;
        }
    }
}
