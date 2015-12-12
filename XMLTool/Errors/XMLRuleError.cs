using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLTool.Errors
{
    /// <summary>
    /// XML规则异常
    /// </summary>
    public class XMLRuleError : Exception
    {
        public Rule Rule { get; set; }

        public XMLRuleError(Rule rule, Exception innerException)
            : base("XML规则异常", innerException)
        {
            this.Rule = rule;
        }
    }
}
