using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FzBox.AutoUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {

                //timer2.Stop();
                sb.AppendLine(string.Format("//====================【01】-【{0}】-【自动更新检测开始】====================", DateTime.Now));

                string mac = CommonHelper.GetMac();
                sb.AppendLine(string.Format("//====================【02】-【{0}】-【检测网卡MAC地址】-【{1}】====================", DateTime.Now, mac));

                bool IsUpdate = false;
                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };//创建HttpClient（注意传入HttpClientHandler）
                using (var http = new HttpClient(handler))
                {
                    http.BaseAddress = new Uri(ConfigHelper.ServerHttpPath());
                    var task = http.GetAsync(string.Format("/api/BoxSysUpdateCheck/{0}", mac));//同步远程获取数据
                    if (task.Result.IsSuccessStatusCode)
                    {
                        var rep = task.Result;//在这里会等待task返回。
                        var task2 = rep.Content.ReadAsStringAsync();//读取响应内容
                        var jdata = JsonHelper.DeserializeJsonToObject<m_JsonData>(task2.Result);//在这里会等待task返回。
                        if (jdata != null && jdata.data != null)
                        {
                            IsUpdate = (bool)jdata.data;
                        }
                    }
                }
                sb.AppendLine(string.Format("//====================【03】-【{0}】-【检测该商品是否需要更新？】-【{1}】====================", DateTime.Now, IsUpdate ? "是" : "否"));

                if (IsUpdate)
                {
                    List<string> packs = UpdateSystemBLL.GetPackages(mac);
                    sb.AppendLine(string.Format("//====================【04】-【{0}】-【检测到更新包的数量是？】-【{1}】个====================", DateTime.Now, packs.Count));

                    if (packs != null && packs.Count > 0)
                    {
                        //foreach (var pack in packs)
                        //{
                        string fileName = Guid.NewGuid().ToString();

                        UpdateSystemBLL.DownLoadOneFile(packs[0], fileName);
                        sb.AppendLine(string.Format("//====================【05】-【{0}】-【{1}】-【更新包下载完成】个====================", DateTime.Now, packs[0]));

                        UpdateSystemBLL.UnZipAndReplace(fileName);
                        sb.AppendLine(string.Format("//====================【06】-【{0}】-【{1}】-【更新包解压覆盖完成】个====================", DateTime.Now, packs[0]));

                        UpdateSystemBLL.UpdateSuccess(mac);
                        sb.AppendLine(string.Format("//====================【07】-【{0}】-【{1}】-【通知服务器更新成功】个====================", DateTime.Now, packs[0]));
                        // }
                        CommonHelper.UpdateVersion(packs[1]);//更改版本号
                    }
                    sb.AppendLine(string.Format("//====================【{0}自动更新结束】====================", DateTime.Now));
                    LogHelper.UpdateInfo(sb.ToString());
                }

                // timer2.Start();
            }
            catch (Exception ex)
            {
                // timer2.Start();
                sb.AppendLine(ex.ToString());
                LogHelper.UpdateError(sb.ToString());
            }
        }
    }
}
