// -----------------------------------------------------------------------
// <copyright file="NicknamePatch2.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using HarmonyLib;
using Mistaken.CommandsExtender.Admin.Commands;

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter

namespace Mistaken.CommandsExtender.Admin.Patches
{
    /*[HarmonyPatch(typeof(NicknameSync), "SetNick", typeof(string))]
    internal static class NicknamePatch2
    {
        public static bool Prefix(NicknameSync __instance, ref string nick)
        {
            if (string.IsNullOrWhiteSpace(__instance._hub.characterClassManager.UserId))
                return true;
            NicknamePatch.RealNicknames[__instance._hub.characterClassManager.UserId] = nick;
            if (FakeNickCommand.FullNicknames.TryGetValue(__instance._hub.characterClassManager.UserId, out string newNick))
                nick = newNick;
            return true;
        }
    }*/
}
