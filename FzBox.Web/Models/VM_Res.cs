using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FzBox.Web.Models
{
    public class VM_Res
    {
        /// <summary>
        /// 书本编号
        /// </summary>
        public string book_id { get; set; }
        /// <summary>
        /// 条件
        /// </summary>
        public string conditions { get; set; }
        /// <summary>
        /// 目录编号
        /// </summary>
        public string catalog_id { get; set; }
        /// <summary>
        /// 类型编号
        /// </summary>
        public string type_id { get; set; }
        /// <summary>
        /// 展示类型
        /// </summary>
        public List<int> listType { get; set; }
        /// <summary>
        /// 子节点编号
        /// </summary>
        public string kidcatalog_id { get; set; }
    }
}