using Demo.Model.data;
using Demo.Model.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.extend
{
    public static class SpectrumFilter
    {
        public static IEnumerable<Spectrum> Filter(
            this IEnumerable<Spectrum> data,
            IndexSpectrumSortFilterPage options)
        {
            var res = data;

            if (options.To > DateTime.MinValue)
                res = res.Where(x => x.Created <= options.To);

            return res;
        }
    }
}
