using Egg.Lark;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ssm.Jian.Engine
{
    /// <summary>
    /// 脚本函数集合
    /// </summary>
    public class ScriptFunctions : Egg.Lark.ScriptFunctions
    {
        // 执行对象
        private void FuncInvoke(ScriptEngine engine, object? func)
        {
            // 为空跳出
            if (func is null) return;
            // 为函数则执行
            if (func is ScriptFunction)
            {
                ((ScriptFunction)func).Execute(engine);
                return;
            }
            // 抛出异常
            throw new ScriptException($"不可执行的类型'{func.GetType().FullName}'。");
        }

        public ScriptFunctions() {
            base.Reg("依次执行", args => {
                
            });
        }
    }
}
