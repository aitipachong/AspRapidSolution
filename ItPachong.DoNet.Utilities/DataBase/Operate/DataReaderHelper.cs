// ********************************************************************
// * 项目名称：		    ItPachong.DoNet.Utilities
// * 程序集名称：	    ItPachong.DoNet.Utilities.DataBase.Operate
// * 文件名称：		    DataReaderHelper.cs
// * 编写者：		    赖强
// * 编写日期：		    2017-05-19
// * 程序功能描述：
// *        DataReader对象操作类
// *
// * 程序变更日期：
// * 程序变更者：
// * 变更说明：
// * 
// ********************************************************************
using System;
using System.Data;

namespace ItPachong.DoNet.Utilities.DataBase.Operate
{
    /// <summary>
    /// DataReader操作类
    /// </summary>
    public class DataReaderHelper
    {
        #region Private Readonly Fields

        private readonly DateTime _defaultDate;     //默认日期时间

        private readonly IDataReader _reader;       //数据输出流读取访问对象接口

        #endregion

        #region Initialization

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="reader">IDataReader对象</param>
        public DataReaderHelper(IDataReader reader)
        {
            _defaultDate = System.Convert.ToDateTime("01/01/1970 00:00:00");
            _reader = reader;
        }

        #endregion

        #region Methods
        /// <summary>
        /// 继续读取下一条数据操作
        /// </summary>
        /// <returns></returns>
        public bool Next()
        {
            return _reader.Read();
        }

        /// <summary>
        /// 读取数据，转换为int类型数据
        /// </summary>
        /// <param name="column">列名</param>
        /// <returns></returns>
        public int GetInt32(string column)
        {
            return GetInt32(column, 0);
        }

        /// <summary>
        /// 读取数据，转换为int类型数据
        /// </summary>
        /// <param name="column">列名</param>
        /// <param name="defaultIfNull">如果数据内容为Null的默认值</param>
        /// <returns></returns>
        public int GetInt32(string column, int defaultIfNull)
        {
            return (_reader.IsDBNull(_reader.GetOrdinal(column))) 
                    ? defaultIfNull : int.Parse(_reader[column].ToString());
        }

        /// <summary>
        /// 读取数据，转换为int?数据类型
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public int? GetInt32Nullable(string column)
        {
            return (_reader.IsDBNull(_reader.GetOrdinal(column))) 
                    ? (int?)null : int.Parse(_reader[column].ToString());
        }

        /// <summary>
        /// 读取数据，转换为short（int16）数据类型
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public short GetInt16(string column)
        {
            return GetInt16(column, 0);
        }

        /// <summary>
        /// 读取数据，转换为short（int16）数据类型
        /// </summary>
        /// <param name="column"></param>
        /// <param name="defaultIfNull"></param>
        /// <returns></returns>
        public short GetInt16(string column, short defaultIfNull)
        {
            return (_reader.IsDBNull(_reader.GetOrdinal(column)))
                    ? defaultIfNull : short.Parse(_reader[column].ToString());
        }

        /// <summary>
        /// 读取数据，转换为short?（int16?）数据类型
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public short? GetInt16Nullable(string column)
        {
            return (_reader.IsDBNull(_reader.GetOrdinal(column)))
                    ? (short?)null : short.Parse(_reader[column].ToString());
        }

        /// <summary>
        /// 转换为Float类型数据
        /// </summary>
        /// <returns>返回值</returns>
        /// <param name="column">列名</param>
        public float GetFloat(string column)
        {
            return GetFloat(column, 0);
        }

        /// <summary>
        /// 转换为Float类型数据
        /// </summary>
        /// <returns>返回值</returns>
        /// <param name="column">列名</param>
        /// <param name="defaultIfNull">如果为空的默认值</param>
        public float GetFloat(string column, float defaultIfNull)
        {
            return (_reader.IsDBNull(_reader.GetOrdinal(column)))
                    ? defaultIfNull : float.Parse(_reader[column].ToString());
        }

        /// <summary>
        /// 转换为Float类型数据
        /// </summary>
        /// <returns>返回值</returns>
        /// <param name="column">列名</param>
        public float? GetFloatNullable(string column)
        {
            return (_reader.IsDBNull(_reader.GetOrdinal(column)))
                   ? (float?)null : float.Parse(_reader[column].ToString());
        }

        /// <summary>
        /// 转换为Double类型数据
        /// </summary>
        /// <returns>返回值</returns>
        /// <param name="column">列名</param>
        public double GetDouble(string column)
        {
            return GetDouble(column, 0);
        }

        /// <summary>
        /// 转换为Double类型数据
        /// </summary>
        /// <returns>返回值</returns>
        /// <param name="column">列名</param>
        /// <param name="defaultIfNull">如果为空的默认值</param>
        public double GetDouble(string column, double defaultIfNull)
        {
            return (_reader.IsDBNull(_reader.GetOrdinal(column)))
                   ? defaultIfNull : double.Parse(_reader[column].ToString());
        }

        /// <summary>
        /// 转换为Double类型数据(可空类型）
        /// </summary>
        /// <returns>返回值</returns>
        /// <param name="column">列名</param>
        public double? GetDoubleNullable(string column)
        {
            return (_reader.IsDBNull(_reader.GetOrdinal(column)))
                    ? (double?)null : double.Parse(_reader[column].ToString());
        }

        /// <summary>
        /// 转换为Decimal类型数据
        /// </summary>
        /// <returns>返回值</returns>
        /// <param name="column">列名</param>
        public decimal GetDecimal(string column)
        {
            return GetDecimal(column, 0);
        }

        /// <summary>
        /// 转换为Decimal类型数据
        /// </summary>
        /// <returns>返回值</returns>
        /// <param name="column">列名</param>
        /// <param name="defaultIfNull">如果为空的默认值</param>
        public decimal GetDecimal(string column, decimal defaultIfNull)
        {
            return (_reader.IsDBNull(_reader.GetOrdinal(column)))
                    ? defaultIfNull : decimal.Parse(_reader[column].ToString());
        }

        /// <summary>
        /// 转换为Decimal类型数据(可空类型）
        /// </summary>
        /// <returns>返回值</returns>
        /// <param name="column">列名</param>
        public decimal? GetDecimalNullable(string column)
        {
            return (_reader.IsDBNull(_reader.GetOrdinal(column)))
                    ? (decimal?)null : decimal.Parse(_reader[column].ToString());
        }

        /// <summary>
        /// 转换为Single类型数据
        /// </summary>
        /// <returns>返回值</returns>
        /// <param name="column">列名</param>
        public Single GetSingle(string column)
        {
            return GetSingle(column, 0);
        }

        /// <summary>
        /// 转换为Single类型数据
        /// </summary>
        /// <returns>返回值</returns>
        /// <param name="column">列名</param>
        /// <param name="defaultIfNull">如果为空的默认值</param>
        public Single GetSingle(string column, Single defaultIfNull)
        {
            return (_reader.IsDBNull(_reader.GetOrdinal(column)))
                   ? defaultIfNull : Single.Parse(_reader[column].ToString());
        }

        /// <summary>
        /// 转换为Single类型数据(可空类型）
        /// </summary>
        /// <returns>返回值</returns>
        /// <param name="column">列名</param>
        public Single? GetSingleNullable(string column)
        {
            return (_reader.IsDBNull(_reader.GetOrdinal(column)))
                    ? (Single?)null : Single.Parse(_reader[column].ToString());
        }

        /// <summary>
        /// 转换为布尔类型数据
        /// </summary>
        /// <returns>返回值</returns>
        /// <param name="column">列名</param>
        public bool GetBoolean(string column)
        {
            return GetBoolean(column, false);
        }

        /// <summary>
        /// 转换为布尔类型数据
        /// </summary>
        /// <returns>返回值</returns>
        /// <param name="column">列名</param>
        /// <param name="defaultIfNull">如果为空的默认值</param>
        public bool GetBoolean(string column, bool defaultIfNull)
        {
            string str = _reader[column].ToString();
            try
            {
                int i = System.Convert.ToInt32(str);
                return i > 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 转换为布尔类型数据(可空类型）
        /// </summary>
        /// <returns>返回值</returns>
        /// <param name="column">列名</param>
        public bool? GetBooleanNullable(string column)
        {
            string str = _reader[column].ToString();
            try
            {
                int i = System.Convert.ToInt32(str);
                return i > 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 转换为字符串类型数据
        /// </summary>
        /// <returns>返回值</returns>
        /// <param name="column">列名</param>
        public string GetString(string column)
        {
            return GetString(column, "");
        }

        /// <summary>
        /// 转换为字符串类型数据
        /// </summary>
        /// <returns>返回值</returns>
        /// <param name="column">列名</param>
        /// <param name="defaultIfNull">如果为空的默认值</param>
        public string GetString(string column, string defaultIfNull)
        {
            return (_reader.IsDBNull(_reader.GetOrdinal(column))) ? defaultIfNull : _reader[column].ToString();
        }

        /// <summary>
        /// 转换为Byte字节数据类型数据
        /// </summary>
        /// <returns>返回值</returns>
        /// <param name="column">列名</param>
        public byte[] GetBytes(string column)
        {
            return GetBytes(column, null);
        }

        /// <summary>
        /// 转换为Byte字节数据类型数据
        /// </summary>
        /// <returns>返回值</returns>
        /// <param name="column">列名</param>
        /// <param name="defaultIfNull">如果为空的默认值</param>
        public byte[] GetBytes(string column, string defaultIfNull)
        {
            string data = (_reader.IsDBNull(_reader.GetOrdinal(column))) ? defaultIfNull : _reader[column].ToString();
            return System.Text.Encoding.UTF8.GetBytes(data);
        }

        /// <summary>
        /// 转换为Guid类型数据
        /// </summary>
        /// <returns>返回值</returns>
        /// <param name="column">列名</param>
        public Guid GetGuid(string column)
        {
            return GetGuid(column, null);
        }

        /// <summary>
        /// 转换为Guid类型数据
        /// </summary>
        /// <returns>返回值</returns>
        /// <param name="column">列名</param>
        /// <param name="defaultIfNull">如果为空的默认值</param>
        public Guid GetGuid(string column, string defaultIfNull)
        {
            string data = (_reader.IsDBNull(_reader.GetOrdinal(column))) ? defaultIfNull : _reader[column].ToString();
            Guid guid = Guid.Empty;
            if (data != null)
            {
                guid = new Guid(data);
            }
            return guid;
        }

        /// <summary>
        /// 转换为Guid类型数据(可空类型）
        /// </summary>/// <returns>返回值</returns> 
        /// <param name="column">列名</param>
        public Guid? GetGuidNullable(string column)
        {
            string data = (_reader.IsDBNull(_reader.GetOrdinal(column))) ? null : _reader[column].ToString();
            Guid? guid = null;
            if (data != null)
            {
                guid = new Guid(data);
            }
            return guid;
        }

        /// <summary>
        /// 转换为DateTime类型数据
        /// </summary>
        /// <returns>返回值</returns>
        /// <param name="column">列名</param>
        public DateTime GetDateTime(string column)
        {
            return GetDateTime(column, _defaultDate);
        }

        /// <summary>
        /// 转换为DateTime类型数据
        /// </summary>
        /// <returns>返回值</returns>
        /// <param name="column">列名</param>
        /// <param name="defaultIfNull">如果为空的默认值</param>
        public DateTime GetDateTime(string column, DateTime defaultIfNull)
        {
            return (_reader.IsDBNull(_reader.GetOrdinal(column)))
                    ? defaultIfNull : System.Convert.ToDateTime(_reader[column].ToString());
        }

        /// <summary>
        /// 转换为可空DateTime类型数据
        /// </summary>
        /// <returns>返回值</returns>
        /// <param name="column">列名</param>
        public DateTime? GetDateTimeNullable(string column)
        {
            return (_reader.IsDBNull(_reader.GetOrdinal(column)))
                    ? (DateTime?)null : System.Convert.ToDateTime(_reader[column].ToString());
        }

        #endregion
    }
}