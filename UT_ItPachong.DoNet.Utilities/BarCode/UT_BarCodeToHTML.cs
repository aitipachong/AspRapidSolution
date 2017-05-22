using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ItPachong.DoNet.Utilities.BarCode;

namespace UT_ItPachong.DoNet.Utilities.BarCode
{
    [TestClass]
    public class UT_BarCodeToHTML
    {
        [TestMethod]
        public void UT_Get39_V1()
        {
            try
            {
                string result = BarCodeToHTML.Get39("laiqiang", 10, 4);
                if(!string.IsNullOrEmpty(result))
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.Fail("生成HTML格式的条形码错误!!");
                }
            }
            catch(Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }


        [TestMethod]
        public void UT_GetEAN13()
        {
            try
            {
                string result = BarCodeToHTML.GetEAN13("!@#$", 10, 4);
                if (!string.IsNullOrEmpty(result))
                {
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.Fail("生成HTML格式的条形码错误!!");
                }

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
