namespace Evo.Scm.Extensions;

public static class DecimalExtension
{
    /// <summary>
    /// 转换四舍五入
    /// </summary>
    /// <param name="num">要转换的数值</param>
    /// <param name="precision">小数位数</param>
    /// <returns></returns>
    public static decimal ToRound(this decimal num, int precision) => Math.Round((decimal) num, precision, MidpointRounding.AwayFromZero);
}