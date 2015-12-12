using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLTool.Errors
{
    /// <summary>
    /// XML的XPath异常
    /// </summary>
    public class XMLNodeError : Exception
    {
        public string XPath { get; set; }

        public XMLNodeError(string xPath)
            : base("原XML的XPath没有读取到数据")
        {
            this.XPath = xPath;
        }


        public XMLNodeError(string xPath, Exception innerException)
            : base("原XML的XPath没有读取到数据", innerException)
        {
            this.XPath = xPath;
        }
    }
}
