using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLTool.Errors
{
    /// <summary>
    /// XML报警
    /// </summary>
    public class XMLNodeWarning : Exception
    {
        public string XPath { get; set; }
        public XMLNodeWarning(string xPath, string msg)
            : base(msg)
        {
            this.XPath = xPath;
        }


        public XMLNodeWarning(string msg)
            : base(msg)
        {

        }
    }
}
