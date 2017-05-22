// ********************************************************************
// * 项目名称：		    ItPachong.DoNet.Utilities
// * 程序集名称：	    ItPachong.DoNet.Utilities.Config
// * 文件名称：		    ConfigHelper.cs
// * 编写者：		    赖强
// * 编写日期：		    2017-05-04
// * 程序功能描述：
// *        对web.config配置文件的操作类
// *
// * 程序变更日期：
// * 程序变更者：
// * 变更说明：
// * 
// ********************************************************************
using ItPachong.DoNet.Utilities.Convert;
using ItPachong.DoNet.Utilities.Cookie_Session_Cache;
using ItPachong.DoNet.Utilities.String;
using System;
using System.Configuration;

namespace ItPachong.DoNet.Utilities.Config
{
    /// <summary>
    /// 对web.config配置文件的操作类
    /// </summary>
    public sealed class ConfigHelper
    {
        /// <summary>
        /// 获取<AppSettings></AppSettings>节点中的配置字符串信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigString(string key)
        {
            string cacheKey = "AppSettings-" + key;
            object objModel = CacheHelper.GetCache(cacheKey);
            if(objModel == null)
            {
                try
                {
                    objModel = ConfigurationManager.AppSettings[key];
                    if(objModel != null)
                    {
                        //参数缓存，10分钟过期
                        CacheHelper.SetCache(cacheKey, objModel, DateTime.Now.AddMinutes(10), TimeSpan.Zero);
                    }
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }

            return objModel + "";
        }

        /// <summary>
        /// 获取<AppSettings></AppSettings>中的配置字符串信息（数组）
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string[] GetConfigStringArr(string key)
        {
            try
            {
                string cacheKey = "AppSettings-" + key;
                var objModel = (string[])CacheHelper.GetCache(cacheKey);
                if(objModel == null)
                {
                    var item = ConfigurationManager.AppSettings[key];
                    objModel = StringHelper.SplitMulti(item, ",");
                    if(objModel != null)
                    {
                        //参数缓存，10分钟过期
                        CacheHelper.SetCache(cacheKey, objModel, DateTime.Now.AddMinutes(10), TimeSpan.Zero);
                    }
                }

                return objModel;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// 获取<AppSettings></AppSettings>中配置的bool信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetConfigBool(string key)
        {
            bool result = false;
            string cfgVal = GetConfigString(key);
            if(!string.IsNullOrEmpty(cfgVal))
            {
                if (cfgVal == "1" || cfgVal.ToLower() == "true")
                    result = true;
            }

            return result;
        }

        /// <summary>
        /// 获取<AppSettings></AppSettings>中配置的Decimal信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static decimal GetConfigDecimal(string key)
        {
            decimal result = 0;
            string cfgVal = GetConfigString(key);
            if(!string.IsNullOrEmpty(cfgVal))
            {
                try
                {
                    result = ConvertHelper.Cdecimal(cfgVal);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }

            return result;
        }

        /// <summary>
        /// 获取<AppSettings></AppSettings>中配置的Int信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetConfigInt(string key)
        {
            int result = 0;
            string cfgVal = GetConfigString(key);
            if(!string.IsNullOrEmpty(cfgVal))
            {
                try
                {
                    result = ConvertHelper.Cint(cfgVal);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }

            return result;
        }
             
    }
}