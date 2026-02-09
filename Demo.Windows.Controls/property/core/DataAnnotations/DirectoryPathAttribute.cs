// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectoryPathAttribute.cs" company="Demo.Windows.Controls.property.core">
//   Copyright (c) 2014 Demo.Windows.Controls.property.core contributors
// </copyright>
// <summary>
//   Specifies that the property is a directory path.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Demo.Windows.Controls.property.core.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the property is a directory path.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DirectoryPathAttribute : Attribute
    {
    }
}