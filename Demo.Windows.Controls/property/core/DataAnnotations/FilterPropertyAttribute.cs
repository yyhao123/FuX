// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterPropertyAttribute.cs" company="Demo.Windows.Controls.property.core">
//   Copyright (c) 2014 Demo.Windows.Controls.property.core contributors
// </copyright>
// <summary>
//   Specifies the name of a property that contains a file path filter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Demo.Windows.Controls.property.core.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies the name of a property that contains a file path filter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FilterPropertyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterPropertyAttribute" /> class.
        /// </summary>
        /// <param name="propertyName">Name of the property that contains the filter.</param>
        public FilterPropertyAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; set; }
    }
}