// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDragSource.cs" company="Demo.Windows.Controls.property.core">
//   Copyright (c) 2014 Demo.Windows.Controls.property.core contributors
// </copyright>
// <summary>
//   Allows an object to be dragged.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Demo.Windows.Controls.property.core
{
    /// <summary>
    /// Allows an object to be dragged.
    /// </summary>
    public interface IDragSource
    {
        /// <summary>
        /// Gets a value indicating whether this instance is possible to drag.
        /// </summary>
        bool IsDraggable { get; }

        /// <summary>
        /// Detaches this instance (for move and drop somewhere else).
        /// </summary>
        void Detach();
    }
}