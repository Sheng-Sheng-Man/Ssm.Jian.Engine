using Egg.Lark;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ssm.Jian.Engine
{
    /// <summary>
    /// 声声慢·简脚本语言引擎
    /// </summary>
    public class ScriptEngine : Egg.Lark.ScriptEngine
    {
        /// <summary>
        /// 声声慢·简脚本语言引擎
        /// </summary>
        /// <param name="script"></param>
        /// <param name="funcs"></param>
        public ScriptEngine(string script, ScriptFunctions funcs) : base(script, funcs)
        {

        }
    }
}
