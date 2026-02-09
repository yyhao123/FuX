// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContentAttribute.cs" company="Demo.Windows.Controls.property.core">
//   Copyright (c) 2014 Demo.Windows.Controls.property.core contributors
// </copyright>
// <summary>
//   Specifies that the value contains content that should be handled by a ContentControl.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Demo.Windows.Controls.property.core.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the value contains content that should be handled by a ContentControl.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ContentAttribute : Attribute
    {
    }
}