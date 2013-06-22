using Jasen.Framework.Transform;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Jasen.Framework.Transform.Test
{
    
    
    /// <summary>
    ///这是 ResultTransferTest 的测试类，旨在
    ///包含所有 ResultTransferTest 单元测试
    ///</summary>
    [TestClass()]
    public class ResultTransferTest
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
        public void ParseTest()
        {
            IList<IList<string>> entityRows = new List<IList<string>>();
            entityRows.Add(new List<string>() { "3", "NameValue", "CodeValue", "ExchangeTypeValue", "6", "invalid" });

            var contracts = ResultTransfer.Parse<ContinousContract>(entityRows);

            Assert.IsNotNull(contracts);
            Assert.IsTrue(contracts.Count == 1);
            Assert.AreEqual(contracts[0].Code, "CodeValue");
            Assert.AreEqual(contracts[0].Name, "NameValue");
            Assert.AreEqual(contracts[0].ExchangeType, "ExchangeTypeValue");
            Assert.AreEqual(contracts[0].OrgidID, 3);
            Assert.AreEqual(contracts[0].ExchangeTypeValue, 6);
        }


        [TestMethod()]
        public void ParseWithInvalidArgTest()
        {
            IList<IList<string>> entityRows = new List<IList<string>>();
            entityRows.Add(new List<string>() { "sss", "NameValue", "CodeValue", "ExchangeTypeValue", "6", "invalid" });

            var contracts = ResultTransfer.Parse<ContinousContract>(entityRows);

            Assert.IsNotNull(contracts);
            Assert.IsTrue(contracts.Count == 1);
            Assert.AreEqual(contracts[0].Code, "CodeValue");
            Assert.AreEqual(contracts[0].Name, "NameValue");
            Assert.AreEqual(contracts[0].ExchangeType, "ExchangeTypeValue");
            Assert.AreEqual(contracts[0].OrgidID, 0);
            Assert.AreEqual(contracts[0].ExchangeTypeValue, 6);
        }

        [TestMethod()]
        public void ParseWithArgTest()
        {
            IList<IList<string>> entityRows = new List<IList<string>>();
            entityRows.Add(new List<string>() { "3", "NameValue", "ExchangeTypeValue", "6", "invalid" });
            var propertyNames = new List<string>() { "ExchangeTypeValue", "Name", "", "ExchangeType" };

            var contracts = ResultTransfer.Parse<ContinousContract>(entityRows, propertyNames.ToArray());

            Assert.IsNotNull(contracts);
            Assert.IsTrue(contracts.Count == 1);
            Assert.AreEqual(contracts[0].Code, null);
            Assert.AreEqual(contracts[0].Name, "NameValue");
            Assert.AreEqual(contracts[0].ExchangeType, "ExchangeTypeValue");
            Assert.AreEqual(contracts[0].OrgidID, 0);
            Assert.AreEqual(contracts[0].ExchangeTypeValue, 3);
        }
    }
}
