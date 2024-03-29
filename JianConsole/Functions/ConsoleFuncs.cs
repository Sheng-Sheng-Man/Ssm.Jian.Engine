﻿using Egg.Lark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianConsole.Functions
{
    /// <summary>
    /// 控制台函数
    /// </summary>
    public class ConsoleFuncs : ScriptFunctionRegistrBase
    {
        [Func("输出")]
        public void Write(string content)
        {
            Console.Write(content);
        }

        [Func("输出换行")]
        public void WriteLine(string content)
        {
            Console.WriteLine(content);
        }

        [Func("等待输入")]
        public double ReadKey()
        {
            return (int)Console.ReadKey().Key;
        }
    }
}
