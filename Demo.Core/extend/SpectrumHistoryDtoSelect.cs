using Demo.Model.data;
using Demo.Model.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.extend
{
    public static class SpectrumHistoryDtoSelect
    {
        public static IEnumerable<SpectrumHistoryDto> Select(
            this IEnumerable<Spectrum> data)
        {
            if (data == null) yield break;

            foreach (var item in data)
            {
                yield return new SpectrumHistoryDto(item);
            }
        }
    }
}
