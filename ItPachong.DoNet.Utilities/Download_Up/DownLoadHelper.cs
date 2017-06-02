// ********************************************************************
// * 项目名称：		    ItPachong.DoNet.Utilities
// * 程序集名称：	    ItPachong.DoNet.Utilities.Download_Up
// * 文件名称：		    DownLoadHelper.cs
// * 编写者：		    赖强
// * 编写日期：		    2017-05-31
// * 程序功能描述：
// *        Web文件下载帮助类
// *            1）输出服务器硬盘文件，提供下载 支持大文件、续传、速度限制、资源占用小
// *            2）普通文件下载
// *            3）分块文件下载
// *
// * 程序变更日期：
// * 程序变更者：
// * 变更说明：
// * 
// ********************************************************************

using System;
using System.IO;
using System.Threading;
using System.Web;

namespace ItPachong.DoNet.Utilities.Download_Up
{
    /// <summary>
    /// Web文件下载帮助类（断点续传）
    /// </summary>
    public class DownloadHelper
    {
        #region 输出服务器硬盘文件，提供下载 支持大文件、续传、速度限制、资源占用小

        /// <summary>
        /// 下载硬盘文件，提供下载 支持大文件、续传、速度限制、资源占用小
        /// </summary>
        /// <param name="_request">Request对象</param>
        /// <param name="_response">Response对象</param>
        /// <param name="_fileName">下载文件名</param>
        /// <param name="_fullPath">带文件名的文件绝对路径</param>
        /// <param name="_speed">每秒允许下载的字节数</param>
        /// <returns>返回是否成功</returns>
        public bool ResponseFile(HttpRequest _request, HttpResponse _response, string _fileName, string _fullPath, long _speed)
        {
            try
            {
                FileStream myFile = new FileStream(_fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(myFile);
                try
                {
                    _response.AddHeader("Accept-Ranges", "bytes");
                    _response.Buffer = false;

                    long fileLength = myFile.Length;
                    long startBytes = 0;
                    int pack = 10240;       //10k bytes;
                    int sleep = (int)Math.Floor((double)(1000 * pack / _speed)) + 1; 
                    
                    //续传设置
                    if(_request.Headers["Range"] != null)
                    {
                        _response.StatusCode = 206;
                        string[] range = _request.Headers["Range"].Split(new char[] { '=', '-' });
                        startBytes = System.Convert.ToInt64(range[1]);
                    }
                    _response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    if(startBytes != 0)
                    {
                        _response.AddHeader("Content-Length", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                    }
                    _response.AddHeader("Connection", "Keep-Alive");
                    _response.ContentType = "application/octet-stream";
                    _response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(_fileName, System.Text.Encoding.UTF8));

                    //“读”设置
                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    int maxCount = (int)Math.Floor((double)((fileLength - startBytes) / pack)) + 1;
                    for(int i = 0; i < maxCount; i++)
                    {
                        if(_response.IsClientConnected)
                        {
                            _response.BinaryWrite(br.ReadBytes(pack));
                            Thread.Sleep(sleep);
                        }
                        else
                        {
                            i = maxCount;
                        }
                    }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    br.Close();
                    myFile.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Web 文件下载
        /// <summary>
        /// 获取Web下载文件在服务端的绝对路径,包括文件扩展名
        /// </summary>
        /// <param name="fileName">相对路径</param>
        /// <returns></returns>
        private string FileNameExtension(string fileName)
        {
            return Path.GetExtension(MapPathFile(fileName));
        }

        /// <summary>
        /// 获取服务端下载文件的绝对路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string MapPathFile(string fileName)
        {
            return HttpContext.Current.Server.MapPath(fileName);
        }

        /// <summary>
        /// 普通下载
        /// </summary>
        /// <param name="fileName">文件虚拟路径</param>
        public void DownLoad(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;
            string destFileName = MapPathFile(fileName);
            if(File.Exists(destFileName))
            {
                FileInfo fi = new FileInfo(destFileName);
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.Buffer = false;
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachement;filename=" + HttpUtility.UrlEncode(Path.GetFileName(destFileName), System.Text.Encoding.UTF8));
                HttpContext.Current.Response.AppendHeader("Content-Length", fi.Length.ToString());
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.WriteFile(destFileName);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// 分块下载
        /// </summary>
        /// <param name="fileName">文件虚拟路径</param>
        /// <param name="blockSize">块的大小（默认：204800B|200K）</param>
        public void Download(string fileName, long blockSize = 204800)
        {
            if (string.IsNullOrEmpty(fileName)) return;
            string filePath = MapPathFile(fileName);
            long chunkSize = blockSize;             //指定快大小
            byte[] buffer = new byte[chunkSize];    //建立一个指定块大小的缓冲区
            long dataToRead = 0;                    //已读的字节数
            FileStream stream = null;

            try
            {
                if (!File.Exists(filePath)) return;
                //打开文件
                stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);      //读取文件到文件流
                dataToRead = stream.Length;

                //添加Http头 
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachement;filename=" + HttpUtility.UrlEncode(Path.GetFileName(filePath)));
                HttpContext.Current.Response.AddHeader("Content-Length", dataToRead.ToString());

                while(dataToRead > 0)
                {
                    if(HttpContext.Current.Response.IsClientConnected)
                    {
                        int length = stream.Read(buffer, 0, System.Convert.ToInt32(chunkSize));
                        HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);
                        HttpContext.Current.Response.Flush();
                        HttpContext.Current.Response.Clear();
                        dataToRead -= length;
                    }
                    else
                    {
                        dataToRead = -1;        //防止客户端失去连接
                    }
                }
            }
            catch(Exception ex)
            {
                HttpContext.Current.Response.Write("Error:" + ex.Message);
            }
            finally
            {
                if (stream != null) stream.Close();
                HttpContext.Current.Response.Close();
            }
        }
        #endregion
    }
}