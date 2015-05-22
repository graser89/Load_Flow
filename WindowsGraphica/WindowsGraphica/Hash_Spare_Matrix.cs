using System;
using System.Collections;
using System.Collections.Generic;

namespace WindowsGraphica
{
    public class HashSpareMatrix : IMatrix
    {
        Hashtable _Matrix;
        int n;

        public HashSpareMatrix(int N)
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
}