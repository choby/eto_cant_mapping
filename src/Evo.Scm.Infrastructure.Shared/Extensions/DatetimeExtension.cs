namespace Evo.Scm.Extensions;

public static class DatetimeExtension
{
    /// <summary>
    /// 当天23时59分59秒
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static DateTime ToDatetime(this DateTime time) => time.AddDays(1).AddSeconds(-1);
}