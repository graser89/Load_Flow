using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


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
}
