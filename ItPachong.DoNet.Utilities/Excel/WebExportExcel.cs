// ********************************************************************
// * 项目名称：		    ItPachong.DoNet.Utilities
// * 程序集名称：	    ItPachong.DoNet.Utilities.Excel
// * 文件名称：		    WebExportExcel.cs
// * 编写者：		    赖强
// * 编写日期：		    2017-06-06
// * 程序功能描述：
// *        Web网页相关类型导出为Excel文件
// *            1、将整个网页导出到Excel
// *            2、将GridView数据导出到Excel
// *
// * 程序变更日期：
// * 程序变更者：
// * 变更说明：
// * 
// ********************************************************************
using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ItPachong.DoNet.Utilities.Excel
{
    /// <summary>
    /// Web网页相关类型导出为Excel文件
    /// </summary>
    public class WebExportExcel
    {
        /// <summary>
        /// 将网页导出到Excel（保存为2003格式）
        /// </summary>
        /// <param name="strContent">网页内容</param>
        /// <param name="fileName">保存Excel的绝对路径</param>
        public void WebPageExportToExcel(string strContent, string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("Excel存储路径为空.");
            fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmsslll");
            if (System.IO.File.Exists(fileName))
            {
                try
                {
                    System.IO.File.Delete(fileName);
                }
                catch
                {
                    throw new Exception("该文件正在使用中，关闭文件或重新命名导出文件再试!");
                }
            }

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "gb2312";
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            //增加头信息，为“文件下载/另存为”对话框指定默认文件名
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ".xls");
            //把文件流发送到客户端
            HttpContext.Current.Response.Write("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            HttpContext.Current.Response.Write(strContent);
            HttpContext.Current.Response.Write("</body></html>");
        }

        /// <summary>
        /// 将GridView数据导出到Excel（保存为2003格式）
        /// </summary>
        /// <param name="obj"></param>
        public void GridViewExportToExcel(GridView obj)
        {
            try
            {
                string style = "";
                if (obj.Rows.Count > 0)
                {
                    style = @"<style> .text { mso-number-format:\@; } </script> ";
                }
                else
                {
                    style = "no data.";
                }

                HttpContext.Current.Response.ClearContent();
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmsslll");
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=ExportData" + fileName + ".xls");
                HttpContext.Current.Response.ContentType = "application/ms-excel";
                HttpContext.Current.Response.Charset = "GB2312";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                obj.RenderControl(htw);
                HttpContext.Current.Response.Write(style);
                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}