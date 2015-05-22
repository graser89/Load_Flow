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
        static HashSpareMatrix _G;
        static HashSpareMatrix _B;
        static double[] _Gb;
        static double[] _Bb;
        static int _method = 0;
        static double[] _Gbb;
        static double[] _Bbb;
        static double[] _uR;
        static double[] _deltaR;
        static double[] _x;
        static List<Uzel> GU;
        readonly SortSpareMatrixComplex _yShemi;

        public regim(List<double> U1, List<double> delta1, List<double> Pi1, List<double> Qi1, List<int> index_opor_uzlov1, double Ubasis1, IMatrix G1, IMatrix B1, double[] Gb1, double[] Bb1, List<Uzel> GU1, double[] Gbb1, double[] Bbb1, Complex[,] Y_1)
        {

            U = U1;
            delta = delta1;
            Pi = Pi1;
            Qi = Qi1;
            index_opor_uzlov = index_opor_uzlov1;
            Ubasis = Ubasis1;
            _G = (HashSpareMatrix)G1;
            _B = (HashSpareMatrix)B1;
            _Gb = Gb1;
            _Bb = Bb1;
            _Gbb = Gbb1;
            _Bbb = Bbb1;
            _method = 2;
            GU = GU1;
            _yShemi = new SortSpareMatrixComplex(Pi.Count, Ubasis);
            for (int i = 0; i < Pi.Count; i++)
                for (int j = 0; j < Pi.Count; j++)
                    _yShemi.setValue(i, j, Y_1[i, j]);
            _yShemi.Convert_to_crs();
            _yShemi.set_Y_bal(_Gb, _Bb);
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

        

        private int method_polyarniy(StreamWriter writer, int MaxItter, double eps, List<int> nomera)
        {
            int count = Pi.Count * 2 - index_opor_uzlov.Count;
            _x = new double[count];
            int kolvo = 0;
            for (int i = 0; i < Pi.Count; i++)
            {
                bool flag = false;
                foreach (int ind in index_opor_uzlov)
                    if (i == ind)
                        flag = true;
                if (!flag)
                {
                    _x[i] = delta_pribl[i];
                    _x[Pi.Count + kolvo] = U_pribl[i];
                    kolvo++;
                }
                else
                    _x[i] = delta_pribl[i];
            }
            index_opor_uzlov.Sort();
            var func = new double[count];
            var resPol = new double[count];   //потом обдумать
            var Jacobi = new double[count, count];
            int itter = 1;
            double lamb = 1;
            double f1 = 0;
            double f = 0;
            bool flag_iterr = true;


            do
            {
                object obj = 0;


                //Raschet_dw_pol(x, func, Jacobian, obj);
                Raschet_fi_pol(_x, func, obj);
                
                f = Raschet_F(func);
                if ((Math.Abs(f - f1) < eps) & (f < eps))
                {
                    break;
                }
                else
                    if ((f > f1) && (itter > 1))
                    {
                        lamb = Raschet_Priraschenia_x_pol(resPol, false, Math.Abs(f1 / f ));
                        continue;
                    }
                int info;                
                alglib.densesolverreport rep1;

                Raschet_dw_pol(ref Jacobi);
#if TRACE
                var writter2 = new StreamWriter("newton_jacobis" + itter + ".csv");
                Save_jacobi(writter2, Jacobi, func);
                writter2.Close();
#endif
                alglib.rmatrixsolve(Jacobi, count,  func, out info, out rep1, out resPol);
                
                lamb = Raschet_Priraschenia_x_pol(resPol, true, Math.Sqrt(f) / Pi.Count);
                f1 = f;
                Save_Result_pol(itter, f, nomera, writer);
                itter++;
            }
            while (itter < MaxItter);


            delta_pribl = _deltaR;
            U_pribl = _uR;
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
                    _x[i] = _x[i] + resPoll[i];
                }
                else
                    _x[i] = _x[i] - resPoll[i] + lamb * resPoll[i];
            }
            return lamb;
        }


        private static void Raschet_fi_pol(double[] x, double[] fi, object obj)
        {

            _uR = new double[Pi.Count];  //вот здесь с опорными
            _deltaR = new double[Pi.Count];
            int kolvo = 0;
            for (int i = 0; i < Pi.Count; i++)
            {
                bool flag0 = false;
                foreach (int ind in index_opor_uzlov)
                    if (i == ind)
                        flag0 = true;
                if (!flag0)
                {
                    _uR[i] = x[Pi.Count + kolvo];
                    _deltaR[i] = x[i];
                    kolvo++;
                }
                else
                {
                    _uR[i] = U[i];
                    _deltaR[i] = x[i];
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
                        sum1 = sum1 + _uR[j] * (_G.getValue(i, j) * Math.Cos(_deltaR[i] - _deltaR[j]) - _B.getValue(i, j) * Math.Sin(_deltaR[i] - _deltaR[j]));
                        sum2 = sum2 + _uR[j] * (_G.getValue(i, j) * Math.Sin(_deltaR[i] - _deltaR[j]) + _B.getValue(i, j) * Math.Cos(_deltaR[i] - _deltaR[j]));
                    }
                fi[i] = -(_uR[i] * _uR[i] * _G.getValue(i, i) + _uR[i] * sum1 + (_Gb[i] * Math.Cos(_deltaR[i]) - _Bb[i] * Math.Sin(_deltaR[i])) * _uR[i] * Ubasis - Pi[i]);
                if (flag)
                {
                    Qi[i] = _uR[i] * _uR[i] * _B.getValue(i, i) + _uR[i] * sum2 + (_Bb[i] * Math.Cos(_deltaR[i]) + _Gb[i] * Math.Sin(_deltaR[i])) * _uR[i] * Ubasis;
                }
                else
                {
                    fi[Pi.Count + per_po_Q] = -(_uR[i] * _uR[i] * _B.getValue(i, i) + _uR[i] * sum2 + (_Bb[i] * Math.Cos(_deltaR[i]) + _Gb[i] * Math.Sin(_deltaR[i])) * _uR[i] * Ubasis - Qi[i]);
                    per_po_Q++;
                }

            }
        }
        //function1_jac(double[] x, double[] fi, double[,] jac, object obj)
        private static void Raschet_dw_pol(double[] x, double[] fi, HashSpareMatrix jac, object obj)
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
                        jac.setValue(i, j, _uR[i] * _uR[j] * (_G.getValue(i, j) * Math.Sin(_deltaR[i] - _deltaR[j]) + _B.getValue(i, j) * Math.Cos(_deltaR[i] - _deltaR[j])));
                        sum1 = sum1 + _uR[j] * (_G.getValue(i, j) * Math.Sin(_deltaR[i] - _deltaR[j]) + _B.getValue(i, j) * Math.Cos(_deltaR[i] - _deltaR[j]));
                        sum2 = sum2 + _uR[j] * (_G.getValue(i, j) * Math.Cos(_deltaR[i] - _deltaR[j]) - _B.getValue(i, j) * Math.Sin(_deltaR[i] - _deltaR[j]));
                    }
                    bool flag1 = false;
                    foreach (int ind1 in index_opor_uzlov)
                        if (j == ind1)
                            flag1 = true;// узел опорный

                    if (!flag1)
                    {
                        if (i != j)
                        {
                            jac.setValue(i, Pi.Count + kolvo_QQ, _uR[i] * (_G.getValue(i, j) * Math.Cos(_deltaR[i] - _deltaR[j]) - _B.getValue(i, j) * Math.Sin(_deltaR[i] - _deltaR[j])));
                            if (!flag)
                                jac.setValue(Pi.Count + kolvo_Q, Pi.Count + kolvo_QQ, _uR[i] * (_G.getValue(i, j) * Math.Sin(_deltaR[i] - _deltaR[j]) + _B.getValue(i, j) * Math.Cos(_deltaR[i] - _deltaR[j])));
                        }
                        kolvo_QQ++;
                    }
                    if (!flag)
                        if (i != j)
                            jac.setValue(Pi.Count + kolvo_Q, j, -_uR[i] * _uR[j] * (_G.getValue(i, j) * Math.Cos(_deltaR[i] - _deltaR[j]) - _B.getValue(i, j) * Math.Sin(_deltaR[i] - _deltaR[j])));

                }
                jac.setValue(i, i, -_uR[i] * sum1 - _uR[i] * Ubasis * (_Bb[i] * Math.Cos(_deltaR[i]) + _Gb[i] * Math.Sin(_deltaR[i])));
                if (!flag)
                {
                    jac.setValue(Pi.Count + kolvo_Q, i, _uR[i] * sum2 - Ubasis * (_Gb[i] * Math.Cos(_deltaR[i]) - _Bb[i] * Math.Sin(_deltaR[i])) * _uR[i]);
                    jac.setValue(Pi.Count + kolvo_Q, Pi.Count + kolvo_Q, 2 * _B.getValue(i, i) * _uR[i] + sum1 + Ubasis * (_Bb[i] * Math.Cos(_deltaR[i]) + _Gb[i] * Math.Sin(_deltaR[i])));
                    jac.setValue(i, Pi.Count + kolvo_Q, 2 * _G.getValue(i, i) * _uR[i] + sum2 + Ubasis * (_Gb[i] * Math.Cos(_deltaR[i]) - _Bb[i] * Math.Sin(_deltaR[i])));
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
                        jac[i, j] = _uR[i] * _uR[j] * (_G.getValue(i, j) * Math.Sin(_deltaR[i] - _deltaR[j]) + _B.getValue(i, j) * Math.Cos(_deltaR[i] - _deltaR[j]));
                        sum1 = sum1 + _uR[j] * (_G.getValue(i, j) * Math.Sin(_deltaR[i] - _deltaR[j]) + _B.getValue(i, j) * Math.Cos(_deltaR[i] - _deltaR[j]));
                        sum2 = sum2 + _uR[j] * (_G.getValue(i, j) * Math.Cos(_deltaR[i] - _deltaR[j]) - _B.getValue(i, j) * Math.Sin(_deltaR[i] - _deltaR[j]));
                    }
                    bool flag1 = false;
                    foreach (int ind1 in index_opor_uzlov)
                        if (j == ind1)
                            flag1 = true;// узел опорный

                    if (!flag1)
                    {
                        if (i != j)
                        {
                            jac[i, Pi.Count + kolvo_QQ] = _uR[i] * (_G.getValue(i, j) * Math.Cos(_deltaR[i] - _deltaR[j]) - _B.getValue(i, j) * Math.Sin(_deltaR[i] - _deltaR[j]));
                            if (!flag)
                                jac[Pi.Count + kolvo_Q, Pi.Count + kolvo_QQ] = _uR[i] * (_G.getValue(i, j) * Math.Sin(_deltaR[i] - _deltaR[j]) + _B.getValue(i, j) * Math.Cos(_deltaR[i] - _deltaR[j]));
                        }
                        kolvo_QQ++;
                    }
                    if (!flag)
                        if (i != j)
                            jac[Pi.Count + kolvo_Q, j] = -_uR[i] * _uR[j] * (_G.getValue(i, j) * Math.Cos(_deltaR[i] - _deltaR[j]) - _B.getValue(i, j) * Math.Sin(_deltaR[i] - _deltaR[j]));

                }
                jac[i, i] = -_uR[i] * sum1 - _uR[i] * Ubasis * (_Bb[i] * Math.Cos(_deltaR[i]) + _Gb[i] * Math.Sin(_deltaR[i]));
                if (!flag)
                {
                    jac[Pi.Count + kolvo_Q, i] = _uR[i] * sum2 - Ubasis * (_Gb[i] * Math.Cos(_deltaR[i]) - _Bb[i] * Math.Sin(_deltaR[i])) * _uR[i];
                    jac[Pi.Count + kolvo_Q, Pi.Count + kolvo_Q] = 2 * _B.getValue(i, i) * _uR[i] + sum1 + Ubasis * (_Bb[i] * Math.Cos(_deltaR[i]) + _Gb[i] * Math.Sin(_deltaR[i]));
                    jac[i, Pi.Count + kolvo_Q] = 2 * _G.getValue(i, i) * _uR[i] + sum2 + Ubasis * (_Gb[i] * Math.Cos(_deltaR[i]) - _Bb[i] * Math.Sin(_deltaR[i]));
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
                        jac[i, j] = _uR[i] * _uR[j] * (_G.getValue(i, j) * Math.Sin(_deltaR[i] - _deltaR[j]) + _B.getValue(i, j) * Math.Cos(_deltaR[i] - _deltaR[j]));
                        sum1 = sum1 + _uR[j] * (_G.getValue(i, j) * Math.Sin(_deltaR[i] - _deltaR[j]) + _B.getValue(i, j) * Math.Cos(_deltaR[i] - _deltaR[j]));
                        sum2 = sum2 + _uR[j] * (_G.getValue(i, j) * Math.Cos(_deltaR[i] - _deltaR[j]) - _B.getValue(i, j) * Math.Sin(_deltaR[i] - _deltaR[j]));
                    }


                    if (!index_opor_uzlov.Contains(j))
                    {
                        if (i != j)
                        {
                            jac[i, Pi.Count + kolvo_QQ] = _uR[i] * (_G.getValue(i, j) * Math.Cos(_deltaR[i] - _deltaR[j]) - _B.getValue(i, j) * Math.Sin(_deltaR[i] - _deltaR[j]));
                            if (!index_opor_uzlov.Contains(i))
                                jac[Pi.Count + kolvo_Q, Pi.Count + kolvo_QQ] = _uR[i] * (_G.getValue(i, j) * Math.Sin(_deltaR[i] - _deltaR[j]) + _B.getValue(i, j) * Math.Cos(_deltaR[i] - _deltaR[j]));
                        }
                        kolvo_QQ++;
                    }
                    if (index_opor_uzlov.Contains(i)) continue;
                    if (i != j)
                        jac[Pi.Count + kolvo_Q, j] = -_uR[i] * _uR[j] * (_G.getValue(i, j) * Math.Cos(_deltaR[i] - _deltaR[j]) - _B.getValue(i, j) * Math.Sin(_deltaR[i] - _deltaR[j]));
                }
                jac[i, i] = -_uR[i] * sum1 - _uR[i] * Ubasis * (_Bb[i] * Math.Cos(_deltaR[i]) + _Gb[i] * Math.Sin(_deltaR[i]));
                if (!index_opor_uzlov.Contains(i))
                {
                    jac[Pi.Count + kolvo_Q, i] = _uR[i] * sum2 + Ubasis * (_Gb[i] * Math.Cos(_deltaR[i]) - _Bb[i] * Math.Sin(_deltaR[i])) * _uR[i];
                    jac[Pi.Count + kolvo_Q, Pi.Count + kolvo_Q] = 2 * _B.getValue(i, i) * _uR[i] + sum1 + Ubasis * (_Bb[i] * Math.Cos(_deltaR[i]) + _Gb[i] * Math.Sin(_deltaR[i]));
                    jac[i, Pi.Count + kolvo_Q] = 2 * _G.getValue(i, i) * _uR[i] + sum2 + Ubasis * (_Gb[i] * Math.Cos(_deltaR[i]) - _Bb[i] * Math.Sin(_deltaR[i]));
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
                wr.WriteLine(nomera[i].ToString() + '	' + (_uR[i] * Math.Cos(_deltaR[i])).ToString() + '	' + (_uR[i] * Math.Sin(_deltaR[i])).ToString() + '	' + _uR[i].ToString() + '	' + _deltaR[i].ToString() + '	');

            }
            wr.WriteLine("F= " + F.ToString() + '	');

        }


        public Complex[] Raschet_Polarniy(double eps, List<int> nomera)
        {
            StreamWriter writer = new StreamWriter("result2.txt");
            U_pribl = U.ToArray();
            delta_pribl = delta.ToArray();

            Complex[] U_res = method_Zeidela(0.0002, 10000, 1);

            for (int i = 0; i < Pi.Count; i++)
            {
                U_pribl[i] = U_res[i].Magnitude;
                delta_pribl[i] = U_res[i].Phase;
            }
            
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
                    item.QGen = (Qi[nomera.IndexOf(item.NomerUzla)]) + item.QLoad;
                    item.UMod = (_uR[nomera.IndexOf(item.NomerUzla)]);
                    writer.Write(item.NomerUzla + "  " + item.QGen + " ");
                    if (!(item.QGen > item.QMax)) continue;
                    if (!index_opor_uzlov.Contains(nomera.IndexOf(item.NomerUzla))) continue;
                    index_opor_uzlov.Remove(nomera.IndexOf(item.NomerUzla));
                    flag01 = true;
                    item.QGen = item.QMax;
                    Qi[nomera.IndexOf(item.NomerUzla)] = (item.QGen - item.QLoad);
                    writer.WriteLine("remove po max " + item.NomerUzla + "  " + item.UZad);
                }

                if (!flag01)
                {
                    writer.WriteLine("vhod v ustranenie pi min");
                    foreach (Uzel item in GU)  //проверка на выход на нижний предел генерации
                    {
                        writer.Write(item.NomerUzla + "  " + item.QGen + " ");
                        if (!(item.QGen < item.QMin)) continue;
                        if (!index_opor_uzlov.Contains(nomera.IndexOf(item.NomerUzla))) continue;
                        index_opor_uzlov.Remove(nomera.IndexOf(item.NomerUzla));
                        flag03 = true;
                        item.QGen = item.QMin;
                        Qi[nomera.IndexOf(item.NomerUzla)] = (item.QGen - item.QLoad);
                        writer.WriteLine("remove po min " + item.NomerUzla + "  " + item.UZad);
                    }
                }


                if (!flag01 && !flag03)
                {
                    writer.WriteLine("vhod v obratniy");
                    foreach (Uzel item in GU)   //проверка на обратный вход в опорные генераторные узлы
                    {
                        writer.Write(item.NomerUzla + "  " + item.QGen + " " + item.UMod);
                        if (item.UMod > item.UZad && item.QGen == item.QMax)
                        {
                            index_opor_uzlov.Add(nomera.IndexOf(item.NomerUzla));
                            flag02 = true;
                            writer.WriteLine("iz NO v OP po Max " + item.NomerUzla);
                        }
                        else if (item.UMod < item.UZad && item.QGen == item.QMin)
                        {
                            index_opor_uzlov.Add(nomera.IndexOf(item.NomerUzla));
                            flag02 = true;
                            writer.WriteLine("iz NO v OP po Min " + item.NomerUzla);
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
                        writer.Write(item.NomerUzla + "  " + item.QGen + " ");
                }
            }

            Complex[] res_pol = new Complex[nomera.Count + 1];
            for (int i = 0; i < nomera.Count; i++)
            {
                res_pol[i] = new Complex(_uR[i], _deltaR[i]);    //тут подумать, как быть
            }
            res_pol[nomera.Count] = new Complex(Raschet_S_Basisnogo_Uzla().Real, Raschet_S_Basisnogo_Uzla().Imaginary);


            writer.Close();
            return res_pol;
        }

        private Complex Raschet_S_Basisnogo_Uzla()
        {
            Complex Sbas = 0;
            for (int i = 0; i < Pi.Count; i++)
                Sbas = Sbas + (new Complex(_Gbb[i], _Bbb[i])) * (new Complex(_uR[i] * Math.Cos(_deltaR[i]), -_uR[i] * Math.Sin(_deltaR[i])));
            Sbas = Sbas + (new Complex(_Gbb[Pi.Count], _Bbb[Pi.Count])) * (new Complex(Ubasis, 0.0));
            return Sbas * Ubasis;
        }


        public Complex[] method_Zeidela(double eps, int maxItter, double w)
        {
            _yShemi.set_Moshnost(Pi, Qi);
            _yShemi.set_U_Nach(U_pribl, delta_pribl);

            return _yShemi.Method_Zeidela(maxItter, eps, w, index_opor_uzlov);
        }




       

    }
}


