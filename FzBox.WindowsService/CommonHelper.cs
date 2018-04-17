using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FzBox.WindowsService
{
    public class CommonHelper
    {
        /// <summary>
        /// 获取本机网卡MAC地址
        /// </summary>
        /// <returns></returns>
        public static string GetMac()
        {
            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                moc = null;
                mc = null;
                return mac.Trim().Replace(':', '-');
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 获取系统当前版本
        /// </summary>
        /// <returns></returns>
        public static string GetVersion()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ConfigHelper.WebPath() + "\\Web.config");
            XmlNode configuration = doc.SelectSingleNode("configuration");
            XmlNode appSettings = configuration.SelectSingleNode("appSettings");
            var nodes = appSettings.SelectNodes("add");
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Attributes["key"].Value == "version")
                {
                    return nodes[i].Attributes["value"].Value;
                }
            }

            return "";
        }
    }
}
