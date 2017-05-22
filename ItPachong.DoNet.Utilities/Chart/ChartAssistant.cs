// ********************************************************************
// * 项目名称：		    ItPachong.DoNet.Utilities
// * 程序集名称：	    ItPachong.DoNet.Utilities.Chart
// * 文件名称：		    ChartAssistant.cs
// * 编写者：		    赖强
// * 编写日期：		    2017-05-04
// * 程序功能描述：
// *        图表助手类：生成图标说明标签等
// *
// * 程序变更日期：
// * 程序变更者：
// * 变更说明：
// * 
// ********************************************************************
using ItPachong.DoNet.Utilities.Config;
using System.Text;

namespace ItPachong.DoNet.Utilities.Chart
{
    /// <summary>
    /// 图表助手类
    /// </summary>
    public sealed class ChartAssistant
    {
        #region 创建显示图表的标签
        /// <summary>
        /// 创建显示图标的标签（flash加点击）
        /// </summary>
        /// <param name="adid"></param>
        /// <param name="filename"></param>
        /// <param name="desc"></param>
        /// <param name="fileType"></param>
        /// <param name="linkURL"></param>
        /// <param name="width"></param>
        /// <param name="high"></param>
        /// <returns></returns>
        public static string CreateTag(string adid, string filename, string desc, string fileType, string linkURL, int width, int high)
        {
            StringBuilder tagStr = new StringBuilder();
            switch(fileType)
            {
                case "image/gif":
                case "image/bmp":
                case "image/pjpeg":
                    {
                        if((linkURL.Trim() != "") && (linkURL.Trim() != "http://"))     //非空
                        {
                            tagStr.Append("<a href=\"");
                            tagStr.Append(ConfigHelper.GetConfigString("URL") + "/FormAdHit.aspx?ADID=" + adid);
                            tagStr.Append("&LinkURL=" + linkURL.Replace("&", "$$$"));
                            tagStr.Append("\"");
                            tagStr.Append(" target=\"_blank\">");
                        }
                        tagStr.Append(" <IMG alt=\"" + desc + "\"");
                        tagStr.Append(" src=\"" + filename + "\"");
                        tagStr.Append(" width=\"" + width + "\" height=\"" + high + "\" ");
                        tagStr.Append(" border=\"0\">");
                        if ((linkURL.Trim() != "") && (linkURL.Trim() != "http://"))
                        {
                            tagStr.Append("</a>");
                        }
                        break;
                    }
                case "application/x-shockwave-flash":
                    {
                        //					TagStr.Append("<object ");
                        ////					TagStr.Append(" width="+Width+" height="+High+" ");
                        //					TagStr.Append("  classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\"  ");
                        //					TagStr.Append(" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,0,0\"> ");
                        ////					TagStr.Append(" <param name=\"movie\" value=\""+filename+"?clickthru=");
                        ////					TagStr.Append("FormAdHit.aspx?ADID="+ADID);
                        ////					TagStr.Append("_LinkURL="+LinkURL);
                        ////					TagStr.Append("\"> ");					
                        //					TagStr.Append(" <param name=\"wmode\" value=\"opaque\"> ");
                        //					TagStr.Append(" <param name=\"quality\" value=\"autohigh\"> ");
                        //					
                        //					TagStr.Append(" <embed  ");
                        //					TagStr.Append(" width="+Width+" height="+High+"  ");
                        //					TagStr.Append(" src=\""+filename+"?clickthru=");
                        //					TagStr.Append("FormAdHit.aspx?ADID="+ADID);
                        //					if((LinkURL.Trim()!="")&&(LinkURL.Trim()!="http://"))
                        //					{
                        //						TagStr.Append("_LinkURL="+LinkURL);
                        //					}
                        //					TagStr.Append("\"  ");	
                        //					TagStr.Append(" quality=\"high\" wmode=\"opaque\" type=\"application/x-shockwave-flash\"  ");
                        //					TagStr.Append(" plugspace=\"http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash\"> ");
                        //					TagStr.Append(" </embed></object> ");

                        tagStr.Append(" <embed ");
                        tagStr.Append(" src=\"" + filename + "\" ");
                        //					TagStr.Append(" src=\""+filename+"?clickthru=");
                        //					TagStr.Append("FormAdHit.aspx?ADID="+ADID);
                        //					if((LinkURL.Trim()!="")&&(LinkURL.Trim()!="http://"))
                        //					{
                        //						TagStr.Append("_LinkURL="+LinkURL);
                        //					}
                        //					TagStr.Append("\"  ");	
                        tagStr.Append(" width=" + width + " height=" + high + "  ");
                        tagStr.Append(" quality=\"high\" ");
                        tagStr.Append(" ></embed>");

                    }

                    break;

                case "video/x-ms-wmv":
                case "video/mpeg":
                case "video/x-ms-asf":
                case "video/avi":
                case "audio/mpeg":
                case "audio/mid":
                case "audio/wav":
                case "audio/x-ms-wma":
                    tagStr.Append("<embed");
                    tagStr.Append(" src=\"" + filename + "\" border=\"0\" ");
                    tagStr.Append(" width=\"" + width + "\" height=\"" + high + "\"");
                    tagStr.Append(" autoStart=\"1\" playCount=\"0\" enableContextMenu=\"0\"");
                    tagStr.Append(" type=\"application/x-mplayer2\"></embed>");
                    break;

                default:
                    //TagStr.Append("不允许该格式文件显示！");
                    break;
            }

            return tagStr.ToString();
        }

        /// <summary>
        /// 创建显示图像的标签(flash无点击)
        /// </summary>		
        public static string CreateTag2(string ADID, string filename, string desc, string FileType, string LinkURL, int Width, int High)
        {
            StringBuilder TagStr = new StringBuilder();
            switch (FileType)
            {
                case "image/gif":
                case "image/bmp":
                case "image/pjpeg":
                    {
                        TagStr.Append("<a href=\"");
                        TagStr.Append(ConfigHelper.GetConfigString("URL") + "\\FormAdHit.aspx?ADID=" + ADID);
                        TagStr.Append("&LinkURL=" + LinkURL);
                        TagStr.Append("\"");
                        TagStr.Append(" target=\"_blank\">");
                        TagStr.Append(" <IMG alt=\"" + desc + "\"");
                        TagStr.Append(" src=\"" + filename + "\"");
                        TagStr.Append(" width=\"" + Width + "\" height=\"" + High + "\" ");
                        TagStr.Append(" border=\"0\">");
                        TagStr.Append("</a>");
                        break;
                    }

                case "application/x-shockwave-flash":
                    {
                        //					TagStr.Append("<a href=\"");
                        //					TagStr.Append(LinkURL);
                        //					TagStr.Append("FormAdHit.aspx?ADID="+ADID);
                        //					TagStr.Append("&LinkURL="+LinkURL);
                        //					TagStr.Append("\"");
                        //					TagStr.Append(" target=\"_blank\">");

                        TagStr.Append(" <embed src=\"" + filename + "\" ");
                        TagStr.Append(" quality=\"high\" bgcolor=\"#f5f5f5\" ");
                        TagStr.Append(" ></embed>");

                        //					TagStr.Append("</a>");
                    }

                    break;

                case "video/x-ms-wmv":
                case "video/mpeg":
                case "video/x-ms-asf":
                case "video/avi":
                case "audio/mpeg":
                case "audio/mid":
                case "audio/wav":
                case "audio/x-ms-wma":

                    //					TagStr.Append("<a href=\"");
                    //					TagStr.Append(LinkURL);
                    //					TagStr.Append("FormAdHit.aspx?ADID="+ADID);
                    //					TagStr.Append("&LinkURL="+LinkURL);
                    //					TagStr.Append("\"");
                    //					TagStr.Append(" target=\"_blank\">");
                    TagStr.Append("<embed");
                    TagStr.Append(" src=\"" + filename + "\" border=\"0\" ");
                    TagStr.Append(" width=\"" + Width + "\" height=\"" + High + "\"");
                    TagStr.Append(" autoStart=\"1\" playCount=\"0\" enableContextMenu=\"0\"");
                    TagStr.Append(" type=\"application/x-mplayer2\"></embed>");
                    //					TagStr.Append("</a>");


                    break;

                default:
                    //					TagStr.Append("不允许该格式文件显示！");
                    break;
            }

            return TagStr.ToString();

        }


        /// <summary>
        /// 创建显示图像的标签(重载)，无宽高限制，(flash加点击)
        /// </summary>
        public static string CreateTag(string ADID, string filename, string desc, string FileType, string LinkURL)
        {
            StringBuilder TagStr = new StringBuilder();
            switch (FileType)
            {
                case "image/gif":
                case "image/bmp":
                case "image/pjpeg":
                    {
                        TagStr.Append("<a href=\"");
                        TagStr.Append(ConfigHelper.GetConfigString("URL") + "\\FormAdHit.aspx?ADID=" + ADID);
                        TagStr.Append("&LinkURL=" + LinkURL);
                        TagStr.Append("\"");
                        TagStr.Append(" target=\"_blank\">");
                        TagStr.Append(" <IMG alt=\"" + desc + "\"");
                        TagStr.Append(" src=\"" + filename + "\"");
                        //					TagStr.Append(" width=\""+Width+"\" height=\""+High+"\" ");
                        TagStr.Append(" border=\"0\">");
                        TagStr.Append("</a>");
                        break;
                    }

                case "application/x-shockwave-flash":
                    {
                        TagStr.Append("<object ");
                        //					TagStr.Append(" width="+Width+" height="+High+" ");
                        TagStr.Append("  classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\"  ");
                        TagStr.Append(" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,0,0\"> ");
                        //					TagStr.Append(" <param name=\"movie\" value=\""+filename+"?clickthru=");
                        //					TagStr.Append("FormAdHit.aspx?ADID="+ADID);
                        //					TagStr.Append("_LinkURL="+LinkURL);
                        //					TagStr.Append("\"> ");					
                        TagStr.Append(" <param name=\"wmode\" value=\"opaque\"> ");
                        TagStr.Append(" <param name=\"quality\" value=\"autohigh\"> ");
                        TagStr.Append(" <embed  ");
                        //					TagStr.Append(" width="+Width+" height="+High+"  ");
                        TagStr.Append(" src=\"" + filename + "?clickthru=");
                        TagStr.Append(ConfigHelper.GetConfigString("URL") + "\\FormAdHit.aspx?ADID=" + ADID);
                        TagStr.Append("_LinkURL=" + LinkURL);
                        TagStr.Append("\"  ");
                        TagStr.Append(" quality=\"autohigh\" wmode=\"opaque\" type=\"application/x-shockwave-flash\"  ");
                        TagStr.Append(" plugspace=\"http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash\"> ");
                        TagStr.Append(" </embed></object> ");
                    }

                    break;

                case "video/x-ms-wmv":
                case "video/mpeg":
                case "video/x-ms-asf":
                case "video/avi":
                case "audio/mpeg":
                case "audio/mid":
                case "audio/wav":
                case "audio/x-ms-wma":
                    TagStr.Append("<embed");
                    TagStr.Append(" src=\"" + filename + "\" border=\"0\" ");
                    //					TagStr.Append(" width=\""+Width+"\" height=\""+High+"\"");	
                    TagStr.Append(" autoStart=\"1\" playCount=\"0\" enableContextMenu=\"0\"");
                    TagStr.Append(" type=\"application/x-mplayer2\"></embed>");


                    break;

                default:
                    break;
            }

            return TagStr.ToString();

        }


        /// <summary>
        /// 创建显示图像的标签(重载)，无宽高限制，(flash无点击)
        /// </summary>
        public static string CreateTag2(string ADID, string filename, string desc, string FileType, string LinkURL)
        {
            StringBuilder TagStr = new StringBuilder();
            switch (FileType)
            {
                case "image/gif":
                case "image/bmp":
                case "image/pjpeg":
                    {
                        TagStr.Append("<a href=\"");
                        TagStr.Append("FormAdHit.aspx?ADID=" + ADID);
                        TagStr.Append("&LinkURL=" + LinkURL);
                        TagStr.Append("\"");
                        TagStr.Append(" target=\"_blank\">");
                        TagStr.Append(" <IMG alt=\"" + desc + "\"");
                        TagStr.Append(" src=\"" + filename + "\"");
                        //					TagStr.Append(" width=\""+Width+"\" height=\""+High+"\" ");
                        TagStr.Append(" border=\"0\">");
                        TagStr.Append("</a>");
                        break;
                    }

                case "application/x-shockwave-flash":
                    {
                        TagStr.Append(" <embed src=\"" + filename + "\" ");
                        TagStr.Append(" quality=\"high\" bgcolor=\"#f5f5f5\" ");
                        TagStr.Append(" ></embed>");
                    }

                    break;

                case "video/x-ms-wmv":
                case "video/mpeg":
                case "video/x-ms-asf":
                case "video/avi":
                case "audio/mpeg":
                case "audio/mid":
                case "audio/wav":
                case "audio/x-ms-wma":
                    TagStr.Append("<embed");
                    TagStr.Append(" src=\"" + filename + "\" border=\"0\" ");
                    //					TagStr.Append(" width=\""+Width+"\" height=\""+High+"\"");	
                    TagStr.Append(" autoStart=\"1\" playCount=\"0\" enableContextMenu=\"0\"");
                    TagStr.Append(" type=\"application/x-mplayer2\"></embed>");


                    break;

                default:
                    break;
            }

            return TagStr.ToString();

        }
        #endregion

        #region
        /// <summary>
        /// 创建显示图像的标签
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="desc"></param>
        /// <param name="FileType"></param>
        /// <param name="LinkURL"></param>
        /// <param name="Width"></param>
        /// <param name="High"></param>
        /// <returns></returns>
        public static string CreateTagOld(string filename, string desc, string FileType, string LinkURL, int Width, int High)
        {
            StringBuilder TagStr = new StringBuilder();
            switch (FileType)
            {
                case "image/gif":
                case "image/bmp":
                case "image/pjpeg":
                    {
                        TagStr.Append("<a href=\"");
                        TagStr.Append(LinkURL);
                        TagStr.Append("\"");
                        TagStr.Append(" target=\"_blank\">");
                        TagStr.Append(" <IMG alt=\"" + desc + "\"");
                        TagStr.Append(" src=\"" + filename + "\"");
                        TagStr.Append(" width=\"" + Width + "\" height=\"" + High + "\" border=\"0\">");
                        TagStr.Append("</a>");
                        break;
                    }

                case "application/x-shockwave-flash":
                    {
                        TagStr.Append("<a href=\"");
                        TagStr.Append(LinkURL);
                        TagStr.Append("\"");
                        TagStr.Append(" target=\"_blank\">");
                        TagStr.Append(" <embed src=\"" + filename + "\" ");
                        TagStr.Append(" quality=\"high\" bgcolor=\"#f5f5f5\"");
                        TagStr.Append(" ></embed>");

                        //					TagStr.Append(" <embed src=\""+filename+"\" ");		
                        //					TagStr.Append("pluginspage=\"http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash\"");					
                        //					TagStr.Append(" type=\"application/x-shockwave-flash\"");
                        //					TagStr.Append(" width=\""+Width+"\" height=\""+High+"\"");
                        //					TagStr.Append(" play=\"true\" loop=\"true\" quality=\"high\" scale=\"showall\" ");					
                        //					TagStr.Append(" ></embed>");

                        TagStr.Append("</a>");
                    }

                    break;

                case "video/x-ms-wmv":
                case "video/mpeg":
                case "video/x-ms-asf":
                case "video/avi":
                case "audio/mpeg":
                case "audio/mid":
                case "audio/wav":
                case "audio/x-ms-wma":
                    //					TagStr.Append("<a href=\"");
                    //					TagStr.Append(LinkURL);
                    //					TagStr.Append("\"");
                    //					TagStr.Append(" target=\"_blank\">");
                    //					TagStr.Append("<OBJECT  classid=\"clsid:6BF52A52-394A-11D3-B153-00C04F79FAA6\" VIEWASTEXT>");
                    //					TagStr.Append("<PARAM NAME=\"URL\" VALUE=\""+filename+"\">");
                    //					TagStr.Append("<PARAM NAME=\"autoStart\" VALUE=\"1\">");
                    //					TagStr.Append("<PARAM NAME=\"enableContextMenu\" VALUE=\"0\" ></OBJECT>");	
                    //					TagStr.Append("</a>");

                    TagStr.Append("<a href=\"");
                    TagStr.Append(LinkURL);
                    TagStr.Append("\"");
                    TagStr.Append(" target=\"_blank\">");
                    TagStr.Append("<embed");
                    TagStr.Append(" src=\"" + filename + "\" border=\"0\" width=\"" + Width + "\" height=\"" + High + "\"");
                    TagStr.Append(" autoStart=\"1\" playCount=\"0\" enableContextMenu=\"0\"");
                    TagStr.Append(" type=\"application/x-mplayer2\"></embed>");
                    TagStr.Append("</a>");


                    break;

                default://其他类型作为附件链接下载
                    TagStr.Append("不允许该格式文件显示！");
                    break;
            }

            return TagStr.ToString();

        }

        #endregion

    }
}