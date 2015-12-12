using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLTool
{
    /// <summary>
    /// 要操作的XML的规则类型
    /// </summary>
    public enum RuleType
    {
        Fugai = 0,
        Zhuijia = 1
    }

    public static class EnumCompile
    {
        /// <summary>
        /// 扩展枚举
        /// </summary>
        /// <param name="ruleType"></param>
        /// <returns></returns>
        public static string Description(this RuleType ruleType)
        {
            switch (ruleType)
            {
                case RuleType.Fugai:
                    return "覆盖";
                case RuleType.Zhuijia:
                    return "追加";
                default:
                    return string.Empty;
            }
        }
    }

}
