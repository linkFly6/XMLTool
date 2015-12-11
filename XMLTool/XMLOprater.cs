using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLTool
{
    public class XMLOprater
    {
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
