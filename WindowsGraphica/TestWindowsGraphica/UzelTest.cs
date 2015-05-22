using WindowsGraphica;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TestWindowsGraphica
{
    
    
    /// <summary>
    ///Это класс теста для UzelTest, в котором должны
    ///находиться все модульные тесты UzelTest
    ///</summary>
    [TestClass()]
    public class UzelTest
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


        /// <summary>
        ///Тест для Конструктор Uzel
        ///</summary>
///        [TestMethod()]
//        public void UzelConstructorTest()
//        {
//            Uzel target = new Uzel();
//            //Assert.Inconclusive("TODO: реализуйте код для проверки целевого объекта");
//        }

        /// <summary>
        ///Тест для CompareTo
        ///</summary>
        [TestMethod()]
        public void CompareToTest()
        {
            List<Uzel> Uzli = new List<Uzel>(); 

            Uzel target = new Uzel(); // TODO: инициализация подходящего значения
            target.TipUzla = 0;
            target.UNom = 110;
            Uzli.Add(target);

            target = new Uzel(); // TODO: инициализация подходящего значения
            target.TipUzla = -1;
            target.UNom = 10;
            Uzli.Add(target);

            target = new Uzel(); // TODO: инициализация подходящего значения
            target.TipUzla = 0;
            target.UNom = 220;
            Uzli.Add(target);

            target = new Uzel(); // TODO: инициализация подходящего значения
            target.TipUzla = 0;
            target.UNom = 220;
            Uzli.Add(target);

            target = new Uzel(); // TODO: инициализация подходящего значения
            target.TipUzla = 1;
            target.UNom = 220;
            Uzli.Add(target);

            target = new Uzel(); // TODO: инициализация подходящего значения
            target.TipUzla = 1;
            target.UNom = 110;
            Uzli.Add(target);

            target = new Uzel(); // TODO: инициализация подходящего значения
            target.TipUzla = 0;
            target.UNom = 10;
            Uzli.Add(target);

            target = new Uzel(); // TODO: инициализация подходящего значения
            target.TipUzla = 0;
            target.UNom = 10;
            Uzli.Add(target);


            Uzli.Sort();

            Uzel expected = new Uzel(); // TODO: инициализация подходящего значения
            expected.TipUzla = -1;
            expected.UNom = 10;
            
           // object obj = null; // TODO: инициализация подходящего значения
           // int expected = 0; // TODO: инициализация подходящего значения
           // int actual;
           // actual = target.CompareTo(obj);
            Assert.AreEqual(expected.TipUzla, Uzli[7].TipUzla);
            //Assert.Inconclusive("Проверьте правильность этого метода теста.");
        }
        ///01234567890123456789
        //"0201         10.   500.0  5000.0   400.0      .0      0,   525.0  -9999.   9999."
        //"0202          10  224.99    0.23"
        [TestMethod()]
        public void LoadFromCDUTest()
        {
            const string pusto = "        ";
            Uzel target = new Uzel();

            string str = "0201         10.   500.0  5000.0   400.0      .0      0,   525.0  -9999.   9999.";
            if (target.LoadFromCDU(str))
            {
                int nomer = 10;
                decimal U_nom = 500;
                decimal P_load = 5000;
                decimal Q_load = 400;
                decimal P_gen = 0;
                decimal Q_gen = 0;
                decimal U_zad = 525;
                int q_min = -9999;
                int q_max = 9999;
                Assert.AreEqual(nomer, target.NomerUzla);
                Assert.AreEqual(U_nom, target.UNom);
                Assert.AreEqual(P_load, target.PLoad);
                Assert.AreEqual(Q_load, target.QLoad);
                Assert.AreEqual(P_gen, target.PGen);
                Assert.AreEqual(Q_gen, target.QGen);
                Assert.AreEqual(U_zad, target.UZad);
                Assert.AreEqual(q_min, target.QMin);
                Assert.AreEqual(q_max, target.QMax);
                Assert.AreEqual(1, target.TipUzla);
            }
            else
                Assert.Inconclusive("Ошибка");


            //"0201   0     140     220                                                        "/
            target = new Uzel();
            str = "0201   0      10     220                                                        ";
            if (target.LoadFromCDU(str))
            {
                int nomer = 10;
                decimal U_nom = 220;
                decimal P_load = 0;
                decimal Q_load = 0;
                decimal P_gen = 0;
                decimal Q_gen = 0;
                decimal U_mod = 0;
                int q_min = 0;
                int q_max = 0;
                Assert.AreEqual(nomer, target.NomerUzla);
                Assert.AreEqual(U_nom, target.UNom);
                Assert.AreEqual(P_load, target.PLoad);
                Assert.AreEqual(Q_load, target.QLoad);
                Assert.AreEqual(P_gen, target.PGen);
                Assert.AreEqual(Q_gen, target.QGen);
                Assert.AreEqual(U_mod, target.UMod);
                Assert.AreEqual(q_min, target.QMin);
                Assert.AreEqual(q_max, target.QMax);
                Assert.AreEqual(0, target.TipUzla);
            }
            else
                Assert.Inconclusive("Ошибка");

            str = "0202          10  224.99    0.23";
            if (target.LoadFromCDU(str))
            {
                int nomer = 10;
                decimal U_mod = new decimal(224.99);
                decimal angel = new decimal(0.23);
                
                Assert.AreEqual(nomer, target.NomerUzla);
                Assert.AreEqual(U_mod, target.UMod);
                Assert.AreEqual(angel, target.Angle);
                
            }
            else
                Assert.Inconclusive("Ошибка во второй строке");
            str = "0301          10                     653";
            if (str.Substring(16,8)==pusto)
            {
                if (target.LoadFromCDU(str))
                {
                    int nomer = 10;
                    decimal b_sh = new decimal(653);                    
                    Assert.AreEqual(nomer, target.NomerUzla);                    
                    Assert.AreEqual(b_sh, target.BSh);

                }
                else
                    Assert.Inconclusive("Ошибка во второй строке");   
            }
            else
                if (Convert.ToInt16(str.Substring(16, 8)) == 0)
                { }
                else
                    Assert.Inconclusive("Ошибка во третей строке");

            str = "0302           1                     653";
            if (target.LoadFromCDU(str))
            {
                Assert.Inconclusive("Ошибка Иначе");
            }

        }


        [TestMethod()]
        public void SaveForCDUTest()
        {
            Uzel target = new Uzel();
            string str = "0201         10.   500.0  5000.0   400.0      .0      0,   525.0  -9999.   9999.";
            if (target.LoadFromCDU(str))
            {
                str = "0202          10  224.99    0.23";
                if (target.LoadFromCDU(str))
                {
                    List<string> sp = new List<string>(target.SaveToCDU());
                    Assert.AreEqual(2, sp.Count);
                }
                else
                    Assert.Inconclusive("Ошибка во второй строке");
                
            }
            else
                Assert.Inconclusive("Ошибка в строке");
        }
    }
}
