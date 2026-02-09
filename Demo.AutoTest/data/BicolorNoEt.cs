using Demo.Windows.Core.mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.AutoTest.data
{
    public class BicolorNoEt 
    {

        /// <summary>
        /// 比色皿编号
        /// </summary>
        [DescriptionAttribute("比色皿编号")]
        [Browsable(false)]
        public string BicolorNo { get; set; }

        
    }
}
