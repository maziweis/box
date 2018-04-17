using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzBox.OperationLog
{
    public class LogHelper
    {
        /// <summary>
        /// 记录上传操作日志错误
        /// </summary>
        /// <param name="msg"></param>
        public static void UpLogError(string msg)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\UpLogError.txt";//该日志文件会存在windows服务程序目录下
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                FileStream fs;
                fs = File.Create(path);
                fs.Close();
            }

            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(DateTime.Now.ToString() + "|" + msg);
                }
            }
        }

        /// <summary>
        /// 自动更新错误日志
        /// </summary>
        /// <param name="msg"></param>
        public static void UpdateError(string msg)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\UpLogError.txt";//该日志文件会存在windows服务程序目录下
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                FileStream fs;
                fs = File.Create(path);
                fs.Close();
            }

            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(DateTime.Now.ToString() + "|" + msg);
                }
            }
        }

        /// <summary>
        /// 自动更新详细情况日志
        /// </summary>
        /// <param name="msg"></param>
        public static void UpdateInfo(string msg)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\UpdateInfo.txt";//该日志文件会存在windows服务程序目录下
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                FileStream fs;
                fs = File.Create(path);
                fs.Close();
            }

            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(msg);
                    sw.WriteLine("");
                }
            }
        }
    }
}
