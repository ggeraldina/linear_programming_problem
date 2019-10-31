using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows;
namespace LinearProgrammingProblem_GrushevskayaIT31
{
    public class LinearProgrammingProblem
    {
        // коэффициенты целевой функции
        public Fraction[,] matrix1 { get; set; }
        // коэффициенты условий
        public Fraction[,] matrix2 { get; set; }
        // коэффициенты базиса
        public Fraction[,] matrixG { get; set; }
        // флаги базисных функций
        public bool[,] matrixGFlag { get; set; }
        // матрицы, полученные при расчете симплекс методом
        public List<Fraction[,]> simplexMethod { get; set; }
        // матрицы, полученные при расчете методом искуственного базиса
        public List<Fraction[,]> methodOfArtificialBasis { get; set; }
        // матрицы, полученные при расчете методом Гаусса
        public List<Fraction[,]> methodGauss { get; set; }
        // номер отображаемой матрицы (метод иск. баз.)
        public Int32 nowNumMatrixMAB { get; set; }
        // номер отображаемой матрицы (симпл. мет)
        public Int32 nowNumMatrixSM { get; set; }
        // номер отображаемой матрицы (гаус. мет)
        public Int32 nowNumMatrixG { get; set; }
        // что ищем?
        public String extremum { get; set; }
        // максимальная размерность
        public Int32 maxDimension { get; set; }

        // конструктор
        public LinearProgrammingProblem()
        {
            nowNumMatrixMAB = 0;
            nowNumMatrixSM = 0;
            nowNumMatrixG = 0;
            extremum = "";
            maxDimension = 16;
            methodOfArtificialBasis = new List<Fraction[,]>();
            simplexMethod = new List<Fraction[,]>();
            methodGauss = new List<Fraction[,]>();
        }
        // функция заполнения матриц с коэф-ми цел. ф-ии и условий
        public static void ReadTextGrid(MainWindow w, Fraction[,] matrix1, Fraction[,] matrix2, Fraction[,] matrixG, bool[,] matrixGFlag)
        {
            // grid1
            int columns1 = w.grid1.ColumnDefinitions.Count;
            int rows1 = w.grid1.RowDefinitions.Count;
            // заполнить массив matrix1 коэф. цел. ф-ии
            for (int i = 0; i < rows1; i++)
            {
                for (int j = 0; j < columns1 - 2; j += 2)
                {
                    TextBox t = (TextBox)w.grid1.Children[i * (columns1 - 1) + i + j];
                    int row = Grid.GetRow(t);
                    int column = Grid.GetColumn(t) / 2;
                    matrix1[column, row] = ReadText(t.Text);
                }
            }
            // grid2
            int columns2 = w.grid2.ColumnDefinitions.Count;
            int rows2 = w.grid2.RowDefinitions.Count;
            // заполнить массив matrix2 коэф. условий 
            for (int i = 0; i < rows2; i++)
            {
                for (int j = 0; j < columns2; j += 2)
                {
                    int num = i * (columns2 - 1) + i + j;
                    TextBox t = (TextBox)w.grid2.Children[num];
                    int row = Grid.GetRow(t);
                    int column = Grid.GetColumn(t) / 2;
                    matrix2[column, row] = ReadText(t.Text);
                }
            }
            // gridG
            int columnsG = w.gridG.ColumnDefinitions.Count;
            int rowsG = w.gridG.RowDefinitions.Count;
            // заполнить массив matrixG коэф. цел. ф-ии
            for (int i = 0; i < rowsG; i++)
            {
                for (int j = 0; j < columnsG - 2; j += 2)
                {
                    TextBox t = (TextBox)w.gridG.Children[i * (columnsG ) + i + j];
                    int row = Grid.GetRow(t);
                    int column = Grid.GetColumn(t) / 2;
                    matrixG[column, row] = ReadText(t.Text);
                }
            }
            // gridGF
            int columnsGF = w.gridGF.ColumnDefinitions.Count;
            int rowsGF = w.gridGF.RowDefinitions.Count;
            // заполнить массив matrixGF коэф. цел. ф-ии
            for (int i = 0; i < rowsG; i++)
            {
                for (int j = 0; j < columnsG - 2; j += 2)
                {
                    CheckBox ch = (CheckBox)w.gridGF.Children[i * (columnsG) + i + j];
                    int row = Grid.GetRow(ch);
                    int column = Grid.GetColumn(ch) / 2;
                    matrixGFlag[column, row] = (bool) ch.IsChecked;
                }
            }
        }
        // для функции ReadTextGrid
        // функция распознования содержимого строки (введенных чисел)

        public static Fraction ReadText(string str)
        {
            Fraction fr = new Fraction();
            // если пустая строка
            if (String.IsNullOrWhiteSpace(str))
            {
                return fr;
            }
            str = str.Replace(" ", "");
            // если неправильная дробь со знаком
            string pattern1 = @"[-,+]{1}\d*/{1}\d*";
            Regex regex1 = new Regex(pattern1);
            if (regex1.IsMatch(str))
            {
                ReadStr1(str, fr);
                return fr;
            }

            // если неправильная дробь без знака
            string pattern2 = @"\d*/{1}\d*";
            Regex regex2 = new Regex(pattern2);
            if (regex2.IsMatch(str))
            {
                ReadStr2(str, fr);
                return fr;
            }

            // если десячная дробь со знаком
            string pattern3 = @"[-,+]{1}\d*[\,,\.]{1}\d*";
            Regex regex3 = new Regex(pattern3);
            if (regex3.IsMatch(str))
            {
                ReadStr3(str, fr);
                return fr;
            }

            // если десячная дробь без знака
            string pattern4 = @"\d*[\,,\.]{1}\d*";
            Regex regex4 = new Regex(pattern4);
            if (regex4.IsMatch(str))
            {
                ReadStr4(str, fr);
                return fr;
            }

            // если целое со знаком
            string pattern5 = @"[-,+]{1}\d*";
            Regex regex5 = new Regex(pattern5);
            if (regex5.IsMatch(str))
            {
                ReadStr5(str, fr);
                return fr;
            }

            // если целое без знака
            string pattern6 = @"\d*";
            Regex regex6 = new Regex(pattern6);
            if (regex6.IsMatch(str))
            {
                ReadStr6(str, fr);
                return fr;
            }
            MessageBox.Show("ошибка: Неверный формат числа");
            return fr;
        }

        private static void ReadStr1(string str, Fraction fr)
        {
            string strNum = "", strDenum = "";
            char sign = str[0];
            int strLen = str.Length;
            int i = 1;
            while (!str[i].Equals('/'))
            {
                strNum += str[i];
                i++;
            }
            i++;
            while (i < strLen)
            {
                strDenum += str[i];
                i++;
            }
            fr.Numerator = Convert.ToInt32(strNum);
            if (sign.Equals('-'))
            {
                fr.Numerator = 0 - fr.Numerator;
            }
            fr.Denumerator = Convert.ToInt32(strDenum);
        }
        private static void ReadStr2(string str, Fraction fr)
        {
            string strNum = "", strDenum = "";
            int strLen = str.Length;
            int i = 0;
            while (!str[i].Equals('/'))
            {
                strNum += str[i];
                i++;
            }
            i++;
            while (i < strLen)
            {
                strDenum += str[i];
                i++;
            }
            fr.Numerator = Convert.ToInt32(strNum);            
            fr.Denumerator = Convert.ToInt32(strDenum);
        }
        private static void ReadStr3(string str, Fraction fr)
        {
            string strN = "";
            char sign = str[0];
            int strLen = str.Length;
            int i = 1;
            while (!str[i].Equals(',') && !str[i].Equals('.'))
            {
                strN += str[i];
                i++;
            }
            // ","
            i++;
            // дробь
            int count = 0;
            while (i < strLen)
            {
                strN += str[i];
                i++;
                count++;
            }
            int num = Convert.ToInt32(strN);
            int denum = 1;
            if (count != 0)
            {
                denum = count * 10;
            }
            fr.Numerator = num;
            if (sign.Equals('-'))
            {
                fr.Numerator = 0 - fr.Numerator;
            }
            fr.Denumerator = denum;
            fr = Fraction.Reduce(fr);
        }

        private static void ReadStr4(string str, Fraction fr)
        {
            string strN = "";
            int strLen = str.Length;
            int i = 0;
            while (!str[i].Equals(',') && !str[i].Equals('.'))
            {
                strN += str[i];
                i++;
            }
            // ","
            i++;
            // дробь
            int count = 0;
            while (i < strLen)
            {
                strN += str[i];
                i++;
                count++;
            }
            int num = Convert.ToInt32(strN);
            int denum = 1;
            if (count != 0)
            {
                denum = count * 10;
            }
            fr.Numerator = num;
            fr.Denumerator = denum;
            fr = Fraction.Reduce(fr);
        }

        private static void ReadStr5(string str, Fraction fr)
        {
            string strN = "";
            char sign = str[0];
            int strLen = str.Length;
            int i = 1;
            while (i < strLen)
            {
                strN += str[i];
                i++;
            }
            int num = Convert.ToInt32(strN);
            int denum = 1;
            fr.Numerator = num;
            if (sign.Equals('-'))
            {
                fr.Numerator = 0 - fr.Numerator;
            }
            fr.Denumerator = denum;
        }

        private static void ReadStr6(string str, Fraction fr)
        {
            string strN = "";
            int strLen = str.Length;
            int i = 0;
            while (i < strLen)
            {
                strN += str[i];
                i++;
            }
            int num = Convert.ToInt32(strN);
            int denum = 1;            
            fr.Numerator = num;
            fr.Denumerator = denum;
        }


    }
}
