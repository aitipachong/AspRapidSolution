// ********************************************************************
// * 项目名称：		    ItPachong.DoNet.Utilities
// * 程序集名称：	    ItPachong.DoNet.Utilities.DataBase.Operate
// * 文件名称：		    DataTableHelper.cs
// * 编写者：		    赖强
// * 编写日期：		    2017-05-19
// * 程序功能描述：
// *        DataTable对象操作类
// *
// * 程序变更日期：
// * 程序变更者：
// * 变更说明：
// * 
// ********************************************************************
using ItPachong.DoNet.Utilities.Convert;
using ItPachong.DoNet.Utilities.String;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ItPachong.DoNet.Utilities.DataBase.Operate
{
    /// <summary>
    /// DataTable对象操作类
    /// </summary>
    public class DataTableHelper<T> where T : new()
    {
        #region 基本操作
        /// <summary>
        /// 类型名称字符串转换为类型
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        private Type ConvertType(string typeName)
        {
            typeName = typeName.ToLower().Replace("system.", "");
            Type newType = typeof(string);
            switch (typeName)
            {
                case "boolean":
                case "bool":
                    newType = typeof(bool);
                    break;
                case "int16":
                case "short":
                    newType = typeof(short);
                    break;
                case "int32":
                case "int":
                    newType = typeof(int);
                    break;
                case "long":
                case "int64":
                    newType = typeof(long);
                    break;
                case "uint16":
                case "ushort":
                    newType = typeof(ushort);
                    break;
                case "uint32":
                case "uint":
                    newType = typeof(uint);
                    break;
                case "uint64":
                case "ulong":
                    newType = typeof(ulong);
                    break;
                case "single":
                case "float":
                    newType = typeof(float);
                    break;
                case "string":
                    newType = typeof(string);
                    break;
                case "guid":
                    newType = typeof(Guid);
                    break;
                case "decimal":
                    newType = typeof(decimal);
                    break;
                case "double":
                    newType = typeof(double);
                    break;
                case "datetime":
                    newType = typeof(DateTime);
                    break;
                case "byte":
                    newType = typeof(byte);
                    break;
                case "char":
                    newType = typeof(char);
                    break;
            }
            return newType;
        }

        /// <summary>
        /// 合并具有相同数据结构的两个DataTable数据
        /// </summary>
        /// <param name="table1"></param>
        /// <param name="table2"></param>
        /// <returns></returns>
        public DataTable Merge(DataTable table1, DataTable table2)
        {
            table1.Merge(table2);
            return table1;
        }

        /// <summary>
        /// 将DataTable对象内数据写入XML文件中
        /// </summary>
        /// <param name="datatable">DataTable对象实例</param>
        /// <param name="fileName">XML文件存储的绝对路径</param>
        /// <returns></returns>
        public bool WriteDataTableToXml(DataTable datatable, string fileName)
        {
            bool result = false;
            try
            {
                if (datatable == null || datatable.Rows.Count <= 0)
                    return result;
                datatable.WriteXml(fileName);
                result = true;
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// 从DataTable中按条件查询，并返回查询行实体数组
        /// </summary>
        /// <param name="table">DataTable对象实例</param>
        /// <param name="queryText">查询条件</param>
        /// <returns></returns>
        public DataRow[] SelectDataTableContents(DataTable table, string filterExpression)
        {
            DataRow[] result = null;
            try
            {
                if (table == null || table.Rows.Count <= 0)
                    return result;
                result = table.Select(filterExpression);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// 实体对象集合转换为DataTable
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public DataTable ToDataTable(List<T> entities)
        {
            if (entities == null || entities.Count <= 0) return null;

            //从List<T>集合中第一个实体对象中获取所有的Properties
            Type entityType = entities[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            //生成DataTable的结构(列头）
            var dt = new DataTable();
            foreach(PropertyInfo t in entityProperties)
            {
                dt.Columns.Add(t.Name, t.PropertyType);
            }

            //将List<T>集合中所有对象的属性值，依次加入DataTable对应列
            foreach(T entity in entities)
            {
                if(entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }

                var entityValues = new object[entityProperties.Length];
                for(int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }

            return dt;
        }

        /// <summary>
        /// DataTable转换为实体集合
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public IList<I> ToList<I>(DataTable dt) where I : new()
        {
            if (dt == null || dt.Rows.Count <= 0)
                return null;

            //定义集合
            var list = new List<I>();

            //建立实体模型
            foreach(DataRow dr in dt.Rows)
            {
                var t = new I();
                var properties = t.GetType().GetProperties();
                foreach(var pi in properties)
                {
                    var tempName = pi.Name;
                    if(dt.Columns.Contains(tempName))
                    {
                        if (!pi.CanWrite)
                            continue;
                        var value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                list.Add(t);
            }
            return list;
        }

        /// <summary>
        /// 枚举转DataTable
        /// </summary>
        /// <param name="enumType">类型</param>
        /// <param name="key">索引</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public DataTable EnumToDataTable(Type enumType, string key, string value)
        {
            var names = Enum.GetNames(enumType);
            var values = Enum.GetValues(enumType);

            var table = new DataTable();
            table.Columns.Add(key, Type.GetType("System.String"));
            table.Columns.Add(value, Type.GetType("System.Int32"));
            table.Columns[key].Unique = true;
            for(int i = 0; i < values.Length; i++)
            {
                var dr = table.NewRow();
                dr[key] = names[i];
                dr[value] = (int)values.GetValue(i);
                table.Rows.Add(dr);
            }

            return table;
        }

        /// <summary>
        /// 根据键值对集合,创建一个DataTable对象（没有值）
        ///     键值对集合与Column关系如下：
        ///         Key：      列名
        ///         Value:     列类型
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public DataTable CreateNullableTable(Dictionary<string, string> columns)
        {
            if (columns == null || columns.Count <= 0) return null;

            var table = new DataTable();
            foreach(KeyValuePair<string, string> item in columns)
            {
                table.Columns.Add(item.Key, Type.GetType(item.Value));
            }

            return table;
        }

        /// <summary>
        /// 通过字符串列表创建一个DataTable对象，字符串格式如下：
        ///     1）a,b,c,d,e 或 a;b;c;d;e
        ///     2）a|int,b|string,c|bool,d|decimal
        /// </summary>
        /// <param name="nameString"></param>
        /// <returns></returns>
        public DataTable CreateNullableTable(string nameString)
        {
            string[] nameArray = nameString.Split(new[] { ',', ';' });
            var dt = new DataTable();
            foreach(string item in nameArray)
            {
                if(!string.IsNullOrEmpty(item))
                {
                    string[] subItems = item.Split('|');
                    if(subItems.Length == 2)
                    {
                        dt.Columns.Add(subItems[0], ConvertType(subItems[1]));
                    }
                    else
                    {
                        dt.Columns.Add(subItems[0]);
                    }
                }
            }

            return dt;
        }

        /// <summary>
        /// DataTable对象实例数据按条件排序
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="sorts">排序字段</param>
        /// <returns></returns>
        public DataTable Sort(DataTable dt, params string[] sorts)
        {
            if(dt != null && dt.Rows.Count > 0)
            {
                string temp = sorts.Aggregate("", (current, t) => current + (t + ","));
                dt.DefaultView.Sort = temp.TrimEnd(',');
            }

            return dt.DefaultView.ToTable();
        }

        #endregion

        #region 从DataTable中查询指定值
        /// <summary>
        /// 从DataTable查找指定值
        /// </summary>
        /// <param name="dt">DataTable实体对象</param>
        /// <param name="where">查询条件</param>
        /// <param name="strField">查询的字段名</param>
        /// <param name="rowIndex">行索引</param>
        /// <returns></returns>
        public object Select(DataTable dt, string where, string strField, int rowIndex = 0)
        {
            if (dt == null || dt.Rows.Count <= 0) return null;
            if (string.IsNullOrEmpty(where) || string.IsNullOrEmpty(strField)) return null;
            if (rowIndex < 0) return null;

            DataRow[] rows = dt.Select(where);
            if (rows == null || rows.Length == 0) return null;
            return rows[rowIndex][strField];
        }

        /// <summary>
        /// 从DataTable查找指定值
        /// </summary>
        /// <param name="dt">DataTable实体对象</param>
        /// <param name="where">查询条件</param>
        /// <param name="strField">查询的字段名</param>
        /// <returns></returns>
        public List<object> Select(DataTable dt, string where, string strField)
        {
            if (dt == null || dt.Rows.Count <= 0) return null;
            if (string.IsNullOrEmpty(where) || string.IsNullOrEmpty(strField)) return null;
            

            DataRow[] rows = dt.Select(where);
            if (rows == null || rows.Length == 0) return null;
            List<object> results = new List<object>();
            for(int i = 0; i < rows.Length; i++)
            {
                results.Add(rows[i][strField]);
            }

            return results;
        }

        /// <summary>
        /// 从DataTable查找指定值
        /// </summary>
        /// <param name="dt">DataTable对象</param>
        /// <param name="value">比较值（查找与该值一致的行）</param>
        /// <param name="strField">比较列</param>
        /// <returns></returns>
        public DataRow SelectReturnDataRow(DataTable dt, string value, string strField)
        {
            if (dt == null || dt.Rows.Count <= 0) return null;
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(strField)) return null;
            
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                if(dt.Rows[i][strField].ToString() == value)
                {
                    return dt.Rows[i];
                }
            }

            return null;
        }

        #endregion

        #region 筛选函数，将数据表中指定的值找出来
        /// <summary>
        /// 在DataTable中查找给定条件的记录，并返回新的DataTable
        /// </summary>
        /// <param name="dt">待查询的DataTable对象实例</param>
        /// <param name="strField">要查询的字段名称(条件名，为空时表示查询全部)</param>
        /// <param name="findValue">要查询的值</param>
        /// <param name="sortField">排序字段名</param>
        /// <param name="orderBy">升序或降序</param>
        /// <returns></returns>
        public DataTable GetFilterData(DataTable dt, string strField, string findValue, string sortField, string orderBy = "ASC")
        {
            var where = string.IsNullOrEmpty(strField) ? "" : strField + "=" + findValue;
            string sort = "";
            if(!string.IsNullOrEmpty(sortField))
            {
                sort = sortField + " " + orderBy;
            }
            return GetFilterData(dt, where, sort);
        }

        /// <summary>
        /// 筛选函数，将数据表中指定的值查询出来
        /// </summary>
        /// <param name="dt">DataTable对象实例</param>
        /// <param name="where">查询条件，例如：Id = 100 and XX = 'XXX'</param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public DataTable GetFilterData(DataTable dt, string where, string sort)
        {
            if (dt == null || dt.Rows.Count == 0) return null;

            try
            {
                DataTable _dt = null;
                DataRow[] drs = null;
                //查询
                if(!string.IsNullOrEmpty(where))
                {
                    drs = dt.Select(where);
                    //CopyToDataTable 必须；引用System.Data.DataSetExtensions
                    _dt = drs.Length > 0 ? drs.CopyToDataTable() : null;
                }
                if(!string.IsNullOrEmpty(sort) && _dt != null)
                {
                    _dt.DefaultView.Sort = sort;
                    _dt = _dt.DefaultView.ToTable();
                }
                dt.Dispose();
                return _dt;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取数组
        /// <summary>
        /// 根据DataTable，返回指定列数据列表，使用","进行分隔
        /// </summary>
        /// <param name="dt">DataTable实例对象</param>
        /// <param name="colName">查询列名</param>
        /// <returns></returns>
        public string GetColList(DataTable dt, string colName)
        {
            string sRet = "";
            if (dt == null || dt.Rows.Count == 0) return sRet;

            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append(dt.Rows[i][colName].ToString() + ",");
            }
            dt.Dispose();

            sRet = sb.ToString();
            if (sRet.Length > 0)
                return StringHelper.DelLastComma(sRet);

            return sRet;
        }

        /// <summary>
        /// 根据DataTable,返回指定列数据的String[]
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public string[] GetArrayString(DataTable dt, string colName)
        {
            if (dt == null || dt.Rows.Count == 0) return null;

            string[] arr = new string[dt.Rows.Count];
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                arr[i] = dt.Rows[i][colName].ToString();
            }
            dt.Dispose();
            return arr;
        }

        /// <summary>
        /// 根据DataTable,返回指定列数据的int[]
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public int[] GetArrayInt(DataTable dt, string colName)
        {
            if (dt == null || dt.Rows.Count == 0) return null;

            int[] arr = new int[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                arr[i] = ConvertHelper.Cint0(dt.Rows[i][colName].ToString());
            }
            dt.Dispose();
            return arr;
        }

        /// <summary>
        /// 根据DataTable,返回第一行各列数据到String[]
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string[] GetColumnsString(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0) return null;

            string[] arr = new string[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                arr[i] = dt.Rows[0][i].ToString();
            }
            dt.Dispose();
            return arr;
        }

        /// <summary>
        /// 根据DataTable，返回n行n列的数据到string[,]
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string[,] GetArrayString(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0) return null;

            int rows = dt.Rows.Count;
            int cols = dt.Columns.Count;
            string[,] arr = new string[rows, cols];
                
            for(int i = 0; i < rows;i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    arr[i, j] = dt.Rows[i][j].ToString();
                }
            }
            dt.Dispose();
            return arr;
        }
        #endregion

        #region 整理DataTable数据，以便于在有层次感的数据容器中使用
        /// <summary>
        /// 整理DataTable数据，以便于在有层次感的数据容器中使用
        /// </summary>
        /// <param name="dt">DataTable数据源</param>
        /// <param name="pkFiled">主键ID列名</param>
        /// <param name="parentIdFiled">父级ID列名</param>
        /// <returns></returns>
        public DataTable DataTableTidyUp(DataTable dt, string pkFiled, string parentIdFiled)
        {
            return DataTableTidyUp(dt, pkFiled, parentIdFiled, 0);
        }

        /// <summary>
        /// 整理DataTable数据，以便于在有层次感的数据容器中使用
        /// </summary>
        /// <param name="dt">DataTable数据源</param>
        /// <param name="pkFiled">主键ID列名</param>
        /// <param name="parentIdFiled">父级ID列名</param>
        /// <param name="parentId">父ID值，用于查询分类列表时，只显示指定一级分类和下面的全部分类</param>
        /// <returns></returns>
        public DataTable DataTableTidyUp(DataTable dt, string pkFiled, string parentIdFiled, int parentId)
        {
            //判断当前内存表中是否存在指定的主键列
            if (!dt.Columns.Contains(pkFiled) || !dt.Columns.Contains(parentIdFiled)) return null;

            //设定主键列
            dt.PrimaryKey = new DataColumn[] { dt.Columns[pkFiled] };

            //克隆内存表中的结构与约束
            DataTable tidyUpData = dt.Clone();

            //父ID列表，用于使用条件查询时，只将指定父ID节点（根节点）以及它下面的子节点显示出来，其他节点不显示
            string parentIDList = ",";
            foreach(DataRow item in dt.Rows)    //循环读取表中的记录
            {
                //获取父ID值
                int pid = int.Parse(item[parentIdFiled].ToString());
                //判断当前的父ID是否为0（即是否是根节点），为0则直接加入，否则寻找其父ID的位置
                if(pid == 0)
                {
                    //如果指定了只显示指定根节点以及它的子节点，则判断当前父节点是否为制定的父节点，不是则终止本次循环
                    if(parentId > 0 && int.Parse(item[pkFiled].ToString()) != parentId)
                    {
                        continue;
                    }
                    else
                    {
                        //如果指定了只显示指定根节点以及它的子节点，则将当前节点ID加入列表
                        if(parentId > 0)
                        {
                            parentIDList += item[pkFiled].ToString() + ",";
                        }
                        //添加一行记录
                        tidyUpData.ImportRow(item);
                        continue;
                    }
                }

                //如果指定了只显示指定根节点以及子节点，且当前父ID不存在父ID列表中，则终止本次循环
                if(parentId > 0 && parentIDList.IndexOf("," + pid + ",") < 0)
                {
                    continue;
                }
                //将当前ID加入列表中
                if(parentId > 0)
                {
                    parentIDList += item[pkFiled].ToString() + ",";
                }

                //寻找父ID的位置
                DataRow pdrow = tidyUpData.Rows.Find(pid);
                //获取父ID所在行索引号
                int index = tidyUpData.Rows.IndexOf(pdrow);

                int _pid = 0;
                //查找下一个位置的父ID与当前行的父ID是否一样，是则插入行向下移动
                do
                {
                    //索引号增加
                    index++;
                    if (index < tidyUpData.Rows.Count)
                    {
                        try
                        {
                            _pid = ConvertHelper.Cint0(tidyUpData.Rows[index][parentIdFiled]);
                        }
                        catch (Exception)
                        {
                            _pid = 0;
                        }
                    }
                    else
                    {
                        _pid = 0;
                    }
                }
                //如果下一行的父ID值与当前要插入的ID值一样，则循环继续
                while (pid != 0 && pid == _pid);

                //当前行创建新行
                DataRow currentRow = tidyUpData.NewRow();
                currentRow.ItemArray = item.ItemArray;
                //插入新行
                tidyUpData.Rows.InsertAt(currentRow, index);
            }

            return tidyUpData;
        }

        /// <summary>
        /// 整理dataTable数据，以便于在有层次感的数据容器中使用
        /// </summary>
        /// <param name="dt">DataTable数据源</param>
        /// <param name="pkField">主键ID列名</param>
        /// <param name="parentIDField">父级ID列名</param>
        /// <returns></returns>
        public DataSet DataSetTidyUp(DataTable dt, string pkField, string parentIDField)
        {
            DataSet dset = new DataSet();
            DataTable dt1 = DataTableTidyUp(dt, pkField, parentIDField);
            dset.Tables.Add(dt1);
            return dset;
        }
        #endregion

        #region DataTable排序
        /// <summary>
        /// DataTable排序
        /// </summary>
        /// <param name="dt">DataTable数据源</param>
        /// <param name="pkField">主键ID列名</param>
        /// <param name="parentIdField">父级ID列名</param>
        /// <param name="sortName">排序条件</param>
        /// <returns></returns>
        public DataTable DataTableTreeSort(DataTable dt, string pkField, string parentIdField = "ParentId",
                                        string sortName = "ParentId asc, SortId asc")
        {
            //判断当前内存表中是否存在指定的列
            if (!dt.Columns.Contains(pkField) || !dt.Columns.Contains(parentIdField)) return null;

            //设定主键列
            dt.PrimaryKey = new DataColumn[] { dt.Columns[pkField] };

            //----------------------------------------------------------
            // 先排序
            DataRow[] rows = dt.Select("", sortName);
            DataTable tmp = dt.Clone();
            tmp.Rows.Clear();
            foreach(DataRow row in rows)
            {
                tmp.ImportRow(row);
            }
            dt = tmp;

            //----------------------------------------------------------
            // 克隆内存表中的结构与约束
            DataTable dt1 = dt.Clone();     //克隆
            dt1.Rows.Clear();
            int ti = dt.Rows.Count;
            int tj = ti;
            int dtIndex = 0;
            string pid = "";
            
            for(int i = 0; i < ti; i++)
            {
                DataRow rs = dt.Rows[i];
                if(rs[parentIdField].ToString() != pid)
                {
                    pid = rs[parentIdField].ToString();
                    dtIndex = 0;
                }

                if(rs[parentIdField].ToString() == "0")
                {
                    dt.ImportRow(rs);
                }
                else
                {
                    if(dtIndex > 0)
                    {
                        if(dtIndex < ti - 1)
                        {
                            DataRow dr2 = dt1.NewRow();
                            dr2.ItemArray = rs.ItemArray;
                            dtIndex++;
                            dt1.Rows.InsertAt(dr2, dtIndex);
                        }
                    }
                    else
                    {
                        DataRow dr1 = dt1.Rows.Find(rs[parentIdField].ToString());
                        if(dr1 != null)
                        {
                            dtIndex = dt1.Rows.IndexOf(dr1);
                            if(dtIndex < ti - 1)
                            {
                                DataRow dr2 = dt1.NewRow();
                                dr2.ItemArray = rs.ItemArray;
                                dtIndex++;
                                dt1.Rows.InsertAt(dr2, dtIndex);
                            }
                        }
                    }
                }
            }

            return dt1;
        }
        #endregion
    }
}