// -----------------------------------------------------------------------
// <copyright file="PluginHandler.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using HarmonyLib;
using Mistaken.API.Diagnostics;
using Mistaken.CommandsExtender.Admin.Logs;
using Mistaken.Updater.API.Config;

namespace Mistaken.CommandsExtender.Admin
{
    /// <inheritdoc cref="Plugin{IConfig}" />
    public class PluginHandler : Plugin<Config>, IAutoUpdateablePlugin
    {
        /// <inheritdoc/>
        public override string Author => "Mistaken Devs";

        /// <inheritdoc/>
        public override string Name => "CommandsExtender-Admin";

        /// <inheritdoc/>
        public override string Prefix => "MCE-A";

        /// <inheritdoc/>
        public override PluginPriority Priority => PluginPriority.Default;

        /// <inheritdoc/>
        public override Version RequiredExiledVersion => new(5, 0, 0);

        /// <inheritdoc />
        public AutoUpdateConfig AutoUpdateConfig => new()
        {
            Type = SourceType.GITLAB,
            Url = "https://git.mistaken.pl/api/v4/projects/30",
        };

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            Instance = this;

            this.harmony = new Harmony("com.mistaken.commandsextenderadmin");
            this.harmony.PatchAll();

            Module.RegisterHandler<CommandsHandler>(this);
            Module.RegisterHandler<LogHandler>(this);
            Module.OnEnable(this);
            Events.Handlers.CustomEvents.LoadedPlugins += CustomEvents_LoadedPlugins;

            base.OnEnabled();
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            this.harmony.UnpatchAll();
            Module.OnDisable(this);
            Events.Handlers.CustomEvents.LoadedPlugins -= CustomEvents_LoadedPlugins;

            base.OnDisabled();
        }

        internal static PluginHandler Instance { get; private set; }

        private static void CustomEvents_LoadedPlugins()
        {
            if (Exiled.Loader.Loader.Plugins.Any(x => x.Name == "CustomStructures"))
                CustomStructuresIntegration.Init();
        }

        private Harmony harmony;
    }
}
