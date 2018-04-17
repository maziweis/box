using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzBox.ClearData
{
    class Program
    {
        static void Main(string[] args)
        {
            //string name = "FzBox.ClearData";
            //if (GetPidByProcessName(name).Count > 1)
            //    Console.WriteLine("正在运行中");
            //else
            //    Console.WriteLine("指定程序未运行");

            //Console.ReadKey();
            try
            {
                string _fileurl = System.Configuration.ConfigurationManager.AppSettings["webpath"].ToString() + "/Resources/DecFile/";
                if (Directory.Exists(_fileurl))
                {
                    try
                    {
                        DirectoryInfo dir = new DirectoryInfo(_fileurl);
                        FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                        foreach (FileSystemInfo i in fileinfo)
                        {
                            if (i is DirectoryInfo)            //判断是否文件夹
                            {
                                DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                                subdir.Delete(true);          //删除子目录和文件
                            }
                            else
                            {
                                File.Delete(i.FullName);      //删除指定文件
                            }
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

                // throw;
            }
        }

        public static List<int> GetPidByProcessName(string processName)
        {
            List<int> _list = new List<int>();

            Process[] arrayProcess = Process.GetProcessesByName(processName);
            foreach (Process p in arrayProcess)
            { _list.Add(p.Id); }
            return _list;
        }
    }

}
