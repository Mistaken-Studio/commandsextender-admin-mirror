// -----------------------------------------------------------------------
// <copyright file="CustomStructuresIntegration.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Mistaken.CustomStructures;
using UnityEngine;

namespace Mistaken.CommandsExtender.Admin
{
    internal static class CustomStructuresIntegration
    {
        public static object Asset { get; private set; }

        public static void Init()
        {
            if (CustomStructuresHandler.TryGetAsset("Talk_Void_Room", out var asset))
                Asset = asset;
        }

        public static GameObject SpawnAsset(Vector3 position)
            => ((Asset)Asset).Spawn(position, Vector3.zero, Vector3.one).transform.GetChild(0).gameObject;
    }
}
