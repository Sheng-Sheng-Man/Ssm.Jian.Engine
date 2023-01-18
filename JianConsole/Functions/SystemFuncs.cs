using Egg.Lark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianConsole.Functions
{
    /// <summary>
    /// 系统函数
    /// </summary>
    public class SystemFuncs
    {
        [Func("大于")]
        public bool GreaterThan(double value1, double value2)
        {
            return value1 > value2;
        }

        [Func("大于等于")]
        public bool GreaterThanOrEqual(double value1, double value2)
        {
            return value1 > value2;
        }

        [Func("小于")]
        public bool LessThan(double value1, double value2)
        {
            return value1 < value2;
        }

        [Func("小于等于")]
        public bool LessThanOrEqual(double value1, double value2)
        {
            return value1 <= value2;
        }
    }
}
