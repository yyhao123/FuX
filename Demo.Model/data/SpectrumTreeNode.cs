using Demo.Model.@enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    /// <summary>
    /// 列表树
    /// </summary>
    public class SpectrumTreeNode
    {
        public int Id { get; set; }

        public int ParentId { get; set; }

        public string SpectrumId { get; set; }

        public string NodeText { get; set; }

        public SpectrumTreeNodeType NodeType { get; set; }

        public Color Color { get; set; }

        public ZedTypeBox ZedTypeBox { get; set; } = ZedTypeBox.D;

        public bool IsExpanded { get; set; }
    }
}
