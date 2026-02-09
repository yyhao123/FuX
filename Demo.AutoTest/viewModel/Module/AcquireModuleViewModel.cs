using CommunityToolkit.Mvvm.Input;
using Demo.AutoTest.data;
using Demo.Model.data;
using Demo.Model.@enum;
using Demo.Windows.Core.mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Demo.AutoTest.viewModel.Module
{
    public class AcquireModuleDataInfo:BindNotify
    {
        public int NextNodeId()
        {
            return _nextNodeId++;
        }

        private int _nextNodeId = 1;
        /// <summary>
        /// 记录光栅的旋转角度
        /// </summary>
        public double Qvalue { get; set; }
        /// <summary>
        /// 光栅数
        /// </summary>
        public double gratingLines { get; set; }

        public SpecInfo SpecInfo { get; set; }

        /// <summary>
        /// 激发波长
        /// </summary>
        public int Wavelength { get; set; }

        public ZedTypeBox ZedTypeBox { get; set; } = ZedTypeBox.D;

        /// <summary>
        /// TreeList列表显示的谱图
        /// </summary>
        public IList<SpectrumNode> SpectrumList { get; set; } = new List<SpectrumNode>();

        /// <summary>
        /// Treelist node 节点列表
        /// </summary>
        public ObservableCollection<SpectrumTreeNode> SpectrumTreeNodes = new ObservableCollection<SpectrumTreeNode>();

        /// <summary>
        /// Treelist node 节点列表
        /// </summary>
        public IList<SpectrumTreeNode> SpectrumTreeNodes1 { get; set; }


        /// <summary>
        /// 显示的谱图数据类型
        /// </summary>
        public SpectrumDataType DataType { get; set; } = SpectrumDataType.DarkSubtracted;

        public SpectrumXAxisUnit SpectrumXAsisType { get; set; } = SpectrumXAxisUnit.RamanShift;

        /// <summary>
        /// ZedGraph图表上显示的谱图
        /// </summary>
        public IList<SpectrumNode> LineItemList { get; set; } = new List<SpectrumNode>();

        /// <summary>
        /// Pin spectrum list
        /// </summary>
        public IList<SpectrumNode> PinSpectrumList { get; set; } = new List<SpectrumNode>();

        /// <summary>
        /// graph line color index
        /// </summary>
        public int CurveColorIndex { get; set; } = 0;

        /// <summary>
        /// 历史记录
        /// </summary>
        public IList<SpectrumHistoryDto> SpectrumHistory = new List<SpectrumHistoryDto>();

        /// <summary>
        /// 添加谱图到列表
        /// </summary>
        /// <param name="spectrum"></param>
        public bool AddSpectrumToList(SpectrumDto spectrum, Color spectrumColor, ZedTypeBox zedTypeBox = ZedTypeBox.D, SpecInfo specInfo = null)
        {
            if (spectrum == null) return false;

            if (SpectrumList.Any(x => x.SpectrumId == spectrum.Id && x.ZedTypeBox == zedTypeBox))
            {
                return false;
            }

            var node = new SpectrumNode(spectrum, spectrumColor, zedTypeBox, specInfo);
            node.ZedTypeBox = zedTypeBox;
            SpectrumList.Add(node);

            return true;
        }


        

    }
}
