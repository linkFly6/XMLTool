using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLTool.Helper
{
    /// <summary>
    /// 日志类
    /// </summary>
    public class Logger : IDisposable
    {
        public string FilePath { get; set; }

        /// <summary>
        /// 文件流
        /// </summary>
        public FileStream Fs { get; set; }

        /// <summary>
        /// 写入流
        /// </summary>
        public StreamWriter Sw { get; set; }

        /// <summary>
        /// 根据文件路径创建一个日志类
        /// </summary>
        /// <param name="filePath"></param>
        public Logger(string filePath)
        {
            this.FilePath = filePath;
            this.Load();
        }

        /// <summary>
        /// 加载日志文件，如果不存在则创建
        /// </summary>
        private void Load()
        {
            this.Fs = new FileStream(this.FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            this.Sw = new StreamWriter(this.Fs, Encoding.UTF8);
        }

        public void Write(Exception e)
        {
            this.Sw.WriteLine("");
            this.Sw.WriteLine("*******************************[   {0}  ]***********************************", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            this.Sw.WriteLine("");
            this.Sw.WriteLine("异常信息：{0}", e.ToString());
            this.Sw.WriteLine("");
            this.Sw.WriteLine("*****************************************************************************************");
            this.Sw.WriteLine("");
        }


        public void Write(string msg)
        {
            this.Sw.WriteLine("");
            this.Sw.WriteLine("*******************************[   {0}  ]***********************************", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            this.Sw.WriteLine("");
            this.Sw.WriteLine(msg);
            this.Sw.WriteLine("");
            this.Sw.WriteLine("*****************************************************************************************");
            this.Sw.WriteLine("");
        }

        public void Write(string msg, Exception e)
        {
            this.Sw.WriteLine("");
            this.Sw.WriteLine("*******************************[ {0} ]***********************************", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            this.Sw.WriteLine("");
            this.Sw.WriteLine("错误消息：{0}", msg);
            this.Sw.WriteLine("");
            this.Sw.WriteLine("异常信息：{0}", e.ToString());
            this.Sw.WriteLine("");
            this.Sw.WriteLine("*****************************************************************************************");
            this.Sw.WriteLine("");
        }

        public void Dispose()
        {
            if (this.Sw != null)
                this.Sw.Close();
            if (this.Fs != null)
                Fs.Close();
        }
    }
}
