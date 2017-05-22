using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ItPachong.DoNet.Utilities.API;

namespace UT_ItPachong.DoNet.Utilities.API
{
    [TestClass]
    public class UT_MAC
    {
        [TestMethod]
        public void UT_GetMacAddressByNetBios_V1()
        {
            try
            {
                string mac = MAC.GetMacAddressByNetBios();
                if (string.IsNullOrEmpty(mac))
                {
                    Assert.Fail("获取MAC地址失败！");
                }
                else
                {
                    Assert.IsTrue(true);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }
    }
}
