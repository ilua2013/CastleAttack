using System;
using System.Collections.Generic;
using System.Linq;

public static class Extensions
{
    public static IEnumerable<IEnumerable<T>> Split<T>(this T[] arr, int size)
    {
        for (var i = 0; i < arr.Length / size + 1; i++)
        {
            yield return arr.Skip(i * size).Take(size);
        }
    }

    public static List<T[]> SplitEven<T>(this T[] array, int count)
    {
        long remainder;
        long divCount = Math.DivRem(array.Count(), count, out remainder);
        int ajustedCount = (int)((divCount > remainder)
                           ? (divCount / remainder)
                           : (remainder / divCount)) + count;
        int groupCount = (ajustedCount * divCount) > array.Count()
            ? (int)divCount
            : (int)divCount++;
        return Enumerable.Range(0, groupCount).Select(g => array.Skip(g * ajustedCount).Take(ajustedCount).ToArray()).ToList();
    }
}
