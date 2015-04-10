using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscreteApproach
{
    public static class StringExtensions
    {
        public static string Repeat(this string input, int count)
        {
            if (!string.IsNullOrEmpty(input))
            {
                StringBuilder builder = new StringBuilder(input.Length * count);

                for (int i = 0; i < count; i++) builder.Append(input);

                return builder.ToString();
            }

            return string.Empty;
        }

        public static int IndexMax<T>(this IEnumerable<T> list, Func<T, double> func)
        {
            int indexMax = !list.Any()
                               ? -1
                               : list
                                     .Select((value, index) => new { Value = func(value), Index = index })
                                     .Aggregate((a, b) => (a.Value > b.Value) ? a : b)
                                     .Index;
            return indexMax;
        }

    }
}
