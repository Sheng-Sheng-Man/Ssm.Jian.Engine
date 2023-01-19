using Egg.Lark;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Ssm.Jian.Engine
{
    /// <summary>
    /// 脚本函数集合
    /// </summary>
    public class ScriptFunctions : Egg.Lark.ScriptFunctions
    {
        public ScriptFunctions()
        {
            base.Reg("依次执行", args =>
            {
                for (int i = 0; i < args.Count; i++)
                {
                    FuncInvoke(this.Engine, args[i]);
                }
            });
            base.Reg("计算", args =>
            {
                if (args.Count < 1) throw new ScriptException($"函数'计算'缺少必要参数");
                return GetValue(this.Engine, args[0]);
            });
            base.Reg("加", args =>
            {
                if (args.Count < 2) throw new ScriptException($"函数'加'缺少必要参数");
                double dblValue = GetValue<double>(this.Engine, args[0]);
                for (int i = 1; i < args.Count; i++)
                {
                    dblValue += GetValue<double>(this.Engine, args[i]);
                }
                return dblValue;
            });
            base.Reg("减", args =>
            {
                if (args.Count < 2) throw new ScriptException($"函数'减'缺少必要参数");
                double dblValue = GetValue<double>(this.Engine, args[0]);
                for (int i = 1; i < args.Count; i++)
                {
                    dblValue -= GetValue<double>(this.Engine, args[i]);
                }
                return dblValue;
            });
            base.Reg("乘", args =>
            {
                if (args.Count < 2) throw new ScriptException($"函数'乘'缺少必要参数");
                double dblValue = GetValue<double>(this.Engine, args[0]);
                for (int i = 1; i < args.Count; i++)
                {
                    dblValue *= GetValue<double>(this.Engine, args[i]);
                }
                return dblValue;
            });
            base.Reg("除", args =>
            {
                if (args.Count < 2) throw new ScriptException($"函数'除'缺少必要参数");
                double dblValue = GetValue<double>(this.Engine, args[0]);
                for (int i = 1; i < args.Count; i++)
                {
                    dblValue /= GetValue<double>(this.Engine, args[i]);
                }
                return dblValue;
            });
            base.Reg("字符串", args =>
            {
                if (args.Count < 1) throw new ScriptException($"函数'字符串'缺少必要参数");
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < args.Count; i++)
                {
                    var value = GetValue(this.Engine, args[i]);
                    if (value != null) sb.Append(value);
                }
                return sb.ToString();
            });
            base.Reg("赋值", args =>
            {
                if (args.Count < 2) throw new ScriptException($"函数'赋值'缺少必要参数");
                var letVariable = args[0];
                string letVariableName;
                if (letVariable is ScriptVariable)
                {
                    letVariableName = ((ScriptVariable)letVariable).Name;
                }
                else throw new ScriptException($"函数'赋值'的首参数必须是变量");
                // 设置变量
                this.Engine.Memory.Set(letVariableName, GetValue(this.Engine, args[1]));
            });
            // 判断语句
            base.Reg("判断", args =>
            {
                if (args.Count < 1) throw new ScriptException($"函数'判断'缺少必要参数");
                var ifJudge = GetValue<bool>(this.Engine, args[0]);
                if (ifJudge)
                {
                    if (args.Count >= 2) FuncInvoke(this.Engine, args[1]);
                }
                else
                {
                    if (args.Count >= 3) FuncInvoke(this.Engine, args[2]);
                }
            });
            // 判断循环
            base.Reg("判断循环", args =>
            {
                if (args.Count < 1) throw new ScriptException($"函数'判断循环'缺少必要参数");
                while (GetValue<bool>(this.Engine, args[0]))
                {
                    if (args.Count >= 2) FuncInvoke(this.Engine, args[1]);
                }
            });
            // 规律循环
            base.Reg("规律循环", args =>
            {
                if (args.Count < 4) throw new ScriptException($"函数'规律循环'缺少必要参数");
                var forVariable = args[0];
                string forVariableName;
                if (forVariable is ScriptVariable)
                {
                    forVariableName = ((ScriptVariable)forVariable).Name;
                    if (!this.Engine.Memory.ContainsKey(forVariableName)) this.Engine.Memory.Set(forVariableName, null);
                }
                else throw new ScriptException($"函数'规律循环'的首参数必须是变量");
                var forStart = GetValue<double>(this.Engine, args[1]);
                var forEnd = GetValue<double>(this.Engine, args[2]);
                var forStep = GetValue<double>(this.Engine, args[3]);
                object? forFunc = null;
                if (args.Count > 4) forFunc = args[4];
                for (double dblValue = forStart; dblValue <= forEnd; dblValue += forStep)
                {
                    // 设置循环变量
                    this.Engine.Memory[forVariableName] = dblValue;
                    if (forFunc != null) FuncInvoke(this.Engine, forFunc);
                }
            });
            // 依次循环
            base.Reg("依次循环", args =>
            {
                if (args.Count < 2) throw new ScriptException($"函数'依次循环'缺少必要参数");
                var forVariable = args[0];
                string forVariableName;
                if (forVariable is ScriptVariable)
                {
                    forVariableName = ((ScriptVariable)forVariable).Name;
                    if (!this.Engine.Memory.ContainsKey(forVariableName)) this.Engine.Memory.Set(forVariableName, null);
                }
                else throw new ScriptException($"函数'依次循环'的首参数必须是变量");
                var forList = GetValue(this.Engine, args[1]);
                if (forList is null) throw new ScriptException($"函数'依次循环'缺少列表对象");
                var listType = forList.GetType();
                string listFullName = listType.Namespace + "." + listType.Name;
                object? forFunc = null;
                if (args.Count > 2) forFunc = args[2];
                if (ScriptMemory.IsList(listType)) // 处理列表
                {
                    var listItem = listType.GetProperty("Item");
                    var listCount = listType.GetProperty("Count");
                    int listCountValue = (int)listCount.GetValue(forList);
                    for (var i = 0; i < listCountValue; i++)
                    {
                        // 设置循环变量
                        this.Engine.Memory.Set(forVariableName, listItem.GetValue(forList, new object[] { i }));
                        if (forFunc != null) FuncInvoke(this.Engine, forFunc);
                    }
                }
                else if (ScriptMemory.IsDictionary(listType)) // 处理字典
                {
                    var objTypeEnumerator = listType.GetMethod("GetEnumerator");
                    IDictionaryEnumerator objEnumerator = (IDictionaryEnumerator)objTypeEnumerator.Invoke(forList, new object[] { });
                    while (objEnumerator.MoveNext())
                    {
                        // 设置循环变量
                        this.Engine.Memory.Set(forVariableName, objEnumerator.Current);
                        if (forFunc != null) FuncInvoke(this.Engine, forFunc);
                    }
                }
                else throw new ScriptException($"函数'依次循环'缺少可列表的对象");
            });
            // 判断相等
            base.Reg("相等", args =>
            {
                if (args.Count < 2) throw new ScriptException($"函数'相等'缺少必要参数");
                var value1 = GetValue(this.Engine, args[0]);
                var value2 = GetValue(this.Engine, args[1]);
                if (value1 is null)
                {
                    if (value2 is null) return true;
                    return false;
                }
                if (value2 is null) return false;
                // 对象直接相等
                if (value1 == value2) return true;
                // 处理数字情况
                if (isNumber(value1))
                {
                    if (isNumber(value2)) return Convert.ToDouble(value1) == Convert.ToDouble(value2);
                    return false;
                }
                if (isNumber(value2)) return false;
                // 处理字符串情况
                if (value1 is string)
                {
                    if (value2 is string) return value1.ToString() == value2.ToString();
                    return false;
                }
                return false;
            });
            // 取反
            base.Reg("取反", args =>
            {
                if (args.Count < 1) throw new ScriptException($"函数'取反'缺少必要参数");
                bool bValue = GetValue<bool>(this.Engine, args[0]);
                return !bValue;
            });
            // 比较
            base.Reg("比较", args =>
            {
                if (args.Count < 2) throw new ScriptException($"函数'比较'缺少必要参数");
                var dbValue1 = GetValue<double>(this.Engine, args[0]);
                var dbValue2 = GetValue<double>(this.Engine, args[1]);
                if (dbValue1 > dbValue2) return 1;
                if (dbValue1 < dbValue2) return -1;
                return 0;
            });
            // 同时成立
            base.Reg("同时成立", args =>
            {
                if (args.Count < 1) throw new ScriptException($"函数'同时成立'缺少必要参数");
                for (int i = 0; i < args.Count; i++)
                {
                    if (!GetValue<bool>(this.Engine, args[i])) return false;
                }
                return true;
            });
            // 单一成立
            base.Reg("单一成立", args =>
            {
                if (args.Count < 1) throw new ScriptException($"函数'单一成立'缺少必要参数");
                for (int i = 0; i < args.Count; i++)
                {
                    if (GetValue<bool>(this.Engine, args[i])) return true;
                }
                return false;
            });
        }
    }
}
