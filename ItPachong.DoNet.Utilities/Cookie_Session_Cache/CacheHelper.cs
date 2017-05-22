// ********************************************************************
// * 项目名称：		    ItPachong.DoNet.Utilities
// * 程序集名称：	    ItPachong.DoNet.Utilities.Cookie_Session_Cache
// * 文件名称：		    CacheHelper.cs
// * 编写者：		    赖强
// * 编写日期：		    2017-05-04
// * 程序功能描述：
// *        缓存处理类
// *
// * 程序变更日期：
// * 程序变更者：
// * 变更说明：
// * 
// ********************************************************************
using ItPachong.DoNet.Utilities.String;
using System;
using System.Collections;
using System.Web;

namespace ItPachong.DoNet.Utilities.Cookie_Session_Cache
{
    /// <summary>
    /// 缓存处理类
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// 获取数据缓存
        /// </summary>
        /// <param name="CacheKey">键</param>
        public static object GetCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey];
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache(string CacheKey, object objObject)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache(string CacheKey, object objObject, TimeSpan Timeout)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, DateTime.MaxValue, Timeout, System.Web.Caching.CacheItemPriority.NotRemovable, null);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);
        }

        /// <summary>清除单一键缓存</summary>
        /// <param name="cacheKey">缓存名称</param>
        public static void RemoveOneCache(string cacheKey)
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            _cache.Remove(cacheKey);
        }

        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        public static void RemoveAllCache(string CacheKey)
        {
            RemoveOneCache(CacheKey);
        }

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                _cache.Remove(CacheEnum.Key.ToString());
            }
        }

        /// <summary>清除所有缓存</summary>
        public static void RemoveManagersAllCache()
        {
            try
            {
                System.Web.Caching.Cache objCache = HttpRuntime.Cache;
                IDictionaryEnumerator cacheEnum = objCache.GetEnumerator();
                if (objCache.Count > 0)
                {
                    var al = new ArrayList();
                    //设置后台在线列表缓存标识
                    const string ss = "OnlineUsers";
                    int keyLen = ss.Length;

                    while (cacheEnum.MoveNext())
                    {
                        //如果是后台在线列表相关缓存，则不进行删除操作
                        if (StringHelper.Left(cacheEnum.Key.ToString(), keyLen) == ss)
                            continue;

                        al.Add(cacheEnum.Key);
                    }

                    foreach (string key in al)
                    {
                        objCache.Remove(key);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>以列表形式返回已存在的所有缓存</summary>
        /// <returns></returns> 
        public static ArrayList ShowAllCache()
        {
            var al = new ArrayList();
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            if (objCache.Count > 0)
            {
                IDictionaryEnumerator cacheEnum = objCache.GetEnumerator();
                while (cacheEnum.MoveNext())
                {
                    al.Add(cacheEnum.Key);
                }
            }
            return al;
        }
    }
}
