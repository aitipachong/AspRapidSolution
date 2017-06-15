// ********************************************************************
// * 项目名称：		    ItPachong.DoNet.Utilities
// * 程序集名称：	    ItPachong.DoNet.Utilities.Image
// * 文件名称：		    ImageDown.cs
// * 编写者：		    赖强
// * 编写日期：		    2017-06-13
// * 程序功能描述：
// *        图片下载类
// *
// * 程序变更日期：
// * 程序变更者：
// * 变更说明：
// * 
// ********************************************************************
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace ItPachong.DoNet.Utilities.Image
{
    /// <summary>
    /// 图片下载
    /// </summary>
    public class ImageDown
    {
        public ImageDown()
        { }

        #region 私有方法
        /// <summary>
        /// 获取图片标志
        /// </summary>
        private string[] GetImgTag(string htmlStr)
        {
            System.Text.RegularExpressions.Regex regObj = new System.Text.RegularExpressions.Regex("<img.+?>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            string[] strAry = new string[regObj.Matches(htmlStr).Count];
            int i = 0;
            foreach (Match matchItem in regObj.Matches(htmlStr))
            {
                strAry[i] = GetImgUrl(matchItem.Value);
                i++;
            }
            return strAry;
        }

        /// <summary>
        /// 获取图片URL地址
        /// </summary>
        private string GetImgUrl(string imgTagStr)
        {
            string str = "";
            System.Text.RegularExpressions.Regex regObj = new System.Text.RegularExpressions.Regex("http://.+.(?:jpg|gif|bmp|png)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (Match matchItem in regObj.Matches(imgTagStr))
            {
                str = matchItem.Value;
            }
            return str;
        }
        #endregion

        /// <summary>
        /// 下载图片到本地
        /// </summary>
        /// <param name="strHTML">HTML</param>
        /// <param name="path">路径</param>
        /// <param name="nowyymm">年月</param>
        /// <param name="nowdd">日</param>
        public string SaveUrlPics(string strHTML, string path)
        {
            string nowym = DateTime.Now.ToString("yyyy-MM");  //当前年月
            string nowdd = DateTime.Now.ToString("dd");       //当天号数
            path = path + nowym + "/" + nowdd;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            string[] imgurlAry = GetImgTag(strHTML);
            try
            {
                for (int i = 0; i < imgurlAry.Length; i++)
                {
                    string preStr = System.DateTime.Now.ToString() + "_";
                    preStr = preStr.Replace("-", "");
                    preStr = preStr.Replace(":", "");
                    preStr = preStr.Replace(" ", "");
                    WebClient wc = new WebClient();
                    wc.DownloadFile(imgurlAry[i], path + "/" + preStr + imgurlAry[i].Substring(imgurlAry[i].LastIndexOf("/") + 1));
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return strHTML;
        }
    }
}