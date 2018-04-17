using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;

namespace FzBox.Web
{
    /// <summary>
    /// 定时上传操作日志
    /// </summary>
    public class TimedTask
    {
        System.Threading.Timer timer;

        public TimedTask()
        {
            timer = new System.Threading.Timer(UpLog, null, 0, 1000 * 300);//5分钟执行一次
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpLog(object obj)
        {
            int h = DateTime.Now.Hour;
            if (h >= 0 && h <= 4)//每天0点到4点上传日志
            {
                string filepath = System.Web.Hosting.HostingEnvironment.MapPath("~/") + "Log";
                string[] filenames = Directory.GetFiles(filepath);
                foreach (string f in filenames)
                {
                    if (f.Substring(f.LastIndexOf('.') + 1, 3) == "xml")
                    {
                        string fn = f.Substring(f.LastIndexOf('\\') + 1, 10);
                        if (DateTime.Parse(fn).Date < DateTime.Now.Date)
                        {
                            using (var client = new HttpClient())
                            {
                                using (var content = new MultipartFormDataContent())
                                {
                                    client.BaseAddress = new Uri(AppConfig.Url);
                                    var fileContent = new ByteArrayContent(File.ReadAllBytes(f));
                                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                                    {
                                        FileName = "a.xml"
                                    };

                                    content.Add(fileContent);
                                    //content.Headers.Add("MAC", "ssssss");
                                    var result = client.PostAsync("/api/BoxUpLog", content).Result;

                                    if (result.IsSuccessStatusCode)
                                    {
                                        var jdata = Fz.Common.JsonHelper.DecodeJson<Fz.Common.Model.JsonData>(result.Content.ReadAsStringAsync().Result);//在这里会等待task返回。
                                        if ((bool)jdata.data)
                                        {
                                            if (File.Exists(f))
                                            {
                                                File.Delete(f);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }//for结束
            }
        }
    }

    /// <summary>
    /// 系统定时更新
    /// </summary>
    public class TimedTaskUpdate
    {
        System.Threading.Timer timer;

        public TimedTaskUpdate()
        {
            DateTime dtNow = DateTime.Now;
            DateTime dt23 = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 10, 5, 0);//获得当天23：00
            if (dt23 < dtNow)
            {
                dt23 = dt23.AddDays(1);//如果现在已过23：00，则获得明天的23：00
            }
            TimeSpan ts = dt23 - dtNow;
            timer = new System.Threading.Timer(Update, null, 0, 5 * 60 * 1000);//24小时执行一次Convert.ToInt32(ts.TotalMilliseconds)
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Update(object obj)
        {
            BLL.Common.WriteLogUpdate(string.Format("自动检测更新：" + DateTime.Now.ToString()));
            try
            {
                string mac = BLL.Common.GetMac();
                bool IsUpdate = false;
                string url = string.Format("{0}/api/BoxSysUpdateCheck/{1}", BLL.Common.GetServerAddress(), mac);
                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };//创建HttpClient（注意传入HttpClientHandler）
                using (var http = new HttpClient(handler))
                {
                    var task = http.GetAsync(url);//同步远程获取数据
                    if (task.Result.IsSuccessStatusCode)
                    {
                        var rep = task.Result;//在这里会等待task返回。
                        var task2 = rep.Content.ReadAsStringAsync();//读取响应内容
                        var jdata = Fz.Common.JsonHelper.DecodeJson<Fz.Common.Model.JsonData>(task2.Result);//在这里会等待task返回。
                        if (jdata != null && jdata.data != null)
                        {
                            IsUpdate = (bool)jdata.data;
                        }
                    }
                }

                if (IsUpdate)
                {
                    List<string> packs = BLL.UpdateSystem.GetPackages(mac);

                    if (packs != null && packs.Count > 0)
                    {
                        foreach (var pack in packs)
                        {
                            string fileName = Guid.NewGuid().ToString();

                            BLL.UpdateSystem.DownLoadOneFile(pack, fileName);

                            BLL.UpdateSystem.UnZipAndReplace(fileName);

                            BLL.UpdateSystem.UpdateSuccess(mac);
                        }
                        //BLL.Common.RestartIIS();
                    }
                }
            }
            catch (Exception ex)
            {
                BLL.Common.WriteLogError(ex.ToString());
            }

        }
    }
}