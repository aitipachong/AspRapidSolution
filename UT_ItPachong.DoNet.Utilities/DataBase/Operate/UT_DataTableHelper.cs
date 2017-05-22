using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using ItPachong.DoNet.Utilities.DataBase.Operate;
using System.IO;
using System.Collections.Generic;

namespace UT_ItPachong.DoNet.Utilities.DataBase.Operate
{
    [TestClass]
    public class UT_DataTableHelper
    {
        [TestMethod]
        public void UT_Merge_V1()
        {
            try
            {
                DataTable table1 = this.CreateTestDataTable1();
                DataTable table2 = this.CreateTestDataTable2();

                ItPachong.DoNet.Utilities.DataBase.Operate.DataTableHelper<Emptemployee> helper = new DataTableHelper<Emptemployee>();
                DataTable result = helper.Merge(table1, table2);
                if(result == null || result.Rows.Count <= 0)
                {
                    Assert.Fail("两个相同结构的DataTable对象合并失败");
                }
                else
                {
                    Assert.AreEqual(8, result.Rows.Count);
                }
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void UT_WriteDataTableToXml_V1()
        {
            try
            {
                DataTable table = this.CreateTestDataTable();
                string xmlFilePath = Path.Combine("D:/", "TempDataTable.xml");
                if (File.Exists(xmlFilePath)) File.Delete(xmlFilePath);

                ItPachong.DoNet.Utilities.DataBase.Operate.DataTableHelper<Emptemployee> helper = new DataTableHelper<Emptemployee>();
                bool result = helper.WriteDataTableToXml(table, xmlFilePath);

                Assert.AreEqual(true, result);
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void UT_SelectDataTableContents_V1()
        {
            try
            {
                DataTable table = this.CreateTestDataTable();
                string filterExpression = "Name='赖强'";

                ItPachong.DoNet.Utilities.DataBase.Operate.DataTableHelper<Emptemployee> helper = new DataTableHelper<Emptemployee>();
                DataRow[] rows = helper.SelectDataTableContents(table, filterExpression);

                if (rows == null || rows.Length <= 0)
                    Assert.Fail("DataTable对象按条件查询失败");
                else
                    Assert.AreEqual(1, rows.Length);
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void UT_ToDataTable_V1()
        {
            try
            {
                List<Emptemployee> list = this.CreateTestList();
                ItPachong.DoNet.Utilities.DataBase.Operate.DataTableHelper<Emptemployee> helper = new DataTableHelper<Emptemployee>();
                DataTable dt = helper.ToDataTable(list);
                if(dt == null || dt.Rows.Count <= 0)
                {
                    Assert.Fail("对象实体集合转换为DataTable失败");
                }
                else
                {
                    Assert.AreEqual(8, dt.Rows.Count);
                }
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void UT_ToList_V1()
        {
            try
            {
                DataTable table = this.CreateTestDataTable();
                ItPachong.DoNet.Utilities.DataBase.Operate.DataTableHelper<Emptemployee> helper = new DataTableHelper<Emptemployee>();
                IList<Emptemployee> iList = helper.ToList<Emptemployee>(table);

                if(iList == null || iList.Count <= 0)
                {
                    Assert.Fail("对象实体集合转换为DataTable失败");
                }
                else
                {
                    Assert.AreEqual(8, iList.Count);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void UT_EnumToDataTable_V1()
        {
            try
            {
                ItPachong.DoNet.Utilities.DataBase.Operate.DataTableHelper<Emptemployee> helper = new DataTableHelper<Emptemployee>();
                DataTable table = helper.EnumToDataTable(typeof(EmptemployeeType), "EnumName", "EnumValue");

                if (table == null || table.Rows.Count <= 0)
                {
                    Assert.Fail("枚举转换为DataTable失败");
                }
                else
                {
                    Assert.AreEqual(6, table.Rows.Count);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void UT_Sort_V1()
        {
            try
            {
                DataTable table = this.CreateTestDataTable();
                ItPachong.DoNet.Utilities.DataBase.Operate.DataTableHelper<Emptemployee> helper = new DataTableHelper<Emptemployee>();
                DataTable newTable = helper.Sort(table, "id DESC");

                if (newTable == null || newTable.Rows.Count <= 0)
                {
                    Assert.Fail("排序失败");
                }
                else
                {
                    Assert.AreEqual(8, int.Parse(newTable.Rows[0]["Id"].ToString()));
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void UT_Select_V1()
        {
            try
            {
                DataTable table = this.CreateTestDataTable();
                ItPachong.DoNet.Utilities.DataBase.Operate.DataTableHelper<Emptemployee> helper = new DataTableHelper<Emptemployee>();
                object result = helper.Select(table, "id > 4", "Name");
                if(result == null)
                {
                    Assert.Fail("查询失败");
                }
                else
                {
                    Assert.AreEqual("冯翔", result.ToString());
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void UT_Select_V2()
        {
            try
            {
                DataTable table = this.CreateTestDataTable();
                ItPachong.DoNet.Utilities.DataBase.Operate.DataTableHelper<Emptemployee> helper = new DataTableHelper<Emptemployee>();
                DataRow result = helper.Select(table, "赖强", "Name");
                if (result == null)
                {
                    Assert.Fail("查询失败");
                }
                else
                {
                    Assert.AreEqual(1, int.Parse(result["Id"].ToString()));
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        private DataTable CreateTestDataTable()
        {
            DataTable dt = new DataTable("人资组人员信息");

            //Columns
            dt.Columns.Add("Id", System.Type.GetType("System.Int32"));
            dt.Columns.Add("JobNumber", System.Type.GetType("System.String"));
            dt.Columns.Add("Name", System.Type.GetType("System.String"));
            dt.Columns.Add("EMail", System.Type.GetType("System.String"));

            //Data
            for (int i = 1; i < 9; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Id"] = i;
                switch (i)
                {
                    case 1:
                        dr["JobNumber"] = "0000014610";
                        dr["Name"] = "赖强";
                        dr["EMail"] = "laiqiang@chinasoftinc.com";
                        break;
                    case 2:
                        dr["JobNumber"] = "0000039419";
                        dr["Name"] = "王涛";
                        dr["EMail"] = "wangtao14@chinasoftinc.com";
                        break;
                    case 3:
                        dr["JobNumber"] = "0000048312";
                        dr["Name"] = "贾晓康";
                        dr["EMail"] = "jiaxiaokang@chinasoftinc.com";
                        break;
                    case 4:
                        dr["JobNumber"] = "0000016880";
                        dr["Name"] = "郭宁波";
                        dr["EMail"] = "guoningbo@chinasoftinc.com";
                        break;
                    case 5:
                        dr["JobNumber"] = "0000077365";
                        dr["Name"] = "冯翔";
                        dr["EMail"] = "fengxiang@chinasoftinc.com";
                        break;
                    case 6:
                        dr["JobNumber"] = "0000077900";
                        dr["Name"] = "苏晓博";
                        dr["EMail"] = "suxiaobo@chinasoftinc.com";
                        break;
                    case 7:
                        dr["JobNumber"] = "0000076916";
                        dr["Name"] = "陈润坤";
                        dr["EMail"] = "chenrunkun@chinasoftinc.com";
                        break;
                    case 8:
                        dr["JobNumber"] = "0000090532";
                        dr["Name"] = "谢紫薇";
                        dr["EMail"] = "xieziwei@chinasoftinc.com";
                        break;
                    default:
                        break;
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        private DataTable CreateTestDataTable1()
        {
            DataTable dt = new DataTable("人资组人员信息");

            //Columns
            dt.Columns.Add("Id", System.Type.GetType("System.Int32"));
            dt.Columns.Add("JobNumber", System.Type.GetType("System.String"));
            dt.Columns.Add("Name", System.Type.GetType("System.String"));
            dt.Columns.Add("EMail", System.Type.GetType("System.String"));

            //Data
            for (int i = 1; i < 5; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Id"] = i;
                switch (i)
                {
                    case 1:
                        dr["JobNumber"] = "0000014610";
                        dr["Name"] = "赖强";
                        dr["EMail"] = "laiqiang@chinasoftinc.com";
                        break;
                    case 2:
                        dr["JobNumber"] = "0000039419";
                        dr["Name"] = "王涛";
                        dr["EMail"] = "wangtao14@chinasoftinc.com";
                        break;
                    case 3:
                        dr["JobNumber"] = "0000048312";
                        dr["Name"] = "贾晓康";
                        dr["EMail"] = "jiaxiaokang@chinasoftinc.com";
                        break;
                    case 4:
                        dr["JobNumber"] = "0000016880";
                        dr["Name"] = "郭宁波";
                        dr["EMail"] = "guoningbo@chinasoftinc.com";
                        break;
                    default:
                        break;
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        private DataTable CreateTestDataTable2()
        {
            DataTable dt = new DataTable("人资组人员信息");

            //Columns
            dt.Columns.Add("Id", System.Type.GetType("System.Int32"));
            dt.Columns.Add("JobNumber", System.Type.GetType("System.String"));
            dt.Columns.Add("Name", System.Type.GetType("System.String"));
            dt.Columns.Add("EMail", System.Type.GetType("System.String"));

            //Data
            for (int i = 1; i < 5; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Id"] = i;
                switch (i)
                {
                    case 1:
                        dr["JobNumber"] = "0000077365";
                        dr["Name"] = "冯翔";
                        dr["EMail"] = "fengxiang@chinasoftinc.com";
                        break;
                    case 2:
                        dr["JobNumber"] = "0000077900";
                        dr["Name"] = "苏晓博";
                        dr["EMail"] = "suxiaobo@chinasoftinc.com";
                        break;
                    case 3:
                        dr["JobNumber"] = "0000076916";
                        dr["Name"] = "陈润坤";
                        dr["EMail"] = "chenrunkun@chinasoftinc.com";
                        break;
                    case 4:
                        dr["JobNumber"] = "0000090532";
                        dr["Name"] = "谢紫薇";
                        dr["EMail"] = "xieziwei@chinasoftinc.com";
                        break;
                    default:
                        break;
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        private List<Emptemployee> CreateTestList()
        {
            List<Emptemployee> list = new List<Emptemployee>();
            for(int i = 1; i < 9; i++)
            {
                Emptemployee emp = new Emptemployee();
                emp.Id = i;
                switch (i)
                {
                    case 1:
                        emp.JobNumber = "0000014610";
                        emp.Name = "赖强";
                        emp.EMail = "laiqiang@chinasoftinc.com";
                        break;
                    case 2:
                        emp.JobNumber = "0000039419";
                        emp.Name = "王涛";
                        emp.EMail = "wangtao14@chinasoftinc.com";
                        break;
                    case 3:
                        emp.JobNumber = "0000048312";
                        emp.Name = "贾晓康";
                        emp.EMail = "jiaxiaokang@chinasoftinc.com";
                        break;
                    case 4:
                        emp.JobNumber  = "0000016880";
                        emp.Name = "郭宁波";
                        emp.EMail = "guoningbo@chinasoftinc.com";
                        break;
                    case 5:
                        emp.JobNumber = "0000077365";
                        emp.Name = "冯翔";
                        emp.EMail = "fengxiang@chinasoftinc.com";
                        break;
                    case 6:
                        emp.JobNumber = "0000077900";
                        emp.Name = "苏晓博";
                        emp.EMail = "suxiaobo@chinasoftinc.com";
                        break;
                    case 7:
                        emp.JobNumber = "0000076916";
                        emp.Name = "陈润坤";
                        emp.EMail = "chenrunkun@chinasoftinc.com";
                        break;
                    case 8:
                        emp.JobNumber = "0000090532";
                        emp.Name = "谢紫薇";
                        emp.EMail = "xieziwei@chinasoftinc.com";
                        break;
                    default:
                        break;
                }
                list.Add(emp);
            }

            return list;
        }
    }

    public class Emptemployee
    {
        public int Id { get; set; }
        public string JobNumber { get; set; }
        public string Name { get; set; }

        public string EMail { get; set; }
    }

    public enum EmptemployeeType
    {
        ProductManager,
        ProjectManager,
        VersionManager,
        TestManager,
        Developer,
        Tester
    }
}
