using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzBox.AutoUpdate
{
    public enum JsonDataFlag
    {
        /// <summary>
        /// 成功
        /// </summary>
        Succeed = 1,
        /// <summary>
        /// 验证失败
        /// </summary>
        ValidFailed = 0,
        /// <summary>
        /// 系统异常
        /// </summary>
        Exception = -1,
        /// <summary>
        /// 没有权限
        /// </summary>
        NoPermission = -2,
        /// <summary>
        /// 登录超时
        /// </summary>
        OverTime = -3,
    }

    public class m_JsonData
    {
        /// <summary>
        /// 返回结果类型：1-成功，2-验证失败，3-系统异常，4-没有权限，5-登录超时(或者没有登录)
        /// </summary>
        public JsonDataFlag flag { get; set; }

        /// <summary>
        /// 返回结果消息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 返回结果数据
        /// </summary>
        public object data { get; set; }
    }
}
