// ********************************************************************
// * 项目名称：		    ItPachong.DoNet.Utilities
// * 程序集名称：	    ItPachong.DoNet.Utilities.Cookie_Session_Cache
// * 文件名称：		    CacheHelper.cs
// * 编写者：		    赖强
// * 编写日期：		    2017-05-15
// * 程序功能描述：
// *        Cookie处理类
// *
// * 程序变更日期：
// * 程序变更者：
// * 变更说明：
// * 
// ********************************************************************
using System;
using System.Web;

namespace ItPachong.DoNet.Utilities.Cookie_Session_Cache
{
    /// <summary>
    /// Cookie处理类
    /// </summary>
    public class CookieHelper
    {
        /// <summary>
        /// 清理指定Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        public static void ClearCookie(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if(cookie != null)
            {
                cookie.Expires = DateTime.Now.AddYears(-3);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// 获取指定Cookie值
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string GetCookieValue(string cookieName)
        {
            if(HttpContext.Current.Request.Cookies != null &&
                HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                return HttpContext.Current.Request.Cookies[cookieName].Value;
            }

            return "";
        }

        /// <summary>
        /// 读取Cookie值，Cookies[key]
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetCookieValue(string cookieName, string key)
        {
            if(HttpContext.Current.Request.Cookies != null &&
                HttpContext.Current.Request.Cookies[cookieName] != null &&
                HttpContext.Current.Request.Cookies[cookieName][key] != null)
            {
                return HttpContext.Current.Request.Cookies[cookieName][key].ToString();
            }

            return "";
        }

        /// <summary>
        /// 添加一个Cookie(1年后过期）
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="cookieValue"></param>
        public static void SetCookie(string cookieName, string cookieValue)
        {
            SetCookie(cookieName, cookieValue, DateTime.Now.AddYears(1));
        }

        /// <summary>
        /// 添加一个Cookie，带过期时间
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="cookieValue"></param>
        /// <param name="expires"></param>
        public static void SetCookie(string cookieName, string cookieValue, DateTime expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if(cookie == null)
            {
                cookie = new HttpCookie(cookieName);
            }
            cookie.Value = cookieValue;
            cookie.Expires = expires;
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写Cookie值，Cookies[key](1年后过期)
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="key"></param>
        /// <param name="cookieValue"></param>
        public static void SetCookie(string cookieName, string key, string cookieValue)
        {
            SetCookie(cookieName, key, cookieName, DateTime.Now.AddYears(1));
        }

        /// <summary>
        /// 写Cookie值，Cookies[Key]，带过期时间
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="key"></param>
        /// <param name="cookieValue"></param>
        /// <param name="expires"></param>
        public static void SetCookie(string cookieName, string key, string cookieValue, DateTime expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if(cookie == null)
            {
                cookie = new HttpCookie(cookieName);
            }
            cookie[key] = cookieValue;
            cookie.Expires = expires;
            HttpContext.Current.Response.AppendCookie(cookie);
        }
    }
}