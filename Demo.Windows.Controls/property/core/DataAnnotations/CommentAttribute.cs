// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommentAttribute.cs" company="Demo.Windows.Controls.property.core">
//   Copyright (c) 2014 Demo.Windows.Controls.property.core contributors
// </copyright>
// <summary>
//   Specifies that the value is a comment.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Demo.Windows.Controls.property.core.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the value is a comment.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CommentAttribute : Attribute
    {
    }
}