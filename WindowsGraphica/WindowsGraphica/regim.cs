using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Numerics;

namespace WindowsGraphica
{
    public class regim
    {

        static List<double> U;
        static List<double> delta;
        static double[] U_pribl;   //нужны для того, чтобы при последовательном учете ограничений отталкиваться от предыдущих расчетов.
        static double[] delta_pribl;
        static List<double> Pi;
        static List<double> Qi;
        static List<int> index_opor_uzlov;
        static double Ubasis;
        static Hash_Spare_Matrix _G;
        static Hash_Spare_Matrix _B;
        static double[] _Gb;
        static double[] _Bb;
        static int method = 0;
        static double[] _U_1;
        static double[] _U_2;
        static double[] _Gbb;
        static double[] _Bbb;
        static double[] U_r;
        static double[] delta_r;
        static double[] x;
        static List<Uzel> GU;
        double[] dx;
        Sort_Spare_Matrix_complex Y_shemi;

        public regim(List<double> U1, List<double> delta1, List<double> Pi1, List<double> Qi1, List<int> index_opor_uzlov1, double Ubasis1, IMatrix G1, IMatrix B1, double[] Gb1, double[] Bb1, List<Uzel> GU1, double[] Gbb1, double[] Bbb1, Complex[,] Y_1)
        {

            U = U1;
            delta = delta1;
            Pi = Pi1;
            Qi = Qi1;
            index_opor_uzlov = index_opor_uzlov1;
            Ubasis = Ubasis1;
            _G = (Hash_Spare_Matrix)G1;
            _B = (Hash_Spare_Matrix)B1;
            _Gb = Gb1;
            _Bb = Bb1;
            _Gbb = Gbb1;
            _Bbb = Bbb1;
            method = 2;
            GU = GU1;
            Y_shemi = new Sort_Spare_Matrix_complex(Pi.Count, Ubasis);
            for (int i = 0; i < Pi.Count; i++)
                for (int j = 0; j < Pi.Count; j++)
                    Y_shemi.setValue(i, j, Y_1[i, j]);
            Y_shemi.Convert_to_crs();
            Y_shemi.set_Y_bal(_Gb, _Bb);
        }

        #region Прямоугольная форма

        public Complex[] Raschet(double eps, List<int> nomera)
        {
            StreamWriter writer = new StreamWriter("result2.txt");
            bool flag01 = true, flag02 = true, flag03 = true;
            //            index_opor_uzlov.Clear();
            int iter = method3(writer, 400, 0.001, nomera);

            while (flag01 || flag02 || flag03)
            {
                writer.WriteLine("vhod v zony rascheta");
                flag01 = false;
                flag02 = false;
                flag03 = false;
                foreach (Uzel item in GU)     //проверка на выход на верхний предел генерации
                {
                    item.Q_gen = (Qi[nomera.IndexOf(item.Nomer_uzla)]) + item.Q_load;
                    item.U_mod = Math.Sqrt((_U_1[nomera.IndexOf(item.Nomer_uzla)] * _U_1[nomera.IndexOf(item.Nomer_uzla)] + _U_2[nomera.IndexOf(item.Nomer_uzla)] * _U_2[nomera.IndexOf(item.Nomer_uzla)]));
                    writer.Write(item.Nomer_uzla + "  " + item.Q_gen + " ");
                    if (item.Q_gen > item.Q_max)
                    {
                        if (index_opor_uzlov.Contains(nomera.IndexOf(item.Nomer_uzla)))
                        {
                            index_opor_uzlov.Remove(nomera.IndexOf(item.Nomer_uzla));
                            flag01 = true;
                            item.Q_gen = item.Q_max;
                            Qi[nomera.IndexOf(item.Nomer_uzla)] = (item.Q_gen - item.Q_load);
                            writer.WriteLine("remove po max " + item.Nomer_uzla + "  " + item.U_zad);
                        }
                    }

                }

                if (!flag01)
                {
                    writer.WriteLine("vhod v ustranenie pi min");
                    foreach (Uzel item in GU)  //проверка на выход на нижний предел генерации
                    {
                        writer.Write(item.Nomer_uzla + "  " + item.Q_gen + " ");
                        if (item.Q_gen < item.Q_min)
                        {
                            if (index_opor_uzlov.Contains(nomera.IndexOf(item.Nomer_uzla)))
                            {
                                index_opor_uzlov.Remove(nomera.IndexOf(item.Nomer_uzla));
                                flag03 = true;
                                item.Q_gen = item.Q_min;
                                Qi[nomera.IndexOf(item.Nomer_uzla)] = (item.Q_gen - item.Q_load);
                                writer.WriteLine("remove po min " + item.Nomer_uzla + "  " + item.U_zad);
                            }

                        }

                    }
                }


                if (!flag01 && !flag03)
                {
                    writer.WriteLine("vhod v obratniy");
                    foreach (Uzel item in GU)   //проверка на обратный вход в опорные генераторные узлы
                    {
                        writer.Write(item.Nomer_uzla + "  " + item.Q_gen + " " + item.U_mod);
                        if (item.U_mod > item.U_zad && item.Q_gen == item.Q_max)
                        {
                            index_opor_uzlov.Add(nomera.IndexOf(item.Nomer_uzla));
                            Qi[nomera.IndexOf(item.Nomer_uzla)] = (item.U_zad * item.U_zad);
                            flag02 = true;
                            writer.WriteLine("iz NO v OP po Max " + item.Nomer_uzla);
                        }
                        else if (item.U_mod < item.U_zad && item.Q_gen == item.Q_min)
                        {
                            index_opor_uzlov.Add(nomera.IndexOf(item.Nomer_uzla));
                            Qi[nomera.IndexOf(item.Nomer_uzla)] = (item.U_zad * item.U_zad);
                            flag02 = true;
                            writer.WriteLine("iz NO v OP po Min " + item.Nomer_uzla);
                        }

                    }
                }

                if (flag01 || flag02 || flag03)
                {
                    method3(writer, 400, 0.001, nomera);
                    foreach (int item in index_opor_uzlov)
                        writer.Write(item + " ");
                    writer.WriteLine();
                    foreach (Uzel item in GU)
                        writer.Write(item.Nomer_uzla + "  " + item.Q_gen + " ");
                }
            }


            //Save_Result(0, itter, nomera, writer);
            //         int iter = method3(writer, 400, 0.001, nomera);

            Complex[] res = new Complex[nomera.Count + 1];
            for (int i = 0; i < nomera.Count; i++)
            {
                res[i] = new Complex(_U_1[i], _U_2[i]);
            }
            res[Pi.Count] = new Complex(Raschet_S_Basisnogo_Uzla_rectangular().Real, Raschet_S_Basisnogo_Uzla_rectangular().Imaginary);

            writer.Close();
            return res;
        }

        private int method3(StreamWriter writer, int MaxItter, double eps, List<int> nomera)
        {
            x = new double[Pi.Count + Qi.Count];
            for (int i = 0; i < (Pi.Count + Pi.Count); i++)
            {
                if (i >= Pi.Count)
                    x[i] = U[i - Pi.Count] * Math.Sin(delta[i - Pi.Count]);
                else
                    x[i] = U[i] * Math.Cos(delta[i]);
            }
            index_opor_uzlov.Sort();
            double[] func = new double[Pi.Count + Pi.Count];
            double[] res = new double[Pi.Count + Pi.Count];
            double[,] Jacobian = new double[Pi.Count * 2, Pi.Count * 2];
            double lamb = 1;
            int itter = 1;
            double f1 = 0;
            double f = 0;
            bool flag_itter = true;

            do
            {
                Raschet_fi(x, ref func);
                if (flag_itter || (itter % 3 == 0))
                    Raschet_dw(Jacobian);
                f = Raschet_F(func);
                if ((Math.Abs(f - f1) < eps) & (f < eps))
                    break;



                int info;
                alglib.matinvreport rep;
                if (flag_itter || (itter % 3 == 0))
                {
                    alglib.rmatrixinverse(ref Jacobian, out info, out rep);
                }
                alglib.ablas.rmatrixmv(Pi.Count * 2, Pi.Count * 2, Jacobian, 0, 0, 0, func, 0, ref res, 0);

                /*
                int info;
                alglib.matinvreport rep;
                alglib.rmatrixinverse(ref Jacobian, out info, out rep);
                alglib.ablas.rmatrixmv(Pi.Count * 2, Pi.Count * 2, Jacobian, 0, 0, 0, func, 0, ref res, 0);
                */
                if ((f > f1) && (itter > 1))
                {
                    lamb = Raschet_Priraschenia_x(res, false, Math.Abs(f1 / f));
                    //f1 = f;
                    //continue;
                }
                else
                    lamb = Raschet_Priraschenia_x(res, true, Math.Sqrt(f) / Pi.Count);
                f1 = f;
                Save_Result(itter, f, nomera, writer);
                itter++;
            }
            while (itter < MaxItter);
            for (int i = 0; i < (Pi.Count); i++)
            {
                Complex Ui = new Complex(x[i], x[i + Pi.Count]);
                U[i] = Ui.Magnitude;
                delta[i] = Ui.Phase;

            }
            return itter;
        }

        private static double Raschet_Priraschenia_x(double[] res, bool znak, double nebal_na_perem)
        {
            double lamb = 0;

            lamb = 50 / (nebal_na_perem) + 0.1;
            if (!znak) lamb = nebal_na_perem;
            if (lamb > 1) lamb = 1;




            /*
            if (nebal_na_perem < 19)
                lamb = (nebal_na_perem + 3.0) / 20;
            else
                lamb = 1;
            */



            for (int i = 0; i < (Pi.Count * 2); i++)
            {
                //res_poll[i] = lamb * res_poll[i];
                if (znak)
                    x[i] = x[i] + lamb * res[i];
                else
                    x[i] = x[i] + lamb * res[i];
            }
            return lamb;
        }

        /*private static double Raschet_Priraschenia_x(double[] res, bool znak, double nebal_na_perem)
        {
            double lamb = 0;
            if (nebal_na_perem < 7)
                lamb = (nebal_na_perem + 3.0) / 10;
            else
                lamb = 1;

            if (!znak)
                lamb = nebal_na_perem;
            for (int i = 0; i < (Pi.Count * 2); i++)
            {
                res[i] = lamb * res[i];
                if (znak)
                    x[i] = x[i] + res[i];
                else
                    x[i] = x[i] - res[i];
            }
            return lamb;
        }
        */
        private static int Method2(StreamWriter writer)
        {
            x = new double[Pi.Count + Qi.Count];
            for (int i = 0; i < (Pi.Count + Pi.Count); i++)
            {
                if (i >= Pi.Count)
                    x[i] = U[i - Pi.Count] * Math.Sin(delta[i - Pi.Count]);
                else
                    x[i] = U[i] * Math.Cos(delta[i]);

            }

            double epsg = 0;
            double epsf = 0.0002;
            double epsx = 0;
            int maxits = 4000;
            alglib.minlmstate state;
            alglib.minlmreport rep;

            //alglib.minlmcreatevj(Pi.Count + Qi.Count, x, out state);
            //alglib.minlmsetbc(state, min, max);
            alglib.minlmcreatev(Pi.Count + Qi.Count, x, 0.0000001, out state);
            alglib.minlmsetcond(state, epsg, epsf, epsx, maxits);
            //          alglib.minlmoptimize(state, function1_fvec, null, null);
            //alglib.minlmoptimize(state, function1_fvec, function1_jac, null, null);
            alglib.minlmresults(state, out x, out rep);
            //x = Raschet_fi(x);
            writer.WriteLine("{0}", rep.terminationtype);
            writer.WriteLine("{0}", alglib.ap.format(x, 6));
            int itter = rep.iterationscount;
            return itter;
        }

        private double Raschet_F(double[] fi)
        {
            double Sum = 0;
            foreach (double value in fi)
            {
                Sum = Sum + value * value;
            }
            //    return Math.Sqrt(Sum);
            return Sum;

        }


        private static void Raschet_fi(double[] x, ref double[] fi)
        {

            _U_1 = new double[Pi.Count];
            _U_2 = new double[Pi.Count];
            for (int i = 0; i < (Pi.Count + Pi.Count); i++)
            {
                if (i >= Pi.Count)
                    _U_2[i - Pi.Count] = x[i];
                else
                    _U_1[i] = x[i];
            }

            //_WP = new double[Pi.Count];
            //_WQ = new double[Pi.Count];

            double sum1 = 0;
            double sum2 = 0;
            double sum3 = 0;
            double sum4 = 0;
            bool flag;
            for (int i = 0; i < Pi.Count; i++)
            {
                sum1 = 0;
                sum2 = 0;
                sum3 = 0;
                sum4 = 0;
                flag = false;
                foreach (int ind in index_opor_uzlov)
                    if (i == ind)
                        flag = true;
                for (int j = 0; j < Pi.Count; j++)
                    if (i != j)
                    {
                        sum1 = sum1 + _G.getValue(i, j) * _U_1[j] + _B.getValue(i, j) * _U_2[j];
                        sum2 = sum2 - _G.getValue(i, j) * _U_2[j] + _B.getValue(i, j) * _U_1[j];
                        sum3 = sum3 - _G.getValue(i, j) * _U_2[j] + _B.getValue(i, j) * _U_1[j];
                        sum4 = sum4 + _G.getValue(i, j) * _U_1[j] + _B.getValue(i, j) * _U_2[j];
                    }
                fi[i] = -((_U_1[i] * _U_1[i] + _U_2[i] * _U_2[i]) * _G.getValue(i, i) + _U_1[i] * sum1 - _U_2[i] * sum2 + (_U_1[i] * _Gb[i] - _U_2[i] * _Bb[i]) * Ubasis - Pi[i]);
                if (flag)
                {
                    Qi[i] = (_U_1[i] * _U_1[i] + _U_2[i] * _U_2[i]) * _B.getValue(i, i) + _U_1[i] * sum3 + _U_2[i] * sum4 + (_U_1[i] * _Bb[i] + _U_2[i] * _Gb[i]) * Ubasis;
                    fi[i + Pi.Count] = -(_U_1[i] * _U_1[i] + _U_2[i] * _U_2[i] - U[i] * U[i]);
                }
                else
                    fi[i + Pi.Count] = -((_U_1[i] * _U_1[i] + _U_2[i] * _U_2[i]) * _B.getValue(i, i) + _U_1[i] * sum3 + _U_2[i] * sum4 + (_U_1[i] * _Bb[i] + _U_2[i] * _Gb[i]) * Ubasis - Qi[i]);
            }
        }

        private void Raschet_dw(double[,] jac)
        {
            //jac = new double[Pi.Count * 2, Pi.Count * 2];

            bool flag = false;

            if (method == 2)
            {
                for (int i = 0; i < Pi.Count; i++)
                {
                    flag = false;
                    if (index_opor_uzlov.IndexOf(i) != -1)
                        flag = true;
                    /*foreach (int ind in index_opor_uzlov)
                        if (i == ind)
                            flag = true;// узел опорный
                    */
                    if (flag)
                    {

                        jac[i + Pi.Count, i] = (2 * _U_1[i]);
                        jac[i + Pi.Count, i + Pi.Count] = (2 * _U_2[i]);
                    }
                    else
                    {
                        double sum3 = 0;
                        double sum4 = 0;

                        for (int j = 0; j < Qi.Count; j++)
                            if (i != j)
                            {
                                sum3 = sum3 + -_G.getValue(i, j) * _U_2[j] + _B.getValue(i, j) * _U_1[j];
                                sum4 = sum4 + _G.getValue(i, j) * _U_1[j] + _B.getValue(i, j) * _U_2[j];
                                jac[i + Pi.Count, j] = _G.getValue(i, j) * _U_2[i] + _B.getValue(i, j) * _U_1[i];
                                jac[i + Pi.Count, j + Pi.Count] = -_G.getValue(i, j) * _U_1[i] + _B.getValue(i, j) * _U_2[i];
                            }


                        jac[i + Pi.Count, i] = ((2 * _U_1[i]) * _B.getValue(i, i) + sum3 + _Bb[i] * Ubasis);
                        jac[i + Pi.Count, i + Pi.Count] = ((2 * _U_2[i]) * _B.getValue(i, i) + sum4 + _Gb[i] * Ubasis);
                    }
                    double sum1 = 0;
                    double sum2 = 0;


                    for (int j = 0; j < Pi.Count; j++)
                    {
                        if (i != j)
                        {
                            sum1 = sum1 + _G.getValue(i, j) * _U_1[j] + _B.getValue(i, j) * _U_2[j];
                            sum2 = sum2 + -_G.getValue(i, j) * _U_2[j] + _B.getValue(i, j) * _U_1[j];

                            jac[i, j] = _G.getValue(i, j) * _U_1[i] - _B.getValue(i, j) * _U_2[i];
                            jac[i, j + Pi.Count] = _G.getValue(i, j) * _U_2[i] + _B.getValue(i, j) * _U_1[i];
                        }
                    }
                    jac[i, i] = (2 * _U_1[i]) * _G.getValue(i, i) + sum1 + _Gb[i] * Ubasis;
                    jac[i, i + Pi.Count] = (2 * _U_2[i]) * _G.getValue(i, i) - sum2 - _Bb[i] * Ubasis;


                }
            }



            //return jac;
        }

        private void Raschet_dw(Hash_Spare_Matrix jac)
        {
            //jac = new double[Pi.Count * 2, Pi.Count * 2];

            bool flag = false;

            if (method == 2)
            {
                for (int i = 0; i < Pi.Count; i++)
                {
                    flag = false;
                    if (index_opor_uzlov.IndexOf(i) != -1)
                        flag = true;
                    /*foreach (int ind in index_opor_uzlov)
                        if (i == ind)
                            flag = true;// узел опорный
                    */
                    if (flag)
                    {

                        jac.setValue(i + Pi.Count, i, (2 * _U_1[i]));
                        jac.setValue(i + Pi.Count, i + Pi.Count, (2 * _U_2[i]));
                    }
                    else
                    {
                        double sum3 = 0;
                        double sum4 = 0;
                        int[] index_row_G = new int[0];
                        double[] values_G = new double[0];
                        _G.GetNRow(i, ref index_row_G, ref values_G);
                        for (int j = 0; j < index_row_G.Length; j++)
                        {
                            sum3 = sum3 + -values_G[j] * _U_2[index_row_G[j]];
                            sum4 = sum4 + values_G[j] * _U_1[index_row_G[j]];
                            jac.setValue(i + Pi.Count, index_row_G[j], values_G[j] * _U_2[i]);
                            jac.setValue(i + Pi.Count, index_row_G[j] + Pi.Count, -values_G[j] * _U_1[i]);
                        }
                        int[] index_row_B = new int[0];
                        double[] values_B = new double[0];
                        _B.GetNRow(i, ref index_row_B, ref values_B);
                        for (int j = 0; j < index_row_B.Length; j++)
                        {
                            sum3 = sum3 + values_B[j] * _U_1[index_row_B[j]];
                            sum4 = sum4 + values_B[j] * _U_2[index_row_B[j]];
                            jac.addValue(i + Pi.Count, index_row_B[j], values_B[j] * _U_1[i]);
                            jac.addValue(i + Pi.Count, index_row_B[j] + Pi.Count, values_B[j] * _U_2[i]);
                        }
                        /*
                        for (int j = 0; j < Qi.Count; j++)
                            if (i != j)
                            {
                                sum3 = sum3 + -_G.getValue(i,j) * _U_2[j] + _B.getValue(i,j) * _U_1[j];
                                sum4 = sum4 + _G.getValue(i,j) * _U_1[j] + _B.getValue(i,j) * _U_2[j];
                                jac[i + Pi.Count, j] = _G.getValue(i,j) * _U_2[i] + _B.getValue(i,j) * _U_1[i];
                                jac[i + Pi.Count, j + Pi.Count] = -_G.getValue(i,j) * _U_1[i] + _B.getValue(i,j) * _U_2[i];
                            }
                         */

                        jac.setValue(i + Pi.Count, i, ((2 * _U_1[i]) * _B.getValue(i, i) + sum3 + _Bb[i] * Ubasis));
                        jac.setValue(i + Pi.Count, i + Pi.Count, ((2 * _U_2[i]) * _B.getValue(i, i) + sum4 + _Gb[i] * Ubasis));
                    }
                    double sum1 = 0;
                    double sum2 = 0;

                    int[] index_row_G1 = new int[0];
                    double[] values_G1 = new double[0];
                    _G.GetNRow(i, ref index_row_G1, ref values_G1);
                    for (int j = 0; j < index_row_G1.Length; j++)
                    {
                        sum1 = sum1 + values_G1[j] * _U_1[index_row_G1[j]];
                        sum2 = sum2 + -values_G1[j] * _U_2[index_row_G1[j]];
                        jac.setValue(i, index_row_G1[j], values_G1[j] * _U_1[i]);
                        jac.setValue(i, index_row_G1[j] + Pi.Count, values_G1[j] * _U_2[i]);
                    }
                    int[] index_row_B1 = new int[0];
                    double[] values_B1 = new double[0];
                    _B.GetNRow(i, ref index_row_B1, ref values_B1);
                    for (int j = 0; j < index_row_B1.Length; j++)
                    {
                        sum1 = sum1 + values_B1[j] * _U_2[index_row_B1[j]];
                        sum2 = sum2 + values_B1[j] * _U_1[index_row_B1[j]];
                        jac.addValue(i, index_row_B1[j], -values_B1[j] * _U_2[i]);
                        jac.addValue(i, index_row_B1[j] + Pi.Count, values_B1[j] * _U_1[i]);
                    }
                    jac.setValue(i, i, ((2 * _U_1[i]) * _G.getValue(i, i) + sum1 + _Gb[i] * Ubasis));
                    jac.setValue(i, i + Pi.Count, ((2 * _U_2[i]) * _G.getValue(i, i) - sum2 - _Bb[i] * Ubasis));
                    /*
                    for (int j = 0; j < Pi.Count; j++)
                    {
                        if (i != j)
                        {
                            sum1 = sum1 + _G.getValue(i,j) * _U_1[j] + _B.getValue(i,j) * _U_2[j];
                            sum2 = sum2 + -_G.getValue(i,j) * _U_2[j] + _B.getValue(i,j) * _U_1[j];

                            jac[i, j] = _G.getValue(i,j) * _U_1[i] - _B.getValue(i,j) * _U_2[i];
                            jac[i, j + Pi.Count] = _G.getValue(i,j) * _U_2[i] + _B.getValue(i,j) * _U_1[i];
                        }
                    }
                    jac[i, i] = (2 * _U_1[i]) * _G.getValue(i,i) + sum1 + _Gb[i] * Ubasis;
                    jac[i, i + Pi.Count] = (2 * _U_2[i]) * _G.getValue(i,i) - sum2 - _Bb[i] * Ubasis;

                     */
                }
            }



            //return jac;
        }

        private void Save_Result(int Nomer, double F, List<int> nomera, StreamWriter wr)
        {

            wr.WriteLine("Иттерация номер " + Nomer.ToString() + '	');
            wr.WriteLine("Номер узла " + '	' + "U1 " + '	' + "U2 " + '	' + "U " + '	' + "delta " + '	');
            for (int i = 0; i < nomera.Count; i++)
            {
                wr.WriteLine(nomera[i].ToString() + '	' + _U_1[i].ToString() + '	' + (_U_2[i]).ToString() + '	' + (Math.Abs(Math.Sqrt(_U_1[i] * _U_1[i] + _U_2[i] * _U_2[i]))).ToString() + '	' + (Math.Atan(_U_2[i] / _U_1[i]) * 180 / Math.PI).ToString() + '	');

            }
            wr.WriteLine("F= " + F.ToString() + '	');

        }

        private Complex Raschet_S_Basisnogo_Uzla_rectangular()
        {
            Complex Sbas = 0;
            for (int i = 0; i < Pi.Count; i++)
                Sbas = Sbas + (new Complex(_Gbb[i], _Bbb[i])) * (new Complex(_U_1[i], -_U_2[i]));
            Sbas = Sbas + (new Complex(_Gbb[Pi.Count], _Bbb[Pi.Count])) * (new Complex(Ubasis, 0.0));
            return Sbas * Ubasis;
        }

        #endregion




        //ДАЛЕЕ ПОЙДУТ ВСЕ МЕТОДЫ ДЛЯ РАСЧЕТА В ПОЛЯРНОЙ ФОРМЕ



        /*      public Complex[] Raschet_Polarniy1(double eps, List<int> nomera)
              {
                  StreamWriter writer = new StreamWriter("D://result2.txt");
                  int iter = method_polyarniy(writer, 400, 0.001, nomera, S_basis);
                  Complex[] res_pol = new Complex[nomera.Count];
                  for (int i = 0; i < nomera.Count; i++)
                  {
                          res_pol[i] = new Complex(U_r[i], delta_r[i]);    //тут подумать, как быть
                  }
                  writer.Close();
                  return res_pol;
              }        */


        private int method_polyarniy(StreamWriter writer, int MaxItter, double eps, List<int> nomera)
        {
            int count = Pi.Count * 2 - index_opor_uzlov.Count;
            x = new double[count];
            int kolvo = 0;
            for (int i = 0; i < Pi.Count; i++)
            {
                bool flag = false;
                foreach (int ind in index_opor_uzlov)
                    if (i == ind)
                        flag = true;
                if (!flag)
                {
                    x[i] = delta_pribl[i];
                    x[Pi.Count + kolvo] = U_pribl[i];
                    kolvo++;
                }
                else
                    x[i] = delta_pribl[i];
            }
            index_opor_uzlov.Sort();
            double[] func = new double[count];
            double[] res_pol = new double[count];   //потом обдумать
            double[,] Jacobian = new double[count, count];
            int itter = 1;
            double lamb = 1;
            double f1 = 0;
            double f = 0;
            bool flag_iterr = true;


            do
            {
                object obj = 0;


                //Raschet_dw_pol(x, func, Jacobian, obj);
                Raschet_fi_pol(x, func, obj);
                
                f = Raschet_F(func);
                if ((Math.Abs(f - f1) < eps) & (f < eps))
                {
                    break;
                }
                else
                    if ((f > f1) && (itter > 1))
                    {
                        lamb = Raschet_Priraschenia_x_pol(res_pol, false, Math.Abs(f1 / f ));

                        continue;
                    }



                int info;
                /*
                alglib.matinvreport rep;


                alglib.rmatrixinverse(ref Jacobian, out info, out rep);
                alglib.ablas.rmatrixmv(count, count, Jacobian, 0, 0, 0, func, 0, ref res_pol, 0);
                */
                alglib.densesolverreport rep1;



                Raschet_dw_pol(ref Jacobian);
                var writter2 = new StreamWriter("C:/TestResult/newton_jacobis" + itter + ".csv");
                Save_jacobi(writter2, Jacobian, func);
                writter2.Close();
                alglib.rmatrixsolve(Jacobian, count,  func, out info, out rep1, out res_pol);
                //alglib.rmatrixsolve(Jacobian, count, func, out info, out rep1, out res_pol);

                /*
                if ((info !=1)||(rep1.r1/rep1.rinf<1.0) )
                    lamb = Raschet_Priraschenia_x_pol(res_pol, true, rep1.rinf / rep1.r1 * 10 * Pi.Count * Math.Sqrt(f));
                else
                    lamb = Raschet_Priraschenia_x_pol(res_pol, true, Math.Sqrt(f) / Pi.Count);
                 * */
                lamb = Raschet_Priraschenia_x_pol(res_pol, true, Math.Sqrt(f) / Pi.Count);
                f1 = f;
                Save_Result_pol(itter, f, nomera, writer);
                itter++;
            }
            while (itter < MaxItter);


            delta_pribl = delta_r;
            U_pribl = U_r;
            return itter;
        }

        private void Save_jacobi(TextWriter writer, double[,] jacobian, double[] func)
        {
            int count = 2*Pi.Count - index_opor_uzlov.Count;
            for (var i = 0; i < count; i++)
            {
                for (var j = 0; j < count; j++)
                {
                    writer.Write(jacobian[i, j] + ";");
                }
                writer.WriteLine(";" + func[i] + ";");
            }
            writer.WriteLine();
        }

        private static double Raschet_Priraschenia_x_pol(double[] resPoll, bool znak, double nebalNaPerem)
        {
            double lamb = 0;

            lamb = 30 / (nebalNaPerem) ;            
            if (lamb > 1) lamb = 1;
            if (lamb < 0.1) 
                lamb = 0.1;
            if (!znak) lamb = nebalNaPerem;

            /*
            if (nebal_na_perem < 19)
                lamb = (nebal_na_perem + 3.0) / 20;
            else
                lamb = 1;
            */



            for (int i = 0; i < (Pi.Count * 2 - index_opor_uzlov.Count); i++)
            {
                //res_poll[i] = lamb * res_poll[i];
                if (znak)
                {
                    resPoll[i] = lamb * resPoll[i];
                    x[i] = x[i] + resPoll[i];
                }
                else
                    x[i] = x[i] - resPoll[i] + lamb * resPoll[i];
            }
            return lamb;
        }


        private static void Raschet_fi_pol(double[] x, double[] fi, object obj)
        {

            U_r = new double[Pi.Count];  //вот здесь с опорными
            delta_r = new double[Pi.Count];
            int kolvo = 0;
            for (int i = 0; i < Pi.Count; i++)
            {
                bool flag0 = false;
                foreach (int ind in index_opor_uzlov)
                    if (i == ind)
                        flag0 = true;
                if (!flag0)
                {
                    U_r[i] = x[Pi.Count + kolvo];
                    delta_r[i] = x[i];
                    kolvo++;
                }
                else
                {
                    U_r[i] = U[i];
                    delta_r[i] = x[i];
                }
            }
            double sum1 = 0;
            double sum2 = 0;
            bool flag;
            int per_po_Q = 0;
            for (int i = 0; i < Pi.Count; i++)
            {
                sum1 = 0;
                sum2 = 0;
                flag = false;
                foreach (int ind in index_opor_uzlov)
                    if (i == ind)
                        flag = true;
                for (int j = 0; j < Pi.Count; j++)
                    if (i != j)
                    {
                        sum1 = sum1 + U_r[j] * (_G.getValue(i, j) * Math.Cos(delta_r[i] - delta_r[j]) - _B.getValue(i, j) * Math.Sin(delta_r[i] - delta_r[j]));
                        sum2 = sum2 + U_r[j] * (_G.getValue(i, j) * Math.Sin(delta_r[i] - delta_r[j]) + _B.getValue(i, j) * Math.Cos(delta_r[i] - delta_r[j]));
                    }
                fi[i] = -(U_r[i] * U_r[i] * _G.getValue(i, i) + U_r[i] * sum1 + (_Gb[i] * Math.Cos(delta_r[i]) - _Bb[i] * Math.Sin(delta_r[i])) * U_r[i] * Ubasis - Pi[i]);
                if (flag)
                {
                    Qi[i] = U_r[i] * U_r[i] * _B.getValue(i, i) + U_r[i] * sum2 + (_Bb[i] * Math.Cos(delta_r[i]) + _Gb[i] * Math.Sin(delta_r[i])) * U_r[i] * Ubasis;
                }
                else
                {
                    fi[Pi.Count + per_po_Q] = -(U_r[i] * U_r[i] * _B.getValue(i, i) + U_r[i] * sum2 + (_Bb[i] * Math.Cos(delta_r[i]) + _Gb[i] * Math.Sin(delta_r[i])) * U_r[i] * Ubasis - Qi[i]);
                    per_po_Q++;
                }

            }
        }
        //function1_jac(double[] x, double[] fi, double[,] jac, object obj)
        private static void Raschet_dw_pol(double[] x, double[] fi, Hash_Spare_Matrix jac, object obj)
        {
            Raschet_fi_pol(x, fi, obj);
            bool flag = false;
            int kolvo_Q = 0;
            for (int i = 0; i < Pi.Count; i++)
            {
                int kolvo_QQ = 0;
                flag = false;
                double sum1 = 0;
                double sum2 = 0;
                foreach (int ind in index_opor_uzlov)
                    if (i == ind)
                        flag = true;// узел опорный
                for (int j = 0; j < Pi.Count; j++)
                {
                    if (i != j)
                    {
                        jac.setValue(i, j, U_r[i] * U_r[j] * (_G.getValue(i, j) * Math.Sin(delta_r[i] - delta_r[j]) + _B.getValue(i, j) * Math.Cos(delta_r[i] - delta_r[j])));
                        sum1 = sum1 + U_r[j] * (_G.getValue(i, j) * Math.Sin(delta_r[i] - delta_r[j]) + _B.getValue(i, j) * Math.Cos(delta_r[i] - delta_r[j]));
                        sum2 = sum2 + U_r[j] * (_G.getValue(i, j) * Math.Cos(delta_r[i] - delta_r[j]) - _B.getValue(i, j) * Math.Sin(delta_r[i] - delta_r[j]));
                    }
                    bool flag1 = false;
                    foreach (int ind1 in index_opor_uzlov)
                        if (j == ind1)
                            flag1 = true;// узел опорный

                    if (!flag1)
                    {
                        if (i != j)
                        {
                            jac.setValue(i, Pi.Count + kolvo_QQ, U_r[i] * (_G.getValue(i, j) * Math.Cos(delta_r[i] - delta_r[j]) - _B.getValue(i, j) * Math.Sin(delta_r[i] - delta_r[j])));
                            if (!flag)
                                jac.setValue(Pi.Count + kolvo_Q, Pi.Count + kolvo_QQ, U_r[i] * (_G.getValue(i, j) * Math.Sin(delta_r[i] - delta_r[j]) + _B.getValue(i, j) * Math.Cos(delta_r[i] - delta_r[j])));
                        }
                        kolvo_QQ++;
                    }
                    if (!flag)
                        if (i != j)
                            jac.setValue(Pi.Count + kolvo_Q, j, -U_r[i] * U_r[j] * (_G.getValue(i, j) * Math.Cos(delta_r[i] - delta_r[j]) - _B.getValue(i, j) * Math.Sin(delta_r[i] - delta_r[j])));

                }
                jac.setValue(i, i, -U_r[i] * sum1 - U_r[i] * Ubasis * (_Bb[i] * Math.Cos(delta_r[i]) + _Gb[i] * Math.Sin(delta_r[i])));
                if (!flag)
                {
                    jac.setValue(Pi.Count + kolvo_Q, i, U_r[i] * sum2 - Ubasis * (_Gb[i] * Math.Cos(delta_r[i]) - _Bb[i] * Math.Sin(delta_r[i])) * U_r[i]);
                    jac.setValue(Pi.Count + kolvo_Q, Pi.Count + kolvo_Q, 2 * _B.getValue(i, i) * U_r[i] + sum1 + Ubasis * (_Bb[i] * Math.Cos(delta_r[i]) + _Gb[i] * Math.Sin(delta_r[i])));
                    jac.setValue(i, Pi.Count + kolvo_Q, 2 * _G.getValue(i, i) * U_r[i] + sum2 + Ubasis * (_Gb[i] * Math.Cos(delta_r[i]) - _Bb[i] * Math.Sin(delta_r[i])));
                    kolvo_Q++;
                }

            }


            //return jac;
        }

        private static void Raschet_dw_pol(double[] x, double[] fi, double[,] jac, object obj)
        {
            Raschet_fi_pol(x, fi, obj);
            bool flag = false;
            int kolvo_Q = 0;
            for (int i = 0; i < Pi.Count; i++)
            {
                int kolvo_QQ = 0;
                flag = false;
                double sum1 = 0;
                double sum2 = 0;
                foreach (int ind in index_opor_uzlov)
                    if (i == ind)
                        flag = true;// узел опорный
                for (int j = 0; j < Pi.Count; j++)
                {
                    if (i != j)
                    {
                        jac[i, j] = U_r[i] * U_r[j] * (_G.getValue(i, j) * Math.Sin(delta_r[i] - delta_r[j]) + _B.getValue(i, j) * Math.Cos(delta_r[i] - delta_r[j]));
                        sum1 = sum1 + U_r[j] * (_G.getValue(i, j) * Math.Sin(delta_r[i] - delta_r[j]) + _B.getValue(i, j) * Math.Cos(delta_r[i] - delta_r[j]));
                        sum2 = sum2 + U_r[j] * (_G.getValue(i, j) * Math.Cos(delta_r[i] - delta_r[j]) - _B.getValue(i, j) * Math.Sin(delta_r[i] - delta_r[j]));
                    }
                    bool flag1 = false;
                    foreach (int ind1 in index_opor_uzlov)
                        if (j == ind1)
                            flag1 = true;// узел опорный

                    if (!flag1)
                    {
                        if (i != j)
                        {
                            jac[i, Pi.Count + kolvo_QQ] = U_r[i] * (_G.getValue(i, j) * Math.Cos(delta_r[i] - delta_r[j]) - _B.getValue(i, j) * Math.Sin(delta_r[i] - delta_r[j]));
                            if (!flag)
                                jac[Pi.Count + kolvo_Q, Pi.Count + kolvo_QQ] = U_r[i] * (_G.getValue(i, j) * Math.Sin(delta_r[i] - delta_r[j]) + _B.getValue(i, j) * Math.Cos(delta_r[i] - delta_r[j]));
                        }
                        kolvo_QQ++;
                    }
                    if (!flag)
                        if (i != j)
                            jac[Pi.Count + kolvo_Q, j] = -U_r[i] * U_r[j] * (_G.getValue(i, j) * Math.Cos(delta_r[i] - delta_r[j]) - _B.getValue(i, j) * Math.Sin(delta_r[i] - delta_r[j]));

                }
                jac[i, i] = -U_r[i] * sum1 - U_r[i] * Ubasis * (_Bb[i] * Math.Cos(delta_r[i]) + _Gb[i] * Math.Sin(delta_r[i]));
                if (!flag)
                {
                    jac[Pi.Count + kolvo_Q, i] = U_r[i] * sum2 - Ubasis * (_Gb[i] * Math.Cos(delta_r[i]) - _Bb[i] * Math.Sin(delta_r[i])) * U_r[i];
                    jac[Pi.Count + kolvo_Q, Pi.Count + kolvo_Q] = 2 * _B.getValue(i, i) * U_r[i] + sum1 + Ubasis * (_Bb[i] * Math.Cos(delta_r[i]) + _Gb[i] * Math.Sin(delta_r[i]));
                    jac[i, Pi.Count + kolvo_Q] = 2 * _G.getValue(i, i) * U_r[i] + sum2 + Ubasis * (_Gb[i] * Math.Cos(delta_r[i]) - _Bb[i] * Math.Sin(delta_r[i]));
                    kolvo_Q++;
                }

            }


            //return jac;
        }

        private void Raschet_dw_pol(ref double[,] jac)    //расчет якобиана в полярной форме
        {
            int kolvo_Q = 0;
            for (int i = 0; i < Pi.Count; i++)
            {
                int kolvo_QQ = 0;
                double sum1 = 0;
                double sum2 = 0;
                for (int j = 0; j < Pi.Count; j++)
                {
                    if (i != j)
                    {
                        jac[i, j] = U_r[i] * U_r[j] * (_G.getValue(i, j) * Math.Sin(delta_r[i] - delta_r[j]) + _B.getValue(i, j) * Math.Cos(delta_r[i] - delta_r[j]));
                        sum1 = sum1 + U_r[j] * (_G.getValue(i, j) * Math.Sin(delta_r[i] - delta_r[j]) + _B.getValue(i, j) * Math.Cos(delta_r[i] - delta_r[j]));
                        sum2 = sum2 + U_r[j] * (_G.getValue(i, j) * Math.Cos(delta_r[i] - delta_r[j]) - _B.getValue(i, j) * Math.Sin(delta_r[i] - delta_r[j]));
                    }


                    if (!index_opor_uzlov.Contains(j))
                    {
                        if (i != j)
                        {
                            jac[i, Pi.Count + kolvo_QQ] = U_r[i] * (_G.getValue(i, j) * Math.Cos(delta_r[i] - delta_r[j]) - _B.getValue(i, j) * Math.Sin(delta_r[i] - delta_r[j]));
                            if (!index_opor_uzlov.Contains(i))
                                jac[Pi.Count + kolvo_Q, Pi.Count + kolvo_QQ] = U_r[i] * (_G.getValue(i, j) * Math.Sin(delta_r[i] - delta_r[j]) + _B.getValue(i, j) * Math.Cos(delta_r[i] - delta_r[j]));
                        }
                        kolvo_QQ++;
                    }
                    if (!index_opor_uzlov.Contains(i))
                        if (i != j)
                            jac[Pi.Count + kolvo_Q, j] = -U_r[i] * U_r[j] * (_G.getValue(i, j) * Math.Cos(delta_r[i] - delta_r[j]) - _B.getValue(i, j) * Math.Sin(delta_r[i] - delta_r[j]));

                }
                jac[i, i] = -U_r[i] * sum1 - U_r[i] * Ubasis * (_Bb[i] * Math.Cos(delta_r[i]) + _Gb[i] * Math.Sin(delta_r[i]));
                if (!index_opor_uzlov.Contains(i))
                {
                    jac[Pi.Count + kolvo_Q, i] = U_r[i] * sum2 + Ubasis * (_Gb[i] * Math.Cos(delta_r[i]) - _Bb[i] * Math.Sin(delta_r[i])) * U_r[i];
                    jac[Pi.Count + kolvo_Q, Pi.Count + kolvo_Q] = 2 * _B.getValue(i, i) * U_r[i] + sum1 + Ubasis * (_Bb[i] * Math.Cos(delta_r[i]) + _Gb[i] * Math.Sin(delta_r[i]));
                    jac[i, Pi.Count + kolvo_Q] = 2 * _G.getValue(i, i) * U_r[i] + sum2 + Ubasis * (_Gb[i] * Math.Cos(delta_r[i]) - _Bb[i] * Math.Sin(delta_r[i]));
                    kolvo_Q++;
                }

            }
        }
        private void Save_Result_pol(int Nomer, double F, List<int> nomera, StreamWriter wr)
        {

            wr.WriteLine("Итерация номер " + Nomer.ToString() + '	');
            wr.WriteLine("Номер узла " + '	' + "U1 " + '	' + "U2 " + '	' + "U " + '	' + "delta " + '	');
            for (int i = 0; i < nomera.Count; i++)
            {
                wr.WriteLine(nomera[i].ToString() + '	' + (U_r[i] * Math.Cos(delta_r[i])).ToString() + '	' + (U_r[i] * Math.Sin(delta_r[i])).ToString() + '	' + U_r[i].ToString() + '	' + delta_r[i].ToString() + '	');

            }
            wr.WriteLine("F= " + F.ToString() + '	');

        }


        public Complex[] Raschet_Polarniy(double eps, List<int> nomera)
        {
            StreamWriter writer = new StreamWriter("C:/TestResult/result2.txt");
            U_pribl = U.ToArray();
            delta_pribl = delta.ToArray();

           /* Complex[] U_res = method_Zeidela(0.0002, 10000, 1);

            for (int i = 0; i < Pi.Count; i++)
            {
                U_pribl[i] = U_res[i].Magnitude;
                delta_pribl[i] = U_res[i].Phase;
            }
            */
            bool flag01 = true, flag02 = true, flag03 = true;
            //       index_opor_uzlov.Clear();
            int iter = method_polyarniy(writer, 400, 0.001, nomera);

            while (flag01 || flag02 || flag03)
            {
                writer.WriteLine("vhod v zony rascheta");
                flag01 = false;
                flag02 = false;
                flag03 = false;
                foreach (Uzel item in GU)     //проверка на выход на верхний предел генерации
                {
                    item.Q_gen = (Qi[nomera.IndexOf(item.Nomer_uzla)]) + item.Q_load;
                    item.U_mod = (U_r[nomera.IndexOf(item.Nomer_uzla)]);
                    writer.Write(item.Nomer_uzla + "  " + item.Q_gen + " ");
                    if (item.Q_gen > item.Q_max)
                    {
                        if (index_opor_uzlov.Contains(nomera.IndexOf(item.Nomer_uzla)))
                        {
                            index_opor_uzlov.Remove(nomera.IndexOf(item.Nomer_uzla));
                            flag01 = true;
                            item.Q_gen = item.Q_max;
                            Qi[nomera.IndexOf(item.Nomer_uzla)] = (item.Q_gen - item.Q_load);
                            writer.WriteLine("remove po max " + item.Nomer_uzla + "  " + item.U_zad);
                        }
                    }

                }

                if (!flag01)
                {
                    writer.WriteLine("vhod v ustranenie pi min");
                    foreach (Uzel item in GU)  //проверка на выход на нижний предел генерации
                    {
                        writer.Write(item.Nomer_uzla + "  " + item.Q_gen + " ");
                        if (item.Q_gen < item.Q_min)
                        {
                            if (index_opor_uzlov.Contains(nomera.IndexOf(item.Nomer_uzla)))
                            {
                                index_opor_uzlov.Remove(nomera.IndexOf(item.Nomer_uzla));
                                flag03 = true;
                                item.Q_gen = item.Q_min;
                                Qi[nomera.IndexOf(item.Nomer_uzla)] = (item.Q_gen - item.Q_load);
                                writer.WriteLine("remove po min " + item.Nomer_uzla + "  " + item.U_zad);
                            }

                        }

                    }
                }


                if (!flag01 && !flag03)
                {
                    writer.WriteLine("vhod v obratniy");
                    foreach (Uzel item in GU)   //проверка на обратный вход в опорные генераторные узлы
                    {
                        writer.Write(item.Nomer_uzla + "  " + item.Q_gen + " " + item.U_mod);
                        if (item.U_mod > item.U_zad && item.Q_gen == item.Q_max)
                        {
                            index_opor_uzlov.Add(nomera.IndexOf(item.Nomer_uzla));
                            flag02 = true;
                            writer.WriteLine("iz NO v OP po Max " + item.Nomer_uzla);
                        }
                        else if (item.U_mod < item.U_zad && item.Q_gen == item.Q_min)
                        {
                            index_opor_uzlov.Add(nomera.IndexOf(item.Nomer_uzla));
                            flag02 = true;
                            writer.WriteLine("iz NO v OP po Min " + item.Nomer_uzla);
                        }

                    }
                }

                if (flag01 || flag02 || flag03)
                {
/*                    U_res = method_Zeidela(0.0001, 1000000, 0.9);

                    for (int i = 0; i < Pi.Count; i++)
                    {
                        U_pribl[i] = U_res[i].Magnitude;
                        delta_pribl[i] = U_res[i].Phase;
                    }
*/
                    method_polyarniy(writer, 400, 0.01, nomera);
                    foreach (int item in index_opor_uzlov)
                        writer.Write(item + " ");
                    writer.WriteLine();
                    foreach (Uzel item in GU)
                        writer.Write(item.Nomer_uzla + "  " + item.Q_gen + " ");
                }
            }

            Complex[] res_pol = new Complex[nomera.Count + 1];
            for (int i = 0; i < nomera.Count; i++)
            {
                res_pol[i] = new Complex(U_r[i], delta_r[i]);    //тут подумать, как быть
            }
            res_pol[nomera.Count] = new Complex(Raschet_S_Basisnogo_Uzla().Real, Raschet_S_Basisnogo_Uzla().Imaginary);


            writer.Close();
            return res_pol;
        }

        private Complex Raschet_S_Basisnogo_Uzla()
        {
            Complex Sbas = 0;
            for (int i = 0; i < Pi.Count; i++)
                Sbas = Sbas + (new Complex(_Gbb[i], _Bbb[i])) * (new Complex(U_r[i] * Math.Cos(delta_r[i]), -U_r[i] * Math.Sin(delta_r[i])));
            Sbas = Sbas + (new Complex(_Gbb[Pi.Count], _Bbb[Pi.Count])) * (new Complex(Ubasis, 0.0));
            return Sbas * Ubasis;
        }


        public Complex[] method_Zeidela(double eps, int Max_itter, double w)
        {
            Y_shemi.set_Moshnost(Pi, Qi);
            Y_shemi.set_U_Nach(U_pribl, delta_pribl);

            return Y_shemi.Method_Zeidela(Max_itter, eps, w, index_opor_uzlov);
        }




        /*     public Complex[] metod_Zeidela(List<int> nomera, int MaxIt)     //НЕ РАБОТАЮЩИЙ МЕТОД ЗЕЙДЕЛЯ
             {
                 StreamWriter writer = new StreamWriter("D://result2.txt");
                 U_complex = new Complex[Pi.Count];
                 Complex U_buf = new Complex();
                 Complex U_previous = new Complex();
                 double xxx = 0.0, eps=0.00001;
                 double Q1 = 0.0, Q2 = 0.0, Q_av = 0.0;
                 int nomer_uzla = 0, it = 0, kolvo=0;
                 bool flag = false, flag1 = false, priznak=false;
                 for (int i = 0; i < Pi.Count; i++)
                 {
                     U_complex[i] = new Complex(U[i] * Math.Cos(delta[i]), U[i] * Math.Sin(delta[i]));
                 }
                 while (it <= MaxIt && !priznak)
                 {
                     Save_Result_Zeidel(it, nomera, writer);
                     kolvo = 0;
                     for (int i = 0; i < Pi.Count; i++)
                     {
                         U_previous = U_complex[i];
                         Complex sum1 = new Complex(0.0, 0.0);
                         for (int j = 0; j < Pi.Count; j++)
                             if (i!=j)
                                 sum1 = sum1 + (new Complex(_G[i, j], _B[i, j])) * U_complex[j];
                         flag = false;
                         flag1 = false;
                         foreach (Uzel item in GU)
                             if (nomera.IndexOf(item.Nomer_uzla) == i)
                             {
                                 flag1 = true;                //узел вообще генераторный
                                 nomer_uzla = GU.IndexOf(item);   //записывается номер узла в массиве генераторных
                             }
                         foreach (int ind in index_opor_uzlov)
                             if (i == ind)
                                 flag = true;// узел опорный генераторный
                         if (flag)
                         {
                             U_buf = (Pi[i] / (new Complex(U_complex[i].Real, -U_complex[i].Imaginary)) - (new Complex(_Gb[i], _Bb[i])) * Ubasis - sum1);
                             xxx = U_buf.Imaginary * _G[i, i] * U_complex[i].Real + U_buf.Real * _B[i, i] * U_complex[i].Real + U_buf.Imaginary * _B.getValue(i,i) * U_complex[i].Imaginary - U_buf.Real * _G[i, i] * U_complex[i].Imaginary;
                             Q1 = xxx + Math.Sqrt(xxx * xxx - (_G[i, i] * _G[i, i] + _B.getValue(i,i) * _B.getValue(i,i)) * (U_complex[i].Real * U_complex[i].Real + U_complex[i].Imaginary * U_complex[i].Imaginary) *
                                 ((U_buf.Real * U_buf.Real + U_buf.Imaginary * U_buf.Imaginary) - (U_complex[i].Real * U_complex[i].Real + U_complex[i].Imaginary * U_complex[i].Imaginary)));
                             Q2 = xxx - Math.Sqrt(xxx * xxx - (_G[i, i] * _G[i, i] + _B[i, i] * _B[i, i]) * (U_complex[i].Real * U_complex[i].Real + U_complex[i].Imaginary * U_complex[i].Imaginary) *
                                 ((U_buf.Real * U_buf.Real + U_buf.Imaginary * U_buf.Imaginary) - (U_complex[i].Real * U_complex[i].Real + U_complex[i].Imaginary * U_complex[i].Imaginary)));
                             if (Q1 < (double)(GU[nomer_uzla].Q_max - GU[nomer_uzla].Q_load) && Q1 > (double)(GU[nomer_uzla].Q_min - GU[nomer_uzla].Q_load))
                             {
                                 Qi[i] = Q1;
                                 U_complex[i] = U_buf + (new Complex(0.0, -1.0)) * (1 / (new Complex(_G[i, i], _B[i, i]))) * (Qi[i] / (new Complex(U_complex[i].Real, -U_complex[i].Imaginary)));
                             }
                             else if ((Q2 < (double)(GU[nomer_uzla].Q_max - GU[nomer_uzla].Q_load) && Q2 > (double)(GU[nomer_uzla].Q_min - GU[nomer_uzla].Q_load)))
                             {
                                 Qi[i] = Q2;
                                 U_complex[i] = U_buf + (new Complex(0.0, -1.0)) * (1 / (new Complex(_G[i, i], _B[i, i]))) * (Qi[i] / (new Complex(U_complex[i].Real, -U_complex[i].Imaginary)));
                             }
                             else
                             {
                                 Q_av = (double)(GU[nomer_uzla].Q_max - GU[nomer_uzla].Q_load + GU[nomer_uzla].Q_min - GU[nomer_uzla].Q_load) / 2;
                                 if (Math.Abs(Q1 - Q_av) < Math.Abs(Q2 - Q_av))
                                 {
                                     if (Q1 > (double)(GU[nomer_uzla].Q_max - GU[nomer_uzla].Q_load))
                                         Qi[i] = (double)(GU[nomer_uzla].Q_max - GU[nomer_uzla].Q_load);
                                     else
                                         Qi[i] = (double)(GU[nomer_uzla].Q_min - GU[nomer_uzla].Q_load);
                                 }
                                 else
                                 {
                                     if (Q2 > (double)(GU[nomer_uzla].Q_max - GU[nomer_uzla].Q_load))
                                         Qi[i] = (double)(GU[nomer_uzla].Q_max - GU[nomer_uzla].Q_load);
                                     else
                                         Qi[i] = (double)(GU[nomer_uzla].Q_min - GU[nomer_uzla].Q_load);
                                 }
                                 index_opor_uzlov.Remove(nomera.IndexOf(i));
                                 U_complex[i] = U_buf + (new Complex(0.0, -1.0)) * (1 / (new Complex(_G[i, i], _B[i, i]))) * (Qi[i] / (new Complex(U_complex[i].Real, -U_complex[i].Imaginary)));
                             }
                         }
                         else if (flag1)
                         {
                             U_complex[i] = (((new Complex(Pi[i], -Qi[i])) / (new Complex(U_complex[i].Real, -U_complex[i].Imaginary)) - (new Complex(_Gb[i], _Bb[i])) * Ubasis - sum1)) / (new Complex(_G[i, i], _B[i, i]));
                             if (Qi[i] == (double)(GU[nomer_uzla].Q_max - GU[nomer_uzla].Q_load) && U_complex[i].Magnitude > (double)GU[nomer_uzla].U_zad)
                             {
                                 index_opor_uzlov.Add(nomera.IndexOf(i));
                                 U_complex[i] = new Complex(((double)GU[nomer_uzla].U_zad * Math.Cos(U_complex[i].Phase)), ((double)GU[nomer_uzla].U_zad * Math.Sin(U_complex[i].Phase)));
                             }
                             else if (Qi[i] == (double)(GU[nomer_uzla].Q_min - GU[nomer_uzla].Q_load) && U_complex[i].Magnitude < (double)GU[nomer_uzla].U_zad)
                             {
                                 index_opor_uzlov.Add(nomera.IndexOf(i));
                                 U_complex[i] = new Complex(((double)GU[nomer_uzla].U_zad * Math.Cos(U_complex[i].Phase)), ((double)GU[nomer_uzla].U_zad * Math.Sin(U_complex[i].Phase)));
                             }
                         }
                         else
                         {
                             U_complex[i] = (((new Complex(Pi[i], -Qi[i])) / (new Complex(U_previous.Real, -U_previous.Imaginary))) - ((new Complex(_Gb[i], _Bb[i])) * (new Complex (Ubasis, 0.0))) - sum1) / (new Complex(_G[i, i], _B[i, i]));
                        //     U_complex[i] = (((new Complex(Pi[i], -Qi[i])) / (new Complex(U_complex[i].Real, -U_complex[i].Imaginary)) - (new Complex(_Gb[i], _Bb[i])) * (new Complex (Ubasis, 0.0)) - sum1)) / (new Complex(_G[i, i], _B[i, i]));
                         }
                         if (Math.Abs(U_complex[i].Magnitude - U_previous.Magnitude) < eps && Math.Abs(U_complex[i].Phase - U_previous.Phase) < eps)
                             kolvo++;
                     }
                     if (kolvo == Pi.Count)
                         priznak = true;
                     it++;
                 }
                 writer.Close();
                 return U_complex;


             }   


             private void Save_Result_Zeidel(int Nomer, List<int> nomera, StreamWriter wr)
             {

                 wr.WriteLine("Итерация номер " + Nomer.ToString() + '	');
                 wr.WriteLine("Номер узла " + '	' + "U1 " + '	' + "U2 " + '	' + "U " + '	' + "delta " + '	');
                 for (int i = 0; i < nomera.Count; i++)
                 {
                     wr.WriteLine(nomera[i].ToString() + '	' + U_complex[i].Real.ToString() + '	' + U_complex[i].Imaginary.ToString() + '	' + U_complex[i].Magnitude.ToString() + '	' + U_complex[i].Phase.ToString() + '	');

                 }
             }         */




    }
}


