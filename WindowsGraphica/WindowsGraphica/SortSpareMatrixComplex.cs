using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;

namespace WindowsGraphica
{
    public class SortSpareMatrixComplex
    {
        SortedList[] _Stroki;
        List<int>[] _stolbci;
        int n;
        Complex[] ADiag;
        Complex[] AElement_crs;
        int[] jptr_crs;
        int[] iptr_crs;

        List<int> index_opor_uzlov;
        Complex[] U;
        Complex[] Moshnst;
        Complex[] Yb;
        double U_basis;


        public SortSpareMatrixComplex(int N, double Ubas)
        {
            n = N;
            _Stroki = new SortedList[n];
            _stolbci = new List<int>[n];
            for (int i = 0; i < n; i++)
            {
                _Stroki[i] = new SortedList();
                _stolbci[i] = new List<int>();
            }
            U_basis = Ubas;
        }

        private void Find_Index_Element(int row, int col, out bool nalichie)
        {
            nalichie = _Stroki[row].ContainsKey(col);
        }

        public void set_U_Nach(List<double> Napr, List<double> delta)
        {
            List<Complex> U1 = new List<Complex>();
            for (int i = 0; i < Napr.Count; i++)
            {
                U1.Add(new Complex(Napr[i] * Math.Cos(delta[i]), Napr[i] * Math.Sin(delta[i])));
            }
            U = U1.ToArray();
        }

        public void set_U_Nach(double[] Napr, double[] delta)
        {
            List<Complex> U1 = new List<Complex>();
            for (int i = 0; i < Napr.Length; i++)
            {
                U1.Add(new Complex(Napr[i] * Math.Cos(delta[i]), Napr[i] * Math.Sin(delta[i])));
            }
            U = U1.ToArray();
        }

        public void set_Moshnost(List<double> P, List<double> Q)
        {
            List<Complex> Moshnst1 = new List<Complex>();
            for (int i = 0; i < P.Count; i++)
            {
                Moshnst1.Add(new Complex(P[i], Q[i]));
            }
            Moshnst = Moshnst1.ToArray();
        }

        public void set_Y_bal(double[] _G_b, double[] _B_b)
        {
            List<Complex> Y_bal = new List<Complex>();
            for (int i = 0; i < n; i++)
            {
                Y_bal.Add(new Complex(_G_b[i], -_B_b[i]));
            }
            Yb = Y_bal.ToArray();
        }

        public Complex Raschet_J(int i)
        {
            Complex J = new Complex();
            Complex S;
            if (index_opor_uzlov.Contains(i))
                S = new Complex(Moshnst[i].Real, 0);
            else
                S = Complex.Conjugate(Moshnst[i]);
            J = S / Complex.Conjugate(U[i]) - Yb[i] * U_basis;
            return J;
        }

        public Complex CalculateFi()
        {
            Complex sum = 0;
            for (int i = 0; i < n; i++)
            {
                Complex current = 0;
                for (int index2 = iptr_crs[i]; index2 < iptr_crs[i + 1]; index2++)
                {
                    current += AElement_crs[index2]*U[jptr_crs[index2]];
                }
                current = current + ADiag[i]*U[i];
                var nebalans= Complex.Conjugate(Raschet_J(i) - current)*U[i];
                if (index_opor_uzlov.Contains(i))
                    nebalans=new Complex(nebalans.Real,0);
                sum += nebalans;
            }
            return sum;
        }

        public Complex[] Method_Zeidela(int maxitter, double MaxEps, double w, List<int> index_op_uzlov)
        {
            
            Stopwatch timer1 = new Stopwatch();
            Stopwatch timer2 = new Stopwatch();
            timer2.Start();
            StreamWriter wri_timer = new StreamWriter("res_Zeidel_timer.csv");
            StreamWriter wriRes = new StreamWriter("res_Zeidel_result.csv");

            wriRes.Write("itteration ;" + "Nebalans.Real ;" + "Nebalans.Imaginary ;");
            wriRes.WriteLine("MaxEPS;");

            index_opor_uzlov = index_op_uzlov;
            int ittr = 0;
            Complex sum = 0;
            double MaxOshibka = 0;
            //int[] poriad = poriadok(resh, B);
            do
            {
                timer1.Restart();
                MaxOshibka = 0;

                for (int i = 0; i < n; i++)
                {
                    int index = 0;
                    if (ittr % 2 == 0)
                        index = n - 1 - i;
                    else
                        index = i;

                    sum = 0;
                    for (int index2 = iptr_crs[index]; index2 < iptr_crs[index + 1]; index2++)
                    {
                        sum += AElement_crs[index2] * U[jptr_crs[index2]] / (ADiag[index]);
                    }
                    Complex result = Raschet_J(index) / ADiag[index] - sum;
                    Complex U_new = 0;
                    Opred_Q(index, ref result, ref U_new, w);

                    Complex delta_U = U_new - U[index];

                    double eps = Math.Abs((delta_U.Magnitude) / (U[index].Magnitude));
                    U[index] = U_new;
                    if (eps > MaxOshibka)
                        MaxOshibka = eps;
                }

                ittr++;

                timer1.Stop();
                wri_timer.WriteLine(ittr.ToString() + ";" + timer1.ElapsedMilliseconds.ToString() + ";" + MaxOshibka.ToString() + ";");
                var fi = CalculateFi();
                wriRes.Write(ittr.ToString() + " ;" + fi.Real.ToString() + " ;" +fi.Imaginary.ToString() + " ;");

                wriRes.WriteLine(MaxOshibka.ToString() + ";");

            }
            while (((MaxOshibka > MaxEps) && (maxitter > ittr)));

            timer2.Stop();
            wri_timer.WriteLine(ittr.ToString() + ";" + timer2.ElapsedMilliseconds.ToString() + ";" + MaxOshibka.ToString() + ";");
            wri_timer.Close();
            wriRes.Close();
            return U;
        }

        private void Opred_Q(int index, ref Complex result, ref Complex U_new, double w)
        {
            if (index_opor_uzlov.Contains(index))
            {
                Complex U_k_1 = U[index];
                Complex V = result;
                Complex Y = ADiag[index];
                double k = (V.Real * (Y.Real * U_k_1.Imaginary - Y.Imaginary * U_k_1.Real) - V.Imaginary * (Y.Imaginary * U_k_1.Imaginary + Y.Real * U_k_1.Real));

                Complex koef_b = Complex.ImaginaryOne * (V * Y * Complex.Conjugate(U_k_1) - Complex.Conjugate(V) * Complex.Conjugate(Y) * U_k_1);
                double koef_c = Y.Magnitude * Y.Magnitude * U_k_1.Magnitude * U_k_1.Magnitude * (V.Magnitude * V.Magnitude - U_k_1.Magnitude * U_k_1.Magnitude);
                double D = k * k - koef_c;
                if (D < 0)
                {
                    double f1 = Y.Phase - U_k_1.Phase;
                    double a = V.Real / (U_k_1.Magnitude * Y.Magnitude * U_k_1.Magnitude);
                    double a1 = Math.Acos(a);
                    double fi = a1 - f1;
                    Complex U1 = new Complex(U_k_1.Magnitude * Math.Cos(fi), U_k_1.Magnitude * Math.Sin(fi));

                    Complex Chi = Y * Complex.Conjugate(U_k_1);
                    Complex Chislo = Chi * (V - U1);
                    double Q1 = Chislo.Imaginary;

                    Complex U2 = V - Complex.ImaginaryOne * Q1 / (Y * Complex.Conjugate(U_k_1));
                    Moshnst[index] = new Complex(Moshnst[index].Real, Q1);
                    U_new = U1;
                }
                else
                {
                    double Q1 = (-k + Math.Sqrt(D));
                    double Q2 = (-k - Math.Sqrt(D));
                    Complex U1 = V - Complex.ImaginaryOne * Q1 / (Y * Complex.Conjugate(U_k_1));
                    Complex U2 = V - Complex.ImaginaryOne * Q2 / (Y * Complex.Conjugate(U_k_1));
                    double d1 = (U1 - U_k_1).Magnitude / U_k_1.Magnitude;
                    double d2 = (U2 - U_k_1).Magnitude / U_k_1.Magnitude;
                    if (Math.Abs(d1) < Math.Abs(d2))
                    {
                        Moshnst[index] = new Complex(Moshnst[index].Real, Q1);
                        U_new = U1;
                    }
                    else
                    {
                        Moshnst[index] = new Complex(Moshnst[index].Real, Q2);
                        U_new = U2;
                    }

                }


            }
            else
            {
                Complex uK1 = U[index];

                U_new = U[index] + w * (result - U[index]);
            }
        }

        private void Insert(int row, int col, Complex value)
        {
            _Stroki[row].Add(col, value);
            _stolbci[col].Add(row);
        }

        private void RemovAt(int row, int col)
        {
            _Stroki[row].Remove(col);
            _stolbci[col].Remove(row);
        }

        public void setValue(int row, int col, Complex value)
        {
            if (col > n) return;
            if (row > n) return;

            bool nalichie;
            Find_Index_Element(row, col, out nalichie);
            if (nalichie)
            {
                if (value.Magnitude == 0)
                    RemovAt(row, col);
                else
                    _Stroki[row][col] = value;
            }
            else
            {
                if (value.Magnitude != 0)
                    Insert(row, col, value);
            }
        }

        // добавляет значение value к ячейке [row,col]
        public void addValue(int row, int col, Complex value)
        {
            if (col > n) return;
            if (row > n) return;
            bool nalichie;
            Find_Index_Element(row, col, out nalichie);
            if (nalichie)
            {
                if (value != 0)
                    _Stroki[row][col] = (Complex)_Stroki[row][col] + value;
            }
            else
                if (value != 0)
                    Insert(row, col, value);
        }

        // возвращает значение ячейки [row,col]
        public Complex getValue(int row, int col)
        {
            if (col > n) return 0;
            if (row > n) return 0;
            bool nalichie;
            Find_Index_Element(row, col, out nalichie);
            if (nalichie)
                return (Complex)_Stroki[row][col];
            else
                return 0;
        }

        // возвращает порядок матрицы
        public int getN()
        {
            return n;
        }

        // возвращает ненулевые значения и индексы ячеек строки d,
        // которые находятся правее главной диагонали
        public void getJRow(int d, ref int[] indexes, ref Complex[] values)
        {
            if (d >= n) return;
            List<int> indexs = new List<int>();
            List<Complex> Val = new List<Complex>();
            for (int i = 0; i < _Stroki[d].Count; i++)
            {
                int j = (int)_Stroki[d].GetKey(i);
                if (j > d)
                {

                    indexs.Add(j);
                    Val.Add((Complex)_Stroki[d][j]);
                }
            }
            indexes = indexs.ToArray();
            values = Val.ToArray();
        }

        // возвращает ненулевые значения и индексы ячеек столбца d, 
        // которые находятся ниже главной диагонали
        public void getJCol(int d, ref int[] indexes, ref Complex[] values)
        {
            if (d >= n) return;
            List<int> indexs = new List<int>();
            List<Complex> Val = new List<Complex>();

            for (int i = 0; i < _stolbci[d].Count; i++)
            {
                int k = _stolbci[d][i];//номер строчки с ненулевым элемнтом в столбце d
                if (k > d)
                {
                    indexs.Add(k);
                    Val.Add((Complex)_Stroki[k][d]);
                }
            }
            indexes = indexs.ToArray();
            values = Val.ToArray();
        }

        //возвращает индексы строк с нулями на главной диагонали
        public void getIndexsZero_dd(ref int[] indexes)
        {
            List<int> list = new List<int>();

            bool nal;
            for (int i = 0; i < n; i++)
            {
                Find_Index_Element(i, i, out nal);
                if (!nal)
                {
                    list.Add(i);
                }
            }
            indexes = list.ToArray();

        }

        //возвращает индекс строки с доминирующим элементом в колонке рассматриваются только низлижащие строки
        public int getDominElemColum(int d)//row индекс строки  поэтому ищем максимальный по мод элемент в столбце с тем же индексом
        {

            if (d >= n) return -1;

            double max = 0;
            int res = -1;
            for (int i = 0; i < _stolbci[d].Count; i++)
            {
                int k = _stolbci[d][i];//номер строчки с ненулевым элемнтом в столбце d
                if (k > d)
                {
                    if (max < ((Complex)_Stroki[k][d]).Magnitude)
                    {
                        max = ((Complex)_Stroki[k][d]).Magnitude;
                        res = k;
                    }
                }
            }
            return res;
        }

        //меняет указанные строки местами
        public void ChangedRow(int row1, int row2)
        {
            if (row1 == row2) return;
            SortedList buff = _Stroki[row1];
            _Stroki[row1] = _Stroki[row2];
            _Stroki[row2] = buff;
            for (int i = 0; i < _Stroki[row2].Count; i++)
            {
                int k = (int)_Stroki[row2].GetKey(i);//номер столбца
                if (_stolbci[k].Contains(row2))
                {
                    int index = _stolbci[k].IndexOf(row2);
                    _stolbci[k][index] = row1;
                }
                int index1 = _stolbci[k].IndexOf(row1);
                _stolbci[k][index1] = row2;
            }
            for (int i = 0; i < _Stroki[row1].Count; i++)
            {
                int k = (int)_Stroki[row1].GetKey(i);//номер столбца
                if (!_stolbci[k].Contains(row1))
                {
                    int index = _stolbci[k].IndexOf(row2);
                    _stolbci[k][index] = row1;
                }
            }
        }

        // возвращает ненулевые значения и индексы ячеек строки d кроме элемента на главной диагонали        
        public void GetNRow(int d, ref int[] indexes, ref Complex[] values)
        {
            if (d >= n) return;
            List<int> indexs = new List<int>();
            List<Complex> Val = new List<Complex>();
            for (int i = 0; i < _Stroki[d].Count; i++)
            {
                int j = (int)_Stroki[d].GetKey(i);
                if (j != d)
                {
                    indexs.Add(j);
                    Val.Add((Complex)_Stroki[d][j]);
                }
            }
            indexes = indexs.ToArray();
            values = Val.ToArray();
        }

        //сохранение в файл
        public void SaveToFile(string path)
        {
            StreamWriter writer = new StreamWriter(path);
            writer.WriteLine(n.ToString());
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < _Stroki[i].Count; j++)
                {
                    int k = (int)_Stroki[i].GetKey(j);
                    writer.WriteLine(i.ToString() + ';' + k.ToString() + ';' + _Stroki[i][k].ToString() + ';');
                }
            }
            writer.Close();
        }

        public void Convert_to_crs()
        {
            List<Complex> AElement = new List<Complex>();
            List<int> jptr = new List<int>();//портрет матрицы по столбцам
            List<int> iptr = new List<int>();//портрет матрицы по строкам
            ADiag = new Complex[n];
            for (int i = 0; i < n; i++)
            {
                bool nal = false;
                Find_Index_Element(i, i, out  nal);
                if (nal)
                {
                    ADiag[i] = (Complex)_Stroki[i][i];
                }

                iptr.Add(jptr.Count);
                for (int k = 0; k < _Stroki[i].Count; k++)
                {
                    int ind = (int)_Stroki[i].GetKey(k);
                    if (ind != i)
                    {
                        jptr.Add(ind);
                        AElement.Add((Complex)_Stroki[i][ind]);
                    }
                }


            }
            iptr.Add(jptr.Count);


            iptr_crs = iptr.ToArray();
            jptr_crs = jptr.ToArray();
            AElement_crs = AElement.ToArray();
        }

        
    }
}