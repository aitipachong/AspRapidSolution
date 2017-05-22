using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using ItPachong.DoNet.Utilities.CSV;
using System.IO;

namespace UT_ItPachong.DoNet.Utilities.CSV
{
    [TestClass]
    public class UT_CsvHelper
    {
        [TestMethod]
        public void UT_Dt2Csv_V1()
        {
            DataTable dt = CreateTestDataTable();
            string filePath = Path.Combine("D:/", dt.TableName + ".csv");
            if (File.Exists(filePath)) File.Delete(filePath);
            try
            {
                bool result = CsvHelper.Dt2Csv(dt, filePath);
                if(!result)
                {
                    Assert.Fail("DataTable对象转换为CSV文件失败");
                }
                else
                {
                    Assert.IsTrue(File.Exists(filePath));
                }
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void UT_Csv2Dt_V1()
        {
            string filePath = Path.Combine("D:/", "人资组人员信息.csv");
            if (File.Exists(filePath))
            {
                DataTable dt = CsvHelper.Csv2Dt(filePath);
                if(dt != null && dt.Rows != null && dt.Rows.Count > 0)
                {
                    Assert.AreEqual(8, dt.Rows.Count);
                }
                else
                {
                    Assert.Fail("CSV文件读取失败");
                }
            } 
            else
            {
                Assert.Fail("CSV文件不存在");
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
            for(int i = 1; i < 9; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Id"] = i;
                switch(i)
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
    }
}
