using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FzBox.AutoUpdate
{
    public class UpdateSystemBLL
    {
        /// <summary>
        /// 检测是否需要更新，并且获取更新包
        /// </summary>
        /// <param name="mac"></param>
        /// <returns></returns>
        public static List<string> GetPackages(string mac)
        {
            List<string> path = new List<string>();
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };//创建HttpClient（注意传入HttpClientHandler）
            using (var http = new HttpClient(handler))
            {
                http.BaseAddress = new Uri(ConfigHelper.ServerHttpPath());
                var task = http.GetAsync(string.Format("/api/BoxSysUpdatePath/{0}", mac));//同步远程获取数据
                if (task.Result.IsSuccessStatusCode)
                {
                    var rep = task.Result;//在这里会等待task返回。
                    var task2 = rep.Content.ReadAsStringAsync();//读取响应内容
                    var jdata = JsonHelper.DeserializeJsonToObject<m_JsonData>(task2.Result);//在这里会等待task返回。
                    if (jdata != null && jdata.data != null)
                    {
                        JArray a = (JArray)jdata.data;
                        foreach (var obj in a)
                        {
                            path.Add((string)obj);
                        }
                    }
                }
            }
            return path;
        }

        /// <summary>
        /// 下载更新文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileName"></param>
        public static void DownLoadOneFile(string url, string fileName)
        {
            string filePath = "";
            string dir = AppDomain.CurrentDomain.BaseDirectory + "\\UpdateFiles";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            filePath = string.Format("{0}\\{1}.zip", dir, fileName);

            FileStream fstream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            WebRequest wRequest = WebRequest.Create(url);
            try
            {
                WebResponse wResponse = wRequest.GetResponse();
                int contentLength = (int)wResponse.ContentLength;

                byte[] buffer = new byte[1024];

                int read_count = 0;
                int total_read_count = 0;
                bool complete = false;

                while (!complete)
                {
                    read_count = wResponse.GetResponseStream().Read(buffer, 0, buffer.Length);
                    if (read_count > 0)
                    {
                        fstream.Write(buffer, 0, read_count);
                        total_read_count += read_count;
                        if (total_read_count <= contentLength)
                        {
                            //double c = Math.Round(Convert.ToDouble(total_read_count) / Convert.ToDouble(contentLength), 2) * 100;
                            //Console.SetCursorPosition(0, 8);
                            //Console.Write(string.Format("当前下载进度：{0}%", c));
                        }
                    }
                    else
                    {
                        complete = true;//下载完成
                    }
                }
                fstream.Flush();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                fstream.Close();
                fstream.Dispose();
                wRequest = null;
            }
        }

        /// <summary>
        /// 解压并替换
        /// </summary>
        /// <param name="fileName"></param>
        public static void UnZipAndReplace(string fileName)
        {
            string extractPath = AppDomain.CurrentDomain.BaseDirectory;//解压到的目录地址
            string zipPath = extractPath + "\\UpdateFiles\\" + fileName + ".zip";//更新包所在地址
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (!Directory.Exists(ConfigHelper.WebPath()))
                    {
                        Directory.CreateDirectory(ConfigHelper.WebPath());
                    }

                    if (entry.FullName.EndsWith("/"))
                    {
                        if (!Directory.Exists(ConfigHelper.WebPath() + "\\" + entry.FullName))
                        {
                            Directory.CreateDirectory(ConfigHelper.WebPath() + "\\" + entry.FullName);
                        }
                    }
                    else
                    {
                        entry.ExtractToFile(Path.Combine(ConfigHelper.WebPath(), entry.FullName), true);
                    }
                }
            }
        }

        /// <summary>
        /// 通知更新成功
        /// </summary>
        /// <param name="mac"></param>
        public static void UpdateSuccess(string mac)
        {
            string ver = CommonHelper.GetVersion();

            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            using (var http = new HttpClient(handler))
            {
                http.BaseAddress = new Uri(ConfigHelper.ServerHttpPath());
                var content = new FormUrlEncodedContent(new Dictionary<string, string>() {
                    { "mac", mac },
                    { "ver", ver }
                });

                var task = http.PostAsync("/api/BoxSysUpdateSuccess", content);
                if (task.Result.IsSuccessStatusCode)
                {
                    var rep = task.Result;//在这里会等待task返回。
                    var task2 = rep.Content.ReadAsStringAsync();//读取响应内容
                    var jdata = JsonHelper.DeserializeJsonToObject<m_JsonData>(task2.Result);//在这里会等待task返回。
                }
            }
        }
    }
}
