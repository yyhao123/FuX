using Demo.Windows.Controls.property.wpf;
using FuX.Core.handler;
using FuX.Unility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace Demo.Windows.Controls.property
{
    public class FuXPropertyGridControlFactory : PropertyGridControlFactory
    {
        public override FrameworkElement CreateControl(PropertyItem pi, PropertyControlFactoryOptions options)
        {
            // Check if the property is of type Range
            if (pi.Is(typeof(Range)))
            {
                // Create a control to edit the Range
                return this.CreateRangeControl(pi, options);
            }
            if (pi.Is(typeof(Enum)))
            {
                // Create a control to edit the Range
                return this.CreateEnumControl(pi, options);
            }

            return base.CreateControl(pi, options);
        }

        protected virtual FrameworkElement CreateRangeControl(PropertyItem pi, PropertyControlFactoryOptions options)
        {
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition());
            grid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition());
            var minimumBox = new TextBox();
            minimumBox.SetBinding(TextBox.TextProperty, new Binding(pi.Descriptor.Name + ".Minimum"));
            var label = new Label { Content = "-" };
            Grid.SetColumn(label, 1);
            var maximumBox = new TextBox();
            Grid.SetColumn(maximumBox, 2);
            maximumBox.SetBinding(TextBox.TextProperty, new Binding(pi.Descriptor.Name + ".Maximum"));
            grid.Children.Add(minimumBox);
            grid.Children.Add(label);
            grid.Children.Add(maximumBox);

            if (pi is ImportantPropertyItem)
            {
                minimumBox.Background = maximumBox.Background = Brushes.Yellow;
            }

            return grid;
        }


        protected override FrameworkElement CreateEnumControl(
            PropertyItem property, PropertyControlFactoryOptions options)
        {
            //  var isBitField = property.Descriptor.PropertyType.GetTypeInfo().GetCustomAttributes<FlagsAttribute>().Any();

            Type propertyType = property.Descriptor.PropertyType;
            Type actualEnumType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
            var values = this.GetEnumDescriptionsWithValues(property.Descriptor.PropertyType).ToArray();
            var style = property.SelectorStyle;
            if (style == Demo.Windows.Controls.property.core.DataAnnotations.SelectorStyle.Auto)
            {
                style = values.Length > options.EnumAsRadioButtonsLimit
                            ? Demo.Windows.Controls.property.core.DataAnnotations.SelectorStyle.ComboBox
                            : Demo.Windows.Controls.property.core.DataAnnotations.SelectorStyle.RadioButtons;
            }

            switch (style)
            {
                case Demo.Windows.Controls.property.core.DataAnnotations.SelectorStyle.RadioButtons:
                    {
                        var c = new RadioButtonList { EnumType = property.Descriptor.PropertyType };
                        c.Orientation = Orientation.Horizontal;
                        c.SetBinding(RadioButtonList.ValueProperty, property.CreateBinding());
                        return c;
                    }

                case Demo.Windows.Controls.property.core.DataAnnotations.SelectorStyle.ComboBox:
                    {
                        var c = new ComboBox { ItemsSource = values, DisplayMemberPath = "Value", SelectedValuePath = "Key" };
                        c.SetBinding(Selector.SelectedValueProperty, property.CreateBinding());
                        return c;
                    }

                case Demo.Windows.Controls.property.core.DataAnnotations.SelectorStyle.ListBox:
                    {
                        var c = new ListBox { ItemsSource = values };
                        c.SetBinding(Selector.SelectedValueProperty, property.CreateBinding());
                        return c;
                    }

                default:
                    return null;
            }
        }

        public static void PrintDes(Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    var attr = field.GetCustomAttribute<DescriptionAttribute>();
                }
            }
        }


        protected virtual IEnumerable<KeyValuePair<object, string>> GetEnumDescriptionsWithValues(Type enumType)
        {
            var underlyingType = Nullable.GetUnderlyingType(enumType);
            bool isNullable = underlyingType != null;

            if (isNullable)
            {
                enumType = underlyingType;
            }

            if (!enumType.IsEnum)
                throw new ArgumentException("Type must be an enum or nullable enum", nameof(enumType));

            var list = new List<KeyValuePair<object, string>>();

            foreach (var value in Enum.GetValues(enumType))
            {
                string description = value.ToDescription();
                description = GetLocalizedString(description);
                list.Add(new KeyValuePair<object, string>(value, description));
            }

            if (isNullable)
            {
                // null 项用于绑定到可空枚举
                list.Insert(0, new KeyValuePair<object, string>(null, "(无)"));
            }

            return list;
        }



        protected string GetLocalizedString(string key)
        {
            // Add a star to show that we have handled this
            // A localization mechanism can be used to localize the strings

            return key.GetLanguageValue();
        }


        public class ImportantPropertyItem : PropertyItem
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ImportantPropertyItem" /> class.
            /// </summary>
            /// <param name="pd">The pd.</param>
            /// <param name="properties">The properties.</param>
            public ImportantPropertyItem(PropertyDescriptor pd, PropertyDescriptorCollection properties)
                : base(pd, properties)
            {
            }
        }
    }
}
