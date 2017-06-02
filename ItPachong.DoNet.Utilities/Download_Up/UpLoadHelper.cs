// ********************************************************************
// * 项目名称：		    ItPachong.DoNet.Utilities
// * 程序集名称：	    ItPachong.DoNet.Utilities.Download_Up
// * 文件名称：		    UploadHelper.cs
// * 编写者：		    赖强
// * 编写日期：		    2017-06-02
// * 程序功能描述：
// *        Web文件上传帮助类
// *
// * 程序变更日期：
// * 程序变更者：
// * 变更说明：
// * 
// ********************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ItPachong.DoNet.Utilities.Download_Up
{
    /// <summary>
    /// Web文件上传帮助类
    /// </summary>
    public class UploadHelper
    {
        /// <summary>
        /// 转换为字节数组
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private byte[] GetBinaryFile(string fileName)
        {
            if(File.Exists(fileName))
            {
                FileStream fs = null;
                try
                {
                    fs = File.OpenRead(fileName);
                    return this.ConvertStreamToByteBuffer(fs);
                }
                catch
                {
                    return new byte[0];
                }
                finally
                {
                    fs.Close();
                }
            }
            else
            {
                return new byte[0];
            }
        }

        /// <summary>
        /// 流转换为字节数组
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private byte[] ConvertStreamToByteBuffer(Stream stream)
        {
            int bi;
            MemoryStream tempStream = new MemoryStream();
            try
            {
                while((bi = stream.ReadByte()) != -1)
                {
                    tempStream.WriteByte((byte)bi);
                }
                return tempStream.ToArray();
            }
            catch
            {
                return new byte[0];
            }
            finally
            {
                tempStream.Close();
            }
        }

        private string GetUploadFileName()
        {
            string result = "";
            DateTime time = DateTime.Now;
            result += time.Year.ToString() + FormatNum(time.Month.ToString(), 2) + FormatNum(time.Day.ToString(), 2) + FormatNum(time.Hour.ToString(), 2) +
                FormatNum(time.Minute.ToString(), 2) + FormatNum(time.Second.ToString(), 2) + FormatNum(time.Millisecond.ToString(), 3);
            return result;
        }

        private string FormatNum(string num, int bit)
        {
            int l = num.Length;
            for(int i = l; i < bit; i++)
            {
                num = "0" + num;
            }

            return num;
        }

        /// <summary>
        /// 上传图片(gif & jpeg)
        /// </summary>
        /// <param name="posPhotoUpload">图片控件</param>
        /// <param name="saveFileName">保存的文件名</param>
        /// <param name="imagePath">保存的文件路径</param>
        /// <returns></returns>
        public string UploadImage(FileUpload posPhotoUpload, string saveFileName, string imagePath)
        {
            string state = "";
            if(posPhotoUpload.HasFile)
            {
                if(posPhotoUpload.PostedFile.ContentLength / 1024 < 10240)
                {
                    string mimeType = posPhotoUpload.PostedFile.ContentType;
                    if(string.Equals(mimeType, "image/gif") || string.Equals(mimeType, "image/pjpeg"))
                    {
                        string extFileString = System.IO.Path.GetExtension(posPhotoUpload.PostedFile.FileName);
                        posPhotoUpload.PostedFile.SaveAs(HttpContext.Current.Server.MapPath(imagePath));
                    }
                    else
                    {
                        state = "上传图片类型不正确";
                    }
                }
                else
                {
                    state = "上传图片不能大于10M";
                }
            }
            else
            {
                state = "没有上传文件";
            }

            return state;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="binData">上传文件的字节数组</param>
        /// <param name="fileName">文件名</param>
        /// <param name="fileType">文件类型（即：文件后缀名）</param>
        /// <example>
        /// byte[] by = GetBinaryFile("E:\\Hello.txt");
        /// this.Upload(by, "Hello", ".txt");
        /// </example>
        public void Upload(byte[] binData, string fileName, string fileType)
        {
            FileStream fs = null;
            MemoryStream ms = new MemoryStream(binData);
            try
            {
                string savePath = HttpContext.Current.Server.MapPath("~/File/");
                if(!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                string file = savePath + fileName + fileType;
                fs = new FileStream(file, FileMode.Create);
                ms.WriteTo(fs);
            }
            finally
            {
                ms.Close();
                fs.Close();
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filePath">存储上传文件的目录路径</param>
        /// <param name="maxSize">上传的最大文件大小</param>
        /// <param name="fileType">允许上传的文件类型数组</param>
        /// <param name="targetFile"><input type=file \>元素</param>
        /// <returns></returns>
        public string UploadFile(string filePath, int maxSize, string[] fileType, HtmlInputFile targetFile)
        {
            string result = "Undefine";
            bool typeFlag = false;
            string strFileName, strNewName, strFilePath;

            if (string.IsNullOrEmpty(targetFile.PostedFile.FileName)) return "FILE_ERROR";
            strFileName = targetFile.PostedFile.FileName;
            targetFile.Accept = "*/*";
            strFilePath = filePath;
            if(!Directory.Exists(strFilePath))
            {
                Directory.CreateDirectory(strFilePath);
            }
            FileInfo myInfo = new FileInfo(strFileName);
            string strOldName = myInfo.Name;
            strNewName = strOldName.Substring(strOldName.LastIndexOf(".")).ToLower();
            if(targetFile.PostedFile.ContentLength <= maxSize)
            {
                for(int i = 0; i <= fileType.GetUpperBound(0); i++)
                {
                    if(strNewName.ToLower() == fileType[i].ToLower())
                    {
                        typeFlag = true;
                        break;
                    }
                }
                if(typeFlag)
                {
                    string strFileNameTemp = GetUploadFileName();
                    string strFilePathTemp = strFilePath;
                    float strFileSize = targetFile.PostedFile.ContentLength;
                    strOldName = strFileNameTemp + strNewName;
                    strFilePath = strFilePath + "\\" + strOldName;
                    targetFile.PostedFile.SaveAs(strFilePath);
                    result = strOldName + "|" + strFileSize;
                    targetFile.Dispose();
                }
                else
                {
                    return "TYPE_ERROR";
                }
            }
            else
            {
                return "SIZE_ERROR";
            }

            return result;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filePath">保存文件地址</param>
        /// <param name="maxSize">文件最大大小</param>
        /// <param name="fileType">文件后缀类型</param>
        /// <param name="TargetFile">控件名</param>
        /// <param name="saveFileName">保存后的文件名和地址</param>
        /// <param name="fileSize">文件大小</param>
        /// <returns></returns>
        public string UploadFile(string filePath, int maxSize, string[] fileType, System.Web.UI.HtmlControls.HtmlInputFile TargetFile, out string saveFileName, out int fileSize)
        {
            saveFileName = "";
            fileSize = 0;

            string Result = "";
            bool typeFlag = false;
            string FilePath = filePath;
            int MaxSize = maxSize;
            string strFileName, strNewName, strFilePath;
            if (TargetFile.PostedFile.FileName == "")
            {
                return "请选择上传的文件";
            }
            strFileName = TargetFile.PostedFile.FileName;
            TargetFile.Accept = "*/*";
            strFilePath = FilePath;
            if (Directory.Exists(strFilePath) == false)
            {
                Directory.CreateDirectory(strFilePath);
            }
            FileInfo myInfo = new FileInfo(strFileName);
            string strOldName = myInfo.Name;
            strNewName = strOldName.Substring(strOldName.LastIndexOf("."));
            strNewName = strNewName.ToLower();
            if (TargetFile.PostedFile.ContentLength <= MaxSize)
            {
                string strFileNameTemp = GetUploadFileName();
                string strFilePathTemp = strFilePath;
                strOldName = strFileNameTemp + strNewName;
                strFilePath = strFilePath + "\\" + strOldName;

                fileSize = TargetFile.PostedFile.ContentLength / 1024;
                saveFileName = strFilePath.Substring(strFilePath.IndexOf("FileUpload\\"));
                TargetFile.PostedFile.SaveAs(strFilePath);
                TargetFile.Dispose();
            }
            else
            {
                return "上传文件超出指定的大小";
            }
            return (Result);
        }

        public string UploadFile(string filePath, int maxSize, string[] fileType, string filename, System.Web.UI.HtmlControls.HtmlInputFile TargetFile)
        {
            string Result = "UnDefine";
            bool typeFlag = false;
            string FilePath = filePath;
            int MaxSize = maxSize;
            string strFileName, strNewName, strFilePath;
            if (TargetFile.PostedFile.FileName == "")
            {
                return "FILE_ERR";
            }
            strFileName = TargetFile.PostedFile.FileName;
            TargetFile.Accept = "*/*";
            strFilePath = FilePath;
            if (Directory.Exists(strFilePath) == false)
            {
                Directory.CreateDirectory(strFilePath);
            }
            FileInfo myInfo = new FileInfo(strFileName);
            string strOldName = myInfo.Name;
            strNewName = strOldName.Substring(strOldName.Length - 3, 3);
            strNewName = strNewName.ToLower();
            if (TargetFile.PostedFile.ContentLength <= MaxSize)
            {
                for (int i = 0; i <= fileType.GetUpperBound(0); i++)
                {
                    if (strNewName.ToLower() == fileType[i].ToString()) { typeFlag = true; break; }
                }
                if (typeFlag)
                {
                    string strFileNameTemp = filename;
                    string strFilePathTemp = strFilePath;
                    strOldName = strFileNameTemp + "." + strNewName;
                    strFilePath = strFilePath + "\\" + strOldName;
                    TargetFile.PostedFile.SaveAs(strFilePath);
                    Result = strOldName;
                    TargetFile.Dispose();
                }
                else
                {
                    return "TYPE_ERR";
                }
            }
            else
            {
                return "SIZE_ERR";
            }
            return (Result);
        }

    }
}