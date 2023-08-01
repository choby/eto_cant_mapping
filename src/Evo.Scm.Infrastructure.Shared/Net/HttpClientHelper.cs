
using System.Net;
using RestSharp;

namespace Evo.Scm.Net;

public static class HttpClientHelper
{
    private static HttpClient _httpClient;
    static HttpClientHelper()
    {
        _httpClient = new HttpClient();
    }

    public static async Task<Stream> GetFile(string url)
    {
        try
        {
            return await _httpClient.GetStreamAsync(url);
        }
        catch (Exception e)
        {
            return Stream.Null;
        }
    }

    public static async Task<string> GetResizeOssImageBase64Async(string url, int width = 40, int height = 40)
    {
        try
        {
            url = $"{url}?x-oss-process=image/resize,m_pad,w_{width},h_{height}";
            var bytes = await _httpClient.GetByteArrayAsync(url);

            if (bytes != null)
                return Convert.ToBase64String(bytes);

        }
        catch (Exception ex)
        {
            return null;

        }

        return null;
    }
}