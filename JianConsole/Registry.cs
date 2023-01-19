using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Egg;

namespace JianConsole
{

    /// <summary>
    /// 注册表操作类
    /// </summary>
    public static class Registry
    {

        /// <summary>
        /// 注册文件关联
        /// </summary>
        /// <param name="ext"></param>
        /// <param name="name"></param>
        /// <param name="decription"></param>
        /// <param name="command"></param>
        /// <param name="iconPath"></param>
        /// <exception cref="Exception"></exception>
        public static void RegisterFileAssociations(string ext, string name, string decription, string command, string iconPath)
        {
            // 注册软件路径
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Windows
                var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);
                var isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
                if (isElevated)
                {
                    // 建立关联产品
                    var keyProduct = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(name, true);
                    if (keyProduct is null) keyProduct = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(name);
                    keyProduct.SetValue("", decription);
                    // 建立关联图标
                    var keyProductDefaultIcon = keyProduct.OpenSubKey("DefaultIcon", true);
                    if (keyProductDefaultIcon is null) keyProductDefaultIcon = keyProduct.CreateSubKey("DefaultIcon");
                    keyProductDefaultIcon.SetValue("", $"\"{iconPath}\"");
                    // 建立shell
                    var keyProductShell = keyProduct.OpenSubKey("shell", true);
                    if (keyProductShell is null) keyProductShell = keyProduct.CreateSubKey("shell");
                    // 建立open
                    var keyProductShellOpen = keyProductShell.OpenSubKey("open", true);
                    if (keyProductShellOpen is null) keyProductShellOpen = keyProductShell.CreateSubKey("open");
                    // 建立command
                    var keyProductShellOpenCommand = keyProductShellOpen.OpenSubKey("command", true);
                    if (keyProductShellOpenCommand is null) keyProductShellOpenCommand = keyProductShellOpen.CreateSubKey("command");
                    keyProductShellOpenCommand.SetValue("", command);
                    // 建立关联扩展名
                    var keyFile = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext, true);
                    if (keyFile is null) keyFile = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(ext);
                    // 建立关联扩展名打开方式
                    var keyFileOpenWithProgids = keyFile.OpenSubKey("OpenWithProgids", true);
                    if (keyFileOpenWithProgids is null) keyFileOpenWithProgids = keyFile.CreateSubKey("OpenWithProgids");
                    keyFileOpenWithProgids.SetValue(name, "");
                    Console.WriteLine("注册成功!");
                }
                else
                {
                    throw new Exception("权限不足，请使用管理员权限运行");
                }
            }
            else
            {
                throw new Exception($"不支持的操作系统'{RuntimeInformation.OSDescription}'");
            }
        }

        /// <summary>
        /// 注册文件关联
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cmdName"></param>
        /// <param name="cmdContent"></param>
        /// <exception cref="Exception"></exception>
        public static void RegisterFileAssociations(string name, string cmdName, string cmdContent)
        {
            // 注册软件路径
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Windows
                var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);
                var isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
                if (isElevated)
                {
                    // 建立关联产品
                    var keyProduct = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(name, true);
                    if (keyProduct is null) keyProduct = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(name);
                    // 建立关联图标
                    var keyProductDefaultIcon = keyProduct.OpenSubKey("DefaultIcon", true);
                    if (keyProductDefaultIcon is null) keyProductDefaultIcon = keyProduct.CreateSubKey("DefaultIcon");
                    // 建立shell
                    var keyProductShell = keyProduct.OpenSubKey("shell", true);
                    if (keyProductShell is null) keyProductShell = keyProduct.CreateSubKey("shell");
                    // 建立open
                    var keyProductShellOpen = keyProductShell.OpenSubKey("open", true);
                    if (keyProductShellOpen is null) keyProductShellOpen = keyProductShell.CreateSubKey("open");
                    // 建立右键自定义名称
                    var keyProductShellDebug = keyProductShell.OpenSubKey(cmdName, true);
                    if (keyProductShellDebug is null) keyProductShellDebug = keyProductShell.CreateSubKey(cmdName);
                    // 建立command
                    var keyProductShellDebugCommand = keyProductShellDebug.OpenSubKey("command", true);
                    if (keyProductShellDebugCommand is null) keyProductShellDebugCommand = keyProductShellDebug.CreateSubKey("command");
                    keyProductShellDebugCommand.SetValue("", cmdContent);
                    Console.WriteLine("注册成功!");
                }
                else
                {
                    throw new Exception("权限不足，请使用管理员权限运行");
                }
            }
            else
            {
                throw new Exception($"不支持的操作系统'{RuntimeInformation.OSDescription}'");
            }
        }

    }
}
