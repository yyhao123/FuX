// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndentationLevelAttribute.cs" company="Demo.Windows.Controls.property.core">
//   Copyright (c) 2014 Demo.Windows.Controls.property.core contributors
// </copyright>
// <summary>
//   Specifies the indentation level for the decorated property.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Demo.Windows.Controls.property.core.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies the indentation level for the decorated property.
    /// </summary>
    public class IndentationLevelAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndentationLevelAttribute" /> class.
        /// </summary>
        /// <param name="indentationLevel">The indentation level.</param>
        public IndentationLevelAttribute(int indentationLevel)
        {
            this.IndentationLevel = indentationLevel;
        }

        /// <summary>
        /// Gets or sets the indentation level.
        /// </summary>
        /// <value>
        /// The indentation level.
        /// </value>
        public int IndentationLevel { get; set; }
    }
}