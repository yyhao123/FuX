using Demo.AutoTest.ViewModels.UserControls;
using Demo.Windows.Controls.property.wpf;
using FuX.Core.handler;
using FuX.Model.attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Demo.AutoTest.data.CustomPropertyGridControlFactory;


namespace Demo.AutoTest.data
{
    public class CustomPropertyGridOperator : PropertyGridOperator
    {
        protected override PropertyItem CreateCore(PropertyDescriptor pd, PropertyDescriptorCollection properties)
        {
            // Check if the property is decorated with an "ImportantAttribute"
            var ia = pd.GetFirstAttributeOrDefault<ImportantAttribute>();
            if (ia != null)
            {
                // Create a custom PropertyItem instance
                return new ImportantPropertyItem(pd, properties);
            }

            return base.CreateCore(pd, properties);
        }

        protected override string GetDisplayName(PropertyDescriptor pd, Type declaringType)
        {
            // Use the property name as display name - this will be passed to the GetLocalizedString later
            return pd.DisplayName;
        }

        protected override string GetLocalizedString(string key, Type declaringType)
        {
            // Add a star to show that we have handled this
            // A localization mechanism can be used to localize the strings

            return FuX.Core.handler.LanguageHandler.GetLanguageValue(key, App.LanguageOperate);
        }
    }
}
