using Demo.AutoTest.Core.handler;
using Demo.AutoTest.view.userControls;
using Demo.AutoTest.view.userControls.bars;
using Demo.AutoTest.viewModel.userControls.bars;
using Demo.Windows.Core.handler;
using Demo.Windows.Core.mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.AutoTest.viewModel.userControls
{
    public class MiddleGraphAreaViewModel : NotifyObject
    {
        
        private subTopLeftView topLeftView;
        private subTopRightView topRightView;
        private subRightBottomView rightBottomView;
        private subLeftBottomView leftBottomView;

        /// <summary>
        /// 控件
        /// </summary>
        public subTopLeftView TopLeftView
        {
            get => topLeftView;
            set => SetProperty(ref topLeftView, value);
        }

        /// <summary>
        /// 控件
        /// </summary>
        public subTopRightView TopRightView
        {
            get => topRightView;
            set => SetProperty(ref topRightView, value);
        }

        /// <summary>
        /// 控件
        /// </summary>
        public subRightBottomView RightBottomView
        {
            get => rightBottomView;
            set => SetProperty(ref rightBottomView, value);
        }

        /// <summary>
        /// 控件
        /// </summary>
        public subLeftBottomView LeftBottomView
        {
            get => leftBottomView;
            set => SetProperty(ref leftBottomView, value);
        }

        public MiddleGraphAreaViewModel()
        {
            topLeftView = InjectionWpf.UserControl<subTopLeftView, TopLeftViewModel>(true);
            topRightView = InjectionWpf.UserControl<subTopRightView, TopRightViewModel>(true);
            rightBottomView = InjectionWpf.UserControl<subRightBottomView, RightBottomViewModel>(true);
            leftBottomView = InjectionWpf.UserControl<subLeftBottomView, LeftBottomViewModel>(true);
        }
    }
}
