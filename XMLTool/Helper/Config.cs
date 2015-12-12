using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLTool.Helper
{
    public class Config
    {
        /// <summary>
        /// 配置文件存放目录，在我的文档
        /// </summary>
        private static readonly string CONFIGFOLDER = string.Format("{0}\\XMLTool", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

        /// <summary>
        /// 配置文件名称
        /// </summary>
        private const string CONFIGFILENAME = "appConfig.xml";

        /// <summary>
        /// 完整文件路径
        /// </summary>
        private static readonly string CONFIGFULLPATH = string.Format("{0}\\{1}", CONFIGFOLDER, CONFIGFILENAME);

        /// <summary>
        /// 默认xml模版
        /// </summary>
        private const string DEFAULTCONFIGXML = @"<?xml version='1.0' encoding='utf-8'?>
<configuration>
  <LastSaveFolderPath/>
  <NewFileSelectFolderPath/>
  <LastNewXPath/>
  <OldFileSelectFolderPath/>
  <LastOldXPath/>
</configuration>";

        private static Config config;


        private Config()
        {
            this.Document = new XmlDocument();
            if (!File.Exists(CONFIGFULLPATH))
            {
                CheckOrCreate();
                this.Document.LoadXml(DEFAULTCONFIGXML);
            }
            else
                this.Document.Load(CONFIGFULLPATH);
        }

        private XmlDocument Document { get; set; }



        public static Config Load()
        {
            return config ?? new Config();
        }

        /// <summary>
        /// 检测配置文件是否存在，不存在则创建
        /// </summary>
        private Config CheckOrCreate()
        {
            if (!File.Exists(CONFIGFILENAME))
            {
                //没有目录则创建目录
                if (!Directory.Exists(CONFIGFOLDER))
                {
                    Directory.CreateDirectory(CONFIGFOLDER);
                }
                using (File.Create(CONFIGFULLPATH))
                {

                }
            }
            return this;
        }


        /// <summary>
        /// 获取和设置配置项
        /// </summary>
        /// <param name="name">参阅ConfigEnum中的属性</param>
        /// <returns>返回相应配置的属性值</returns>
        public string this[string name]
        {
            get
            {
                XmlNode node = this.Document.SelectSingleNode("/configuration/" + name);
                return node == null ? string.Empty : node.InnerText;
            }
            set
            {
                XmlNode node = this.Document.SelectSingleNode("/configuration/" + name);
                if (node == null)
                {
                    node = this.Document.CreateElement(name);
                    this.Document.FirstChild.AppendChild(node);
                }
                node.InnerText = value;
                CheckOrCreate();
                this.Document.Save(CONFIGFULLPATH);
            }
        }

    }
}
