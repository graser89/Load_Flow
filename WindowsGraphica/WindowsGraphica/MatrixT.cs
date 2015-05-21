using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Numerics;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;



namespace WindowsGraphica
{
    public class Gauss
    {
        // матрица
        private IMatrix matrix;
        private bool flag = false;
        private List<int> List_row1;
        private List<int> List_row2;
        // конструктор, принимает созданную матрицу коэффициентов
        public Gauss(IMatrix matrix)
        {
            this.matrix = matrix;
            List_row1 = new List<int>();
            List_row2 = new List<int>();
        }
        private void Changed(double[] B)
        {
            for (int i = 0; i < List_row1.Count; i++)
            {
                double c = B[List_row1[i]];
                B[List_row1[i]] = B[List_row2[i]];
                B[List_row2[i]] = c;
            }
        }

        // главный метод, возвращающий решение, принимает вектор свободных членов
        public double[] calculate(double[] B)
        {

            if (!flag)
            {
                List_row1 = new List<int>();
                List_row2 = new List<int>();
                int[] index_dd = new int[0];

                for (int i = 0; i < matrix.getN(); i++)
                {
                    List_row1.Add(i);
                    int j = matrix.getDominElemColum(i);
                    List_row2.Add(j);
                    matrix.ChangedRow(i, j);

                }
                matrix.getIndexsZero_dd(ref index_dd);
                while (index_dd.Length > 0)
                {

                    foreach (int i in index_dd)
                    {
                        List_row1.Add(i);
                        int j = matrix.getDominElemColum(i);
                        List_row2.Add(j);
                        matrix.ChangedRow(i, j);
                    }
                    index_dd = new int[0];
                    matrix.getIndexsZero_dd(ref index_dd);
                }
                Changed(B);

                // обнуляем нижнюю полуматрицу, перебирая сверху вниз все строки
                // и складывая каждую со всеми нижележащими
                for (int row = 0; row < (matrix.getN() - 1); row++)
                {
                    // get all non-zero values of zeroing column
                    int[] colIndexes = new int[0];
                    double[] colValues = new double[0];
                    matrix.getJCol(row, ref  colIndexes, ref  colValues);

                    // получаем все ненулевые значения обнуляемого столбца
                    // получаем индексы и значения ячеек строки, правее главной диагонали
                    int[] rowIndexes = new int[0];
                    double[] rowValues = new double[0];
                    matrix.getJRow(row, ref  rowIndexes, ref  rowValues);

                    // получаем элемент главной диагонали, которым будем обнулять столбец
                    double dd = matrix.getValue(row, row);
                    for (int i = 0; i < colValues.Length; i++)
                    {
                        double k = colValues[i] / dd;// высчитываем коэффициент 

                        // k подобран таким образом чтобы обнулить ячейку столбца,
                        //matrix.setValue(colIndexes[i], row, 0);

                        // складываем строки
                        for (int ii = 0; ii < rowIndexes.Length; ii++)
                        {
                            matrix.addValue(colIndexes[i], rowIndexes[ii], -rowValues[ii] * k);
                        }

                        // складываем соответствующие свободные члены
                        B[colIndexes[i]] -= B[row] * k;
                    }
                }
                flag = true;
            }
            else
            {
                Changed(B);
                // обнуляем нижнюю полуматрицу, перебирая сверху вниз все строки
                // и складывая каждую со всеми нижележащими
                for (int row = 0; row < (matrix.getN() - 1); row++)
                {
                    // get all non-zero values of zeroing column
                    int[] colIndexes = new int[0];
                    double[] colValues = new double[0];
                    matrix.getJCol(row, ref  colIndexes, ref  colValues);

                    // получаем все ненулевые значения обнуляемого столбца
                    // получаем индексы и значения ячеек строки, правее главной диагонали
                    int[] rowIndexes = new int[0];
                    double[] rowValues = new double[0];
                    matrix.getJRow(row, ref  rowIndexes, ref  rowValues);

                    // получаем элемент главной диагонали, которым будем обнулять столбец
                    double dd = matrix.getValue(row, row);
                    for (int i = 0; i < colValues.Length; i++)
                    {
                        double k = colValues[i] / dd;// высчитываем коэффициент 

                        // k подобран таким образом чтобы обнулить ячейку столбца,
                        //matrix.setValue(colIndexes[i], row, 0);                        

                        // складываем соответствующие свободные члены
                        B[colIndexes[i]] -= B[row] * k;
                    }
                }
            }


            // создаем вектор неизвестных
            double[] x = new double[matrix.getN()];

            // используя обратный ход находим неизвестные
            for (int row = (matrix.getN() - 1); row >= 0; row--)
            {
                double e = 0;
                int[] indexes = new int[0];
                double[] values = new double[0];
                matrix.getJRow(row, ref  indexes, ref  values);
                for (int i = 0; i < indexes.Length; i++) e += x[indexes[i]] * values[i];
                x[row] = (B[row] - e) / matrix.getValue(row, row);
            }
            return x;
        }
    }

    public interface IMatrix
    {
        // устанавливает значение value в ячейку с координатами [row,col];
        // row - номер строки матрицы
        // col - номер столбца матрицы
        void setValue(int row, int col, double value);

        // добавляет значение value к ячейке [row,col]
        void addValue(int row, int col, double value);

        // возвращает значение ячейки [row,col]
        double getValue(int row, int col);

        // возвращает порядок матрицы
        int getN();

        // возвращает ненулевые значения и индексы ячеек строки d,
        // которые находятся правее главной диагонали
        void getJRow(int d, ref int[] indexes, ref double[] values);

        // возвращает ненулевые значения и индексы ячеек столбца d, 
        // которые находятся ниже главной диагонали
        void getJCol(int d, ref int[] indexes, ref double[] values);

        //возвращает индексы строк с нулями на главной диагонали
        void getIndexsZero_dd(ref int[] indexes);

        //возвращает индекс строки с доминирующим элементом в колонке где ноль на главной диагонали
        int getDominElemColum(int row);

        //меняет указанные строки местами
        void ChangedRow(int row1, int row2);

        // возвращает ненулевые значения и индексы ячеек строки d кроме элемента на главной диагонали        
        void GetNRow(int d, ref int[] indexes, ref double[] values);
    }

    public class Hash_Spare_Matrix : IMatrix
    {
        Hashtable _Matrix;
        int n;

        public Hash_Spare_Matrix(int N)
        {
            n = N;
            _Matrix = new Hashtable();
        }

        private void Find_Index_Element(int row, int col, out int index, out bool nalichie)
        {
            index = row * n + col;
            nalichie = _Matrix.ContainsKey(index);
        }

        private void Insert(int row, int col, int index, double value)
        {
            _Matrix.Add(index, value);
        }

        private void RemovAt(int index)
        {
            _Matrix.Remove(index);
        }

        public void setValue(int row, int col, double value)
        {
            if (col > n) return;
            if (row > n) return;
            int index;
            bool nalichie;
            Find_Index_Element(row, col, out index, out nalichie);
            if (nalichie)
            {
                if (value == 0)
                    RemovAt(index);
                else
                    _Matrix[index] = value;
            }
            else
            {
                if (value != 0)
                    Insert(row, col, index, value);
            }
        }

        public void addValue(int row, int col, double value)
        {
            if (col > n) return;
            if (row > n) return;
            int index;
            bool nalichie;
            Find_Index_Element(row, col, out index, out nalichie);
            if (nalichie)
            {
                if (value != 0)
                    _Matrix[index] = (double)_Matrix[index] + value;
            }
            else
                if (value != 0)
                    Insert(row, col, index, value);
        }

        public double getValue(int row, int col)
        {
            if (col > n) return 0;
            if (row > n) return 0;
            int index;
            bool nalichie;
            Find_Index_Element(row, col, out index, out nalichie);
            if (nalichie)
                return (double)_Matrix[index];
            else
                return 0;
        }

        public int getN()
        {
            return n;
        }

        public void getJRow(int d, ref int[] indexes, ref double[] values)
        {
            if (d >= n) return;
            List<int> indexs = new List<int>();
            List<double> Val = new List<double>();
            int index;
            bool nal;
            for (int i = d + 1; i < n; i++)
            {
                Find_Index_Element(d, i, out index, out nal);
                if (nal)
                {
                    indexs.Add(i);
                    Val.Add((double)_Matrix[index]);
                }
            }
            indexes = new int[indexs.Count];
            values = new double[indexs.Count];
            for (int i = 0; i < indexs.Count; i++)
            {
                indexes[i] = indexs[i];
                values[i] = Val[i];
            }

        }

        public void getJCol(int d, ref int[] indexes, ref double[] values)
        {
            if (d >= n) return;
            List<int> indexs = new List<int>();
            List<double> Val = new List<double>();
            int index;
            bool nal;
            for (int i = d + 1; i < n; i++)
            {
                Find_Index_Element(i, d, out index, out nal);
                if (nal)
                {
                    indexs.Add(i);
                    Val.Add((double)_Matrix[index]);
                }
            }
            indexes = new int[indexs.Count];
            values = new double[indexs.Count];
            for (int i = 0; i < indexs.Count; i++)
            {
                indexes[i] = indexs[i];
                values[i] = Val[i];
            }
        }

        public void getIndexsZero_dd(ref int[] indexes)
        {
            List<int> list = new List<int>();

            int index;
            bool nal;
            for (int i = 0; i < n; i++)
            {
                Find_Index_Element(i, i, out index, out nal);
                if (!nal)
                {
                    list.Add(i);
                }
            }
            indexes = new int[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                indexes[i] = list[i];
            }

        }

        public int getDominElemColum(int row)//row индекс строки с нулем по диагонали поэтому ищем максимальный по мод элемент в столбце с тем же индексом
        {

            if (row >= n) return -1;
            int index;
            bool nal;
            double max = 0;
            int res = -1;
            for (int i = 0; i < n; i++)
            {
                Find_Index_Element(i, row, out index, out nal);
                if (nal)
                {
                    if (max < Math.Abs((double)_Matrix[index]))
                    {
                        max = Math.Abs((double)_Matrix[index]);
                        res = i;
                    }
                }
            }
            return res;
        }

        public void ChangedRow(int row1, int row2)
        {
            if (row1 == row2) return;
            List<double> list1 = new List<double>();
            List<int> List1_index = new List<int>();
            List<double> list2 = new List<double>();
            List<int> List2_index = new List<int>();
            for (int i = 0; i < n; i++)
            {
                int ind1;
                bool nal1;
                int ind2;
                bool nal2;
                Find_Index_Element(row1, i, out ind1, out nal1);
                Find_Index_Element(row2, i, out ind2, out nal2);
                if (nal1)
                {
                    list1.Add((double)_Matrix[ind1]);
                    List1_index.Add(i);
                    RemovAt(ind1);
                }
                if (nal2)
                {
                    list2.Add((double)_Matrix[ind2]);
                    List2_index.Add(i);
                    RemovAt(ind2);
                }
            }

            for (int j = 0; j < List1_index.Count; j++)
            {
                setValue(row2, List1_index[j], list1[j]);
            }
            for (int j = 0; j < List2_index.Count; j++)
            {
                setValue(row1, List2_index[j], list2[j]);
            }
        }

        public void GetNRow(int d, ref int[] indexes, ref double[] values)
        {
            if (d >= n) return;
            List<int> indexs = new List<int>();
            List<double> Val = new List<double>();
            int index;
            bool nal;
            for (int i = 0; i < n; i++)
            {
                if (i != d)
                {
                    Find_Index_Element(d, i, out index, out nal);
                    if (nal)
                    {
                        indexs.Add(i);
                        Val.Add((double)_Matrix[index]);
                    }
                }
            }
            indexes = new int[indexs.Count];
            values = new double[indexs.Count];
            for (int i = 0; i < indexs.Count; i++)
            {
                indexes[i] = indexs[i];
                values[i] = Val[i];
            }
        }
    }

    public class Sort_Spare_Matrix_complex
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


        public Sort_Spare_Matrix_complex(int N, double Ubas)
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

        public Complex[] Method_Zeidela(int maxitter, double MaxEps, double w, List<int> index_op_uzlov)
        {
            Random r = new Random();
            int nomer1 = r.Next(Moshnst.Length);
            int nomer2 = r.Next(Moshnst.Length);
            int nomer3 = r.Next(Moshnst.Length);

            Stopwatch timer1 = new Stopwatch();
            Stopwatch timer2 = new Stopwatch();
            timer2.Start();
            StreamWriter wri_timer = new StreamWriter("res_Zeidel_timer.csv");
            StreamWriter wri_res = new StreamWriter("res_Zeidel_result.csv");

            wri_res.Write("itteration ;" + nomer1.ToString() + ".Real ;" + nomer1.ToString() + ".Imaginary ;");
            wri_res.Write(nomer2.ToString() + ".Real ;" + nomer2.ToString() + ".Imaginary ;");
            wri_res.WriteLine(nomer3.ToString() + ".Real ;" + nomer3.ToString() + ".Imaginary ;");

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

                wri_res.Write(ittr.ToString() + " ;" + U[nomer1].Real.ToString() + " ;" + U[nomer1].Imaginary.ToString() + " ;");
                wri_res.Write(U[nomer2].Real.ToString() + " ;" + U[nomer2].Imaginary.ToString() + " ;");
                wri_res.WriteLine(U[nomer3].Real.ToString() + " ;" + U[nomer3].Imaginary.ToString() + " ;");

            }
            while (((MaxOshibka > MaxEps) && (maxitter > ittr)));

            timer2.Stop();
            wri_timer.WriteLine(ittr.ToString() + ";" + timer2.ElapsedMilliseconds.ToString() + ";" + MaxOshibka.ToString() + ";");
            wri_timer.Close();
            wri_res.Close();
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
                Complex U_k_1 = U[index];

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

        //открыть файл
        /*
        void OpenFromFile(string path)
        {
            StreamReader reader = new StreamReader(path);
            string s;
            s = reader.ReadLine();
            n = Convert.ToInt32(s);
            _Stroki = new SortedList[n];
            _stolbci = new List<int>[n];
            for (int i = 0; i < n; i++)
            {
                _Stroki[i] = new SortedList();
                _stolbci[i] = new List<int>();
            }

            do
            {
                s = reader.ReadLine();
                if ((s == null) || (s == ""))
                    break;
                string[] str = s.Split(';');
                int row = Convert.ToInt32(str[0]);
                int col = Convert.ToInt32(str[1]);
                double val = Convert. (str[2]);
                setValue(row, col, val);

            }
            while (true);
        }
         */
    }


}
