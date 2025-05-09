﻿using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumUtil
{
    public static IEnumerable<T> GetValues<T>()
    {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }
}

public static class RandomExtensionMethods
{
    /// <summary>
    /// Returns a random long from min (inclusive) to max (exclusive)
    /// </summary>
    /// <param name="random">The given random instance</param>
    /// <param name="min">The inclusive minimum bound</param>
    /// <param name="max">The exclusive maximum bound.  Must be greater than min</param>
    /// Code by: BlueRaja - Danny Pflughoeft and Mark Amery: https://stackoverflow.com/questions/6651554/random-number-in-long-range-is-this-the-way
    public static long NextLong(this Random random, long min, long max)
    {
        if (max <= min)
            throw new ArgumentOutOfRangeException("max", "max must be > min!");

        //Working with ulong so that modulo works correctly with values > long.MaxValue
        ulong uRange = (ulong)(max - min);

        //Prevent a modolo bias; see https://stackoverflow.com/a/10984975/238419
        //for more information.
        //In the worst case, the expected number of calls is 2 (though usually it's
        //much closer to 1) so this loop doesn't really hurt performance at all.
        ulong ulongRand;
        do
        {
            byte[] buf = new byte[8];
            random.NextBytes(buf);
            ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
        } while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

        return (long)(ulongRand % uRange) + min;
    }

    /// <summary>
    /// Returns a random long from 0 (inclusive) to max (exclusive)
    /// </summary>
    /// <param name="random">The given random instance</param>
    /// <param name="max">The exclusive maximum bound.  Must be greater than 0</param>
    public static long NextLong(this Random random, long max)
    {
        return random.NextLong(0, max);
    }

    /// <summary>
    /// Returns a random long over all possible values of long (except long.MaxValue, similar to
    /// random.Next())
    /// </summary>
    /// <param name="random">The given random instance</param>
    public static long NextLong(this Random random)
    {
        return random.NextLong(long.MinValue, long.MaxValue);
    }
}

