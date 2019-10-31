using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LinearProgrammingProblem_GrushevskayaIT31
{
    class GaussMethod
    {
        public int n { get; set; }
        public int m { get; set; }
        public int Rank { get; set; }
        public List<Fraction[,]> Matrixs { get; set; }
        private Fraction[,] MatrixTemp { get; set; }

        public GaussMethod(int n, int m, Fraction[,] matrix)
        {
            this.n = n - 1;
            this.m = m;
            Matrixs = new List<Fraction[,]>();
            // копируем входную матрицу
            Matrixs.Add((Fraction[,])matrix.Clone());
            MatrixTemp = (Fraction[,])matrix.Clone();
            Method();
            RankMatrix();
        }
        private void Method()
        {
            if (TransformMatrixPart1(MatrixTemp) == -1)
            {
                TransformMatrixPart2(MatrixTemp);                
            }
        }
        private int TransformMatrixPart1(Fraction[,] matrix)
        {   
            // i - строка с "1" на [i,i]
            for (int i = 0; i < m; i++)
            {
                if (matrix[i, i].Numerator == 0)
                {
                    // j - строки ниже i (из которых нужно вычесть i-ую строку)
                    for (int j = i; j < m; j++)
                    {
                        // i - столбец, j- строка
                        // среди элементов столбца матрицы выбираем ненулевой
                        // и перемещам его на крайнее верхнее положение
                        if (matrix[i, j].Numerator != 0)
                        {
                            if (i != j)
                            {
                                SwapRows(matrix, i, j);
                            }
                            break;
                        }
                    }
                    if (matrix[i, i].Numerator == 0)
                    {
                        //MessageBox.Show("ошибка: В условии есть нулевой столбец № " + i +
                        //    "\nПеременная с таким номером может быть любой ");
                        return i;
                    }
                }
                // нормируем [i,i], делим i-ую строку на коэф. при [i,i]
                for (int l = n ; l >= 0; l--)
                {
                    if (matrix[i, i].Numerator != 0)
                    {
                        matrix[l, i] = matrix[l, i] / matrix[i, i];
                    }
                }
                // сохраняем этап нормирования в Matrixs
                Matrixs.Add((Fraction[,])matrix.Clone());
                // вычитаем из каждой k-ой строки i-ую
                for (int k = i + 1; k < m; k++)
                {
                    SubtractRow(matrix, i, k);
                }                
            }
            return -1;
        }
        private void TransformMatrixPart2(Fraction[,] matrix)
        {
            for (int i = m - 1; i >= 0; i--)
            {
                for (int k = m - 1; k >= i + 1; k--)
                {
                    SubtractRow(matrix, k, i);
                }
            }
        }
        private void SubtractRow(Fraction[,] matrix, int row1, int row2)
        {
            // если коэф. = 0, то вычитать из этой строки ничего не нужно 
            if (matrix[row2, row2].Numerator == 0)
            {
                return;
            }
            // иначе
            for (int j = n; j >= row1; j--)
            {
                matrix[j, row2] = matrix[j, row2] - matrix[j, row1] * matrix[row1, row2];
            }
            // копируем входную матрицу
            //Fraction[,] matrixClone = (Fraction[,])matrix.Clone();
            //Matrixs.Add(matrixClone);
        }
        private void SwapRows(Fraction[,] matrix, int row1, int row2)
        {
            
            for (int j = 0; j <= n; j++)
            {
                Fraction temp = matrix[j, row1];
                matrix[j, row1] = matrix[j, row2];
                matrix[j, row2] = temp;
            }
            // копируем входную матрицу
            //Fraction[,] matrixClone = (Fraction[,])matrix.Clone();
            //Matrixs.Add(matrixClone);
        }
        private void RankMatrix()
        {
            Fraction[,] matrix = (Fraction[,])Matrixs[0].Clone();
            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);
            int resultTransform = -1;
            while (n >= m)
            {
                resultTransform = TransformMatrixPart1(matrix);
                if (resultTransform == -1)
                {
                    TransformMatrixPart2(matrix);
                    RankRow();
                    return;
                }
                else
                {
                    for (int i = 0; i < m; i++)
                    {
                        for (int j = resultTransform ; j < n - 1; j++)
                        {
                            matrix[j, i] = matrix[j + 1, i];
                        }
                    }
                    n--;
                    Rank = resultTransform;
                }
            }
        }
        private void RankRow()
        {
            int rank = 0;
            bool flag0 = true;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j <= n; j++)
                {
                    if (MatrixTemp[j, i].Numerator != 0)
                    {
                        flag0 = false;
                        break;
                    }
                }
                if (!flag0)
                {
                    rank++;
                }
                flag0 = true;
            }
            Rank = rank;
        }






        // заполнить первую матрицу для расчета 
        public static void InitializeG(MainWindow w)
        {
            // список матриц очистить
            w.lpp.methodGauss.Clear();
            // размерность первой матрицы
            int columns = w.lpp.matrix2.GetLength(0) + 1;
            int rows = w.lpp.matrix2.GetLength(1) + 1;
            // создать первую матрицу
            Fraction[,] matrixG = new Fraction[columns, rows];
            // добавить номер
            matrixG[0, 0] = new Fraction();
            matrixG[0, 0].Text = "№0";
            // правый верх. углы оставить пустыми
            matrixG[columns - 1, 0] = new Fraction();
            matrixG[columns - 1, 0].Text = " ";
            // добвить х по горизонтали сверху
            int count = 1;
            for (; count < columns - 1; count++)
            {
                matrixG[count, 0] = new Fraction(count);
                matrixG[count, 0].Text = "x" + count;
            }
            // добавить " " по вертикали слева
            for (int i = 1; i < rows; i++, count++)
            {
                matrixG[0, i] = new Fraction(count);
                matrixG[0, i].Text = " ";
            }
            // заполнить матрицу коэффициентами условий
            for (int i = 0; i < w.lpp.matrix2.GetLength(0); i++)
            {
                for (int j = 0; j < w.lpp.matrix2.GetLength(1); j++)
                {
                    matrixG[i + 1, j + 1] = w.lpp.matrix2[i, j];
                }
            }
            // добавить матрицу в список
            w.lpp.methodGauss.Add(matrixG);
        }

        public static void TransformMatrix(LinearProgrammingProblem lpp,
            Fraction[,] matrixBasis, bool[,] matrixFlag, List<Fraction[,]> matrixs, Label mes)
        {
            Fraction[,] matrix = CopyMatrix(matrixs[matrixs.Count - 1]);
            int columns = matrix.GetLength(0);
            int rows = matrix.GetLength(1);
            int count = 1;
            int countM = lpp.matrix2.GetLength(1);
            // проверка коол-ва условий и кол-ва баз. переменных
            for (int i = 1; i < columns - 1; i++)
            {
                if (matrixFlag[i - 1, 0])
                {
                    if (count > countM)
                    {
                        mes.Content = "Количество условий и базисных переменных не совпадает (много)";
                        return;
                    }
                    count++;                        
                }
            }
            count--;
            if (count != countM)
            {
                mes.Content = "Количество условий и базисных переменных не совпадает (мало)";
                return;
            }
            // метод гаусса
            count = 1;
            // двежение слева направо
            for (int i = 1; i < columns - 1; i++)
            {
                if (matrixFlag[i - 1, 0])
                    {
                        if (matrix[i, count].IsEquals(new Fraction(0)))
                        {
                            for (int k = count + 1; k <= rows && matrix[i, count].IsEquals(new Fraction(0)); k++)
                            {
                                if (k == rows)
                                {
                                    mes.Content = "Базис не совпал. Выразить через базисные переменные не удалось";
                                    return;
                                }
                                SwapRows2(matrix, count, k);
                                matrix[0, 0].Text = "№" + (lpp.nowNumMatrixG + 1);
                                lpp.methodGauss.Add(CopyMatrix(matrix));
                                lpp.nowNumMatrixG++;
                            }
                        }
                        // нормирование строки
                        NormalizeString(lpp, matrix, i, count);
                        // обнуление столбца снизу под баз. эл.
                        TransformMatrixColumnPart1(lpp, matrix, i, count, mes);                        
                        count++;
                    }
            }
            count--;
            // движение справа налево
            for (int i = columns - 2; i >= 1; i--)
            {
                if (matrixFlag[i - 1, 0])
                    {
                        // обнуление столбца сверху над баз.эл.
                        TransformMatrixColumnPart2(lpp, matrix, i, count, mes);
                        count--;
                    }
            }

            int n = lpp.matrix2.GetLength(0);
            Fraction[] arr = new Fraction[n];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = new Fraction(0);
            }
            // проверка на совпадение введенного базиса и полученного
            countM = 1;
            for (int i = 1; i < columns - 1; i++)
            {
                if (matrixFlag[i - 1, 0])
                {
                    if(!matrixBasis[i - 1, 0].IsEquals(matrix[columns-1, countM]))
                    {
                        mes.Content = "Базис не совпал";
                        return;
                    }
                    countM++;
                }
            }
        }

        private static void NormalizeString(LinearProgrammingProblem lpp,
            Fraction[,] matrix, int column, int row)
        {
            Fraction elem = matrix[column, row];
            if (elem.IsEquals(new Fraction(1)))
            {
                return;
            }
            int columns = matrix.GetLength(0);
            for (int j = 1; j < columns; j++)
            {
                matrix[j, row] = matrix[j, row] / elem;
            }
            matrix[0, 0].Text = "№" + (lpp.nowNumMatrixG + 1);
            lpp.methodGauss.Add(CopyMatrix(matrix));
            lpp.nowNumMatrixG++;
                        
        }
        private static Fraction[,] CopyMatrix(Fraction[,] matrix){
            int columns = matrix.GetLength(0);
            int rows = matrix.GetLength(1);
            Fraction[,] arr = new Fraction[columns, rows];
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    arr[i, j] = matrix[i, j];
                }
            }
            return arr;
        }
        private static void TransformMatrixColumnPart2(LinearProgrammingProblem lpp,
            Fraction[,] matrix, int column, int row, Label mes)
        {
            // вычитаем 
            for (int i = 1; i < row; i++)
            {
                SubtractRow(matrix, i, row, column, mes);
                matrix[0, 0].Text = "№" + (lpp.nowNumMatrixG + 1);
                lpp.methodGauss.Add(CopyMatrix(matrix));
                lpp.nowNumMatrixG++;
            } 

        }
        private static void TransformMatrixColumnPart1(LinearProgrammingProblem lpp,
            Fraction[,] matrix, int column, int row, Label mes)
        {
            int rows = matrix.GetLength(1);
            // вычитаем 

            
            for (int i = row + 1; i < rows; i++)
            {
                
                SubtractRow(matrix, i, row, column, mes);
                matrix[0, 0].Text = "№" + (lpp.nowNumMatrixG + 1);
                lpp.methodGauss.Add(CopyMatrix(matrix));
                lpp.nowNumMatrixG++;
            } 
        }

        // row1 - row2
        private static void SubtractRow(Fraction[,] matrix,
            int row1, int row2, int column, Label mes)
        {
            // если коэф. = 0, то вычитать из этой строки ничего не нужно 
            if (matrix[column, row1].Numerator == 0)
            {
                return;
            }
            Fraction elem = matrix[column, row1];
            int columns = matrix.GetLength(0);
            // иначе
            for (int j = 1; j < columns; j++)
            {
                try
                {
                    Fraction fr = matrix[j, row2] * elem;
                    matrix[j, row1] = matrix[j, row1] - fr;
                }
                catch
                {
                    matrix[j, row1] = new Fraction();
                    mes.Content = "Неудачный выбор базисных переменных";
                    return;
                }
            }
        }

        private static void SwapRows2(Fraction[,] matrix, int row1, int row2)
        {
            int columns = matrix.GetLength(0);
            for (int j = 1; j < columns; j++)
            {
                Fraction temp = matrix[j, row1];
                matrix[j, row1] = matrix[j, row2];
                matrix[j, row2] = temp;
            }
            // копируем входную матрицу
            //Fraction[,] matrixClone = (Fraction[,])matrix.Clone();
            //Matrixs.Add(matrixClone);
        }

    }
}
