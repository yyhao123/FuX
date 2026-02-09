// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyAttribute.cs" company="Demo.Windows.Controls.property.core">
//   Copyright (c) 2014 Demo.Windows.Controls.property.core contributors
// </copyright>
// <summary>
//   Specifies whether the property this attribute is bound to is read-only or read/write.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Demo.Windows.Controls.property.core.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies whether the property this attribute is bound to is read-only or read/write.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ReadOnlyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyAttribute" /> class.
        /// </summary>
        /// <param name="isReadOnly">true to show that the property this attribute is bound to is read-only; false to show that the property is read/write.</param>
        public ReadOnlyAttribute(bool isReadOnly)
        {
            this.IsReadOnly = isReadOnly;
        }

        /// <summary>
        /// Gets a value indicating whether the property this attribute is bound to is read-only.
        /// </summary>
        /// <value>true if the property this attribute is bound to is read-only; false if the property is read/write.</value>
        public bool IsReadOnly { get; private set; }
    }
}