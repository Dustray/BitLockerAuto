using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitlockerCore
{
    internal sealed class CMDUtils : IDisposable
    {
        System.Diagnostics.Process process;

        public CMDUtils(bool noWindow = false)
        {
            process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            process.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            process.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            process.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            process.StartInfo.CreateNoWindow = noWindow;//不显示程序窗口
            process.Start();//启动程序
        }
        public bool Send(string command)
        {
            Console.WriteLine("CMD输入> " + command);
            process.StandardInput.WriteLine(command + "&exit");
            process.StandardInput.AutoFlush = true;
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            Console.WriteLine("CMD输出> " + output +"\n"+ error);
            process.WaitForExit();
            if (string.IsNullOrEmpty(error))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public  string[] RunCmd(string cmd, string args, string workdir = null)
        {
            var res = new string[2];
            var p = CreateCmdProcess(cmd, args, workdir);
            res[0] = p.StandardOutput.ReadToEnd();
            res[1] = p.StandardError.ReadToEnd();
            p.Close();
            return res;
        }

        public  System.Diagnostics.Process CreateCmdProcess(string cmd, string args, string workdir = null)
        {
            var pStartInfo = new System.Diagnostics.ProcessStartInfo(cmd);
            pStartInfo.Arguments = args;
            pStartInfo.CreateNoWindow = false;
            pStartInfo.UseShellExecute = false;
            pStartInfo.RedirectStandardError = true;
            pStartInfo.RedirectStandardInput = true;
            pStartInfo.RedirectStandardOutput = true;
            pStartInfo.StandardErrorEncoding = System.Text.UTF8Encoding.UTF8;
            pStartInfo.StandardOutputEncoding = System.Text.UTF8Encoding.UTF8;
            if (!string.IsNullOrEmpty(workdir))
                pStartInfo.WorkingDirectory = workdir;
            return System.Diagnostics.Process.Start(pStartInfo);
        }

        public void Dispose()
        {
            if (process != null)
            {
                process.WaitForExit();//等待程序执行完退出进程
                process.Close();
                process.Dispose();
                process = null;
            }
        }
    }
}
