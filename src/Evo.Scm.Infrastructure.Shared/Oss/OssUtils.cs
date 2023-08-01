namespace Evo.Scm.Oss;

public static class OssUtils
{
    public static string GetResizeUrl(string url, int width = 40, int height = 40)
    {
        return $"{url}?x-oss-process=image/resize,m_pad,w_{width},h_{height}";
    }
}