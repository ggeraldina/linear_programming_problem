using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.IO;

namespace LinearProgrammingProblem_GrushevskayaIT31
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public LinearProgrammingProblem lpp { get; set; }
        public string selectedMethod { get; set; }

        public MainWindow()
        {
            lpp = new LinearProgrammingProblem();
            InitializeComponent();
            // заполнить comboBoxColumn
            if (comboBoxColumn != null)
            {
                comboBoxColumn.Items.Clear();
                int columns = lpp.maxDimension;
                for (int i = 1; i <= columns; i++)
                    comboBoxColumn.Items.Add(i.ToString());
            }
            selectedMethod = "MAB";
            SelectedMethod();
            comboBoxColumn.SelectedIndex = 0;
            comboBoxRow.SelectedIndex = 0;            
        }

        public void SelectedMethod()
        {
            switch (selectedMethod)
            {
                case "Gauss":
                    radioButtonG.IsChecked = true;
                    break;
                case "MAB":
                    radioButtonMAB.IsChecked = true;
                    break;
            }
        }

        private void MatrixDimensionsChangedColumns(object sender, SelectionChangedEventArgs e)
        {
            // заполнить  comboBoxRow
            if (comboBoxColumn != null && comboBoxRow != null)
            {
                comboBoxRow.Items.Clear();
                int columns = comboBoxColumn.SelectedIndex + 1;
                for (int i = 1; i <= columns; i++)
                    comboBoxRow.Items.Add(i.ToString());
            }
            comboBoxRow.SelectedIndex = 0;
            MatrixDimensionsChanged(sender, e);
        }

        private void MatrixDimensionsChanged(object sender, SelectionChangedEventArgs e)
        {
            // размер матриц
            int m1rows = 1;
            int m1columns = 1;
            int m2rows = 1;
            int m2columns = 1;
            // если размер задан пользователем, то применить его
            if (comboBoxColumn != null)
            {
                m2columns = comboBoxColumn.SelectedIndex + 2;
                m1columns = m2columns;
            }
            if (comboBoxRow != null)
                m2rows = comboBoxRow.SelectedIndex + 1;
            // создать матрицы для цел. ф-ии и условий
            lpp.matrix1 = new Fraction[m1columns, m1rows];
            lpp.matrix2 = new Fraction[m2columns, m2rows];
            lpp.matrixG = new Fraction[m1columns, m1rows];
            lpp.matrixGFlag = new bool[m1columns, m1rows];
            // отобразить значения матриц на экране
            Display.InitializeGrid_1_2(this, grid1, lpp.matrix1);
            Display.InitializeGrid_1_2(this, grid2, lpp.matrix2);
            Display.InitializeGrid_G(this, gridG, lpp.matrixG);
            Display.InitializeGrid_GF(this, gridGF, lpp.matrixG, lpp.matrixGFlag);
            switch (selectedMethod)
            {
                case "MAB":
                    gridG.Visibility = Visibility.Collapsed;
                    gridGF.Visibility = Visibility.Collapsed;
                    string str = "вспомогательная задача: ";
                    int col = lpp.matrix2.GetLength(0);
                    int r = lpp.matrix2.GetLength(1);
                    for (int i = col; i < col + r; i++)
                    {
                        str += ("x" + i);
                        if (i != (col + r - 1))
                        {
                            str += "+";
                        }
                    }
                    messageGF.Content = str + "-> min";
                    break;                
            } 
            // очистить и скрыть все, что находится ниже на экране 
            HideAll();
        }

        private void HideAll()
        {
            // очистить и скрыть все, что находится ниже на экране 
            ClearMAB();
            ClearSM();                  
            // скрыть кнопки навигации
            Display.ChangePropertiesButton_VisibilityCollapsed_1(this);
            Display.ChangePropertiesButton_VisibilityCollapsed_2(this);
        }

        // очистить сетку для 1 части решения
        private void ClearMAB()
        {
            // очистить матрицу
            if (grid3 != null)
            {
                grid3.Children.Clear();
                grid3.ColumnDefinitions.Clear();
                grid3.RowDefinitions.Clear();
                message.Content = "";
                messageAnswer.Content = "";
                grid3.Width = 0;
                grid3.Height = 0;
            }
            if (lpp.methodOfArtificialBasis != null)
            {
                lpp.methodOfArtificialBasis.Clear();
            }
            lpp.nowNumMatrixMAB = 0;
            
        }

        // очистить сетку для 1 части решения
        private void ClearG()
        {
            // очистить матрицу
            if (grid3 != null)
            {
                grid3.Children.Clear();
                grid3.ColumnDefinitions.Clear();
                grid3.RowDefinitions.Clear();
                message.Content = "";
                messageAnswer.Content = "";
                grid3.Width = 0;
                grid3.Height = 0;
            }
            if (lpp.methodGauss != null)
            {
                lpp.methodGauss.Clear();
            }
            lpp.nowNumMatrixG = 0;

        }

        // очистить сетку для 2 части решения
        private void ClearSM()
        {
            // очистить матрицу
            if (grid4 != null)
            {
                grid4.Children.Clear();
                grid4.ColumnDefinitions.Clear();
                grid4.RowDefinitions.Clear();
                message2.Content = "";
                messageAnswer2.Content = "";
                grid4.Width = 0;
                grid4.Height = 0;
            }
            if (lpp.simplexMethod != null)
            {
                lpp.simplexMethod.Clear();
            }
            lpp.nowNumMatrixSM = 0;            
        }
        

        // calculateButton кнопка перехода к 1 части решения
        private void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            message.Content = "";
            message2.Content = "";
            messageAnswer.Content = "";
            messageAnswer2.Content = "";
            // заполнить массивы с коэф-ми цел. ф-ии и условий
            LinearProgrammingProblem.ReadTextGrid(this, lpp.matrix1, lpp.matrix2, lpp.matrixG, lpp.matrixGFlag);
            // запомнить min или max ищем
            int columns1 = grid1.ColumnDefinitions.Count;
            ComboBox cB = (ComboBox)grid1.Children[columns1 - 1];
            lpp.extremum = (string)cB.SelectionBoxItem;
            // проверка ранга 
            GaussMethod MatrixsG = new GaussMethod(lpp.matrix2.GetLength(0), lpp.matrix2.GetLength(1), lpp.matrix2);
            if (MatrixsG.Rank == MatrixsG.m)
            {
                // очистить матрицы симпл. метода
                ClearSM();
                // скрыть/ сделать недоступными кнопки навигации
                Display.ChangePropertiesButton_EnabledFalse_1(this);
                Display.ChangePropertiesButton_VisibilityCollapsed_2(this);            
                // очистить для новых записей
                ClearMAB();
                ClearG();
                // 1 часть Гаусс/ мет иск. баз.
                switch (selectedMethod)
                {
                    case "Gauss":
                        CalculateG();
                        break;
                    case "MAB":
                        CalculateMAB();
                        break;
                }                
            }
            else
            {
                MessageBox.Show("ошибка: условий больше, чем требуется");
            }
        }

        private void CalculateMAB()
        {
            DeleteClickButton();

            prevButton.Click += prevButton_Click;
            nextButton.Click += nextButton_Click;
            startButton.Click += startButton_Click;
            endButton.Click += endButton_Click;
            autoButton.Visibility = Visibility.Visible;
            // заполнить матрицу для расчета методом иск. базиса
            MAB_SM.InitializeMAB(this);
            //lpp.methodOfArtificialBasis[lpp.nowNumMatrixMAB][0, 0].Text = "№" + (lpp.nowNumMatrixMAB).ToString();
            // проверка совместности системы 
            IsBottomStringPositive("MAB", lpp.methodOfArtificialBasis[lpp.nowNumMatrixMAB]);
            // отобразить таблицу иск. базиса
            Display.InitializeGrid_3_4(this, "MAB", grid3, lpp.methodOfArtificialBasis[lpp.nowNumMatrixMAB]);
        }

        private void DeleteClickButton()
        {
            prevButton.Click -= prevButton_Click_G;
            nextButton.Click -= nextButton_Click_G;
            startButton.Click -= startButton_Click_G;
            endButton.Click -= endButton_Click_G;
            prevButton.Click -= prevButton_Click;
            nextButton.Click -= nextButton_Click;
            startButton.Click -= startButton_Click;
            endButton.Click -= endButton_Click;
        }

        private void CalculateG()
        {

            DeleteClickButton();

            prevButton.Click += prevButton_Click_G;
            nextButton.Click += nextButton_Click_G;
            startButton.Click += startButton_Click_G;
            endButton.Click += endButton_Click_G;

            autoButton.Visibility = Visibility.Collapsed;
            // заполнить матрицу для расчета методом иск. базиса
            GaussMethod.InitializeG(this);
            GaussMethod.TransformMatrix(lpp, lpp.matrixG, lpp.matrixGFlag, lpp.methodGauss, message);
            lpp.nowNumMatrixG = lpp.methodGauss.Count - 1;
            if (message.Content.Equals(""))
            {
                message.Content = "Найден базис";
            }
            // отобразить таблицу иск. базиса
            Display.InitializeGrid_3_intermediary(this, grid3, lpp.methodGauss[lpp.nowNumMatrixG]);
           
        }

        // нажатие опорного элемента, определение в какой части решения эл. выбран
        public void ClickedButton(object sender, RoutedEventArgs e)
        {
            Button butt = (Button)sender;
            string method = "";
            if (grid3.Children.Contains(butt))
            {
                // очистить вторую матрицу
                if (grid4 != null)
                {
                    // очистить grid
                    grid4.Children.Clear();
                    grid4.ColumnDefinitions.Clear();
                    grid4.RowDefinitions.Clear(); 
                    grid4.Width = 0;
                    grid4.Height = 0;
                }
                message.Content = "";
                message2.Content = "";
                messageAnswer.Content = "";
                messageAnswer2.Content = "";
                // скрыть/ сделать недоступными кнопки навигации
                Display.ChangePropertiesButton_VisibilityCollapsed_2(this);
                method = "MAB";
                ClickedButton(method, butt, lpp.methodOfArtificialBasis, grid3);
            }
            else
            {
                message2.Content = "";
                method = "SM";
                ClickedButton(method, butt, lpp.simplexMethod, grid4);
            }
        }
        // результат нажатия опорного элемента
        private void ClickedButton(string method, Button butt,
            List<Fraction[,]> matrixs, Grid grid)
        {
            // определить какая матрица на экране в данный момент
            int nowNum = MAB_SM.getNowNum(lpp, method);
            // очистить хвост списка (все матрицы большие по номеру, чем текущая)           
            int count = matrixs.Count - nowNum - 1;
            matrixs.RemoveRange(nowNum + 1, count);
            // определить координаты кнопки
            int row = Grid.GetRow(butt);
            int column = Grid.GetColumn(butt);
            // кол-во столбцов в матрице
            int columns = matrixs[nowNum].GetLength(0);
            MessageBox.Show("Вы выбрали опорным элемент,\n" +
                "расположенный на пересечении x" + column + " и x" + (row + columns - 2));
            // сделать шаг мет.иск.баз./симп.метода
            MAB_SM.Step(lpp, column, row, method, nowNum, matrixs);
            // определить № последней матрицы в списке
            nowNum = matrixs.Count - 1;
            // проверка на положительность всех эл. в нижней строке
            IsBottomStringPositive(method, matrixs[nowNum]);
            switch (method)
            {
                case "SM": 
                    // проверка на отр. столбец
                    IsContainNegativeColumn(method, matrixs[nowNum]);
                    break;
            }
            // отобразить расчет
            Display.InitializeGrid_3_4(this, method, grid, matrixs[nowNum]);
        }

        public  void TakeAnswer(string method, Fraction[,] matrix)
        {
            Label label = new Label();
            switch (method)
            {
                case "MAB":
                    label = messageAnswer;
                    label.Content = "";
                    label.Content = "X (";            
                    
                    break;
                case "SM":
                    label = messageAnswer2;
                    label.Content = "";
                    label.Content = lpp.extremum + " F (";            
                    break;
            }
            int n = lpp.matrix2.GetLength(0);
            Fraction[] arr = new Fraction[n];
            int columns = matrix.GetLength(0);
            int rows = matrix.GetLength(1);
            for (int i = 1; i < columns - 1; i++)
            {
                arr[matrix[i, 0].Numerator] = new Fraction(0);
            }
            for (int i = 1; i < rows - 1; i++)
            {
                arr[matrix[0, i].Numerator] = matrix[columns - 1, i];
            }
            for (int i = 1; i < arr.Length; i++)
            {
                if (i != arr.Length - 1)
                {
                    label.Content += (arr[i].ToString() + "; ");
                }
                else
                {
                    label.Content += (arr[i].ToString() + " )");
                }
            }
            switch (method)
            {                
                case "SM":
                    if (lpp.extremum.Equals("min"))
                    {
                        label.Content += " =" + (new Fraction(0) - matrix[columns - 1, rows - 1]);
                    }
                    else
                    {
                        label.Content += " = " + (matrix[columns - 1, rows - 1]);
                    }
                    break;
            }
        }

        // есть ли отрицательный столбец в матрице
        public bool IsContainNegativeColumn(string method, Fraction[,] matrix)
        {
            bool flagNegative = true;
            int columns = matrix.GetLength(0);
            int rows = matrix.GetLength(1);
            for (int j = 1; j < columns - 1; j++)
            {
                flagNegative = true;
                if (matrix[j, rows - 1].Numerator >= 0)
                {
                    flagNegative = false;
                    continue;
                }
                for (int i = 1; i < rows - 1; i++)
                {
                    if (matrix[j, i] > 0)
                    {
                        flagNegative = false;
                        continue;                        
                    }
                }
                if (flagNegative) 
                {
                    message2.Content = "Функция неограничена";
                    return true; 
                }
            }
            return false;
        }
        // все ли числа (кроме посл. эл.) в нижней строке положительны
        // true - если >= 0
        private bool IsBottomStringPositive(string method, Fraction[,] matrix)
        {
            int nowNum = MAB_SM.getNowNum(lpp, method);
            int columns = matrix.GetLength(0);
            int rows = matrix.GetLength(1);
            bool flagNegative = false;
            bool flagPositive = false;
            for (int i = 1; i < columns - 1; i++)
            {
                if (matrix[i, rows - 1].Numerator < 0)
                {
                    flagNegative = true;
                }
                if (matrix[i, rows - 1].Numerator > 0)
                {
                    flagPositive = true;
                }
            }
            // >=0
            if (!flagNegative && flagPositive)
            {
                switch (method)
                {
                    case "MAB":
                        message.Content = "Система условий несовместна";
                        break;
                    case "SM":
                        message2.Content = "Найдено решение";
                        TakeAnswer(method, matrix);
                        break;
                }
                return true;
            }
            // =0
            if (!flagNegative && !flagPositive)
            {
                switch (method)
                {
                    case "MAB":
                        message.Content = "Найден базис";
                        TakeAnswer(method, matrix);
                        break;
                    case "SM":
                        message2.Content = "Найдено решение";
                        TakeAnswer(method, matrix);
                        break;
                }
                return true;
            }
            return false;
        }

        // автоматическое сохранение задачи (в меню пункт menuItemSave),
        // в случае необходимости раскомментировать
        //
        //private void menuItemSave_Click(object sender, RoutedEventArgs e)
        //{
        //    List<string> tasks = SaveTask();
        //    // добавить к имени дату для уникальности
        //    string dateTime = DateTime.Now.ToString("_yyyyMMdd_HHmmss");
        //    // сохранить
        //    File.WriteAllLines((Directory.GetCurrentDirectory() + "/tasks/task" + dateTime + ".txt"), tasks);
        //    MessageBox.Show("Задача сохранена под именем\ntask" + dateTime);
        //}


        // для сохранения задачи 
        private List<string> SaveTask()
        {
            List<string> tasks = new List<string>();
            Fraction[,] matrix1Save;
            Fraction[,] matrix2Save;
            Fraction[,] matrixGSave;
            bool[,] matrixGFlagSave;
            String extremumSave = "";

            // размер матрицы
            int m1rows = 1;
            int m1columns = 1;
            int m2rows = 1;
            int m2columns = 1;
            if (comboBoxColumn != null)
            {
                m2columns = comboBoxColumn.SelectedIndex + 2;
                m1columns = m2columns;
            }
            if (comboBoxRow != null)
                m2rows = comboBoxRow.SelectedIndex + 1;
            // создать матрицу
            matrix1Save = new Fraction[m1columns, m1rows];
            matrix2Save = new Fraction[m2columns, m2rows];
            matrixGSave = new Fraction[m1columns, m1rows];
            matrixGFlagSave = new bool[m1columns, m1rows];
            // заполнить массивы с коэф-ми цел. ф-ии и условий
            LinearProgrammingProblem.ReadTextGrid(this, matrix1Save, matrix2Save, matrixGSave, matrixGFlagSave);
            // запомнить min или max ищем
            int columns1 = grid1.ColumnDefinitions.Count;
            ComboBox cB = (ComboBox)grid1.Children[columns1 - 1];
            extremumSave = (string)cB.SelectionBoxItem;
            // заполнить список строк задачи
            FullTasks(tasks, matrix1Save, matrix2Save, matrixGSave, extremumSave, m1rows, m1columns, m2rows, m2columns);
            return tasks;
        }
        // для сохранения файла (для функции SaveTask())
        // заполнение tasks
        private static void FullTasks(List<string> tasks, Fraction[,] matrix1Save, Fraction[,] matrix2Save, Fraction[,] matrixGSave, String extremumSave, int m1rows, int m1columns, int m2rows, int m2columns)
        {
            tasks.Add((m2columns - 1).ToString());
            tasks.Add(m2rows.ToString());
            tasks.Add(extremumSave);
            for (int i = 0; i < m1rows; i++)
            {
                string s = "";
                for (int j = 0; j < m1columns - 1; j++)
                {
                    s += (matrix1Save[j, i].ToString() + ";");
                }
                tasks.Add(s);
            }
            for (int i = 0; i < m2rows; i++)
            {
                string s = "";
                for (int j = 0; j < m2columns; j++)
                {
                    s += (matrix2Save[j, i].ToString() + ";");
                }
                tasks.Add(s);
            }
            for (int i = 0; i < m1rows; i++)
            {
                string s = "";
                for (int j = 0; j < m1columns - 1; j++)
                {
                    s += (matrixGSave[j, i].ToString() + ";");
                }
                tasks.Add(s);
            }
        }

        //// для метода иск. баз.
        private void menuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt";
            openFileDialog.InitialDirectory = (Directory.GetCurrentDirectory() + "\\tasks");
            openFileDialog.ShowDialog();
            string selectedFile = openFileDialog.FileName;
            if (!selectedFile.Equals(""))
            {
                try
                {
                    string[] linesSelectedFile = File.ReadAllLines(selectedFile);
                    FillAllComboBox(this, linesSelectedFile);
                    FullGrid1(this, linesSelectedFile);
                    FullGrid2(this, linesSelectedFile);
                    FullGridG(this, linesSelectedFile);
                    // очистить и скрыть все, что находится ниже на экране 
                    HideAll();
                }
                catch
                {
                    MessageBox.Show("ошибка при чтении файла");
                    return;
                }
            }
        }
        // для чтения из файла (для функции menuItemOpen_Click)
        // заполнение grid2
        private static void FullGrid2(MainWindow w, string[] linesSelectedFile)
        {
            // grid2
            int columns2 = w.grid2.ColumnDefinitions.Count;
            int rows2 = w.grid2.RowDefinitions.Count;
            // заполнить коэф. условий 
            for (int i = 0; i < rows2; i++)
            {
                string[] texts;
                if (!linesSelectedFile[4 + i].Equals(""))
                {
                    texts = linesSelectedFile[4 + i].Trim(new char[] { ';' }).Split(new char[] { ';' }); ;

                }
                else
                {
                    MessageBox.Show("ошибка при чтении файла");
                    return;
                }
                for (int j = 0; j < columns2; j += 2)
                {
                    int num = i * (columns2 - 1) + i + j;
                    TextBox t = (TextBox)w.grid2.Children[num];
                    t.Text = texts[j / 2];
                }
            }
        }
        // для чтения из файла (для функции menuItemOpen_Click)
        // заполнение grid1
        private static void FullGrid1(MainWindow w, string[] linesSelectedFile)
        {
            // grid1
            int columns1 = w.grid1.ColumnDefinitions.Count;
            int rows1 = w.grid1.RowDefinitions.Count;
            // заполнить коэф. цел. ф-ии
            for (int i = 0; i < rows1; i++)
            {
                string[] texts;
                if (!linesSelectedFile[3 + i].Equals(""))
                {
                    texts = linesSelectedFile[3 + i].Trim(new char[] { ';' }).Split(new char[] { ';' }); ;
                }
                else
                {
                    MessageBox.Show("ошибка при чтении файла");
                    return;
                }
                for (int j = 0; j < columns1 - 2; j += 2)
                {
                    TextBox t = (TextBox)w.grid1.Children[i * (columns1 - 1) + i + j];
                    t.Text = texts[j / 2];
                }
            }
            return;
        }
        // для чтения из файла (для функции menuItemOpen_Click)
        // заполнение gridG
        private static void FullGridG(MainWindow w, string[] linesSelectedFile)
        {
            // gridG
            int columnsG = w.gridG.ColumnDefinitions.Count;
            int rowsG = w.gridG.RowDefinitions.Count;
            // заполнить коэф. цел. ф-ии
            for (int i = 0; i < rowsG; i++)
            {
                string[] texts;
                if (!linesSelectedFile[4 + Convert.ToInt32(linesSelectedFile[1]) + i].Equals(""))
                {
                    texts = linesSelectedFile[4 + 
                        Convert.ToInt32(linesSelectedFile[1]) + i].Trim(new 
                            char[] { ';' }).Split(new char[] { ';' }); 
                }
                else
                {
                    MessageBox.Show("ошибка при чтении файла");
                    return;
                }
                for (int j = 0; j < columnsG - 2; j += 2)
                {
                    TextBox t = (TextBox)w.gridG.Children[i * (columnsG) + i + j];
                    t.Text = texts[j / 2];
                    CheckBox ch = (CheckBox)w.gridGF.Children[i * (columnsG) + i + j];
                    ch.IsChecked = true;                    
                    if (LinearProgrammingProblem.ReadText(t.Text).Numerator == 0)
                    {
                        ch.IsChecked = false;
                    }
                    
                }
            }
            return;
        }
        // для чтения из файла (для функции menuItemOpen_Click)
        // заполнение comboBox
        private static void FillAllComboBox(MainWindow w, string[] linesSelectedFile)
        {
            if (!linesSelectedFile[0].Equals(""))
            {
                int m2columns = Convert.ToInt32(linesSelectedFile[0]);
                w.comboBoxColumn.SelectedIndex = m2columns - 1;
            }
            else
            {
                MessageBox.Show("ошибка при чтении файла");
                return;
            }
            if (!linesSelectedFile[1].Equals(""))
            {
                int m2rows = Convert.ToInt32(linesSelectedFile[1]);
                w.comboBoxRow.SelectedIndex = m2rows - 1;
            }
            else
            {
                MessageBox.Show("ошибка при чтении файла");
                return;
            }
            if (!linesSelectedFile[2].Equals(""))
            {
                String extremum = linesSelectedFile[2];
                // min или max ищем
                int columns = w.grid1.ColumnDefinitions.Count;
                ComboBox cB = (ComboBox)w.grid1.Children[columns - 1];
                if (!extremum.Equals((string)cB.SelectionBoxItem))
                {
                    cB.SelectedIndex++;
                }
            }
            else
            {
                MessageBox.Show("ошибка при чтении файла");
                return;
            }
            return;
        }


        //// для метода иск. баз.
        private void prevButton_Click(object sender, RoutedEventArgs e)
        {
            lpp.nowNumMatrixMAB--;
            lpp.methodOfArtificialBasis[lpp.nowNumMatrixMAB][0, 0].Text = "№" + (lpp.nowNumMatrixMAB).ToString();
            if (lpp.nowNumMatrixMAB % 2 == 0)
            {
                Display.InitializeGrid_3_4(this, "MAB", grid3, lpp.methodOfArtificialBasis[lpp.nowNumMatrixMAB]);
            }
            else
            {
                Display.InitializeGrid_3_intermediary(this, grid3, lpp.methodOfArtificialBasis[lpp.nowNumMatrixMAB]);
            }
        }
        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            lpp.nowNumMatrixMAB++;
            lpp.methodOfArtificialBasis[lpp.nowNumMatrixMAB][0, 0].Text = "№" + (lpp.nowNumMatrixMAB).ToString();
            if (lpp.nowNumMatrixMAB % 2 == 0)
            {
                Display.InitializeGrid_3_4(this, "MAB", grid3, lpp.methodOfArtificialBasis[lpp.nowNumMatrixMAB]);
            }
            else
            {
                Display.InitializeGrid_3_intermediary(this, grid3, lpp.methodOfArtificialBasis[lpp.nowNumMatrixMAB]);
            }
        }
        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            lpp.nowNumMatrixMAB = 0;
            lpp.methodOfArtificialBasis[lpp.nowNumMatrixMAB][0, 0].Text = "№" + (lpp.nowNumMatrixMAB).ToString();
            Display.InitializeGrid_3_4(this, "MAB", grid3, lpp.methodOfArtificialBasis[lpp.nowNumMatrixMAB]);
        }
        private void endButton_Click(object sender, RoutedEventArgs e)
        {
            lpp.nowNumMatrixMAB = lpp.methodOfArtificialBasis.Count - 1;
            lpp.methodOfArtificialBasis[lpp.nowNumMatrixMAB][0, 0].Text = "№" + (lpp.nowNumMatrixMAB).ToString();
            Display.InitializeGrid_3_4(this, "MAB", grid3, lpp.methodOfArtificialBasis[lpp.nowNumMatrixMAB]);
        }

        //// для метода иск. баз.
        private void autoButton_Click(object sender, RoutedEventArgs e)
        {
            // очистить хвост списка
            if (lpp.nowNumMatrixMAB % 2 != 0)
            {
                lpp.nowNumMatrixMAB++;
            }
            int countMatrix = lpp.methodOfArtificialBasis.Count - lpp.nowNumMatrixMAB - 1;
            lpp.methodOfArtificialBasis.RemoveRange(lpp.nowNumMatrixMAB + 1, countMatrix);
            int column;
            int row;
            // количество шагов
            int countStep = lpp.matrix2.GetLength(1) - (lpp.nowNumMatrixMAB / 2);
            for (int i = 0; i < countStep; i++)
            {
                // найти координаты опорного элемента
                MAB_SM.SelectSupportElem(lpp, out column, out row, "MAB", lpp.methodOfArtificialBasis[lpp.nowNumMatrixMAB]);
                // рассчитать
                if (column > 0 && row > 0)
                {
                    MAB_SM.Step(lpp, column, row, "MAB", lpp.nowNumMatrixMAB, lpp.methodOfArtificialBasis);
                    lpp.methodOfArtificialBasis[lpp.nowNumMatrixMAB][0, 0].Text = "№" + (lpp.nowNumMatrixMAB).ToString();
                    // проверка совместности системы 
                    IsBottomStringPositive("MAB", lpp.methodOfArtificialBasis[lpp.nowNumMatrixMAB]);
                    // отобразить расчет
                    Display.InitializeGrid_3_4(this, "MAB", grid3, lpp.methodOfArtificialBasis[lpp.nowNumMatrixMAB]);
                }
                else
                {
                    if (message.Content.Equals(""))
                    {
                        message.Content = "Нет подходящего опорного элемента. Найден цикл";
                    }
                }
            }
        }

        //// после метода иск. баз.
        private void calculateButton2_Click(object sender, RoutedEventArgs e)
        {
            // очистить матрицы симпл. метода
            ClearSM();
            message2.Content = "";
            messageAnswer2.Content = "";
            // сделать кнопки навигации недоступными
            Display.ChangePropertiesButton_EnabledFalse_2(this);  
          
            // заполнить матрицу для расчета сим. методом 
           switch (selectedMethod)
            {
                case "Gauss":
                    MAB_SM.InitializeSM_G(this);
            
                    break;
                case "MAB":
                    MAB_SM.InitializeSM(this);
            
                    break;
            }

            // проверка 
            IsBottomStringPositive("SM", lpp.simplexMethod[lpp.nowNumMatrixSM]);
            IsContainNegativeColumn("SM", lpp.simplexMethod[lpp.nowNumMatrixSM]);
            if (message2.Content.Equals("Функция неограничена"))
            {
                // отобразить таблицу 
                Display.InitializeGrid_3_intermediary(this, grid4, lpp.simplexMethod[lpp.nowNumMatrixSM]);
                autoButton2.IsEnabled = false;
            }
            else
            {
                // отобразить таблицу 
                Display.InitializeGrid_3_4(this, "SM", grid4, lpp.simplexMethod[lpp.nowNumMatrixSM]);
                autoButton2.IsEnabled = true;
            }
            
        }

        // симлекс метод
        private void autoButton2_Click(object sender, RoutedEventArgs e)
        {
            // очистить хвост списка
            int countMatrix = lpp.simplexMethod.Count - lpp.nowNumMatrixSM - 1;
            lpp.simplexMethod.RemoveRange(lpp.nowNumMatrixSM + 1, countMatrix);
            int column;
            int row;
            while (true)
            {
                MAB_SM.SelectSupportElem(lpp, out column, out row, "SM", lpp.simplexMethod[lpp.nowNumMatrixSM]);
                // рассчитать
                if (column > 0 && row > 0)
                {
                    // проверка 
                    if (IsBottomStringPositive("SM", lpp.simplexMethod[lpp.nowNumMatrixSM]) ||
                        IsContainNegativeColumn("SM", lpp.simplexMethod[lpp.nowNumMatrixSM]))
                    {

                        // отобразить расчет
                        Display.InitializeGrid_3_4(this, "SM", grid4, lpp.simplexMethod[lpp.nowNumMatrixSM]);
                        break;
                    }
                    MAB_SM.Step(lpp, column, row, "SM", lpp.nowNumMatrixSM, lpp.simplexMethod);
                    if (IsBottomStringPositive("SM", lpp.simplexMethod[lpp.nowNumMatrixSM]) ||
                        IsContainNegativeColumn("SM", lpp.simplexMethod[lpp.nowNumMatrixSM]))
                    {
                        // отобразить расчет
                        Display.InitializeGrid_3_4(this, "SM", grid4, lpp.simplexMethod[lpp.nowNumMatrixSM]);
                        break;
                    }
                    // отобразить расчет
                    Display.InitializeGrid_3_4(this, "SM", grid4, lpp.simplexMethod[lpp.nowNumMatrixSM]);
                }
                else
                {
                    if (message2.Content.Equals(""))
                    {
                        message2.Content = "Упс... См. autoButton2_Click";
                    }
                    break;
                }
            }
        }
        // симлекс метод
        private void startButton2_Click(object sender, RoutedEventArgs e)
        {
            lpp.nowNumMatrixSM = 0;

            Display.InitializeGrid_3_4(this, "SM", grid4, lpp.simplexMethod[lpp.nowNumMatrixSM]);
        }
        private void prevButton2_Click(object sender, RoutedEventArgs e)
        {
            lpp.nowNumMatrixSM--;
            Display.InitializeGrid_3_4(this, "SM", grid4, lpp.simplexMethod[lpp.nowNumMatrixSM]);
        }
        private void nextButton2_Click(object sender, RoutedEventArgs e)
        {
            lpp.nowNumMatrixSM++;
            Display.InitializeGrid_3_4(this, "SM", grid4, lpp.simplexMethod[lpp.nowNumMatrixSM]);
        }
        private void endButton2_Click(object sender, RoutedEventArgs e)
        {
            lpp.nowNumMatrixSM = lpp.simplexMethod.Count - 1;
            Display.InitializeGrid_3_4(this, "SM", grid4, lpp.simplexMethod[lpp.nowNumMatrixSM]);
        }

        private void radioButtonG_Checked(object sender, RoutedEventArgs e)
        {
            selectedMethod = "Gauss";
            // очистить и скрыть все, что находится ниже на экране 
            HideAll();
            gridG.Visibility = Visibility.Visible;
            gridGF.Visibility = Visibility.Visible;
            try
            {
                messageGF.Content = "базисные переменные: ";
            }
            catch { }

        }

        private void radioButtonMAB_Checked(object sender, RoutedEventArgs e)
        {
            selectedMethod = "MAB";
            // очистить и скрыть все, что находится ниже на экране 
            HideAll();
            gridG.Visibility = Visibility.Collapsed;
            gridGF.Visibility = Visibility.Collapsed;
            try{
                string str = "вспомогательная задача: ";
                int col = lpp.matrix2.GetLength(0);
                int r = lpp.matrix2.GetLength(1);
                for (int i = col; i < col + r; i++)
                {
                    str += ("x" + i);
                    if (i != (col + r - 1))
                    {
                        str += "+";
                    }
                }
                messageGF.Content = str + "-> min";
            }
            catch { }
        }

        //// для метода гаусса
        private void prevButton_Click_G(object sender, RoutedEventArgs e)
        {
            lpp.nowNumMatrixG--;
            lpp.methodGauss[lpp.nowNumMatrixG][0, 0].Text = "№" + lpp.nowNumMatrixG.ToString();
            Display.InitializeGrid_3_intermediary(this, grid3, lpp.methodGauss[lpp.nowNumMatrixG]);
            
        }
        private void nextButton_Click_G(object sender, RoutedEventArgs e)
        {
            lpp.nowNumMatrixG++;
            lpp.methodGauss[lpp.nowNumMatrixG][0, 0].Text = "№" + lpp.nowNumMatrixG.ToString();
            Display.InitializeGrid_3_intermediary(this, grid3, lpp.methodGauss[lpp.nowNumMatrixG]);
            
        }
        private void startButton_Click_G(object sender, RoutedEventArgs e)
        {
            lpp.nowNumMatrixG = 0;
            lpp.methodGauss[lpp.nowNumMatrixG][0, 0].Text = "№" + lpp.nowNumMatrixG.ToString();
            Display.InitializeGrid_3_intermediary(this, grid3, lpp.methodGauss[lpp.nowNumMatrixG]);
        }
        private void endButton_Click_G(object sender, RoutedEventArgs e)
        {
            lpp.nowNumMatrixG = lpp.methodGauss.Count - 1;
            lpp.methodGauss[lpp.nowNumMatrixG][0, 0].Text = "№" + lpp.nowNumMatrixG.ToString();
            Display.InitializeGrid_3_intermediary(this, grid3, lpp.methodGauss[lpp.nowNumMatrixG]);
        }

        private void menuItemHelp_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Help.ShowHelp(null, "help/HelpAlfa.chm");
            
            // старый вариант меню
            //HelpOld windowHelp = new HelpOld();
            //windowHelp.ShowDialog();
        }

        private void menuItemAboutProgram_Click(object sender, RoutedEventArgs e)
        {
            AboutProgram windowAboutProgram = new AboutProgram();
            windowAboutProgram.ShowDialog();
        }

        private void menuItemSaveAs_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog.InitialDirectory = (Directory.GetCurrentDirectory() + "\\tasks");
           // добавить к имени дату для уникальности
            string dateTime = DateTime.Now.ToString("_yyyyMMdd_HHmmss");
            // сохранить

            saveFileDialog.FileName = "task" + dateTime + ".txt";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
             {
                    return;
             }
            string name = saveFileDialog.FileName;
            List<string> tasks = SaveTask();            
            // сохранить
            File.WriteAllLines(name, tasks);            
        }


    }
}
