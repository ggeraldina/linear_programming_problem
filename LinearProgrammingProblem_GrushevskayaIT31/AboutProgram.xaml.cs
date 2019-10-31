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
using System.Reflection;
using System.IO;
namespace LinearProgrammingProblem_GrushevskayaIT31
{
    /// <summary>
    /// Логика взаимодействия для AboutProgram.xaml
    /// </summary>
    public partial class AboutProgram : Window
    {
        public AboutProgram()
        {
            InitializeComponent();
            Assembly app = Assembly.GetExecutingAssembly();
            AssemblyTitleAttribute title = (AssemblyTitleAttribute)app.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0];
            AssemblyProductAttribute product = (AssemblyProductAttribute)app.GetCustomAttributes(typeof(AssemblyProductAttribute), false)[0];
            AssemblyCopyrightAttribute copyright = (AssemblyCopyrightAttribute)app.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0];

            AssemblyDescriptionAttribute description = (AssemblyDescriptionAttribute)app.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0];
            Version version = app.GetName().Version;


            this.Title = String.Format("О программе \"{0}\"", title.Title);
            labelTitle.Content = title.Title;
            this.labelLogo.Background = new ImageBrush(new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/min.png")));
            this.labelLogo.Content = "";
            this.labelProductName.Content = product.Product;
            this.labelVersion.Content = String.Format("Версия {0}", version.ToString());
            this.labelCopyright.Content = copyright.Copyright.ToString();
            this.labelAuthor.Background = new ImageBrush(new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "/author/author.jpg")));
            this.labelAuthor.Content = "";
            this.Description.Text = description.Description;
        }


        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
