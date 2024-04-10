namespace Blog.Core.Helper
{
    /// <summary>
    /// 简单缓存接口
    /// </summary>
    public interface ICaching
    {
        object Get(string cacheKey);
        void Set(string cacheKey, object cacheValue);
    }
}
