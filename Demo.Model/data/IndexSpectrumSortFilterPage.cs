using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    public class IndexSpectrumSortFilterPage : Pagination<SpectrumHistoryDto>
    {
        public SpectrumOrderByOptions Sort = SpectrumOrderByOptions.SimpleOrder;

        public DateTime To { get; set; }
    }

    public enum SpectrumOrderByOptions
    {
        SimpleOrder = 0
    }
}
