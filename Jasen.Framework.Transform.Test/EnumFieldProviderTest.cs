using Jasen.Framework.Transform;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Jasen.Framework.Transform.Test
{
    
    
    /// <summary>
    ///这是 EnumFieldProviderTest 的测试类，旨在
    ///包含所有 EnumFieldProviderTest 单元测试
    ///</summary>
    [TestClass()]
    public class EnumFieldProviderTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion
 

        [TestMethod()]
        public void GetItemsTest()
        {
            IList<EnumItem<ExchangeType>> expected = null;  
            IList<EnumItem<ExchangeType>> actual = EnumFieldProvider.GetItems<ExchangeType>(true);
            Assert.AreEqual(actual.Count, 2);
            Assert.AreEqual(actual[0].Value, ExchangeType.All);
            Assert.AreEqual(actual[1].Value, ExchangeType.SZSE);
        }

        [TestMethod()]
        public void GetItemsWithFalseArgTest()
        { 
            IList<EnumItem<ExchangeType>> actual = EnumFieldProvider.GetItems<ExchangeType>();
            Assert.AreEqual(actual.Count, 1);
            Assert.AreEqual(actual[0].Value, ExchangeType.SZSE);
        }
    }
}
