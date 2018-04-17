using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzBox.AutoUpdate
{
    public class ConfigHelper
    {
        /// <summary>
        /// 获取外网OMS地址
        /// </summary>
        /// <returns></returns>
        public static string ServerHttpPath()
        {
            return System.Configuration.ConfigurationManager.AppSettings["server"].ToString();
        }

        /// <summary>
        /// 获取网站发布目录
        /// </summary>
        /// <returns></returns>
        public static string WebPath()
        {
            return System.Configuration.ConfigurationManager.AppSettings["webpath"].ToString();
        }
    }
}
