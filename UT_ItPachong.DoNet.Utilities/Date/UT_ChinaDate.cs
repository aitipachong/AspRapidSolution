using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ItPachong.DoNet.Utilities.Date;

namespace UT_ItPachong.DoNet.Utilities.Date
{
    [TestClass]
    public class UT_ChinaDate
    {
        [TestMethod]
        public void UT_GetDaysByMonth_V1()
        {
            try
            {
                int result = ChinaDate.GetDaysByMonth(2017, 5);
                Assert.AreEqual(31, result);
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void UT_GetMondayDateByDate_V1()
        {
            try
            {
                DateTime result = ChinaDate.GetMondayDateByDate(DateTime.Now);
                Assert.AreEqual("2017-05-29", result.ToString("yyyy-MM-dd"));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void UT_GetChinaDate_V1()
        {
            try
            {
                CNDate result = ChinaDate.GetChinaDate(DateTime.Now);
                Assert.IsTrue(result == null ? false : true);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }
    }
}
