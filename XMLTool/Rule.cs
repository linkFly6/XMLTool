using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLTool
{
    public class Rule
    {

        /// <summary>
        /// 要操作的旧XML的XPath
        /// </summary>
        public string OldXPath { get; set; }

        /// <summary>
        /// 要操作的新XML的XPath
        /// </summary>
        public string NewXPath { get; set; }


        /// <summary>
        /// 该规则类型
        /// </summary>
        public RuleType RuleType { get; set; }


        /// <summary>
        /// 数据状态（0：未处理，1：异常，2：已经正确处理 3：警告）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 重载Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(Rule obj)
        {
            if (obj == null) return false;
            return obj.OldXPath == this.OldXPath && obj.RuleType == this.RuleType && obj.NewXPath == this.NewXPath;
        }
    }
}
