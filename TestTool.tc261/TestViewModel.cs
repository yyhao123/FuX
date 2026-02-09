using Demo.Windows.Core.mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTool.tc261
{
    public class TestViewModel:BindNotify
    {
        public ReadOnlyObservableCollection<GalleryDataItemViewModel> DataItems { get; }

        public TestViewModel()
        {
        //    this.DataItems = new(
        //[
        //    GalleryDataItemViewModel.Create("Images\\Blue.png", "Images\\BlueLarge.png", "Blue", "Group A"),
        //    GalleryDataItemViewModel.Create("Images\\Brown.png", "Images\\BrownLarge.png", "Brown", "Group A"),
        //    GalleryDataItemViewModel.Create("Images\\Gray.png", "Images\\GrayLarge.png", "Gray", "Group A"),
        //    GalleryDataItemViewModel.Create("Images\\Green.png", "Images\\GreenLarge.png", "Green", "Group A"),
        //    GalleryDataItemViewModel.Create("Images\\Orange.png", "Images\\OrangeLarge.png", "Orange", "Group A"),
        //    GalleryDataItemViewModel.Create("Images\\Pink.png", "Images\\PinkLarge.png", "Pink", "Group B"),
        //    GalleryDataItemViewModel.Create("Images\\Red.png", "Images\\RedLarge.png", "Red", "Group B"),
        //    GalleryDataItemViewModel.Create("Images\\Yellow.png", "Images\\YellowLarge.png", "Yellow", "Group B")
        //]);
        }
    }
}
