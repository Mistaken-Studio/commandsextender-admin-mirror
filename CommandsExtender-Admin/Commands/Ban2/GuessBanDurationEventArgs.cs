// -----------------------------------------------------------------------
// <copyright file="GuessBanDurationEventArgs.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Exiled.API.Features;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace Mistaken.CommandsExtender.Admin
{
    /// <summary>
    /// Event args.
    /// </summary>
    [PublicAPI]
    public class GuessBanDurationEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GuessBanDurationEventArgs"/> class.
        /// </summary>
        /// <param name="target"><see cref="Target"/>.</param>
        /// <param name="duration"><see cref="RawDuration"/>.</param>
        /// <param name="reason"><see cref="Reason"/>.</param>
        public GuessBanDurationEventArgs(Player target, int duration, string reason)
        {
            this.Target = target;
            this.RawDuration = duration;
            this.Reason = reason;
            this.Response = null;
        }

        /// <summary>
        /// Gets player to be checked.
        /// </summary>
        public Player Target { get; }

        /// <summary>
        /// Gets raw duration (In minutes).
        /// </summary>
        public int RawDuration { get; }

        /// <summary>
        /// Gets ban reason.
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Gets or sets response. If <see langword="null"/> then duration is correct.
        /// </summary>
        public string Response { get; set; }
    }
}
