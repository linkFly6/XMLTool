using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLTool.Errors
{
    /// <summary>
    /// XML加载异常
    /// </summary>
    public class XMLLoadError : Exception
    {
        public string FilePath { get; set; }

        public XMLLoadError(string filePath, Exception innerException)
            : base("加载XML异常", innerException)
        {
            this.FilePath = filePath;
        }
    }
}
