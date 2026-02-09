using PropertyTools.Wpf;
using PropertyTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Windows.Core.handler;
using ScottPlot;
using System.Resources;
using Demo.Model.attribute;


namespace Demo.AutoTest.Model.data
{
    public class PramModel : Observable
    {
        private string name;

        [PropertyTab("Name", PropertyTabScope.Component)]
        [Category("Object owner"), Description("Information about the owner of the object.")]

        public string Name
        {
            get => this.name;
            set => this.SetValue(ref this.name, value);
        }

        private int age;
        private int time;

        [PropertyTab("Age", PropertyTabScope.Component)]
        //[CustomDisplayName("积分时间")]
        public int Age { get; set; }
       

    }
}
    
