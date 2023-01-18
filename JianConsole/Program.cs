using Ssm.Jian.Engine;
using Egg.Lark;
using JianConsole.Functions;

if (args.Length < 0) throw new Exception("未找到脚本文件");
string path = args[0];
string script = egg.IO.ReadUtf8FileContent(path);
//System.Console.WriteLine(script);
var func = ScriptParser.Parse(script, "计算");
//System.Console.WriteLine(func.ToString());
using (Ssm.Jian.Engine.ScriptFunctions funcs = new())
{
    funcs.Reg<SystemFuncs>();
    funcs.Reg<ConsoleFuncs>();
    using (Egg.Lark.ScriptEngine engine = new Egg.Lark.ScriptEngine(func, funcs))
    {
        engine.Execute();
    }
}
