using WindowsGraphica;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;

namespace TestWindowsGraphica
{
    
    
    /// <summary>
    ///Это класс теста для ShemaTest, в котором должны
    ///находиться все модульные тесты ShemaTest
    ///</summary>
    [TestClass()]
    public class ShemaTest
    {
        bool fl1 = false;  //true -плоский старт
        bool fl2 = false;   //стартовый алгоритм
        bool fl3 = true;  //true-поляр  false- прям

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
        ///Тест для Raschet
        ///одна ветвь
        ///</summary>
        [TestMethod()]
        public void RaschetTest()
        {
            bool fl1 = true;  //true -плоский старт
            bool fl2 = true;   //стартовый алгоритм
            bool fl3 = true;  //true-поляр  false- прям
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/PROBA1.CDU");
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (106.10);
                double eps = (0.05);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        /// <summary>
        /// трансформатор
        /// </summary>
        [TestMethod()]
        public void RaschetTest2()
        {
            bool fl1 = true;  //true -плоский старт
            bool fl2 = false;   //стартовый алгоритм
            bool fl3=true;  //true-поляр  false- прям
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/PROBA2.CDU");
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (9.65);
                double eps = (0.01);

                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        /// <summary>
        /// две ветви
        /// </summary>
        [TestMethod()]
        public void RaschetTest3()
        {
            bool fl1 = true;  //true -плоский старт
            bool fl2 = false;   //стартовый алгоритм
            bool fl3=true;  //true-поляр  false- прям
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/PROBA3.CDU");
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (98.51);
                double U3 = (96);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        /// <summary>
        /// две линии с проводимосью на землю
        /// </summary>
        [TestMethod()]
        public void RaschetTest4()
        {
            bool fl1 = true;  //true -плоский старт
            bool fl2 = false;   //стартовый алгоритм
            bool fl3=true;  //true-поляр  false- прям
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/PROBA4.CDU");
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (96.92);
                double U3 = (94.27);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// две линии  и трансформатор
        ///
        [TestMethod()]
        public void RaschetTest5()
        {
            bool fl1 = true;  //true -плоский старт
            bool fl2 = false;   //стартовый алгоритм
            bool fl3=true;  //true-поляр  false- прям
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/PROBA5.CDU");
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (96.64);
                double U3 = (93.13);
                double U4 = (8.09);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }

        ///
        /// кольцо
        ///
        [TestMethod()]
        public void RaschetTest6()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/PROBA6.CDU");
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U2 = (231.9);
                double U3 = (231.59);
                double U4 = (233.27);
                double d2 = (-3.02);
                double d3 = (-4.19);
                double d4 = (-2.84);

                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }

        ///
        /// линия +кольцо
        ///
        [TestMethod()]
        public void RaschetTest7()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/PROBA7.CDU");
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U2 = (236.11);
                double U3 = (232.52);
                double U4 = (232.03);
                double U5 = (233.55);
                double d2 = (-2.25);
                double d3 = (-4.81);
                double d4 = (-6.16);
                double d5 = (-4.98);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// линия +кольцо+второй источник
        ///
        [TestMethod()]
        public void RaschetTest8()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/PROBA8.CDU");
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U2 = (234.00);
                double U3 = (233.52);
                double U4 = (237.35);
                double U5 = (234.76);
                double d2 = (-2.56);
                double d3 = (-3.85);
                double d4 = (-3.28);
                double d5 = (-3.92);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }

        ///
        /// линия +кольцо+опорный узел
        ///
        [TestMethod()]
        public void RaschetTest9()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/PROBA9.CDU");
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U2 = (236.62);
                double U3 = (234.83);
                double U4 = (237.0);
                double U5 = (235.98);
                double d2 = (-2.49);
                double d3 = (-5.12);
                double d4 = (-6.57);
                double d5 = (-5.28);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }

        ///
        /// магистральная линия 5 пс
        ///
        [TestMethod()]
        public void RaschetTest10()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/PROBA10.CDU");
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (224.37);
                double U2 = (209.93);
                double U3 = (198.94);
                double U4 = (191.79);
                double U5 = (188.14);
                double d1 = (-2.99);
                double d2 = (-5.7);
                double d3 = (-7.97);
                double d4 = (-9.65);
                double d5 = (-10.52);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test1.   Всю информацию по тестам смотри в моем блокноте. Вкратце - сначала 21 тест без ОГУ, потом 20 тестов с ГУ.
        ///
        [TestMethod()]
        public void Test1()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test1.CDU");    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (115.0);
                double U2 = (111.09);
                double U3 = (109.67);
                double U4 = (109.69);
                double U5 = (109.15);
                double d1 = (0.0);
                double d2 = (-2.55);
                double d3 = (-3.13);
                double d4 = (-3.11);
                double d5 = (-3.34);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }

        ///
        /// test2.   
        ///
        [TestMethod()]
        public void Test2()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test2.CDU"); ;    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (115.0);
                double U2 = (110.48);
                double U3 = (109.13);
                double U4 = (109.14);
                double U5 = (108.67);
                double d1 = (0.0);
                double d2 = (-3.08);
                double d3 = (-3.65);
                double d4 = (-3.63);
                double d5 = (-3.85);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test3.   
        ///
        [TestMethod()]
        public void Test3()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test3.CDU"); ;    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (115.0);
                double U2 = (112.67);
                double U3 = (110.27);
                double U4 = (109.65);
                double d1 = (0.0);
                double d2 = (-1.59);
                double d3 = (-2.51);
                double d4 = (-2.82);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test4.   
        ///
        [TestMethod()]
        public void Test4()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test4.CDU"); ;    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (110.01);
                double U2 = (111.36);
                double U3 = (113.11);
                double U4 = (112.58);
                double U5 = (115.0);
                double d1 = (-2.25);
                double d2 = (-1.37);
                double d3 = (-0.7);
                double d4 = (-0.96);
                double d5 = (0.0);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test5.   
        ///
        [TestMethod()]
        public void Test5()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test5.CDU"); ;    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (230.0);
                double U2 = (216.11);
                double U3 = (211.93);
                double U4 = (213.6);
                double d1 = (0.0);
                double d2 = (-4.75);
                double d3 = (-5.85);
                double d4 = (-5.75);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test6.   
        ///
        [TestMethod()]
        public void Test6()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("../../../../../FlowTests/test6.CDU"); ;    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, false, fl3);
                double U1 = (230.0);
                double U2 = (214.91);
                double U3 = (200.55);
                double U4 = (206.82);
                double d1 = (0.0);
                double d2 = (-4.78);
                double d3 = (-8.89);
                double d4 = (-7.15);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test7.   
        ///
        [TestMethod()]
        public void Test7()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test7.CDU"); ;    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (230.0);
                double U2 = (234.45);
                double U3 = (231.12);
                double U4 = (231.74);
                double d1 = (0.0);
                double d2 = (-5.36);
                double d3 = (-6.27);
                double d4 = (-6.23);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test8.   
        ///
        [TestMethod()]
        public void Test8()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test8.CDU"); ;    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (230.0);
                double U2 = (212.41);
                double U3 = (208.6);
                double U4 = (209.27);
                double d1 = (0.0);
                double d2 = (-4.81);
                double d3 = (-5.92);
                double d4 = (-5.87);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test9.   
        ///
        [TestMethod()]
        public void Test9()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test9.CDU"); ;    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (230.0);
                double U2 = (219.94);
                double U3 = (216.3);
                double U4 = (216.95);
                double d1 = (0.0);
                double d2 = (-4.7);
                double d3 = (-5.73);
                double d4 = (-5.69);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test10.   
        ///
        [TestMethod()]
        public void Test10()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test10.CDU"); ;    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (230.0);
                double U2 = (213.7);
                double U3 = (209.83);
                double U4 = (211.53);
                double d1 = (0.0);
                double d2 = (-4.68);
                double d3 = (-5.75);
                double d4 = (-5.68);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test11.   
        ///
        [TestMethod()]
        public void Test11()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test11.CDU"); ;    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (230.0);
                double U2 = (221.56);
                double U3 = (108.78);
                double U4 = (110.63);
                double d1 = (0.0);
                double d2 = (-2.75);
                double d3 = (-8.61);
                double d4 = (-7.84);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test12.   
        ///
        [TestMethod()]
        public void Test12()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test12.CDU"); ;    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (230.0);
                double U2 = (221.93);
                double U3 = (112.2);
                double U4 = (113.99);
                double d1 = (0.0);
                double d2 = (-2.76);
                double d3 = (-5.95);
                double d4 = (-5.22);
                double eps = (0.01);    //не пройден, когда два тр-ра
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test12(1).   
        ///
        [TestMethod()]
        public void Test13()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test12(1).CDU"); ;    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (230.0);
                double U2 = (221.62);
                double U3 = (111.28);
                double U4 = (113.08);
                double d1 = (0.0);
                double d2 = (-2.75);
                double d3 = (-5.97);
                double d4 = (-5.23);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test13.   
        ///
        [TestMethod()]
        public void Test14()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test13.CDU"); ;    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (230.0);
                double U2 = (218.13);
                double U3 = (106.81);
                double U4 = (108.69);
                double d1 = (0.0);
                double d2 = (-2.61);
                double d3 = (-8.67);
                double d4 = (-7.87);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test14.   
        ///
        [TestMethod()]
        public void Test15()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test14.CDU");    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (118.0);
                double U2 = (112.47);
                double U3 = (323.26);
                double U4 = (324.03);
                double d1 = (0.0);
                double d2 = (-3.93);
                double d3 = (-6.93);
                double d4 = (-6.02);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test15.   
        ///
        [TestMethod()]
        public void Test16()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("../../../../../FlowTests/test15.CDU"); ;    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(true, false, true);
                double U1 = (235.0);
                double U2 = (225.86);
                double U3 = (106.39);
                double U4 = (104.18);
                double U5 = (103.68);
                double d1 = (0.0);
                double d2 = (-1.69);
                double d3 = (-8.09);
                double d4 = (-8.62);
                double d5 = (-8.54);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test16.   
        ///
        [TestMethod()]
        public void Test17()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test16.CDU"); ;    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (237.0);
                double U2 = (228.99);
                double U3 = (231.14);
                double U4 = (224.29);
                double U5 = (111.96);
                double U6 = (109.48);
                double U7 = (107.87);
                double U8 = (108.66);
                double d1 = (0.0);
                double d2 = (-2.11);
                double d3 = (-1.69);
                double d4 = (-3.37);
                double d5 = (-8.85);
                double d6 = (-9.83);
                double d7 = (-10.57);
                double d8 = (-10.26);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).UMod - U6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).UMod - U7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).UMod - U8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).Angle * (double)(180 / Math.PI) - d6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).Angle * (double)(180 / Math.PI) - d7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).Angle * (double)(180 / Math.PI) - d8) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test17.   
        ///
        [TestMethod()]
        public void Test18()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test17.CDU"); ;    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (520.0);
                double U2 = (498.75);
                double U3 = (227.9);
                double U4 = (220.26);
                double U5 = (222.09);
                double U6 = (216.71);
                double U7 = (108.0);
                double U8 = (104.75);
                double U9 = (103.92);
                double d1 = (0.0);
                double d2 = (-5.79);
                double d3 = (-8.94);
                double d4 = (-11.19);
                double d5 = (-10.72);
                double d6 = (-12.35);
                double d7 = (-17.22);
                double d8 = (-18.29);
                double d9 = (-19.09);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).UMod - U6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).UMod - U7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).UMod - U8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(9).UMod - U9) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).Angle * (double)(180 / Math.PI) - d6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).Angle * (double)(180 / Math.PI) - d7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).Angle * (double)(180 / Math.PI) - d8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(9).Angle * (double)(180 / Math.PI) - d9) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test18.   
        ///
        [TestMethod()]
        public void Test19()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test18.CDU"); ;    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (335.0);
                double U2 = (332.36);
                double U3 = (331.66);
                double U4 = (329.57);
                double U5 = (510.87);
                double U6 = (510.92);
                double U7 = (510.59);
                double U8 = (235.4);
                double U9 = (230.15);
                double U10 = (115.77);
                double U11 = (112.19);
                double U12 = (112.24);
                double U13 = (11.67);
                double U14 = (9.87);
                double d1 = (0.0);
                double d2 = (-4.87);
                double d3 = (-8.76);
                double d4 = (-8.06);
                double d5 = (-13.03);
                double d6 = (-13.77);
                double d7 = (-13.86);
                double d8 = (-11.05);
                double d9 = (-12.46);
                double d10 = (-16.31);
                double d11 = (-17.32);
                double d12 = (-17.26);
                double d13 = (-22.39);
                double d14 = (-21.03);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).UMod - U6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).UMod - U7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).UMod - U8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(9).UMod - U9) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).UMod - U10) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).UMod - U11) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(12).UMod - U12) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(13).UMod - U13) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(14).UMod - U14) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).Angle * (double)(180 / Math.PI) - d6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).Angle * (double)(180 / Math.PI) - d7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).Angle * (double)(180 / Math.PI) - d8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(9).Angle * (double)(180 / Math.PI) - d9) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).Angle * (double)(180 / Math.PI) - d10) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).Angle * (double)(180 / Math.PI) - d11) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(12).Angle * (double)(180 / Math.PI) - d12) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(13).Angle * (double)(180 / Math.PI) - d13) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(14).Angle * (double)(180 / Math.PI) - d14) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }



        ///
        /// test19.   
        ///
        [TestMethod()]
        public void Test20()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test19.CDU"); ;    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (222.37);
                double U2 = (216.23);
                double U3 = (9.85);
                double U4 = (120.6);
                double U5 = (117.42);
                double U6 = (10.49);
                double U7 = (114.27);
                double U8 = (10.62);
                double U9 = (111.14);
                double U10 = (10.58);
                double U11 = (110.67);
                double U12 = (10.48);
                double U13 = (110.98);
                double U14 = (10.52);
                double U15 = (231.0);
                double d1 = (-2.91);
                double d2 = (-6.61);
                double d3 = (-8.29);
                double d4 = (-6.6);
                double d5 = (-8.35);
                double d6 = (-11.11);
                double d7 = (-10.52);
                double d8 = (-13.2);
                double d9 = (-12.22);
                double d10 = (-16.17);
                double d11 = (-12.32);
                double d12 = (-15.95);
                double d13 = (-12.18);
                double d14 = (-15.85);
                double d15 = (0.0);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).UMod - U6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).UMod - U7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).UMod - U8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(9).UMod - U9) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).UMod - U10) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).UMod - U11) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(12).UMod - U12) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(13).UMod - U13) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(14).UMod - U14) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(15).UMod - U15) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).Angle * (double)(180 / Math.PI) - d6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).Angle * (double)(180 / Math.PI) - d7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).Angle * (double)(180 / Math.PI) - d8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(9).Angle * (double)(180 / Math.PI) - d9) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).Angle * (double)(180 / Math.PI) - d10) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).Angle * (double)(180 / Math.PI) - d11) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(12).Angle * (double)(180 / Math.PI) - d12) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(13).Angle * (double)(180 / Math.PI) - d13) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(14).Angle * (double)(180 / Math.PI) - d14) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(15).Angle * (double)(180 / Math.PI) - d15) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test20.   
        ///
        [TestMethod()]
        public void Test21()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test20.CDU"); ;    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (115.0);
                double U2 = (109.4);
                double U3 = (107.95);
                double U4 = (107.97);
                double U5 = (107.42);
                double U6 = (111.16);
                double U7 = (113.39);
                double d1 = (0.0);
                double d2 = (-2.29);
                double d3 = (-2.88);
                double d4 = (-2.86);
                double d5 = (-3.1);
                double d6 = (-2.61);
                double d7 = (0.34);
                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(100).UMod - U6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(200).UMod - U7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(100).Angle * (double)(180 / Math.PI) - d6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(200).Angle * (double)(180 / Math.PI) - d7) < eps);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test21.   
        ///
        [TestMethod()]
        public void Test22()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test21.CDU"); ;    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (230.0);
                double U2 = (220.0);
                double d1 = (0.0);
                double d2 = (-2.67);
                double Q2 = (24.2);
                double eps = (0.01);
                double epsQ = (0.2);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).QGen - Q2) < epsQ);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test22.   
        ///
        [TestMethod()]
        public void Test23()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test22.CDU");    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (240.0);
                double U2 = (226.54);
                double U3 = (220.0);
                double d1 = (0.0);
                double d2 = (-3.65);
                double d3 = (-5.2);
                double Q3 = (31.6);
                double eps = (0.01);
                double epsQ = (0.2);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).QGen - Q3) < epsQ);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test23.   
        ///
        [TestMethod()]
        public void Test24()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test23.CDU");    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (230.0);
                double U2 = (221.56);
                double U3 = (220.0);
                double U4 = (219.55);
                double d1 = (0.0);
                double d2 = (-3.83);
                double d3 = (-4.49);
                double d4 = (-4.64);
                double Q3 = (26.1);
                double eps = (0.015);
                double epsQ = (0.2);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).QGen - Q3) < epsQ);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test23(1).   
        ///
        [TestMethod()]
        public void Test25()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test23(1).CDU");    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (242.0);
                double U2 = (223.25);
                double U3 = (217.2);
                double U4 = (219.19);
                double d1 = (0.0);
                double d2 = (-3.07);
                double d3 = (-3.42);
                double d4 = (-3.74);
                double Q3 = (-50.0);
                double eps = (0.015);
                double epsQ = (0.2);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).QGen - Q3) < epsQ);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test23(2).   
        ///
        [TestMethod()]
        public void Test26()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test23(2).CDU");   //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (232.0);
                double U2 = (227.09);
                double U3 = (226.95);
                double U4 = (225.77);
                double d1 = (0.0);
                double d2 = (-3.89);
                double d3 = (-4.61);
                double d4 = (-4.7);
                double Q3 = (50.0);
                double eps = (0.015);
                double epsQ = (0.2);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).QGen - Q3) < epsQ);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test24.   
        ///
        [TestMethod()]
        public void Test27()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test24.CDU");    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (120.0);
                double U2 = (113.78);
                double U3 = (113.0);
                double U4 = (110.0);
                double U5 = (111.29);
                double d1 = (0.0);
                double d2 = (-2.26);
                double d3 = (-2.16);
                double d4 = (-3.14);
                double d5 = (-2.72);
                double Q3 = (20.1);
                double Q4 = (14.9);
                double eps = (0.015);
                double epsQ = (0.2);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).QGen - Q3) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).QGen - Q4) < epsQ);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test24(1).   
        ///
        [TestMethod()]
        public void Test28()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test24(1).CDU");    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (120.0);
                double U2 = (111.27);
                double U3 = (110.0);
                double U4 = (106.8);
                double U5 = (108.22);
                double d1 = (0.0);
                double d2 = (-0.29);
                double d3 = (0.37);
                double d4 = (0.18);
                double d5 = (0.1);
                double Q3 = (14.4);
                double Q4 = (-30.0);
                double eps = (0.015);
                double epsQ = (0.2);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).QGen - Q3) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).QGen - Q4) < epsQ);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test24(2).   
        ///
        [TestMethod()]
        public void Test29()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test24(2).CDU");    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (120.0);
                double U2 = (112.17);
                double U3 = (110.0);
                double U4 = (108.66);
                double U5 = (108.9);
                double d1 = (0.0);
                double d2 = (-2.09);
                double d3 = (-1.59);
                double d4 = (-3.08);
                double d5 = (-2.37);
                double Q3 = (-8.7);
                double Q4 = (30.0);
                double eps = (0.015);
                double epsQ = (0.2);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).QGen - Q3) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).QGen - Q4) < epsQ);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test24(3).   
        ///
        [TestMethod()]
        public void Test30()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test24(3).CDU");    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (121.0);
                double U2 = (107.85);
                double U3 = (103.98);
                double U4 = (102.89);
                double U5 = (102.97);
                double d1 = (0.0);
                double d2 = (0.22);
                double d3 = (1.69);
                double d4 = (0.82);
                double d5 = (1.12);
                double Q3 = (-30.0);
                double Q4 = (-30.0);
                double eps = (0.015);
                double epsQ = (0.2);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).QGen - Q3) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).QGen - Q4) < epsQ);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test24(4).   
        ///
        [TestMethod()]
        public void Test31()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test24(4).CDU");    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (121.0);
                double U2 = (115.45);
                double U3 = (113.06);
                double U4 = (114.34);
                double U5 = (113.0);
                double d1 = (0.0);
                double d2 = (-0.63);
                double d3 = (0.29);
                double d4 = (-1.06);
                double d5 = (-0.46);
                double Q3 = (-30.0);
                double Q4 = (30.0);
                double eps = (0.015);
                double epsQ = (0.2);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).QGen - Q3) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).QGen - Q4) < epsQ);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test24(5).   
        ///
        [TestMethod()]
        public void Test32()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test24(5).CDU");    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (115.0);
                double U2 = (115.86);
                double U3 = (116.85);
                double U4 = (115.89);
                double U5 = (115.96);
                double d1 = (0.0);
                double d2 = (-1.56);
                double d3 = (-1.6);
                double d4 = (-2.31);
                double d5 = (-2.06);
                double Q3 = (30.0);
                double Q4 = (30.0);
                double eps = (0.015);
                double epsQ = (0.2);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).QGen - Q3) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).QGen - Q4) < epsQ);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test25.   
        ///
        [TestMethod()]
        public void Test33()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test25.CDU");
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(false, fl2, fl3);
                double U1 = (222.72);
                double U2 = (216.95);
                double U3 = (10.5);
                double U4 = (121.0);
                double U5 = (117.83);
                double U6 = (10.53);
                double U7 = (113.87);
                double U8 = (10.51);
                double U9 = (110.52);
                double U10 = (10.5);
                double U11 = (110.0);
                double U12 = (10.41);
                double U13 = (110.38);
                double U14 = (10.46);
                double U15 = (231.0);
                double d1 = (-2.93);
                double d2 = (-6.62);
                double d3 = (-8.28);
                double d4 = (-6.61);
                double d5 = (-8.35);
                double d6 = (-11.09);
                double d7 = (-10.42);
                double d8 = (-13.12);
                double d9 = (-12.1);
                double d10 = (-16.09);
                double d11 = (-12.16);
                double d12 = (-15.83);
                double d13 = (-12.05);
                double d14 = (-15.76);
                double d15 = (0.0);
                double Q3 = (-3.8);
                double Q4 = (17.4);
                double Q8 = (-5.0);
                double Q10 = (-0.9);
                double Q11 = (-1.5);
                double eps = (0.015);
                double epsQ = (0.2);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).UMod - U6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).UMod - U7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).UMod - U8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(9).UMod - U9) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).UMod - U10) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).UMod - U11) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(12).UMod - U12) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(13).UMod - U13) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(14).UMod - U14) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(15).UMod - U15) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).Angle * (double)(180 / Math.PI) - d6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).Angle * (double)(180 / Math.PI) - d7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).Angle * (double)(180 / Math.PI) - d8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(9).Angle * (double)(180 / Math.PI) - d9) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).Angle * (double)(180 / Math.PI) - d10) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).Angle * (double)(180 / Math.PI) - d11) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(12).Angle * (double)(180 / Math.PI) - d12) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(13).Angle * (double)(180 / Math.PI) - d13) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(14).Angle * (double)(180 / Math.PI) - d14) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(15).Angle * (double)(180 / Math.PI) - d15) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).QGen - Q3) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).QGen - Q4) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).QGen - Q8) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).QGen - Q10) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).QGen - Q11) < epsQ);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }



        ///
        /// test25(1).   
        ///
        [TestMethod()]
        public void Test34()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test25(1).CDU");
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (217.97);
                double U2 = (207.19);
                double U3 = (9.97);
                double U4 = (115.55);
                double U5 = (112.19);
                double U6 = (10.01);
                double U7 = (106.12);
                double U8 = (9.75);
                double U9 = (101.34);
                double U10 = (9.44);
                double U11 = (100.37);
                double U12 = (9.45);
                double U13 = (101.22);
                double U14 = (9.54);
                double U15 = (231.0);
                double d1 = (-2.77);
                double d2 = (-6.74);
                double d3 = (-8.57);
                double d4 = (-6.71);
                double d5 = (-8.62);
                double d6 = (-11.65);
                double d7 = (-10.77);
                double d8 = (-13.9);
                double d9 = (-12.48);
                double d10 = (-17.3);
                double d11 = (-12.31);
                double d12 = (-16.75);
                double d13 = (-12.39);
                double d14 = (-16.82);
                double d15 = (0.0);
                double Q3 = (-10.0);
                double Q4 = (-20.0);
                double Q8 = (-5.0);
                double Q10 = (-5.0);
                double Q11 = (-10.0);
                double eps = (0.015);
                double epsQ = (0.2);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).UMod - U6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).UMod - U7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).UMod - U8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(9).UMod - U9) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).UMod - U10) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).UMod - U11) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(12).UMod - U12) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(13).UMod - U13) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(14).UMod - U14) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(15).UMod - U15) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).Angle * (double)(180 / Math.PI) - d6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).Angle * (double)(180 / Math.PI) - d7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).Angle * (double)(180 / Math.PI) - d8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(9).Angle * (double)(180 / Math.PI) - d9) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).Angle * (double)(180 / Math.PI) - d10) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).Angle * (double)(180 / Math.PI) - d11) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(12).Angle * (double)(180 / Math.PI) - d12) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(13).Angle * (double)(180 / Math.PI) - d13) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(14).Angle * (double)(180 / Math.PI) - d14) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(15).Angle * (double)(180 / Math.PI) - d15) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).QGen - Q3) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).QGen - Q4) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).QGen - Q8) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).QGen - Q10) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).QGen - Q11) < epsQ);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test25(2).   
        ///
        [TestMethod()]
        public void Test35()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test25(2).CDU");
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (226.11);
                double U2 = (223.94);
                double U3 = (10.93);
                double U4 = (124.9);
                double U5 = (121.86);
                double U6 = (10.9);
                double U7 = (121.04);
                double U8 = (11.35);
                double U9 = (119.2);
                double U10 = (11.52);
                double U11 = (119.16);
                double U12 = (11.32);
                double U13 = (119.02);
                double U14 = (11.32);
                double U15 = (231.0);
                double d1 = (-3.05);
                double d2 = (-6.56);
                double d3 = (-8.11);
                double d4 = (-6.56);
                double d5 = (-8.19);
                double d6 = (-10.75);
                double d7 = (-10.4);
                double d8 = (-12.78);
                double d9 = (-12.12);
                double d10 = (-15.52);
                double d11 = (-12.41);
                double d12 = (-15.53);
                double d13 = (-12.12);
                double d14 = (-15.29);
                double d15 = (0.0);
                double Q3 = (10.0);
                double Q4 = (20.0);
                double Q8 = (5.0);
                double Q10 = (5.0);
                double Q11 = (10.0);
                double eps = (0.015);
                double epsQ = (0.2);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).UMod - U6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).UMod - U7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).UMod - U8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(9).UMod - U9) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).UMod - U10) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).UMod - U11) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(12).UMod - U12) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(13).UMod - U13) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(14).UMod - U14) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(15).UMod - U15) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).Angle * (double)(180 / Math.PI) - d6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).Angle * (double)(180 / Math.PI) - d7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).Angle * (double)(180 / Math.PI) - d8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(9).Angle * (double)(180 / Math.PI) - d9) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).Angle * (double)(180 / Math.PI) - d10) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).Angle * (double)(180 / Math.PI) - d11) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(12).Angle * (double)(180 / Math.PI) - d12) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(13).Angle * (double)(180 / Math.PI) - d13) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(14).Angle * (double)(180 / Math.PI) - d14) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(15).Angle * (double)(180 / Math.PI) - d15) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).QGen - Q3) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).QGen - Q4) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).QGen - Q8) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).QGen - Q10) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).QGen - Q11) < epsQ);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test25(3).   
        ///
        [TestMethod()]
        public void Test36()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test25(3).CDU");
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (222.73);
                double U2 = (216.95);
                double U3 = (10.5);
                double U4 = (121.0);
                double U5 = (117.83);
                double U6 = (10.53);
                double U7 = (116.89);
                double U8 = (10.95);
                double U9 = (114.95);
                double U10 = (11.1);
                double U11 = (114.9);
                double U12 = (10.9);
                double U13 = (114.76);
                double U14 = (10.89);
                double U15 = (231.0);
                double d1 = (-2.92);
                double d2 = (-6.59);
                double d3 = (-8.26);
                double d4 = (-6.58);
                double d5 = (-8.32);
                double d6 = (-11.06);
                double d7 = (-10.68);
                double d8 = (-13.24);
                double d9 = (-12.52);
                double d10 = (-16.19);
                double d11 = (-12.83);
                double d12 = (-16.19);
                double d13 = (-12.52);
                double d14 = (-15.94);
                double d15 = (0.0);
                double Q3 = (-3.8);
                double Q4 = (-13.9);
                double Q8 = (5.0);
                double Q10 = (5.0);
                double Q11 = (10.0);
                double eps = (0.015);
                double epsQ = (0.2);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).UMod - U6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).UMod - U7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).UMod - U8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(9).UMod - U9) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).UMod - U10) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).UMod - U11) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(12).UMod - U12) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(13).UMod - U13) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(14).UMod - U14) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(15).UMod - U15) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).Angle * (double)(180 / Math.PI) - d6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).Angle * (double)(180 / Math.PI) - d7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).Angle * (double)(180 / Math.PI) - d8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(9).Angle * (double)(180 / Math.PI) - d9) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).Angle * (double)(180 / Math.PI) - d10) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).Angle * (double)(180 / Math.PI) - d11) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(12).Angle * (double)(180 / Math.PI) - d12) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(13).Angle * (double)(180 / Math.PI) - d13) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(14).Angle * (double)(180 / Math.PI) - d14) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(15).Angle * (double)(180 / Math.PI) - d15) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).QGen - Q3) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).QGen - Q4) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).QGen - Q8) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).QGen - Q10) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).QGen - Q11) < epsQ);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }



        ///
        /// test25(4).   
        ///
        [TestMethod()]
        public void Test37()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test25(4).CDU");
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (222.73);
                double U2 = (216.95);
                double U3 = (10.5);
                double U4 = (121.0);
                double U5 = (117.83);
                double U6 = (10.53);
                double U7 = (115.81);
                double U8 = (10.7);
                double U9 = (113.84);
                double U10 = (10.98);
                double U11 = (113.79);
                double U12 = (10.79);
                double U13 = (113.65);
                double U14 = (10.78);
                double U15 = (231.0);
                double d1 = (-2.92);
                double d2 = (-6.6);
                double d3 = (-8.26);
                double d4 = (-6.59);
                double d5 = (-8.33);
                double d6 = (-11.07);
                double d7 = (-10.58);
                double d8 = (-13.19);
                double d9 = (-12.45);
                double d10 = (-16.19);
                double d11 = (-12.77);
                double d12 = (-16.2);
                double d13 = (-12.45);
                double d14 = (-15.94);
                double d15 = (0.0);
                double Q3 = (-3.8);
                double Q4 = (-2.7);
                double Q8 = (-5.0);
                double Q10 = (5.0);
                double Q11 = (10.0);
                double eps = (0.015);
                double epsQ = (0.2);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).UMod - U6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).UMod - U7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).UMod - U8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(9).UMod - U9) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).UMod - U10) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).UMod - U11) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(12).UMod - U12) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(13).UMod - U13) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(14).UMod - U14) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(15).UMod - U15) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).Angle * (double)(180 / Math.PI) - d6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).Angle * (double)(180 / Math.PI) - d7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).Angle * (double)(180 / Math.PI) - d8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(9).Angle * (double)(180 / Math.PI) - d9) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).Angle * (double)(180 / Math.PI) - d10) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).Angle * (double)(180 / Math.PI) - d11) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(12).Angle * (double)(180 / Math.PI) - d12) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(13).Angle * (double)(180 / Math.PI) - d13) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(14).Angle * (double)(180 / Math.PI) - d14) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(15).Angle * (double)(180 / Math.PI) - d15) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).QGen - Q3) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).QGen - Q4) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).QGen - Q8) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).QGen - Q10) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).QGen - Q11) < epsQ);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }



        ///
        /// test25(5).   
        ///
        [TestMethod()]
        public void Test38()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test25(5).CDU");
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (222.14);
                double U2 = (215.74);
                double U3 = (10.5);
                double U4 = (120.32);
                double U5 = (117.13);
                double U6 = (10.47);
                double U7 = (115.03);
                double U8 = (10.77);
                double U9 = (112.07);
                double U10 = (10.55);
                double U11 = (112.32);
                double U12 = (10.64);
                double U13 = (112.38);
                double U14 = (10.66);
                double U15 = (231.0);
                double d1 = (-2.9);
                double d2 = (-6.61);
                double d3 = (-8.29);
                double d4 = (-6.6);
                double d5 = (-8.36);
                double d6 = (-11.13);
                double d7 = (-10.63);
                double d8 = (-13.28);
                double d9 = (-12.32);
                double d10 = (-16.22);
                double d11 = (-12.74);
                double d12 = (-16.26);
                double d13 = (-12.47);
                double d14 = (-16.04);
                double d15 = (0.0);
                double Q3 = (5.8);
                double Q4 = (-20.0);
                double Q8 = (5.0);
                double Q10 = (-5.0);
                double Q11 = (10.0);
                double eps = (0.9);
                double epsQ = (0.2);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).UMod - U6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).UMod - U7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).UMod - U8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(9).UMod - U9) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).UMod - U10) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).UMod - U11) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(12).UMod - U12) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(13).UMod - U13) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(14).UMod - U14) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(15).UMod - U15) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).Angle * (double)(180 / Math.PI) - d6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).Angle * (double)(180 / Math.PI) - d7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).Angle * (double)(180 / Math.PI) - d8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(9).Angle * (double)(180 / Math.PI) - d9) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).Angle * (double)(180 / Math.PI) - d10) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).Angle * (double)(180 / Math.PI) - d11) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(12).Angle * (double)(180 / Math.PI) - d12) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(13).Angle * (double)(180 / Math.PI) - d13) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(14).Angle * (double)(180 / Math.PI) - d14) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(15).Angle * (double)(180 / Math.PI) - d15) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).QGen - Q3) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).QGen - Q4) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).QGen - Q8) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).QGen - Q10) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).QGen - Q11) < epsQ);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }



        ///
        /// test25(6).   
        ///
        [TestMethod()]
        public void Test39()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test25(6).CDU");
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (223.51);
                double U2 = (218.57);
                double U3 = (10.6);
                double U4 = (121.91);
                double U5 = (118.77);
                double U6 = (10.62);
                double U7 = (115.33);
                double U8 = (10.65);
                double U9 = (112.34);
                double U10 = (10.7);
                double U11 = (112.0);
                double U12 = (10.61);
                double U13 = (112.24);
                double U14 = (10.64);
                double U15 = (231.0);
                double d1 = (-2.96);
                double d2 = (-6.6);
                double d3 = (-8.24);
                double d4 = (-6.6);
                double d5 = (-8.31);
                double d6 = (-11.01);
                double d7 = (-10.39);
                double d8 = (-13.02);
                double d9 = (-12.08);
                double d10 = (-15.94);
                double d11 = (-12.23);
                double d12 = (-15.77);
                double d13 = (-12.06);
                double d14 = (-15.65);
                double d15 = (0.0);
                double Q3 = (-0.7);
                double Q4 = (20.0);
                double Q8 = (-5.0);
                double Q10 = (-0.3);
                double Q11 = (2.0);
                double eps = (0.015);
                double epsQ = (0.2);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).UMod - U6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).UMod - U7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).UMod - U8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(9).UMod - U9) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).UMod - U10) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).UMod - U11) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(12).UMod - U12) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(13).UMod - U13) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(14).UMod - U14) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(15).UMod - U15) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).Angle * (double)(180 / Math.PI) - d6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).Angle * (double)(180 / Math.PI) - d7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).Angle * (double)(180 / Math.PI) - d8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(9).Angle * (double)(180 / Math.PI) - d9) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).Angle * (double)(180 / Math.PI) - d10) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).Angle * (double)(180 / Math.PI) - d11) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(12).Angle * (double)(180 / Math.PI) - d12) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(13).Angle * (double)(180 / Math.PI) - d13) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(14).Angle * (double)(180 / Math.PI) - d14) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(15).Angle * (double)(180 / Math.PI) - d15) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).QGen - Q3) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).QGen - Q4) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).QGen - Q8) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).QGen - Q10) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).QGen - Q11) < epsQ);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test25(7).   
        ///
        [TestMethod()]
        public void Test40()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test25(7).CDU");
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (222.23);
                double U2 = (215.94);
                double U3 = (10.41);
                double U4 = (120.43);
                double U5 = (117.25);
                double U6 = (10.48);
                double U7 = (112.96);
                double U8 = (10.42);
                double U9 = (109.61);
                double U10 = (10.56);
                double U11 = (108.4);
                double U12 = (10.25);
                double U13 = (108.97);
                double U14 = (10.32);
                double U15 = (231.0);
                double d1 = (-2.92);
                double d2 = (-6.63);
                double d3 = (-8.31);
                double d4 = (-6.62);
                double d5 = (-8.38);
                double d6 = (-11.15);
                double d7 = (-10.44);
                double d8 = (-13.19);
                double d9 = (-12.18);
                double d10 = (-16.22);
                double d11 = (-11.94);
                double d12 = (-15.73);
                double d13 = (-11.94);
                double d14 = (-15.75);
                double d15 = (0.0);
                double Q3 = (-10.0);
                double Q4 = (20.0);
                double Q8 = (-5.0);
                double Q10 = (5.0);
                double Q11 = (-10.0);
                double eps = (0.015);
                double epsQ = (0.2);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).UMod - U6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).UMod - U7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).UMod - U8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(9).UMod - U9) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).UMod - U10) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).UMod - U11) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(12).UMod - U12) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(13).UMod - U13) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(14).UMod - U14) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(15).UMod - U15) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).Angle * (double)(180 / Math.PI) - d6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).Angle * (double)(180 / Math.PI) - d7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).Angle * (double)(180 / Math.PI) - d8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(9).Angle * (double)(180 / Math.PI) - d9) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).Angle * (double)(180 / Math.PI) - d10) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).Angle * (double)(180 / Math.PI) - d11) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(12).Angle * (double)(180 / Math.PI) - d12) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(13).Angle * (double)(180 / Math.PI) - d13) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(14).Angle * (double)(180 / Math.PI) - d14) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(15).Angle * (double)(180 / Math.PI) - d15) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).QGen - Q3) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).QGen - Q4) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).QGen - Q8) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).QGen - Q10) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).QGen - Q11) < epsQ);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test25(8).   
        ///
        [TestMethod()]
        public void Test41()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            StreamReader reader = new StreamReader("../../../../../FlowTests/test25(8).CDU");
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (222.85);
                double U2 = (217.23);
                double U3 = (10.5);
                double U4 = (121.15);
                double U5 = (117.99);
                double U6 = (10.55);
                double U7 = (114.17);
                double U8 = (10.6);
                double U9 = (110.64);
                double U10 = (10.5);
                double U11 = (110.0);
                double U12 = (10.41);
                double U13 = (110.47);
                double U14 = (10.47);
                double U15 = (231.0);
                double d1 = (-2.94);
                double d2 = (-6.62);
                double d3 = (-8.28);
                double d4 = (-6.61);
                double d5 = (-8.34);
                double d6 = (-11.08);
                double d7 = (-10.42);
                double d8 = (-13.11);
                double d9 = (-12.06);
                double d10 = (-16.04);
                double d11 = (-12.05);
                double d12 = (-15.73);
                double d13 = (-11.99);
                double d14 = (-15.69);
                double d15 = (0.0);
                double Q3 = (-6.0);
                double Q4 = (20.0);
                double Q8 = (-1.0);
                double Q10 = (-1.3);
                double Q11 = (-3.9);
                double eps = (0.015);
                double epsQ = (0.2);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).UMod - U6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).UMod - U7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).UMod - U8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(9).UMod - U9) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).UMod - U10) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).UMod - U11) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(12).UMod - U12) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(13).UMod - U13) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(14).UMod - U14) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(15).UMod - U15) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(6).Angle * (double)(180 / Math.PI) - d6) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(7).Angle * (double)(180 / Math.PI) - d7) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).Angle * (double)(180 / Math.PI) - d8) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(9).Angle * (double)(180 / Math.PI) - d9) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).Angle * (double)(180 / Math.PI) - d10) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).Angle * (double)(180 / Math.PI) - d11) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(12).Angle * (double)(180 / Math.PI) - d12) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(13).Angle * (double)(180 / Math.PI) - d13) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(14).Angle * (double)(180 / Math.PI) - d14) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(15).Angle * (double)(180 / Math.PI) - d15) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).QGen - Q3) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).QGen - Q4) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(8).QGen - Q8) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(10).QGen - Q10) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(11).QGen - Q11) < epsQ);
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }


        ///
        /// test24(2).   
        ///
        [TestMethod()]
        public void Test29_digraph()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения

            StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/test24(2).CDU");    //ВОТ ЗДЕСЬ НУЖНО БУДЕТ ВСТАВИТЬ ВЕРНЫЙ ПУТЬ
            if (target.LoadFromCDU(reader))
            {
                target.Raschet(fl1, fl2, fl3);
                double U1 = (120.0);
                double U2 = (112.17);
                double U3 = (110.0);
                double U4 = (108.66);
                double U5 = (108.9);
                double d1 = (0.0);
                double d2 = (-2.09);
                double d3 = (-1.59);
                double d4 = (-3.08);
                double d5 = (-2.37);
                double Q3 = (-8.7);
                double Q4 = (30.0);
                double eps = (0.015);
                double epsQ = (0.2);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).UMod - U1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).UMod - U5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(1).Angle * (double)(180 / Math.PI) - d1) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(5).Angle * (double)(180 / Math.PI) - d5) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).QGen - Q3) < epsQ);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).QGen - Q4) < epsQ);
                
            }
            else
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }

        ///
        /// кольцо
        ///
        [TestMethod()]
        public void RaschetTest_digrapg1()
        {
            Shema target = new Shema(); // TODO: инициализация подходящего значения
            XmlTextReader reader = new XmlTextReader("C://Users/gramma/Documents/проба11.xml");
            //StreamReader reader = new StreamReader("C://Users/gramma/Documents/Visual Studio 2013/Projects/FlowTests/PROBA6.CDU");
            target.LoadFromFile(reader);

            string filename = "C://Users/gramma/Documents/проба25.csv";
            FileStream fs1 = new FileStream(filename, FileMode.CreateNew);
            StreamWriter fs = new StreamWriter(fs1, System.Text.Encoding.Default);

            target.Raschet(false, false, true);
            target.Opredelenie_rainov();
            
            fs.Close();
            fs1.Close();


               // target.Raschet(fl1, fl2, fl3);
                double U2 = (231.9);
                double U3 = (231.59);
                double U4 = (233.27);
                double d2 = (-3.02);
                double d3 = (-4.19);
                double d4 = (-2.84);

                double eps = (0.01);
                //Assert.AreEqual((0.0447213595499958), target.Y_balanc[0]);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).UMod - U2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).UMod - U3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).UMod - U4) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(2).Angle * (double)(180 / Math.PI) - d2) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(3).Angle * (double)(180 / Math.PI) - d3) < eps);
                Assert.IsTrue(Math.Abs(target.Find_Uzel_by_Nomer(4).Angle * (double)(180 / Math.PI) - d4) < eps);
      
            
                Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");

            //Assert.Inconclusive("Невозможно проверить метод, не возвращающий значение.");
        }
    }
}
