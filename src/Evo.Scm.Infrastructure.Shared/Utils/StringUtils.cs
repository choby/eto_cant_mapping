using System.Security.Cryptography;

namespace Evo.Scm.Utils;

public static class StringUtils
{
    /// <summary>
    /// 生成随机字符串
    /// </summary>
    /// <param name="length">字符串长度</param>
    /// <returns></returns>
    public static string GenerateRandomString(int length)
    {
        using (var crypto = new RNGCryptoServiceProvider())
        {
            var bits = (length * 6);
            var byte_size = ((bits + 7) / 8);
            var bytesarray = new byte[byte_size];
            crypto.GetBytes(bytesarray);
            return Convert.ToBase64String(bytesarray);
        }
    }
}