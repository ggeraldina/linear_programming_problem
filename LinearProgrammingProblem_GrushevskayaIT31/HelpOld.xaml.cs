using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace LinearProgrammingProblem_GrushevskayaIT31
{
    /// <summary>
    /// Логика взаимодействия для HelpOld.xaml
    /// </summary>
    public partial class HelpOld : Window
    {
        public HelpOld()
        {
            InitializeComponent();
            this.textHelp.Text = "";
            try
            {
                string fileWithRulesGame = (new Uri(Directory.GetCurrentDirectory() + "/help.txt")).OriginalString;
                Encoding enc = Encoding.GetEncoding(1251);
                string[] linesRulesGame = File.ReadAllLines(fileWithRulesGame, enc);
                foreach (string lrg in linesRulesGame)
                {
                    this.textHelp.Text += lrg;
                    this.textHelp.Text += "\n";
                }
            }
            catch (IOException ioex)
            {
                Console.WriteLine(ioex.Message);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        
    }
}
