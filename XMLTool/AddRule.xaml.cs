using System;
using System.Collections.Generic;
using System.Configuration;
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
using XMLTool.Helper;

namespace XMLTool
{
    /// <summary>
    /// AddRule.xaml 的交互逻辑
    /// </summary>
    public partial class AddRule : Window
    {
        private MainWindow Owner = null;

        /// <summary>
        /// 表示用户是否输入过：0表示用户没有输入过，2表示用户输入，1表示是系统更新的输入
        /// </summary>
        private int IsUserEdit = 0;

        public AddRule(MainWindow Owner, int openNewWindowCount)
        {
            InitializeComponent();
            this.Owner = Owner;
            if (openNewWindowCount > 0)
            {

                this.oldXPath.Text = Config.Load()["LastOldXPath"];
                this.newXPath.Text = Config.Load()["LastNewXPath"];
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateDescription()
        {
            if (this.TextDescription != null)
            {
                this.TextDescription.Content = string.Format("从{0}{1}数据到{2}", this.newXPath.Text.Trim(), (this.SelectRule.SelectedItem as ComboBoxItem).Content.ToString(), this.oldXPath.Text.Trim());
            }
        }

        private void newXPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsUserEdit == 1 || this.newXPath.Text.Trim() == string.Empty)
            {
                IsUserEdit = 0;
            }
            else
                IsUserEdit = 2;
            UpdateDescription();
        }

        private void oldXPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsUserEdit != 2)
            {
                IsUserEdit = 1;
                this.newXPath.Text = this.oldXPath.Text.Trim();
            }
            UpdateDescription();
        }

        private void SelectRule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDescription();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.oldXPath.Text) || string.IsNullOrWhiteSpace(this.newXPath.Text))
            {
                this.TextDescription.Content = "请填写正确的xPath";
            }
            else
            {
                this.Owner.UpdateListItem(new Rule
                {
                    NewXPath = this.newXPath.Text,
                    OldXPath = this.oldXPath.Text,
                    RuleType = (RuleType)Enum.Parse(typeof(RuleType), this.SelectRule.SelectedIndex.ToString())
                });
                this.Close();
            }
        }

        private void oldXPath_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            oldXPath.SelectAll();
        }

        private void newXPath_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            newXPath.SelectAll();
        }
    }
}
