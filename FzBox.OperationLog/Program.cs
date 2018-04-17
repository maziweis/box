using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FzBox.OperationLog
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string filepath = string.Format("{0}\\Log", ConfigHelper.WebPath());
                if (Directory.Exists(filepath))
                {
                    string[] filenames = Directory.GetFiles(filepath);
                    foreach (string f in filenames)
                    {
                        if (f.Substring(f.LastIndexOf('.') + 1, 3) == "xml")
                        {
                            string fn = f.Substring(f.LastIndexOf('\\') + 1, 10);
                            DateTime dt1;
                            DateTime.TryParse(fn, out dt1);
                            using (var client = new HttpClient())
                            {
                                using (var content = new MultipartFormDataContent())
                                {
                                    client.BaseAddress = new Uri(ConfigHelper.ServerHttpPath());
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
                                        var jdata = JsonHelper.DeserializeJsonToObject<m_JsonData>(result.Content.ReadAsStringAsync().Result);//在这里会等待task返回。
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
                    }//for结束
                }
            }
            catch (Exception ex)
            {
                LogHelper.UpLogError(ex.ToString());
            }
        }
    }
}
