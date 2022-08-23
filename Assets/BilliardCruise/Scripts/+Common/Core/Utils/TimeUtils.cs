using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class TimeUtils
{
    public static long GetCurrentTimestamp()
    {
        DateTime foo = DateTime.UtcNow;
        return (long)((DateTimeOffset)foo).ToUnixTimeSeconds();
    }

    public static double GetCurrentTimestampFull()
    {
        DateTime foo = DateTime.UtcNow;
        return (double)((DateTimeOffset)foo).ToUnixTimeMilliseconds() / 1000;
    }

}
