using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    public class Range
    {
        public double Minimum { get; set; }
        public double Maximum { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.Minimum, this.Maximum);
        }
    }
}
