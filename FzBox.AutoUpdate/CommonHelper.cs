using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FzBox.AutoUpdate
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

        /// <summary>
        /// 更改版本号
        /// </summary>
        /// <param name="val"></param>
        public static void UpdateVersion(string val)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(ConfigHelper.WebPath() + "\\Web.config");
                XmlNode configuration = doc.SelectSingleNode("configuration");
                XmlElement element = (XmlElement)configuration.SelectSingleNode("//add[@key='version']");
                element.SetAttribute("value", val);
                doc.Save(ConfigHelper.WebPath() + "\\Web.config");
            }
            catch (Exception ex)
            {

                // throw;
            }

        }
    }
}
