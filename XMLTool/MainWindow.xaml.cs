using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XMLTool.Errors;
using XMLTool.Helper;

namespace XMLTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// 后续根据需求增加逻辑：
    /// 1、支持复制粘贴XML
    /// 2、支持条件处理
    /// 3、支持远程抓取
    /// </summary>
    public partial class MainWindow : Window
    {

        #region 属性
        private int OpenNewWindowCount = 0;

        /// <summary>
        /// 当前程序运行目录
        /// </summary>
        private string CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 全局配置
        /// </summary>
        Config AppConfig = Config.Load();

        /// <summary>
        /// 日志对象
        /// </summary>
        public Logger logger = new Logger(string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, "xmllogs.txt"));


        /// <summary>
        /// 旧XML
        /// </summary>
        public XMLOprater oldXML { get; set; }

        /// <summary>
        /// 新XML
        /// </summary>
        public XMLOprater newXML { get; set; }

        /// <summary>
        /// 规则集合
        /// </summary>
        private ObservableCollection<Rule> rules = new ObservableCollection<Rule>();

        /// <summary>
        /// 错误消息集合
        /// </summary>
        private IList<string> errorMsgs = new List<string>();
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            this.listViewRules.ItemsSource = rules;
        }


        /// <summary>
        /// 更新一个规则到规则集合中
        /// </summary>
        /// <param name="rule"></param>
        public void UpdateListItem(Rule rule)
        {

            foreach (Rule item in rules)
            {
                //Rule重载了Equals
                if (item.Equals(rule)) return;
            }
            this.AppConfig["LastNewXPath"] = rule.NewXPath;
            this.AppConfig["LastOldXPath"] = rule.OldXPath;
            rules.Add(rule);
            errorMsgs.Add(string.Empty);


            //ListViewItem listviewitem = new ListViewItem();
            //listviewitem.DataContext = rule;
            //listviewitem.Content = string.Format("从{0}{1}数据到{2}", rule.NewXPath, rule.RuleType.Description(), rule.OldXPath);
            //listViewRules.Items.Add(listviewitem);
            //
        }


        /// <summary>
        /// 清空警告
        /// </summary>
        private void ClearWarning()
        {
            this.Description.Content = string.Empty;
        }

        /// <summary>
        /// 显示警告
        /// </summary>
        /// <param name="msg"></param>
        private void ShowWarning(string msg)
        {
            this.Description.Content = msg;
        }


        /// <summary>
        /// 创建一个选择XML的对话框
        /// </summary>
        /// <returns></returns>
        private Microsoft.Win32.OpenFileDialog ShowXMLDialog(string directory = null)
        {
            //创建一个打开文件式的对话框  
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.InitialDirectory = string.IsNullOrWhiteSpace(directory) ? directory : Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            ofd.Filter = "XML文件|*.xml";
            return ofd;
        }

        private void oldFileBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = ShowXMLDialog(this.AppConfig["OldFileSelectFolderPath"]);
            if (dialog.ShowDialog() == true)
            {
                oldFilePath.Text = dialog.FileName;
                this.AppConfig["OldFileSelectFolderPath"] = System.IO.Path.GetDirectoryName(dialog.FileName);
                ClearWarning();
            }
            else return;
            try
            {
                oldXML = new XMLOprater(dialog.FileName);
            }
            catch (XMLLoadError error)
            {
                logger.Write(string.Format("加载旧XML异常，文件路径：{0}", error.FilePath), error);
                ShowWarning("加载旧XML异常，详细信息请检查日志");
                return;
            }
            catch (Exception error)
            {
                ShowWarning("加载旧XML异常，详细信息请检查日志");
                logger.Write("程序进程异常", error);
            }
        }

        private void newFileBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = ShowXMLDialog(this.AppConfig["NewFileSelectFolderPath"]);
            if (dialog.ShowDialog() == true)
            {
                newFilePath.Text = dialog.FileName;
                this.AppConfig["NewFileSelectFolderPath"] = System.IO.Path.GetDirectoryName(dialog.FileName);
                ClearWarning();
            }
            else return;
            try
            {
                newXML = new XMLOprater(dialog.FileName);
            }
            catch (XMLLoadError error)
            {
                logger.Write(string.Format("加载新XML异常，文件路径：{0}", error.FilePath), error);
                ShowWarning("加载新XML异常，详细信息请检查日志");
            }
            catch (Exception error)
            {
                ShowWarning("加载新XML异常，详细信息请检查日志");
                logger.Write("程序进程异常", error);
            }
        }

        private void addRule_Click(object sender, RoutedEventArgs e)
        {
            AddRule addRuleWindow = new AddRule(this, OpenNewWindowCount++);
            addRuleWindow.ShowDialog();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (rules.Count == 0)
            {
                ShowWarning("请添加规则！");
                return;
            }
            if (oldXML == null)
            {
                ShowWarning("请选择要处理的旧XML");
                return;
            }
            if (newXML == null)
            {
                ShowWarning("请选择要处理的新XML");
                return;
            }
            for (int i = 0; i < rules.Count; i++)
            {
                Rule rule = rules[i];
                try
                {
                    oldXML.XMLReplace(rule, newXML);
                    FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                    folderBrowserDialog.Description = "请选择文件保存的文件夹";
                    folderBrowserDialog.ShowNewFolderButton = true;
                    folderBrowserDialog.SelectedPath = this.AppConfig["LastSaveFolderPath"];
                    //folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;//Environment.SpecialFolder.Personal

                    //保存路径
                    if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        oldXML.Save(folderBrowserDialog.SelectedPath);
                        //保存到配置
                        this.AppConfig["LastSaveFolderPath"] = folderBrowserDialog.SelectedPath;
                        ShowWarning("保存成功");
                    }
                }
                catch (XMLNodeError error)
                {
                    errorMsgs[i] = string.Format("XMLNodeError - XML数据读取异常\nXPath：{0}\n错误信息：{1}", error.XPath, error.Message);
                    logger.Write(string.Format("XMLNodeError：\nXPath：{0}", error.XPath), error);
                    this.rules[i].Status = 1;
                    //(this.listViewRules.Items[index] as Rule).Status = 1;

                    //因为Rule对象没有实现双向binding的接口，所以这里要手动刷新
                    this.listViewRules.Items.Refresh();
                    continue;
                }
                catch (XMLNodeWarning error)
                {
                    errorMsgs[i] = string.Format("XMLNodeWarning：\nXPath：{0}\n错误信息：{1}", error.XPath, error.Message);
                    this.rules[i].Status = 3;
                    this.listViewRules.Items.Refresh();
                    continue;
                }
                catch (Exception error)
                {
                    logger.Write("XML数据读取异常", error);
                    errorMsgs[i] = string.Format("Exception - XML处理异常\n错误信息：{0}", error.Message);
                    this.rules[i].Status = 1;
                    this.listViewRules.Items.Refresh();
                }
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            logger.Dispose();
        }

        private void listViewRules_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Rule selectItem = this.listViewRules.SelectedItem as Rule;
            if (selectItem.Status == 1 || selectItem.Status == 3)
            {
                System.Windows.MessageBox.Show(errorMsgs[this.listViewRules.SelectedIndex], "详情", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void clearRules_Click(object sender, RoutedEventArgs e)
        {
            rules.Clear();
            errorMsgs.Clear();
            this.listViewRules.Items.Refresh();
        }

        private void oldFilePath_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            oldFileBtn_Click(sender, e);
        }

        private void newFilePath_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            newFileBtn_Click(sender, e);
        }
    }
}
