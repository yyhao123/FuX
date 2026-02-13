using Demo.AutoTest.Core.handler;
using Demo.AutoTest.Views.UserControls;
using Demo.AutoTest.Views.UserControls.Bars;
using Demo.AutoTest.ViewModels.UserControls.Bars;
using Demo.Windows.Core.handler;
using Demo.Windows.Core.mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.AutoTest.ViewModels.UserControls
{
    public class MiddleGraphAreaViewModel : NotifyObject
    {
        
        private TopLeftBarView topLeftView;
        private TopRightBarView topRightView;
        private RightBottomBarView rightBottomView;
        private LeftBottomBarView leftBottomView;

        /// <summary>
        /// 控件
        /// </summary>
        public TopLeftBarView TopLeftView
        {
            get => topLeftView;
            set => SetProperty(ref topLeftView, value);
        }

        /// <summary>
        /// 控件
        /// </summary>
        public TopRightBarView TopRightView
        {
            get => topRightView;
            set => SetProperty(ref topRightView, value);
        }

        /// <summary>
        /// 控件
        /// </summary>
        public RightBottomBarView RightBottomView
        {
            get => rightBottomView;
            set => SetProperty(ref rightBottomView, value);
        }

        /// <summary>
        /// 控件
        /// </summary>
        public LeftBottomBarView LeftBottomView
        {
            get => leftBottomView;
            set => SetProperty(ref leftBottomView, value);
        }

        public MiddleGraphAreaViewModel()
        {
            topLeftView = InjectionWpf.UserControl<TopLeftBarView, TopLeftBarViewModel>(true);
            topRightView = InjectionWpf.UserControl<TopRightBarView, TopRightBarViewModel>(true);
            rightBottomView = InjectionWpf.UserControl<RightBottomBarView, RightBottomBarViewModel>(true);
            leftBottomView = InjectionWpf.UserControl<LeftBottomBarView, LeftBottomBarViewModel>(true);
        }
    }
}
