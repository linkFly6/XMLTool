using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XMLTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void oldFileBtn_Click(object sender, RoutedEventArgs e)
        {
            //创建一个打开文件式的对话框  
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"C:\Users\jianglai\Desktop";
            ofd.Filter = "XML文件|*.xml";
            if (ofd.ShowDialog() == true)
            {
                oldFilePath.Text = ofd.FileName;
            }
        }

        private void newFileBtn_Click(object sender, RoutedEventArgs e)
        {
            //创建一个打开文件式的对话框  
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"C:\Users\jianglai\Desktop";
            ofd.Filter = "XML文件|*.xml";
            if (ofd.ShowDialog() == true)
            {
                newFilePath.Text = ofd.FileName;
            }
            XMLOprater.XMLReplace(oldFilePath.Text, newFilePath.Text);
        }
    }
}
