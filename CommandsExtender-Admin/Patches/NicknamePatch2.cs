// -----------------------------------------------------------------------
// <copyright file="NicknamePatch2.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using HarmonyLib;
using Mistaken.CommandsExtender.Admin.Commands;

// ReSharper disable InconsistentNaming
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter

namespace Mistaken.CommandsExtender.Admin.Patches
{
    [HarmonyPatch(typeof(NicknameSync), nameof(NicknameSync.SetNick), typeof(string))]
    internal static class NicknamePatch2
    {
        public static bool Prefix(NicknameSync __instance, ref string nick)
        {
            if (string.IsNullOrWhiteSpace(__instance._hub.characterClassManager.UserId))
                return true;
            NicknamePatch.RealNicknames[__instance._hub.characterClassManager.UserId] = nick;
            if (FakeNickCommand.FakeNicknames.TryGetValue(__instance._hub.characterClassManager.UserId, out var newNick))
                nick = newNick;
            return true;
        }
    }
}
