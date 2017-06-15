// ********************************************************************
// * 项目名称：		    ItPachong.DoNet.Utilities
// * 程序集名称：	    ItPachong.DoNet.Utilities.Control
// * 文件名称：		    BindDataControl.cs
// * 编写者：		    赖强
// * 编写日期：		    2017-05-12
// * 程序功能描述：
// *        绑定数据控制类
// *
// * 程序变更日期：
// * 程序变更者：
// * 变更说明：
// * 
// ********************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace ItPachong.DoNet.Utilities.Control
{
    /// <summary>
    /// 绑定数据控制类
    /// </summary>
    public class BindDataControl
    {
        #region 绑定服务器数据控件  简单绑定DataList
        /// <summary>
        /// 简单绑定DataList
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="myDv"></param>
        public static void BindDataList(System.Web.UI.Control ctrl, DataView myDv)
        {
            ((DataList)ctrl).DataSourceID = null;
            ((DataList)ctrl).DataSource = myDv;
            ((DataList)ctrl).DataBind();
        }
        #endregion

        #region 绑定服务器数据控件 SqlDataReader简单绑定DataList
        /// <summary>
        /// SqlDataReader简单绑定DataList
        /// </summary>
        /// <param name="ctrl">控件ID</param>
        /// <param name="mydv">数据视图</param>
        public static void BindDataReaderList(System.Web.UI.Control ctrl, SqlDataReader mydv)
        {
            ((DataList)ctrl).DataSourceID = null;
            ((DataList)ctrl).DataSource = mydv;
            ((DataList)ctrl).DataBind();
        }
        #endregion

        #region 绑定服务器数据控件 简单绑定GridView
        /// <summary>
        /// 简单绑定GridView
        /// </summary>
        /// <param name="ctrl">控件ID</param>
        /// <param name="mydv">数据视图</param>
        public static void BindGridView(System.Web.UI.Control ctrl, DataView mydv)
        {
            ((System.Web.UI.WebControls.GridView)ctrl).DataSourceID = null;
            ((System.Web.UI.WebControls.GridView)ctrl).DataSource = mydv;
            ((System.Web.UI.WebControls.GridView)ctrl).DataBind();
        }
        #endregion

        #region 绑定服务器控件 简单绑定Repeater
        /// <summary>
        /// 绑定服务器控件 简单绑定Repeater
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="myDv"></param>
        public static void BindRepeater(System.Web.UI.Control ctrl, DataView myDv)
        {
            ((Repeater)ctrl).DataSourceID = null;
            ((Repeater)ctrl).DataSource = myDv;
            ((Repeater)ctrl).DataBind();
        }
        #endregion
    }
}