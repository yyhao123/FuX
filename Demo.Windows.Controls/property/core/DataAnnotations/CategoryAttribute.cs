// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryAttribute.cs" company="Demo.Windows.Controls.property.core">
//   Copyright (c) 2014 Demo.Windows.Controls.property.core contributors
// </copyright>
// <summary>
//   Specifies the name of the category in which to group the property or event when displayed in a PropertyGrid control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Demo.Windows.Controls.property.core.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies the name of the category in which to group the property or event when displayed in a PropertyGrid control.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CategoryAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryAttribute"/> class.
        /// </summary>
        /// <param name="category">The category.</param>
        public CategoryAttribute(string category)
        {
            this.Category = category;
        }

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <value>The category.</value>
        public virtual string Category { get; private set; }
    }
}