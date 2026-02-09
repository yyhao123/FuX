// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConverterAttribute.cs" company="Demo.Windows.Controls.property.core">
//   Copyright (c) 2014 Demo.Windows.Controls.property.core contributors
// </copyright>
// <summary>
//   Specifies a converter that should be used for the property.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Demo.Windows.Controls.property.core.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies a converter that should be used for the property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ConverterAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConverterAttribute" /> class.
        /// </summary>
        /// <param name="converterType">Type of the converter.</param>
        public ConverterAttribute(Type converterType)
        {
            this.ConverterType = converterType;
        }

        /// <summary>
        /// Gets or sets the type of the converter.
        /// </summary>
        /// <value>The type of the converter.</value>
        public Type ConverterType { get; set; }
    }
}