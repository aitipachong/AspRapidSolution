// ********************************************************************
// * 项目名称：		    ItPachong.DoNet.Utilities
// * 程序集名称：	    ItPachong.DoNet.Utilities.FTP
// * 文件名称：		    FTPClient.cs
// * 编写者：		    赖强
// * 编写日期：		    2017-06-08
// * 程序功能描述：
// *        FTP客户端帮助类
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
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ItPachong.DoNet.Utilities.FTP
{
    /// <summary>
    /// FTP客户端帮助类
    /// </summary>
    public class FTPClient
    {
        /// <summary>
        /// 闭锁对象
        /// </summary>
        public static object obj = new object();

        /// <summary>
        /// 传输模式枚举
        /// </summary>
        public enum TransferType
        {
            /// <summary>
            /// 二进制
            /// </summary>
            Binary,
            /// <summary>
            /// ASCII码
            /// </summary>
            ASCII
        };

        #region 私有变量
        private string strMsg;                  //服务器返回的应答信息（包含应答码）
        private string strReply;                //服务器返回的应答信息（包含应答码）
        private int iReplyCode;                 //服务器返回的应答码
        private Socket socketControl;           //进行控制连接Socket
        private TransferType trType;            //传输模式
        private static int BLOCK_SIZE = 512;    //接收和发送数据的缓冲区大小
        Encoding ASCII = Encoding.ASCII;        //编码方式
        Byte[] buffer = new Byte[BLOCK_SIZE];   //缓冲区数组
        #endregion

        #region 属性
        /// <summary>
        /// FTP服务器IP地址
        /// </summary>
        public string RemoteHost { get; set; }
        /// <summary>
        /// FTP服务器端口号
        /// </summary>
        public int RemotePort { get; set; }
        /// <summary>
        /// FTP服务端根目录
        /// </summary>
        public string RemotePath { get; set; }
        /// <summary>
        /// FTP登录账户
        /// </summary>
        public string RemoteUser { get; set; }
        /// <summary>
        /// FTP登录密码
        /// </summary>
        public string RemotePass { get; set; }
        /// <summary>
        /// 是否登录FTP服务器
        /// </summary>
        public bool Connected { get; set; }
        #endregion

        #region 构建函数
        public FTPClient()
        {
            this.RemoteHost = "";
            this.RemotePath = "";
            this.RemoteUser = "";
            this.RemotePass = "";
            this.RemotePort = 21;
            this.Connected = false;
        }

        public FTPClient(string remoteHost, string remotePath, string remoteUser, string remotePass, int remotePort)
        {
            this.RemoteHost = remoteHost;
            this.RemotePath = remotePath;
            this.RemoteUser = remoteUser;
            this.RemotePass = remotePass;
            this.RemotePort = remotePort;
            Connect();
        }
        #endregion

        #region 建立连接/关闭连接
        /// <summary>
        /// 建立FTP连接
        /// </summary>
        public void Connect()
        {
            lock(obj)
            {
                socketControl = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(this.RemoteHost), this.RemotePort);
                try
                {
                    socketControl.Connect(ep);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            //获取应答信息
            ReadReply();
            //判断是否连接成功
            if(iReplyCode != 220)
            {
                DisConnect();       //关闭连接
                throw new IOException(strReply.Substring(4));
            }
            //发送命令：用户
            SendCommand("USER" + this.RemoteUser);
            if (!(iReplyCode == 331 || iReplyCode == 230))
            {
                CloseSocketConnect();
                throw new IOException(strReply.Substring(4));
            }
            if(iReplyCode != 230)
            {
                SendCommand("PASS" + this.RemotePass);
                if(!(iReplyCode == 230 || iReplyCode == 202))
                {
                    CloseSocketConnect();
                    throw new IOException(strReply.Substring(4));
                }
            }
            this.Connected = true;
            ChDir(this.RemotePath);
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void DisConnect()
        {
            if(socketControl != null)
            {
                SendCommand("QUIT");
            }
            CloseSocketConnect();
        }
        #endregion

        #region 传输模式
        /// <summary>
        /// 设置传输模式
        /// </summary>
        /// <param name="ttType">传输模式</param>
        public void SetTransferType(TransferType ttType)
        {
            if (ttType == TransferType.Binary)
            {
                SendCommand("TYPE I");//binary类型传输
            }
            else
            {
                SendCommand("TYPE A");//ASCII类型传输
            }
            if (iReplyCode != 200)
            {
                throw new IOException(strReply.Substring(4));
            }
            else
            {
                trType = ttType;
            }
        }

        /// <summary>
        /// 获得传输模式
        /// </summary>
        /// <returns>传输模式</returns>
        public TransferType GetTransferType()
        {
            return trType;
        }
        #endregion

        #region 文件操作
        /// <summary>
        /// 获得文件列表
        /// </summary>
        /// <param name="strMask">文件名的匹配字符串</param>
        public string[] Dir(string strMask)
        {
            if (!Connected)
            {
                Connect();
            }
            Socket socketData = CreateDataSocket();
            SendCommand("NLST " + strMask);
            if (!(iReplyCode == 150 || iReplyCode == 125 || iReplyCode == 226))
            {
                throw new IOException(strReply.Substring(4));
            }
            strMsg = "";
            Thread.Sleep(2000);
            while (true)
            {
                int iBytes = socketData.Receive(buffer, buffer.Length, 0);
                strMsg += ASCII.GetString(buffer, 0, iBytes);
                if (iBytes < buffer.Length)
                {
                    break;
                }
            }
            char[] seperator = { '\n' };
            string[] strsFileList = strMsg.Split(seperator);
            socketData.Close(); //数据socket关闭时也会有返回码
            if (iReplyCode != 226)
            {
                ReadReply();
                if (iReplyCode != 226)
                {

                    throw new IOException(strReply.Substring(4));
                }
            }
            return strsFileList;
        }

        /// <summary>
        /// 存储新的Guid
        /// </summary>
        /// <param name="strFileName"></param>
        /// <param name="strGuid"></param>
        public void newPutByGuid(string strFileName, string strGuid)
        {
            if (!Connected)
            {
                Connect();
            }
            string str = strFileName.Substring(0, strFileName.LastIndexOf("\\"));
            string strTypeName = strFileName.Substring(strFileName.LastIndexOf("."));
            strGuid = str + "\\" + strGuid;
            Socket socketData = CreateDataSocket();
            SendCommand("STOR " + Path.GetFileName(strGuid));
            if (!(iReplyCode == 125 || iReplyCode == 150))
            {
                throw new IOException(strReply.Substring(4));
            }
            FileStream input = new FileStream(strGuid, FileMode.Open);
            input.Flush();
            int iBytes = 0;
            while ((iBytes = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                socketData.Send(buffer, iBytes, 0);
            }
            input.Close();
            if (socketData.Connected)
            {
                socketData.Close();
            }
            if (!(iReplyCode == 226 || iReplyCode == 250))
            {
                ReadReply();
                if (!(iReplyCode == 226 || iReplyCode == 250))
                {
                    throw new IOException(strReply.Substring(4));
                }
            }
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="strFileName">文件名</param>
        /// <returns>文件大小</returns>
        public long GetFileSize(string strFileName)
        {
            if (!Connected)
            {
                Connect();
            }
            SendCommand("SIZE " + Path.GetFileName(strFileName));
            long lSize = 0;
            if (iReplyCode == 213)
            {
                lSize = Int64.Parse(strReply.Substring(4));
            }
            else
            {
                throw new IOException(strReply.Substring(4));
            }
            return lSize;
        }


        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="strFileName">文件名</param>
        /// <returns>文件大小</returns>
        public string GetFileInfo(string strFileName)
        {
            if (!Connected)
            {
                Connect();
            }
            Socket socketData = CreateDataSocket();
            SendCommand("LIST " + strFileName);
            string strResult = "";
            if (!(iReplyCode == 150 || iReplyCode == 125
                || iReplyCode == 226 || iReplyCode == 250))
            {
                throw new IOException(strReply.Substring(4));
            }
            byte[] b = new byte[512];
            MemoryStream ms = new MemoryStream();

            while (true)
            {
                int iBytes = socketData.Receive(b, b.Length, 0);
                ms.Write(b, 0, iBytes);
                if (iBytes <= 0)
                {

                    break;
                }
            }
            byte[] bt = ms.GetBuffer();
            strResult = System.Text.Encoding.ASCII.GetString(bt);
            ms.Close();
            return strResult;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="strFileName">待删除文件名</param>
        public void Delete(string strFileName)
        {
            if (!Connected)
            {
                Connect();
            }
            SendCommand("DELE " + strFileName);
            if (iReplyCode != 250)
            {
                throw new IOException(strReply.Substring(4));
            }
        }

        /// <summary>
        /// 重命名(如果新文件名与已有文件重名,将覆盖已有文件)
        /// </summary>
        /// <param name="strOldFileName">旧文件名</param>
        /// <param name="strNewFileName">新文件名</param>
        public void Rename(string strOldFileName, string strNewFileName)
        {
            if (!Connected)
            {
                Connect();
            }
            SendCommand("RNFR " + strOldFileName);
            if (iReplyCode != 350)
            {
                throw new IOException(strReply.Substring(4));
            }
            //  如果新文件名与原有文件重名,将覆盖原有文件
            SendCommand("RNTO " + strNewFileName);
            if (iReplyCode != 250)
            {
                throw new IOException(strReply.Substring(4));
            }
        }
        #endregion

        #region 上传和下载
        /// <summary>
        /// 下载一批文件
        /// </summary>
        /// <param name="strFileNameMask">文件名的匹配字符串</param>
        /// <param name="strFolder">本地目录(不得以\结束)</param>
        public void Get(string strFileNameMask, string strFolder)
        {
            if (!Connected)
            {
                Connect();
            }
            string[] strFiles = Dir(strFileNameMask);
            foreach (string strFile in strFiles)
            {
                if (!strFile.Equals(""))//一般来说strFiles的最后一个元素可能是空字符串
                {
                    Get(strFile, strFolder, strFile);
                }
            }
        }

        /// <summary>
        /// 下载一个文件
        /// </summary>
        /// <param name="strRemoteFileName">要下载的文件名</param>
        /// <param name="strFolder">本地目录(不得以\结束)</param>
        /// <param name="strLocalFileName">保存在本地时的文件名</param>
        public void Get(string strRemoteFileName, string strFolder, string strLocalFileName)
        {
            Socket socketData = CreateDataSocket();
            try
            {
                if (!Connected)
                {
                    Connect();
                }
                SetTransferType(TransferType.Binary);
                if (strLocalFileName.Equals(""))
                {
                    strLocalFileName = strRemoteFileName;
                }
                SendCommand("RETR " + strRemoteFileName);
                if (!(iReplyCode == 150 || iReplyCode == 125 || iReplyCode == 226 || iReplyCode == 250))
                {
                    throw new IOException(strReply.Substring(4));
                }
                FileStream output = new FileStream(strFolder + "\\" + strLocalFileName, FileMode.Create);
                while (true)
                {
                    int iBytes = socketData.Receive(buffer, buffer.Length, 0);
                    output.Write(buffer, 0, iBytes);
                    if (iBytes <= 0)
                    {
                        break;
                    }
                }
                output.Close();
                if (socketData.Connected)
                {
                    socketData.Close();
                }
                if (!(iReplyCode == 226 || iReplyCode == 250))
                {
                    ReadReply();
                    if (!(iReplyCode == 226 || iReplyCode == 250))
                    {
                        throw new IOException(strReply.Substring(4));
                    }
                }
            }
            catch
            {
                socketData.Close();
                socketData = null;
                socketControl.Close();
                Connected = false;
                socketControl = null;
            }
        }

        /// <summary>
        /// 下载一个文件
        /// </summary>
        /// <param name="strRemoteFileName">要下载的文件名</param>
        /// <param name="strFolder">本地目录(不得以\结束)</param>
        /// <param name="strLocalFileName">保存在本地时的文件名</param>
        public void GetNoBinary(string strRemoteFileName, string strFolder, string strLocalFileName)
        {
            if (!Connected)
            {
                Connect();
            }

            if (strLocalFileName.Equals(""))
            {
                strLocalFileName = strRemoteFileName;
            }
            Socket socketData = CreateDataSocket();
            SendCommand("RETR " + strRemoteFileName);
            if (!(iReplyCode == 150 || iReplyCode == 125 || iReplyCode == 226 || iReplyCode == 250))
            {
                throw new IOException(strReply.Substring(4));
            }
            FileStream output = new FileStream(strFolder + "\\" + strLocalFileName, FileMode.Create);
            while (true)
            {
                int iBytes = socketData.Receive(buffer, buffer.Length, 0);
                output.Write(buffer, 0, iBytes);
                if (iBytes <= 0)
                {
                    break;
                }
            }
            output.Close();
            if (socketData.Connected)
            {
                socketData.Close();
            }
            if (!(iReplyCode == 226 || iReplyCode == 250))
            {
                ReadReply();
                if (!(iReplyCode == 226 || iReplyCode == 250))
                {
                    throw new IOException(strReply.Substring(4));
                }
            }
        }

        /// <summary>
        /// 上传一批文件
        /// </summary>
        /// <param name="strFolder">本地目录(不得以\结束)</param>
        /// <param name="strFileNameMask">文件名匹配字符(可以包含*和?)</param>
        public void Put(string strFolder, string strFileNameMask)
        {
            string[] strFiles = Directory.GetFiles(strFolder, strFileNameMask);
            foreach (string strFile in strFiles)
            {
                Put(strFile);
            }
        }

        /// <summary>
        /// 上传一个文件
        /// </summary>
        /// <param name="strFileName">本地文件名</param>
        public void Put(string strFileName)
        {
            if (!Connected)
            {
                Connect();
            }
            Socket socketData = CreateDataSocket();
            if (Path.GetExtension(strFileName) == "")
                SendCommand("STOR " + Path.GetFileNameWithoutExtension(strFileName));
            else
                SendCommand("STOR " + Path.GetFileName(strFileName));

            if (!(iReplyCode == 125 || iReplyCode == 150))
            {
                throw new IOException(strReply.Substring(4));
            }

            FileStream input = new FileStream(strFileName, FileMode.Open);
            int iBytes = 0;
            while ((iBytes = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                socketData.Send(buffer, iBytes, 0);
            }
            input.Close();
            if (socketData.Connected)
            {
                socketData.Close();
            }
            if (!(iReplyCode == 226 || iReplyCode == 250))
            {
                ReadReply();
                if (!(iReplyCode == 226 || iReplyCode == 250))
                {
                    throw new IOException(strReply.Substring(4));
                }
            }
        }


        /// <summary>
        /// 上传一个文件
        /// </summary>
        /// <param name="strFileName">本地文件名</param>
        public void PutByGuid(string strFileName, string strGuid)
        {
            if (!Connected)
            {
                Connect();
            }
            string str = strFileName.Substring(0, strFileName.LastIndexOf("\\"));
            string strTypeName = strFileName.Substring(strFileName.LastIndexOf("."));
            strGuid = str + "\\" + strGuid;
            System.IO.File.Copy(strFileName, strGuid);
            System.IO.File.SetAttributes(strGuid, System.IO.FileAttributes.Normal);
            Socket socketData = CreateDataSocket();
            SendCommand("STOR " + Path.GetFileName(strGuid));
            if (!(iReplyCode == 125 || iReplyCode == 150))
            {
                throw new IOException(strReply.Substring(4));
            }
            FileStream input = new FileStream(strGuid, FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
            int iBytes = 0;
            while ((iBytes = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                socketData.Send(buffer, iBytes, 0);
            }
            input.Close();
            System.IO.File.Delete(strGuid);
            if (socketData.Connected)
            {
                socketData.Close();
            }
            if (!(iReplyCode == 226 || iReplyCode == 250))
            {
                ReadReply();
                if (!(iReplyCode == 226 || iReplyCode == 250))
                {
                    throw new IOException(strReply.Substring(4));
                }
            }
        }
        #endregion

        #region 目录操作
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="strDirName">目录名</param>
        public void MkDir(string strDirName)
        {
            if (!Connected)
            {
                Connect();
            }
            SendCommand("MKD " + strDirName);
            if (iReplyCode != 257)
            {
                throw new IOException(strReply.Substring(4));
            }
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="strDirName">目录名</param>
        public void RmDir(string strDirName)
        {
            if (!Connected)
            {
                Connect();
            }
            SendCommand("RMD " + strDirName);
            if (iReplyCode != 250)
            {
                throw new IOException(strReply.Substring(4));
            }
        }

        /// <summary>
        /// 改变目录
        /// </summary>
        /// <param name="strDirName">新的工作目录名</param>
        public void ChDir(string strDirName)
        {
            if (strDirName.Equals(".") || strDirName.Equals(""))
            {
                return;
            }
            if (!Connected)
            {
                Connect();
            }
            SendCommand("CWD " + strDirName);
            if (iReplyCode != 250)
            {
                throw new IOException(strReply.Substring(4));
            }
            this.RemotePath = strDirName;
        }
        #endregion

        #region 私有函数
        /// <summary>
        /// 获取FTP服务器应答信息
        ///     将一行应答字符串记录在strReply和strMsg变量中，应答码记录在iReplyCode变量中
        /// </summary>
        private void ReadReply()
        {
            this.strMsg = "";
            this.strReply = ReadLine();
            iReplyCode = Int32.Parse(this.strReply.Substring(0, 3));
        }

        /// <summary>
        /// 读取Socket返回的所有字符串
        /// </summary>
        /// <returns></returns>
        private string ReadLine()
        {
            lock(obj)
            {
                while(true)
                {
                    int iBytes = socketControl.Receive(buffer, buffer.Length, 0);
                    strMsg += ASCII.GetString(buffer, 0, iBytes);
                    if (iBytes < buffer.Length) break;
                }
            }

            char[] seperator = { '\n' };
            string[] mess = strMsg.Split(seperator);
            if(strMsg.Length > 2)
            {
                strMsg = mess[mess.Length - 2];
            }
            else
            {
                strMsg = mess[0];
            }
            if(!strMsg.Substring(3, 1).Equals(" "))
            {
                return ReadLine();
            }
            return strMsg;
        }

        /// <summary>
        /// 发送命令，并获取应答码和最后一行应答字符串
        /// </summary>
        /// <param name="strCommand">命令</param>
        private void SendCommand(string strCommand)
        {
            lock(obj)
            {
                Byte[] cmdBytes = Encoding.ASCII.GetBytes((strCommand + "\r\n").ToCharArray());
                socketControl.Send(cmdBytes, cmdBytes.Length, 0);
                Thread.Sleep(500);
                ReadReply();
            }
        }


        /// <summary>
        /// 关闭socket连接(用于登录以前)
        /// </summary>
        private void CloseSocketConnect()
        {
            lock (obj)
            {
                if (socketControl != null)
                {
                    socketControl.Close();
                    socketControl = null;
                }
                this.Connected = false;
            }
        }

        /// <summary>
        /// 建立进行数据连接的socket
        /// </summary>
        /// <returns>数据连接socket</returns>
        private Socket CreateDataSocket()
        {
            SendCommand("PASV");
            if (iReplyCode != 227)
            {
                throw new IOException(strReply.Substring(4));
            }
            int index1 = strReply.IndexOf('(');
            int index2 = strReply.IndexOf(')');
            string ipData = strReply.Substring(index1 + 1, index2 - index1 - 1);
            int[] parts = new int[6];
            int len = ipData.Length;
            int partCount = 0;
            string buf = "";
            for (int i = 0; i < len && partCount <= 6; i++)
            {
                char ch = Char.Parse(ipData.Substring(i, 1));
                if (Char.IsDigit(ch))
                    buf += ch;
                else if (ch != ',')
                {
                    throw new IOException("Malformed PASV strReply: " + strReply);
                }
                if (ch == ',' || i + 1 == len)
                {
                    try
                    {
                        parts[partCount++] = Int32.Parse(buf);
                        buf = "";
                    }
                    catch (Exception)
                    {
                        throw new IOException("Malformed PASV strReply: " + strReply);
                    }
                }
            }
            string ipAddress = parts[0] + "." + parts[1] + "." + parts[2] + "." + parts[3];
            int port = (parts[4] << 8) + parts[5];
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            try
            {
                s.Connect(ep);
            }
            catch (Exception)
            {
                throw new IOException("无法连接ftp服务器");
            }
            return s;
        }
        #endregion
    }
}