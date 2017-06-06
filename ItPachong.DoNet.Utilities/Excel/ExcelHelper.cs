// ********************************************************************
// * 项目名称：		    ItPachong.DoNet.Utilities
// * 程序集名称：	    ItPachong.DoNet.Utilities.Excel
// * 文件名称：		    ExcelHelper.cs
// * 编写者：		    赖强
// * 编写日期：		    2017-06-06
// * 程序功能描述：
// *        Excel帮助类,主要包括以下内容(注：因考虑到并发的原因，该类下所有方法没有设置为static)
// *            1、对Excel文件对象的基本操作：
// *                1.1、获取Excel连接字符串；
// *                1.2、获取Excel工作表名；
// *            2、数据基本操作：
// *                2.1、Excel数据导入为DataSet对象；
// *                2.2、DataSet对象和DataTable对象数据导出为Excel文件；
// *
// * 程序变更日期：
// * 程序变更者：
// * 变更说明：
// * 
// ********************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;

namespace ItPachong.DoNet.Utilities.Excel
{
    /// <summary>
    /// Excel帮助类
    /// </summary>
    public class ExcelHelper
    {
        #region Excel文件对象基本操作：1、获取Excel连接字符串；2、获取Excel工作表名；

        #region Excel定义
        /// <summary>
        /// 各Excel版本枚举
        /// </summary>
        public enum ExcelType
        {
            Excel_2003,
            Excel_2007,
            Excel_2013,
            Excel_2016
        }

        /// <summary>
        /// IMEX三种模式：IMEX是用来告诉驱动程序使用Excel文件的模式，其中0，1，2三种分别表示导出、导入、混合模式
        /// </summary>
        public enum IMEXType
        {
            /// <summary>
            /// 导出模式
            /// </summary>
            ExportMode = 0,
            /// <summary>
            /// 导入模式
            /// </summary>
            ImportMode = 1,
            /// <summary>
            /// 混合模式
            /// </summary>
            LinkedMode = 2
        }
        #endregion

        #region 获取Excel连接字符串

        /// <summary>
        /// 返回Excel连接字符串(默认：IMEX=1（导入模式）的情况）
        /// </summary>
        /// <param name="excelPath">Excel文件绝对路径</param>
        /// <param name="header">是否第一行为列名</param>
        /// <param name="eType">Excel版本</param>
        /// <returns></returns>
        public string GetExcelConnectionString(string excelPath, bool header, ExcelType eType)
        {
            return GetExcelConnectionString(excelPath, header, eType, IMEXType.ImportMode);
        }

        /// <summary>
        /// 返回Excel连接字符串
        /// </summary>
        /// <param name="excelPath">Excel文件绝对路径</param>
        /// <param name="header">是否第一行为列名</param>
        /// <param name="eType">Excel版本</param>
        /// <param name="imex">IMEX模式</param>
        /// <returns></returns>
        public string GetExcelConnectionString(string excelPath, bool header, ExcelType eType, IMEXType imex)
        {
            if (string.IsNullOrEmpty(excelPath)) throw new ArgumentNullException("Excel路径字符串为空");
            if (!File.Exists(excelPath)) throw new FileNotFoundException(string.Format("Excel文件不存在，路径为：{0}", excelPath));

            string connectionString = "";
            string hdr = "NO";
            if (header) hdr = "YES";

            if(eType == ExcelType.Excel_2003)
            {
                connectionString = "Provider=Microsoft.Jet.OleDb.4.0; data source=" + excelPath +
                    ";Extended Properties='Excel 8.0; HDR=" + hdr + "; IMEX=" + imex.GetHashCode() + "'";
            }
            else
            {
                connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; data source=" + excelPath +
                    ";Extended Properties='Excel 12.0 Xml; HDR=" + hdr + "; IMEX=" + imex.GetHashCode() + "'";
            }
            return connectionString;
        }
        #endregion

        #region 获取Excel工作表名相关方法
        /// <summary>
        /// 获取Excel各个Sheet名称(默认：第一行为列名)
        /// </summary>
        /// <param name="excelPath">Excel文件路径</param>
        /// <param name="eType">Excel版本</param>
        /// <returns></returns>
        public List<string> GetExcelTablesName(string excelPath, ExcelType eType)
        {
            string connectString = this.GetExcelConnectionString(excelPath, true, eType);
            return GetExcelTablesName(connectString);
        }

        /// <summary>
        /// 获取Excel各个Sheet名称
        /// </summary>
        /// <param name="connectString">Excel连接字符串</param>
        /// <returns></returns>
        public List<string> GetExcelTablesName(string connectString)
        {
            using (var conn = new OleDbConnection(connectString))
            {
                return GetExcelTablesName(conn);
            }
        }

        /// <summary>
        /// 获取Excel各个Sheet名称
        /// </summary>
        /// <param name="connection">Excel连接的OleDb对象</param>
        /// <returns></returns>
        public List<string> GetExcelTablesName(OleDbConnection connection)
        {
            var list = new List<string>();
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();

            DataTable dt = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if(dt != null && dt.Rows.Count > 0)
            {
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(dt.Rows[i][2].ToString());
                }
            }

            return list;
        }

        /// <summary>
        /// 获取Excel第一个Sheet名称(默认：第一行为列名)
        /// </summary>
        /// <param name="excelPath">Excel文件路径</param>
        /// <param name="eType">Excel版本</param>
        /// <returns></returns>
        public string GetExcelFirstTableName(string excelPath, ExcelType eType)
        {
            string connectionString = GetExcelConnectionString(excelPath, true, eType);
            return GetExcelFirstTableName(connectionString);
        }

        /// <summary>
        /// 获取Excel第一个Sheet名称
        /// </summary>
        /// <param name="connectionString">Excel连接字符串</param>
        /// <returns></returns>
        public string GetExcelFirstTableName(string connectionString)
        {
            using (var conn = new OleDbConnection(connectionString))
            {
                return GetExcelFirstTableName(conn);
            }
        }

        /// <summary>
        /// 获取Excel第一个Sheet名称
        /// </summary>
        /// <param name="connection">Excel连接的OleDb对象</param>
        /// <returns></returns>
        public string GetExcelFirstTableName(OleDbConnection connection)
        {
            string tableName = string.Empty;
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            DataTable dt = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if(dt != null && dt.Rows.Count > 0)
            {
                tableName = dt.Rows[0][2].ToString();
            }

            return tableName;
        }

        /// <summary>
        /// 获取Excel文件中指定Sheet名称的列(默认：第一行为列名)
        /// </summary>
        /// <param name="excelPath">Excel文件路径</param>
        /// <param name="eType">Excel版本</param>
        /// <param name="tableName">指定Sheet名称，例如：Sheet1$</param>
        /// <returns></returns>
        public List<string> GetColumnsList(string excelPath, ExcelType eType, string tableName)
        {
            if (string.IsNullOrEmpty(tableName)) return null;
            if (tableName.LastOrDefault() != '$') tableName += "$";

            DataTable tableColumns;
            string connectionString = this.GetExcelConnectionString(excelPath, true, eType);
            using (var conn = new OleDbConnection(connectionString))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                tableColumns = GetReaderSchema(tableName, conn);
            }

            return (from DataRow dr in tableColumns.Rows
                    let columnName = dr["ColumnName"].ToString()
                    let datatype = ((OleDbType)dr["ProviderType"]).ToString()
                    let netType = dr["DataType"].ToString()
                    select columnName).ToList();
        }

        private DataTable GetReaderSchema(string tableName, OleDbConnection connection)
        {
            DataTable schemaTable;
            IDbCommand cmd = new OleDbCommand();
            cmd.CommandText = string.Format("select * from [{0}]", tableName);
            cmd.Connection = connection;

            using (IDataReader reader = cmd.ExecuteReader(CommandBehavior.KeyInfo | CommandBehavior.SchemaOnly))
            {
                schemaTable = reader.GetSchemaTable();
            }

            return schemaTable;
        }
        #endregion

        #endregion

        #region Excel数据基本操作

        #region Excel文件导入
        /// <summary>
        /// Excel导入为DataSet对象实体
        /// </summary>
        /// <param name="excelPath">Excel文件路径</param>
        /// <param name="tableName">指定Sheet名称，例如:Sheet1$</param>
        /// <param name="header">是否第一行为列头</param>
        /// <param name="eType">Excel类型</param>
        /// <returns></returns>
        public DataSet Excel2DataSet(string excelPath, string tableName, bool header, ExcelType eType)
        {
            string connectionString = GetExcelConnectionString(excelPath, header, eType);
            return Excel2DataSet(connectionString, tableName);
        }

        /// <summary>
        /// Excel导入为DataSet对象实体
        /// </summary>
        /// <param name="connectionString">Excel连接字符串</param>
        /// <param name="tableName">指定Sheet名称，例如:Sheet1$</param>
        /// <returns></returns>

        public DataSet Excel2DataSet(string connectionString, string tableName)
        {
            using (var conn = new OleDbConnection(connectionString))
            {
                var ds = new DataSet();
                if (IsExistExcelTableName(conn, tableName))
                {
                    var adapter = new OleDbDataAdapter("SELECT * FROM [" + tableName + "]", conn);
                    adapter.Fill(ds, tableName);
                }

                return ds;
            }
        }

        /// <summary>
        /// Excel导入为DataSet对象实体
        /// </summary>
        /// <param name="excelPath">Excel文件绝对路径</param>
        /// <param name="header">是否第一行为列头</param>
        /// <param name="eType">Excel类型</param>
        /// <returns></returns>
        public DataSet Excel2DataSet(string excelPath, bool header, ExcelType eType)
        {
            string connectionString = GetExcelConnectionString(excelPath, header, eType);
            return Excel2DataSet(connectionString);
        }

        /// <summary>
        /// Excel导入为DataSet对象实体
        /// </summary>
        /// <param name="connectionString">Excel连接字符串</param>
        /// <returns></returns>
        public DataSet Excel2DataSet(string connectionString)
        {
            using (var conn = new OleDbConnection(connectionString))
            {
                var ds = new DataSet();
                List<string> tableNames = GetExcelTablesName(conn);

                foreach(string tableName in tableNames)
                {
                    var adapter = new OleDbDataAdapter("SELECT * FROM [" + tableName + "]", conn);
                    adapter.Fill(ds, tableName);
                }

                return ds;
            }
        }

        private bool IsExistExcelTableName(OleDbConnection connection, string tableName)
        {
            List<string> list = this.GetExcelTablesName(connection);
            return list.Any(t => t.ToLower() == tableName.ToLower());
        }

        #endregion

        #region 导出为Excel文件
        /// <summary>
        /// DataSet对象实体类的所有DataTable对象导出到指定的Excel文件中（XML格式方式实现）
        /// </summary>
        /// <param name="source">DataSet实体</param>
        /// <param name="fileName">Excel文件的存储路径</param>
        public void DataSet2Excel(DataSet source, string fileName)
        {
            var excelDoc = new StreamWriter(fileName);
            
            #region Excel格式内容
            const string startExcelXML = "<xml version>\r\n<Workbook " +
                                         "xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"\r\n" +
                                         " xmlns:o=\"urn:schemas-microsoft-com:office:office\"\r\n " +
                                         "xmlns:x=\"urn:schemas-    microsoft-com:office:" +
                                         "excel\"\r\n xmlns:ss=\"urn:schemas-microsoft-com:" +
                                         "office:spreadsheet\">\r\n <Styles>\r\n " +
                                         "<Style ss:ID=\"Default\" ss:Name=\"Normal\">\r\n " +
                                         "<Alignment ss:Vertical=\"Bottom\"/>\r\n <Borders/>" +
                                         "\r\n <Font/>\r\n <Interior/>\r\n <NumberFormat/>" +
                                         "\r\n <Protection/>\r\n </Style>\r\n " +
                                         "<Style ss:ID=\"BoldColumn\">\r\n <Font " +
                                         "x:Family=\"Swiss\" ss:Bold=\"1\"/>\r\n </Style>\r\n " +
                                         "<Style     ss:ID=\"StringLiteral\">\r\n <NumberFormat" +
                                         " ss:Format=\"@\"/>\r\n </Style>\r\n <Style " +
                                         "ss:ID=\"Decimal\">\r\n <NumberFormat " +
                                         "ss:Format=\"#,##0.###\"/>\r\n </Style>\r\n " +
                                         "<Style ss:ID=\"Integer\">\r\n <NumberFormat " +
                                         "ss:Format=\"0\"/>\r\n </Style>\r\n <Style " +
                                         "ss:ID=\"DateLiteral\">\r\n <NumberFormat " +
                                         "ss:Format=\"yyyy-mm-dd;@\"/>\r\n </Style>\r\n " +
                                         "</Styles>\r\n ";
            const string endExcelXML = "</Workbook>";
            #endregion

            int sheetCount = 1;
            excelDoc.Write(startExcelXML);
            //把DataSet对象中的DataTable对象数据，逐个写入Excel文件
            for(int i = 0; i < source.Tables.Count; i++)
            {
                int rowCount = 0;
                DataTable dt = source.Tables[i];

                excelDoc.Write("<Worksheet ss:Name=\"Sheet" + sheetCount + "\">");
                excelDoc.Write("<Table>");
                excelDoc.Write("<Row>");
                
                //写入DataTable的列头
                for(int x = 0; x < dt.Columns.Count; x++)
                {
                    excelDoc.Write("<Cell ss:StyleID=\"BoldColumn\"><Data ss:Type=\"String\">");
                    excelDoc.Write(source.Tables[i].Columns[x].ColumnName);
                    excelDoc.Write("</Data></Cell>");
                }
                excelDoc.Write("</Row>");

                //写入DataTable的数据
                foreach(DataRow x in dt.Rows)
                {
                    rowCount++;
                    //如果数据数量大于64000条记录时，将创建新的Sheet继续写入
                    if(rowCount == 64000)
                    {
                        rowCount = 0;
                        sheetCount++;
                        //end old sheet
                        excelDoc.Write("</Table>");
                        excelDoc.Write(" </Worksheet>");

                        //set new sheet
                        excelDoc.Write("<Worksheet ss:Name=\"Sheet" + sheetCount + "\">");
                        excelDoc.Write("<Table>");
                    }

                    //开始写入数据
                    excelDoc.Write("<Row>");
                    for(int y = 0; y < source.Tables[i].Columns.Count; y++)
                    {
                        Type rowType = x[y].GetType();
                        #region 根据不同数据类型生成内容
                        switch (rowType.ToString())
                        {
                            case "System.String":
                                string xmlString = x[y].ToString().Trim();
                                xmlString = xmlString.Replace("&", "&");
                                xmlString = xmlString.Replace(">", ">");
                                xmlString = xmlString.Replace("<", "<");
                                excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\"><Data ss Type=\"String\">");
                                excelDoc.Write(xmlString);
                                excelDoc.Write("</Data></Cell>");
                                break;
                            case "System.DateTime":
                                //Excel has a specific Date Format of yyyy-MM-dd followed by
                                //the letter 'T' then HH:mm:ss.lll Example: 2017-06-06T14:50:23.000
                                //The Following Code puts the date stored in XMLDate to the format above
                                var xmlDate = (DateTime)x[y];
                                string xmlDateToString = xmlDate.Year + "-" + (xmlDate.Month < 10 ? "0" + xmlDate.Month.ToString() : xmlDate.Month.ToString()) +
                                    "-" + (xmlDate.Day < 10 ? "0" + xmlDate.Day.ToString() : xmlDate.Day.ToString()) +
                                    "T" + (xmlDate.Hour < 10 ? "0" + xmlDate.Hour.ToString() : xmlDate.Hour.ToString()) +
                                    ":" + (xmlDate.Minute < 10 ? "0" + xmlDate.Minute.ToString() : xmlDate.Minute.ToString()) +
                                    ":" + (xmlDate.Second < 10 ? "0" + xmlDate.Second.ToString() : xmlDate.Second.ToString()) +
                                    ".000";
                                excelDoc.Write("<Cell ss:StyleID=\"DateLiteral\"><Data ss Type=\"DateTime\">");
                                excelDoc.Write(xmlDateToString);
                                excelDoc.Write("</Data></Cell>");
                                break;
                            case "System.Boolean":
                                excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\"><Data ss Type=\"String\">");
                                excelDoc.Write(x[y].ToString());
                                excelDoc.Write("</Data></Cell>");
                                break;
                            case "System.Int16":
                            case "System.Int32":
                            case "System.Int64":
                            case "System.Byte":
                                excelDoc.Write("<Cell ss:StyleID=\"Integer\"><Data ss Type=\"Number\">");
                                excelDoc.Write(x[y].ToString());
                                excelDoc.Write("</Data></Cell>");
                                break;
                            case "System.Decimal":
                            case "System.Double":
                                excelDoc.Write("<Cell ss:StyleID=\"Decimal\"><Data ss Type=\"Number\">");
                                excelDoc.Write(x[y].ToString());
                                excelDoc.Write("</Data></Cell>");
                                break;
                            case "System.DBNull":
                                excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\"><Data ss Type=\"String\">");
                                excelDoc.Write("");
                                excelDoc.Write("</Data></Cell>");
                                break;
                            default:
                                throw new Exception(rowType + " not handled.");
                        }
                        #endregion
                    }
                    excelDoc.Write("</Row>");
                }
                excelDoc.Write("</Table>");
                excelDoc.Write(" </Worksheet>");

                sheetCount++;
            }
            excelDoc.Write(endExcelXML);
            excelDoc.Close();
        }

        /// <summary>
        /// DataTable对象导出到指定的Excel文件中（OleDb方式实现）
        /// </summary>
        /// <param name="dataTable">DataTable实体</param>
        /// <param name="fileName">Excel文件的存储路径</param>
        /// <param name="eType">导出Excel格式类型</param>
        public void DataTable2Excel(DataTable dataTable, string fileName, ExcelType eType)
        {
            if(File.Exists(fileName))
            {
                try
                {
                    File.Delete(fileName);
                }
                catch
                {
                    throw new Exception("该文件正在被使用，关闭文件或重新命名后再试导出文件");
                }
            }

            var oleDbConn = new OleDbConnection();
            var oleDbCmd = new OleDbCommand();
            try
            {
                oleDbConn.ConnectionString = this.GetExcelConnectionString(fileName, true, eType);
                oleDbConn.Open();
                oleDbCmd.CommandType = CommandType.Text;
                oleDbCmd.Connection = oleDbConn;
                string sql = "CREATE TABLE " + (string.IsNullOrEmpty(dataTable.TableName) ? "sheet1" : dataTable.TableName) + " (";
                for(int i = 0; i < dataTable.Columns.Count; i++)
                {
                    //注：字段名（列名）如果出现Excel关键字时，会导致错误.
                    if(i < dataTable.Columns.Count - 1)
                    {
                        sql += "[" + dataTable.Columns[i].Caption + "] TEXT(100) ,";
                    }
                    else
                    {
                        sql += "[" + dataTable.Columns[i].Caption + "] TEXT(200) )";
                    }
                }
                oleDbCmd.CommandText = sql;
                oleDbCmd.ExecuteNonQuery();

                for(int i = 0; i < dataTable.Rows.Count; i++)
                {
                    sql = "INSERT INTO " + (string.IsNullOrEmpty(dataTable.TableName) ? "sheet1" : dataTable.TableName) + " VALUES('";
                    for(int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        if(j < dataTable.Columns.Count - 1)
                        {
                            sql += dataTable.Rows[i][j] + " ','";
                        }
                        else
                        {
                            sql += dataTable.Rows[i][j] + " ')";
                        }
                    }
                    oleDbCmd.CommandText = sql;
                    oleDbCmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                oleDbCmd.Dispose();
                oleDbConn.Close();
                oleDbConn.Dispose();
            }
        }
        #endregion

        #endregion
    }
}
