// ********************************************************************
// * 项目名称：		    ItPachong.DoNet.Utilities
// * 程序集名称：	    ItPachong.DoNet.Utilities.API
// * 文件名称：		    API.cs
// * 编写者：		    赖强
// * 编写日期：		    2017-04-25
// * 程序功能描述：
// *        系统API函数调用：涉及内存、计算机、CPU等
// *
// * 程序变更日期：
// * 程序变更者：
// * 变更说明：
// * 
// ********************************************************************
using System;
using System.Runtime.InteropServices;

namespace ItPachong.DoNet.Utilities.API
{
    /// <summary>
    /// API函数调用
    /// </summary>
    public class API
    {
        #region 结构定义
        /// <summary>
        /// 定义内存的信息结构
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_INFO
        {
            /// <summary>
            /// 内存大小
            /// </summary>
            public uint dwLength;
            /// <summary>
            /// 正在使用的内存大小
            /// </summary>
            public uint dwMemoryLoad;
            /// <summary>
            /// 物理内存大小
            /// </summary>
            public uint dwTotalPhys;
            /// <summary>
            /// 可使用的物理内存
            /// </summary>
            public uint dwAvailPhys;
            /// <summary>
            /// 交换文件总大小
            /// </summary>
            public uint dwTotalPageFile;
            /// <summary>
            /// 可使用的交换文件大小
            /// </summary>
            public uint dwAvailPageFile;
            /// <summary>
            /// 总虚拟内存大小
            /// </summary>
            public uint dwTotalVirtual;
            /// <summary>
            /// 可使用的虚拟内存大小
            /// </summary>
            public uint dwAvailVirtual;
        }

        /// <summary>
        /// 定义CPU的信息结构
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct CPU_INFO
        {
            public uint dwOemId;
            public uint dwPageSize;
            public uint lpMinimumApplicationAddress;
            public uint lpMaximumApplicationAddress;
            public uint dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public uint dwProcessorLevel;
            public uint dwProcessorRevision;
        }

        #endregion

        #region API函数

        /// <summary>
        /// 内存API函数
        /// </summary>
        /// <param name="meminfo">需获取的内存类型</param>
        [DllImport("kernel32")]
        public static extern void GlobalMemoryStatus(ref MEMORY_INFO meminfo);

        /// <summary>
        /// 获取计算机名称
        /// </summary>
        /// <param name="ipBuffer"></param>
        /// <param name="nSize"></param>
        /// <returns></returns>
        [DllImport("kernel32", EntryPoint = "GetComputerName", ExactSpelling = false, SetLastError = true)]
        public static extern bool GetComputerName([MarshalAs(UnmanagedType.LPArray)]byte[] ipBuffer, [MarshalAs(UnmanagedType.LPArray)]Int32[] nSize);

        /// <summary>
        /// 获取计算机用户
        /// </summary>
        /// <param name="IpBuffer"></param>
        /// <param name="nSize"></param>
        /// <returns></returns>
        [DllImport("advapi32", EntryPoint = "GetUserName", ExactSpelling = false, SetLastError = true)]
        public static extern bool GetUserName([MarshalAs(UnmanagedType.LPArray)]byte[] IpBuffer, [MarshalAs(UnmanagedType.LPArray)]Int32[] nSize);

        /// <summary>
        /// 获取MAC地址
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="host"></param>
        /// <param name="mac"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        [DllImport("Iphlpapi.dll")]
        public static extern int SendARP(Int32 dest, Int32 host, ref Int64 mac, ref Int32 length);

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [DllImport("Ws2_32.dll")]
        public static extern Int32 inet_addr(string ip);

        #endregion
    }
}