using Demo.AutoTest.data;
using Demo.AutoTest.services;
using Demo.AutoTest.view.Module;
using Demo.AutoTest.view.userControls;
using Demo.AutoTest.viewModel.userControls;
using Demo.AutoTest.viewModel.windows;
using Demo.Model.data;
using Demo.Windows.Core.handler;
using Demo.Windows.Core.mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.AutoTest.viewModel.Module
{
    public class RightSpectrumListViewModel:BindNotify
    {

        public AcquireModuleState AcquireModuleState { get; set; } = new AcquireModuleState();
        /// <summary>
        /// Treelist node 节点列表
        /// </summary>
        public ObservableCollection<SpectrumTreeNode> SpectrumTreeNodes = new ObservableCollection<SpectrumTreeNode>();
        public RightSpectrumListViewModel()
        {
          
          

        }

        public ObservableCollection<SpectrumNodeBrowseStructuralBody> Node
        {
            get
            {
                return GetProperty(() => Node);
            }
            set
            {
                SetProperty<ObservableCollection<SpectrumNodeBrowseStructuralBody>>(() => Node, value);
            }
        }

        public void  RefreshDataSource()
        {
            Node = new ObservableCollection<SpectrumNodeBrowseStructuralBody> ();
            var d = AcquireModuleState.SpectrumTreeNodes.Select(s => new SpectrumNodeBrowseStructuralBody { Name = s.NodeText }).ToList();
            Node = new ObservableCollection<SpectrumNodeBrowseStructuralBody>(d);
        }

    }
}
