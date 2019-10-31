using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System;
using System.Collections.Generic;
using System.Windows.Documents;
using System.IO;
namespace LinearProgrammingProblem_GrushevskayaIT31
{
    class Display
    {
        // отобразить на экране значения матрицы (для цел. ф-ии и условий)
        public static void InitializeGrid_1_2(MainWindow w, Grid grid, Fraction[,] matrix)
        {
            
            // grid - где отображаем
            if (grid != null)
            {
                int columns;
                int rows;
                DisplayGrid_1_2_G(w, grid, matrix, out columns, out rows);
                // заполнить grid
                for (int y = 0; y < rows; y++)
                {
                    w.messageGreaterEqual.Content = "";
                    for (int x = 0; x < columns; x++)
                    {
                        if (x != columns - 1 || !grid.Equals(w.grid1))
                        {
                            // чисела
                            // значение ячейки textBox
                            if (matrix[x, y] == null)
                            {
                                matrix[x, y] = new Fraction();
                            }
                            // значение ячейки
                            Fraction cell = (Fraction)matrix[x, y];
                            TextBox textBox = new TextBox();
                            textBox.Text = cell.ToString();
                            // выровнять textBox
                            textBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            textBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            // задать месторасположение textBox в grid
                            textBox.SetValue(Grid.RowProperty, y);
                            textBox.SetValue(Grid.ColumnProperty, x * 2);
                            grid.Children.Add(textBox);                
                        }
                        else
                        {
                            // экстремум
                            ComboBox comboBox = new ComboBox();
                            comboBox.Items.Add("min");
                            comboBox.Items.Add("max");
                            comboBox.Height = 25;
                            comboBox.Width = 80;
                            comboBox.SelectedIndex = 0;
                            comboBox.SetValue(Grid.RowProperty, y);
                            comboBox.SetValue(Grid.ColumnProperty, x * 2);
                            grid.Children.Add(comboBox);
                        }
                        if (x != columns - 1)
                        {
                            // значение переменной
                            Label label = new Label();
                            if (x == columns - 2)
                            {
                                label.Content = "x" + (x + 1).ToString() + "=";
                                w.messageGreaterEqual.Content += "x" + (x + 1).ToString() + " >= 0 ";
                            }
                            else
                            {
                                label.Content = "x" + (x + 1).ToString() + "+";
                                w.messageGreaterEqual.Content += "x" + (x + 1).ToString() + ", ";
                            }
                            label.Width = 35;
                            label.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            label.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                            // задать месторасположение label
                            label.SetValue(Grid.RowProperty, y);
                            label.SetValue(Grid.ColumnProperty, x * 2 + 1);
                            grid.Children.Add(label);
                        }
                    }
                }
            }
        }        

        private static void DisplayGrid_1_2_G(MainWindow w, Grid grid, Fraction[,] matrix, out int columns, out int rows)
        {
            // очистить grid
            grid.Children.Clear();
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();
            // размер grid
            columns = matrix.GetLength(0);
            rows = matrix.GetLength(1);
            // добавить нужное количество столбцов и строк в grid
            for (int x = 0; x < columns * 2 - 1; x++)
            {
                // GridUnitType.Star - The value is expressed as a weighted proportion of available space
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star), });
            }
            for (int y = 0; y < rows; y++)
            {
                // GridUnitType.Star - The value is expressed as a weighted proportion of available space
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star), });
            }
            // задать параметры grid
            grid.Width = (w.gridBase.Width - grid.Margin.Left - grid.Margin.Right) * columns / w.lpp.maxDimension;
            grid.Height = 444 * rows / w.lpp.maxDimension;
        }

        // отобразить на экране значения матрицы с базисом
        public static void InitializeGrid_G(MainWindow w, Grid grid, Fraction[,] matrix)
        {
            // grid - где отображаем
            if (grid != null)
            {
                int columns;
                int rows;
                DisplayGrid_1_2_G(w, grid, matrix, out columns, out rows);
                // заполнить grid
                for (int y = 0; y < rows; y++)
                {
                    for (int x = 0; x < columns; x++)
                    {
                        if (x != columns - 1)
                        {
                            // чисела
                            // значение ячейки textBox
                            if (matrix[x, y] == null)
                            {
                                matrix[x, y] = new Fraction();
                            }
                            // значение ячейки
                            Fraction cell = (Fraction)matrix[x, y];
                            TextBox textBox = new TextBox();
                            textBox.Text = cell.ToString();
                            // выровнять textBox
                            textBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            textBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            // задать месторасположение textBox в grid
                            textBox.SetValue(Grid.RowProperty, y);
                            textBox.SetValue(Grid.ColumnProperty, x * 2 + 1);
                            grid.Children.Add(textBox);
                        }                     
                        // значение переменной
                        Label label = new Label();
                        label.Content = "; ";
                        if (x == 0)
                        {
                            label.Content = "X (" ;
                        }
                        if (x == columns - 1)
                        {
                            label.Content = ")";
                        }
                        label.Width = 35;
                        label.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        label.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                        // задать месторасположение label
                        label.SetValue(Grid.RowProperty, y);
                        label.SetValue(Grid.ColumnProperty, x * 2);
                        grid.Children.Add(label);
                       
                    }
                }
            }
        }

        // отобразить на экране значения матрицы с базисными переменными
        public static void InitializeGrid_GF(MainWindow w, Grid grid, Fraction[,] matrix, bool[,] matrixF)
        {
            // grid - где отображаем
            if (grid != null)
            {
                int columns;
                int rows;
                DisplayGrid_1_2_G(w, grid, matrix, out columns, out rows);
                // заполнить grid
                for (int y = 0; y < rows; y++)
                {
                    for (int x = 0; x < columns; x++)
                    {
                        if (x != columns - 1)
                        {
                            // 
                            Fraction cell = (Fraction)matrix[x, y];
                            CheckBox checkBox = new CheckBox();
                            // значение ячейки checkBox                            
                            if (cell.Numerator == 0)
                            {
                                checkBox.IsChecked = false;
                            }
                            else
                            {
                                checkBox.IsChecked = true;
                            }
                            // выровнять checkBox 
                            checkBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            checkBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            // задать месторасположение checkBox в grid
                            checkBox.SetValue(Grid.RowProperty, y);
                            checkBox.SetValue(Grid.ColumnProperty, x * 2 + 1);
                            grid.Children.Add(checkBox);
                        }
                        // значение переменной
                        Label label = new Label();
                        if (x != columns - 1)
                        {
                            label.Content = "x" + (x + 1) + ": ";
                        }
                        label.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                        label.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
                        // задать месторасположение label
                        label.SetValue(Grid.RowProperty, 2);
                        label.SetValue(Grid.ColumnProperty, x * 2);
                        grid.Children.Add(label);                        

                    }
                }              
                
            }
        }

        // отбразить матрицу мет. иск. баз./ сим. мет.
        public static void InitializeGrid_3_4(MainWindow w,
            string method, Grid grid, Fraction[,] matrix)
        {
            // изменить доступность кнопок навигации
            switch (method)
            {
                case "MAB":
                    ChangePropertiesButton_Enabled_1(w);
                    break;
                case "SM":
                    ChangePropertiesButton_Enabled_2(w);
                    break;
            }
            // grid - где отображаем
            if (grid != null)
            {
                int columns;
                int rows;
                // разлинеить сетку
                DisplayGrid_3_4(w, grid, matrix, out columns, out rows);
                // минимальные отношения b/a в каждом столбце
                Fraction[] minRatio = new Fraction[columns];
                for (int i = 1; i < columns - 1; i++)
                {
                    minRatio[i] = MAB_SM.FindSupportMinRatio(i, matrix);
                }
                // флаг зацикливания
                bool flagCycle = true;
                // добавить TextBox/Label в grid
                for (int y = 0; y < rows; y++)
                {
                    for (int x = 0; x < columns; x++)
                    {
                        if (MAB_SM.IsCanBeSupperElem(matrix, columns, rows, minRatio, y, x))
                        {
                            switch (method)
                            {
                                case "MAB":
                                    // в строке вспомогательный х
                                    if (matrix[0, y] >= w.lpp.matrix2.GetLength(0))
                                    {
                                        DisplayButton(w, grid, matrix, y, x);
                                        flagCycle = false;
                                    }
                                    else
                                    {
                                        DisplayLabel(grid, matrix, y, x);
                                    };
                                    break;
                                case "SM":
                                    DisplayButton(w, grid, matrix, y, x);
                                    flagCycle = false;
                                    break;
                            }
                        }
                        else
                        {
                            DisplayLabel(grid, matrix, y, x);
                        }
                    }
                }
                if (flagCycle && w.message.Content.Equals(""))
                {
                    w.message.Content = "Нет подходящего опорного элемента. Найден цикл";
                }
            }
        }

        

        // отобразить промежуточную матрицу мет. иск. баз.
        public static void InitializeGrid_3_intermediary(MainWindow w,
           Grid grid, Fraction[,] matrix)
        {
            ChangePropertiesButton_Enabled_1(w);
            if (grid != null)
            {
                int columns;
                int rows;
                DisplayGrid_3_4(w, grid, matrix, out columns, out rows);

                // добавить Label в grid
                for (int y = 0; y < rows; y++)
                {
                    for (int x = 0; x < columns; x++)
                    {
                        DisplayLabel(grid, matrix, y, x);
                    }
                }
            }
        }

        public static void ChangePropertiesButton_VisibilityCollapsed_1(MainWindow w)
        {
            if (w.prevButton != null)
            {
                w.prevButton.Visibility = Visibility.Collapsed;
                w.nextButton.Visibility = Visibility.Collapsed;
                w.startButton.Visibility = Visibility.Collapsed;
                w.endButton.Visibility = Visibility.Collapsed;
                w.autoButton.Visibility = Visibility.Collapsed;
                w.calculateButton2.Visibility = Visibility.Collapsed;
                w.message.Content = "";
            }
        }

        public static void ChangePropertiesButton_VisibilityCollapsed_2(MainWindow w)
        {
            if (w.prevButton2 != null)
            {
                w.prevButton2.Visibility = Visibility.Collapsed;
                w.nextButton2.Visibility = Visibility.Collapsed;
                w.startButton2.Visibility = Visibility.Collapsed;
                w.endButton2.Visibility = Visibility.Collapsed;
                w.autoButton2.Visibility = Visibility.Collapsed;
                w.message2.Content = "";
            }
        }        

        public static void ChangePropertiesButton_EnabledFalse_1(MainWindow w)
        {
            ChangePropertiesButton_VisibilityVisible_1(w);
            ChangePropertiesButton_VisibilityCollapsed_2(w);
            w.prevButton.IsEnabled = false;
            w.nextButton.IsEnabled = false;
            w.startButton.IsEnabled = false;
            w.endButton.IsEnabled = false;
            w.calculateButton2.IsEnabled = false;
        }

        public static void ChangePropertiesButton_EnabledFalse_2(MainWindow w)
        {
            ChangePropertiesButton_VisibilityVisible_2(w);
            w.prevButton2.IsEnabled = false;
            w.nextButton2.IsEnabled = false;
            w.startButton2.IsEnabled = false;
            w.endButton2.IsEnabled = false;
        }

        private static void ChangePropertiesButton_EnabledTrue_1_intermediary(MainWindow w)
        {
            w.nextButton.IsEnabled = true;
            w.prevButton.IsEnabled = true;
            w.startButton.IsEnabled = true;
            w.endButton.IsEnabled = true;
        }

        private static void ChangePropertiesButton_VisibilityVisible_1(MainWindow w)
        {
            w.prevButton.Visibility = Visibility.Visible;
            w.nextButton.Visibility = Visibility.Visible;
            w.startButton.Visibility = Visibility.Visible;
            w.endButton.Visibility = Visibility.Visible;
            w.autoButton.Visibility = Visibility.Visible;
            w.calculateButton2.Visibility = Visibility.Visible;
        }

        private static void ChangePropertiesButton_VisibilityVisible_2(MainWindow w)
        {
            w.prevButton2.Visibility = Visibility.Visible;
            w.nextButton2.Visibility = Visibility.Visible;
            w.startButton2.Visibility = Visibility.Visible;
            w.endButton2.Visibility = Visibility.Visible;
            w.autoButton2.Visibility = Visibility.Visible;
        }

        private static void ChangePropertiesButton_Enabled_1(MainWindow w)
        {
            int nowNum = 0;
            int count = 0;
            switch (w.selectedMethod)
            {
                case "Gauss":
                    nowNum = w.lpp.nowNumMatrixG;
                    count = w.lpp.methodGauss.Count - 1;
                    break;
                case "MAB":
                    nowNum = w.lpp.nowNumMatrixMAB;
                    count = w.lpp.methodOfArtificialBasis.Count - 1;
                    break;
            }
            if (nowNum == 0)
            {
                w.prevButton.IsEnabled = false;
                w.startButton.IsEnabled = false;
            }
            else
            {
                w.prevButton.IsEnabled = true;
                w.startButton.IsEnabled = true;
            }
            if (nowNum == count)
            {
                w.nextButton.IsEnabled = false;
                w.endButton.IsEnabled = false;
            }
            else
            {
                w.nextButton.IsEnabled = true;
                w.endButton.IsEnabled = true;
            }            
            if (w.message.Content.Equals("Найден базис"))
            {
                w.calculateButton2.IsEnabled = true;
                switch (w.selectedMethod)
                {                
                    case "MAB":
                        nowNum = w.lpp.nowNumMatrixMAB;
                        count = w.lpp.methodOfArtificialBasis.Count - 1;
                           
                        
                            break;
                }
                

            }
            else
            {
                w.calculateButton2.IsEnabled = false;
            }
        }

        private static void ChangePropertiesButton_Enabled_2(MainWindow w)
        {
            if (w.lpp.nowNumMatrixSM == 0)
            {
                w.prevButton2.IsEnabled = false;
                w.startButton2.IsEnabled = false;
            }
            else
            {
                w.prevButton2.IsEnabled = true;
                w.startButton2.IsEnabled = true;
            }
            if (w.lpp.nowNumMatrixSM == w.lpp.simplexMethod.Count - 1)
            {
                w.nextButton2.IsEnabled = false;
                w.endButton2.IsEnabled = false;
            }
            else
            {
                w.nextButton2.IsEnabled = true;
                w.endButton2.IsEnabled = true;
            }
        }

        private static void DisplayGrid_3_4(MainWindow w, 
            Grid grid, Fraction[,] matrix, out int columns, out int rows)
        {
            // очистить grid
            grid.Children.Clear();
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();
            // размер grid
            columns = matrix.GetLength(0);
            rows = matrix.GetLength(1);
            // добавить нужное количество столбцов и строк в grid
            for (int x = 0; x < columns; x++)
            {
                // GridUnitType.Star - The value is expressed as a weighted proportion of available space
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star), });
            }
            for (int y = 0; y < rows; y++)
            {
                // GridUnitType.Star - The value is expressed as a weighted proportion of available space
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star), });
            }
            // задать параметры grid
            grid.Width = (w.gridBase.Width - grid.Margin.Left - grid.Margin.Right) * 2.5 * columns / w.lpp.maxDimension;
            grid.Height = 444 * 1.7 * rows / w.lpp.maxDimension;
            grid.ShowGridLines = true;
        }

        private static void DisplayLabel(Grid grid, Fraction[,] matrix, int y, int x)
        {
            Label label = new Label();
            label.Content = matrix[x, y].ToString();
            label.Margin = new System.Windows.Thickness(10);
            label.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            label.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            label.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            if (matrix[x, y].IsSupport)
            {
                label.Background = new SolidColorBrush(Colors.Gray);
            }
            // задать месторасположение label
            label.SetValue(Grid.RowProperty, y);
            label.SetValue(Grid.ColumnProperty, x);
            grid.Children.Add(label);
        }

        private static void DisplayButton(MainWindow w, Grid grid, Fraction[,] matrix, int y, int x)
        {
            // значение ячейки butt 
            Button butt = new Button();
            butt.Content = matrix[x, y].ToString();
            //butt.Width = 35;
            butt.Margin = new System.Windows.Thickness(10);
            // выровнять butt
            butt.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            butt.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            butt.Click += w.ClickedButton;
            // задать месторасположение textBox в grid
            butt.SetValue(Grid.RowProperty, y);
            butt.SetValue(Grid.ColumnProperty, x);
            grid.Children.Add(butt);
        }


    }
}
