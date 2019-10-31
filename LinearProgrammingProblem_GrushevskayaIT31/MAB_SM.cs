
using System.Collections.Generic;
namespace LinearProgrammingProblem_GrushevskayaIT31
{
    class MAB_SM
    {




        // заполнить первую матрицу для расчета мет. иск. базиса
        public static void InitializeMAB(MainWindow w)
        {
            // список матриц очистить
            w.lpp.methodOfArtificialBasis.Clear();
            // размерность первой матрицы
            int columns = w.lpp.matrix2.GetLength(0) + 1;
            int rows = w.lpp.matrix2.GetLength(1) + 2;
            // создать первую матрицу
            Fraction[,] matrixMAB = new Fraction[columns, rows];
            // добавить номер
            matrixMAB[0, 0] = new Fraction();
            matrixMAB[0, 0].Text = "№0";
            // правый верх. и левый нижн. углы оставить пустыми
            matrixMAB[0, rows - 1] = new Fraction();
            matrixMAB[0, rows - 1].Text = " ";
            matrixMAB[columns - 1, 0] = new Fraction();
            matrixMAB[columns - 1, 0].Text = " ";
            // добвить х по горизонтали сверху
            int count = 1;
            for (; count < columns - 1; count++)
            {
                matrixMAB[count, 0] = new Fraction(count);
                matrixMAB[count, 0].Text = "x" + count;
            }
            // добавить х по вертикали слева
            for (int i = 1; i < rows - 1; i++, count++)
            {
                matrixMAB[0, i] = new Fraction(count);
                matrixMAB[0, i].Text = "x" + count;
            }
            // заполнить матрицу коэффициентами условий
            for (int i = 0; i < w.lpp.matrix2.GetLength(0); i++)
            {
                for (int j = 0; j < w.lpp.matrix2.GetLength(1); j++)
                {
                    matrixMAB[i + 1, j + 1] = w.lpp.matrix2[i, j];
                }
            }
            // (коэф. в последнем столбце должны быть > 0)
            for (int i = 1; i < rows - 1; i++)
            {
                if (matrixMAB[columns - 1, i] < 0)
                    for (int j = 1; j < columns; j++)
                    {
                        matrixMAB[j, i].Numerator = 0 - matrixMAB[j, i].Numerator;
                    }
            }
            // высчитать значения в нижней строке
            for (int j = 1; j < columns; j++)
            {
                Fraction fr = new Fraction();
                for (int i = 1; i < rows - 1; i++)
                {
                    fr += matrixMAB[j, i];
                }
                fr.Numerator = 0 - fr.Numerator;
                matrixMAB[j, rows - 1] = new Fraction();
                matrixMAB[j, rows - 1] = fr;
            }
            // добавить матрицу в список
            w.lpp.methodOfArtificialBasis.Add(matrixMAB);
        }

        // заполнить первую матрицу для расчета симп. мет.
        // на основании данных, полученных после мет. иск. баз.
        public static void InitializeSM(MainWindow w)
        {

            // simplexMethod - список матриц очистить
            w.lpp.simplexMethod.Clear();
            // скопировать последнюю матрицу мет. иск. баз.
            Fraction[,] matrix = (Fraction[,])w.lpp.methodOfArtificialBasis[w.lpp.methodOfArtificialBasis.Count - 1].Clone();
            int columns = matrix.GetLength(0);
            int rows = matrix.GetLength(1);
            // заполнить матрицу (переписать матрицу, полученную в мет. иск. баз.)
            for (int i = 0; i < columns; i++)
            {
                Fraction sumCol = new Fraction();
                for (int j = 0; j < rows; j++)
                {
                    if (i != 0 && j != 0 && j != rows - 1)
                    {
                        int numXstr = matrix[0, j].Numerator - 1;
                        Fraction fr = matrix[i, j] * w.lpp.matrix1[numXstr, 0];
                        switch (w.lpp.extremum)
                        {
                            case "min":
                                sumCol = sumCol + fr;
                                break;
                            case "max":
                                sumCol = sumCol - fr;
                                break;
                        }
                    }
                }
                if (i != 0 && i != columns - 1)
                {
                    int numXcol = matrix[i, 0].Numerator - 1;
                    Fraction frBr = new Fraction(0);
                    switch (w.lpp.extremum)
                    {
                        case "min":
                            frBr = w.lpp.matrix1[numXcol, 0];
                            break;
                        case "max":
                            frBr = new Fraction(0) - w.lpp.matrix1[numXcol, 0];
                            break;
                    }
                    sumCol -= frBr;  
                    
                }
                switch (w.lpp.extremum)
                {
                    case "min":
                        sumCol = new Fraction(0) - sumCol;
                        break;
                    case "max":
                        sumCol = new Fraction(0) - sumCol;
                        break;
                } 
                if (i != 0)
                {
                    matrix[i, rows - 1] = sumCol;
                }
            }
            // изменить номер
            matrix[0, 0].Text = "№0";
            w.lpp.simplexMethod.Add(matrix);
        }

        // заполнить первую матрицу для расчета симп. мет.
        // на основании данных, полученных после 
        public static void InitializeSM_G(MainWindow w)
        {

            // simplexMethod - список матриц очистить
            w.lpp.simplexMethod.Clear();
            // скопировать последнюю матрицу 
            Fraction[,] matrix = (Fraction[,])w.lpp.methodGauss[w.lpp.methodGauss.Count - 1].Clone();
            int rows = matrix.GetLength(1) + 1;
            int columnsM = matrix.GetLength(0);
            int columns = columnsM - rows + 2;
            Fraction[,] matrixSM = new Fraction[columns, rows];
            // заполнить матрицу (переписать матрицу, полученную в мет. иск. баз.)
            int count = 0;
            int count2 = 1;
            
            for (int i = 0; i < columnsM; i++)
            {

                if (i == 0)
                {
                    for (int k = 0; k < rows; k++)
                    {
                        matrixSM[0, k] = new Fraction();
                    }
                    count++;
                }
                if (i != 0 && i != columnsM - 1 
                    && w.lpp.matrixGFlag[i - 1, 0] == false)
                {
                    for (int k = 0; k < rows - 1; k++)
                    {
                        matrixSM[count, k] = matrix[i, k];
                    }
                    count++;
                }
                if (i == columnsM - 1)
                {
                    for (int k = 0; k < rows - 1; k++)
                    {
                        matrixSM[count, k] = matrix[i, k];
                    }
                    count++;
                }
                if (i != 0 && i != columnsM - 1
                    && w.lpp.matrixGFlag[i - 1, 0] == true)
                {
                    matrixSM[0, count2] = new Fraction(i);
                    matrixSM[0, count2].Text = "x" + i;
                    count2++;
                }
            }
            // изменить номер
            matrixSM[0, 0].Text = "№0";
            // правый верх. и левый нижн. углы оставить пустыми
            matrixSM[0, rows - 1] = new Fraction();
            matrixSM[0, rows - 1].Text = " ";
            matrixSM[columns - 1, 0] = new Fraction();
            matrixSM[columns - 1, 0].Text = " ";
            columns = matrixSM.GetLength(0);
            rows = matrixSM.GetLength(1);
            for (int i = 0; i < columns; i++)
            {
                Fraction sumCol = new Fraction();
                for (int j = 0; j < rows; j++)
                {
                    if (i != 0 && j != 0 && j != rows - 1)
                    {
                        int numXstr = matrixSM[0, j].Numerator - 1;
                        Fraction frB = new Fraction(0);
                        switch (w.lpp.extremum)
                        {
                            case "min":
                                frB = w.lpp.matrix1[numXstr, 0];
                                break;
                            case "max":
                                frB = new Fraction(0) - w.lpp.matrix1[numXstr, 0];
                                break;
                        }

                        Fraction fr = matrixSM[i, j] * frB;
                        sumCol = sumCol + fr;                        
                        
                    }
                }
                if (i != 0 && i != columns - 1)
                {
                    int numXcol = matrixSM[i, 0].Numerator - 1;
                    Fraction frBr = new Fraction(0);
                    switch (w.lpp.extremum)
                    {
                        case "min":
                            frBr = w.lpp.matrix1[numXcol, 0];
                            break;
                        case "max":
                            frBr = new Fraction(0) - w.lpp.matrix1[numXcol, 0];
                            break;
                    }

                    sumCol -= frBr;    
                }
                switch (w.lpp.extremum)
                {
                    case "min":
                        sumCol = new Fraction(0) - sumCol;
                        break;
                    case "max":
                        sumCol = new Fraction(0) - sumCol;
                        break;
                } 
                if (i != 0)
                {
                    matrixSM[i, rows - 1] = sumCol;
                }
            }

            w.lpp.simplexMethod.Add(matrixSM);
        }

        private static void CopyColumn(int column1, int column2, int rows,
            Fraction[,] m1, Fraction[,] m2){

                for (int i = 0; i < rows; i++)
                {
                    m1[column1, i] = m2[column2, i];
                }
        }

        public static void Step(LinearProgrammingProblem lpp, int column, int row,
            string method, int nowNum, List<Fraction[,]> matrixs)
        {
            int columns = matrixs[nowNum].GetLength(0);
            int rows = matrixs[nowNum].GetLength(1);
            Fraction[,] matrix = Step_do(lpp, column, row, columns, rows, method);
            switch (method)
            {
                case "MAB":
                    Step_deleteColumn(lpp, column, columns, rows, matrix, method);
                    break;
                case "SM":
                    break;
            }

        }

        // удаление "опорного" столбца из таблицы
        private static void Step_deleteColumn(LinearProgrammingProblem lpp, int column, int columns, int rows,
            Fraction[,] matrix, string method)
        {
            Fraction[,] matrixResult = new Fraction[columns - 1, rows];
            for (int j = 0; j < column; j++)
            {
                for (int i = 0; i < rows; i++)
                {
                    matrixResult[j, i] = matrix[j, i];
                }
            }
            for (int j = column; j < columns - 1; j++)
            {
                for (int i = 0; i < rows; i++)
                {
                    matrixResult[j, i] = matrix[j + 1, i];
                }
            }
            switch (method)
            {
                case "MAB":
                    lpp.methodOfArtificialBasis.Add(matrixResult);
                    lpp.nowNumMatrixMAB++;
                    break;
                case "SM":
                    break;
            }
        }

        private static Fraction[,] Step_do(LinearProgrammingProblem lpp,
            int column, int row, int columns, int rows, string method)
        {
            int nowNum = -1;
            List<Fraction[,]> matrixs = new List<Fraction[,]>();
            switch (method)
            {
                case "MAB":
                    nowNum = lpp.nowNumMatrixMAB;
                    matrixs = lpp.methodOfArtificialBasis;
                    break;
                case "SM":
                    nowNum = lpp.nowNumMatrixSM;
                    matrixs = lpp.simplexMethod;
                    break;
            }
            Fraction[,] matrix = new Fraction[columns, rows];
            matrix[0, 0] = new Fraction();
            matrix[0, 0].Text = "№" + (nowNum + 1);
            matrix[0, rows - 1] = new Fraction();
            matrix[0, rows - 1].Text = " ";
            matrix[columns - 1, 0] = new Fraction();
            matrix[columns - 1, 0].Text = " ";

            for (int i = 1; i < columns - 1; i++)
            {
                matrix[i, 0] = new Fraction();
                matrix[i, 0] = matrixs[nowNum][i, 0];
            }
            for (int i = 1; i < rows - 1; i++)
            {
                matrix[0, i] = new Fraction();
                matrix[0, i] = matrixs[nowNum][0, i];
            }
            matrix[column, 0] = matrixs[nowNum][0, row];
            matrix[0, row] = matrixs[nowNum][column, 0];
            Fraction fr1 = new Fraction(1);
            // опорный элемент
            matrix[column, row] = fr1 / matrixs[nowNum][column, row];
            matrix[column, row].IsSupport = true;
            // строки
            for (int i = 1; i < columns; i++)
            {
                if (i != column)
                {
                    matrix[i, row] = matrixs[nowNum][i, row] * matrix[column, row];
                }
            }
            // столбцы
            for (int i = 1; i < rows; i++)
            {
                if (i != row)
                {
                    matrix[column, i] = (-1) * matrixs[nowNum][column, i] * matrix[column, row];
                }
            }
            // остальное
            for (int i = 1; i < rows; i++)
            {
                for (int j = 1; j < columns; j++)
                {
                    if (i != row && j != column)
                    {
                        matrix[j, i] = matrixs[nowNum][j, i] - matrixs[nowNum][column, i] * matrix[j, row];
                    }
                }
            }
            switch (method)
            {
                case "MAB":
                    lpp.methodOfArtificialBasis.Add(matrix);
                    lpp.nowNumMatrixMAB++;
                    break;
                case "SM":
                    lpp.simplexMethod.Add(matrix);
                    lpp.nowNumMatrixSM++;
                    break;
            }
            return matrix;
        }

        // может ли элемент быть опорным
        public static bool IsCanBeSupperElem(Fraction[,] matrix,
            int columns, int rows, Fraction[] minRatio, int y, int x)
        {
            // высчитать отношение
            Fraction ratio = matrix[columns - 1, y] / matrix[x, y];
            return
                // не первый и не последний столбец
                x != 0 && x != columns - 1
                //не первая и не последняя строка
                && y != 0 && y != rows - 1
                // минимальное отношение для этого столбца высчитано
                && minRatio[x] != null
                // отношение совпадает с минимальным
                && minRatio[x].IsEquals(ratio)
                // элемент положительный
                && matrix[x, y] > 0;
        }

        public static Fraction FindSupportMinRatio(int column, Fraction[,] matrix)
        {
            int columns = matrix.GetLength(0);
            int rows = matrix.GetLength(1);
            Fraction min = new Fraction();
            //int minRow = 0;
            bool flagNegative = false;
            if (matrix[column, rows - 1] >= 0)
            {
                return null;
            }
            for (int i = 1; i < rows; i++)
            {
                if (matrix[column, i] > 0)
                {
                    if (!flagNegative)
                    {
                        min = matrix[columns - 1, i] / matrix[column, i];
                        //minRow = i;
                    }
                    else
                    {
                        Fraction ratio = matrix[columns - 1, i] / matrix[column, i];
                        if (min > ratio)
                        {
                            min = ratio;
                            //minRow = i;
                        }
                    }
                    flagNegative = true;
                }
            }
            if (!flagNegative)
            {
                //message.Content = "функция неограничена";
                return null;
            }
            return min;
        }

        // выбор опорного элемента
        public static void SelectSupportElem(LinearProgrammingProblem lpp,
            out int column, out int row, string method, Fraction[,] matrixIn)
        {
            column = -1;
            row = -1;
            Fraction[,] matrix = (Fraction[,])matrixIn.Clone();
            int columns = matrix.GetLength(0);
            int rows = matrix.GetLength(1);
            Fraction[] supportElementsIndex = new Fraction[columns];
            for (int i = 1; i < columns - 1; i++)
            {
                supportElementsIndex[i] = MAB_SM.FindSupportMinRatio(i, matrix);
            }
            bool flagSupportElem = false;
            for (int y = 0; y < rows && !flagSupportElem; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    Fraction ratio = matrix[columns - 1, y] / matrix[x, y];
                    if (x != 0 && x != columns - 1
                        && y != 0 && y != rows - 1
                        && supportElementsIndex[x] != null
                        && supportElementsIndex[x].IsEquals(ratio)
                        && matrix[x, y] > 0
                        && !flagSupportElem)
                    {
                        switch (method)
                        {
                            case "MAB":
                                if (matrix[0, y] >= lpp.matrix2.GetLength(0))
                                {
                                    column = x;
                                    row = y;
                                    flagSupportElem = true;
                                }
                                break;
                            case "SM":
                                column = x;
                                row = y;
                                flagSupportElem = true;
                                break;
                        }

                    }
                }
            }
        }


        public static int getNowNum(LinearProgrammingProblem lpp, string method)
        {
            int nowNum = 0;
            switch (method)
            {
                case "MAB":
                    if (lpp.nowNumMatrixMAB % 2 != 0)
                    {
                        lpp.nowNumMatrixMAB++;
                    }
                    nowNum = lpp.nowNumMatrixMAB;
                    break;
                case "SM":
                    nowNum = lpp.nowNumMatrixSM;
                    break;
            }
            return nowNum;
        }

    }
}
