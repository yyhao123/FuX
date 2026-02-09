using Demo.Model.data;
using Demo.Model.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.extend
{
    public static class SpectrumSort
    {
        public static IEnumerable<Spectrum> Sort(
            this IEnumerable<Spectrum> data,
            IndexSpectrumSortFilterPage options)
        {
            var res = data;

            switch (options.Sort)
            {
                case SpectrumOrderByOptions.SimpleOrder:
                    return data.OrderByDescending(x => x.Created);
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(options.Sort), options.Sort, null);
            }
        }
    }
}
