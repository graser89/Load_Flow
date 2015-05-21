using WindowsGraphica;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestWindowsGraphica
{
    
    
    /// <summary>
    ///Это класс теста для vetvTest, в котором должны
    ///находиться все модульные тесты vetvTest
    ///</summary>
    [TestClass()]
    public class vetvTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Получает или устанавливает контекст теста, в котором предоставляются
        ///сведения о текущем тестовом запуске и обеспечивается его функциональность.
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

        #region Дополнительные атрибуты теста
        // 
        //При написании тестов можно использовать следующие дополнительные атрибуты:
        //
        //ClassInitialize используется для выполнения кода до запуска первого теста в классе
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //ClassCleanup используется для выполнения кода после завершения работы всех тестов в классе
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //TestInitialize используется для выполнения кода перед запуском каждого теста
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //TestCleanup используется для выполнения кода после завершения каждого теста
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        //"0301   0       1     149    0.82    7.44     -95                "/
        /// <summary>
        ///Тест для LoadFromCDU
        ///</summary>
        [TestMethod()]
        public void LoadFromCDUTest()
        {
             
            vetv target = new vetv(); // TODO: инициализация подходящего значения
            string str = "0301   0       1     149    0.82    7.44     -95                ";
            bool actual = target.LoadFromCDU(str);            
            bool expected = true; // TODO: инициализация подходящего значения                        
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(1, target.Nomer_Uzla_Nachal);
            Assert.AreEqual(149, target.Nomer_Uzla_Konca);
            Assert.AreEqual(new decimal(0.82), target.R);
            Assert.AreEqual(new decimal(7.44), target.X);
            Assert.AreEqual(new decimal(-95), target.Bc);
            Assert.AreEqual(new decimal(0), target.Kt1);
            Assert.AreEqual(new decimal(0), target.Kt2);
            //Assert.Inconclusive("Проверьте правильность этого метода теста.");
        }
    }
}
