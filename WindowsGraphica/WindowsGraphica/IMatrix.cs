namespace WindowsGraphica
{
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
}