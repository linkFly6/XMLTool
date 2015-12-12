using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using XMLTool.Errors;

namespace XMLTool
{
    /// <summary>
    /// XML加工类
    /// </summary>
    public class XMLOprater
    {
        public XmlDocument Document { get; set; }

        private string FileName { get; set; }


        public XMLOprater(string filePath)
        {
            try
            {
                this.Document = new XmlDocument();
                this.FileName = Path.GetFileName(filePath);
                Document.Load(filePath);
            }
            catch (Exception e)
            {
                //抛出异常
                throw new XMLLoadError(filePath, e);
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="dirctory"></param>
        public void Save(string dirctory)
        {
            string filePath = string.Format("{0}\\{1}", dirctory, this.FileName);
            //if (File.Exists(filePath))
            //{
            //    filePath = string.Format("{0}\\{1}{2}", dirctory, DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss"), this.FileName);
            //}
            this.Document.Save(filePath);
        }


        /// <summary>
        /// 根据处理规则进行加工
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="newXMLOprater"></param>
        /// <returns></returns>
        public XMLOprater XMLReplace(Rule rule, XMLOprater newXMLOprater)
        {
            return this.XMLReplace(rule, newXMLOprater.Document);
        }


        /// <summary>
        /// 根据处理规则进行加工
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="newXMLDocument"></param>
        /// <returns></returns>
        public XMLOprater XMLReplace(Rule rule, XmlDocument newXMLDocument)
        {
            //目前仅支持简单的XML替换
            XmlNode newNode = newXMLDocument.SelectSingleNode(rule.NewXPath);
            XmlNodeList oldNodeList = this.Document.SelectNodes(rule.OldXPath);
            if (oldNodeList == null || oldNodeList.Count == 0)
            {
                throw new XMLNodeError(rule.OldXPath);
            }
            if (newNode == null || string.IsNullOrWhiteSpace(newNode.InnerXml))
            {
                throw new XMLNodeWarning(rule.NewXPath, "新XML的xPath没有读取到数据");
            }
            switch (rule.RuleType)
            {
                case RuleType.Fugai:
                    foreach (XmlNode item in oldNodeList)
                    {
                        item.InnerXml = newNode.InnerXml;
                    }
                    break;
                case RuleType.Zhuijia:
                    foreach (XmlNode item in oldNodeList)
                    {
                        item.InnerXml += newNode.InnerXml;
                    }
                    /*
                     * 后面要尝试对比追加（就是合并）
                     * 条件追加（判定不同的条件进行追加），在过滤规则那个窗口，可以指定相应的属性、文本
                     */
                    break;
            }
            return this;
        }



        public static void XMLReplace(string oldFilePath, string newFilePath)
        {
            XmlDocument oldXML = new XmlDocument();
            oldXML.Load(oldFilePath);

            XmlDocument newXML = new XmlDocument();
            newXML.Load(newFilePath);

            string newXmlContent = newXML.SelectSingleNode("/DOCUMENT/holidays").InnerXml;

            foreach (XmlNode holidays in oldXML.SelectNodes("//display/holidays"))
            {
                holidays.InnerXml = newXmlContent;
            }
            oldXML.Save(@"C:\Users\jianglai\Desktop\f--k\susuNewXML.xml");
        }

    }
}
