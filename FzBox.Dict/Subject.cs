using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzBox.Dict
{
    /// <summary>
    /// 学科
    /// </summary>
    public class Subject
    {
        private static Dictionary<int, string> d = null;

        static Subject()
        {
            d = new Dictionary<int, string>();
            d.Add(3, "英语");
            d.Add(2, "数学");
            d.Add(1, "语文");
        }

        public static Dictionary<int, string> Get()
        {
            return d;
        }

        public static string GetVal(int key)
        {
            return d.ContainsKey(key) ? d[key] : "";
        }
    }
}
