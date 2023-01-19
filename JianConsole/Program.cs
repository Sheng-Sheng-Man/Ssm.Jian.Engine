using Ssm.Jian.Engine;
using Egg.Lark;
using JianConsole.Functions;
using System.Runtime.InteropServices;
using System.Security.Principal;
using JianConsole;

// 注册模式
if (args.Length <= 0)
{
    Console.Title = $"{egg.Assembly.Name} Ver:{egg.Assembly.Version} - 注册模式";
    // 注册软件路径
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        System.Console.WriteLine("[Windows注册程序]");
        System.Console.WriteLine();
        // Windows
        var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        var isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
        if (isElevated)
        {
            System.Console.Write("正在 注册 ssm 关联文件 ... ");
            Registry.RegisterFileAssociations(".j", "Jian.Script", "ShengShengMan Chinese Language Programming Script", $"\"{egg.Assembly.ExecutionFilePath}\" \"%1\"", $"{egg.IO.GetExecutionPath("script.ico")}");
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine($"使用管理员身份运行可进行Windows文件关联注册！");
            Console.WriteLine();
        }
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
        Console.WriteLine("[Linux安装说明]");
        Console.WriteLine();
        Console.WriteLine($"使用命令行执行'{egg.IO.GetExecutionPath("install.sh")}'可进行进行安装。");
        Console.WriteLine();
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    {
        Console.WriteLine("[MacOS安装说明]");
        Console.WriteLine();
        Console.WriteLine($"使用命令行执行'{egg.IO.GetExecutionPath("install.sh")}'可进行进行安装。");
        Console.WriteLine();
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"不支持的操作系统'{RuntimeInformation.OSDescription}'");
        Console.ForegroundColor = ConsoleColor.DarkGreen;
    }
    Console.ReadKey();
    return;
}

// 脚本模式
string path = args[0];
Console.Title = $"{egg.Assembly.Name} Ver:{egg.Assembly.Version} - {path}";
string script = egg.IO.ReadUtf8FileContent(path);
//System.Console.WriteLine(script);
ScriptParser.ScriptCalculateNames.Add("计算");
var func = ScriptParser.Parse(script);
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
