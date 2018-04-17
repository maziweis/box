using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzBox.Dict
{
    public class ResType
    {
        private static Dictionary<int, string> d = null;

        static ResType()
        {
            d = new Dictionary<int, string>();
            d.Add(27, "互动课件");
            d.Add(10, "课件");
            d.Add(12, "教案");
            d.Add(9, "试卷");
            d.Add(6, "动画");
            d.Add(28, "图片");
            d.Add(5, "音频");
            d.Add(4, "视频");
            d.Add(2, "文本");
            d.Add(15, "教学设计");
            d.Add(8, "试题");
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
