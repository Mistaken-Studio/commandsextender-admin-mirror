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
        public static Asset Asset { get; set; } = null;

        public static GameObject SpawnAsset(Vector3 position)
            => Asset.Spawn(position, Vector3.zero, Vector3.one).transform.GetChild(0).gameObject;
    }
}
