using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzBox.Dict
{
    public class FileType
    {
        private static Dictionary<int, string> d = null;

        static FileType()
        {
            d = new Dictionary<int, string>();
            d.Add(1, "doc");
            d.Add(2, "docx");
            d.Add(3, "pdf");
            d.Add(4, "ppt");
            d.Add(5, "pptx");
            d.Add(6, "psd");
            d.Add(7, "rar");
            d.Add(8, "txt");
            d.Add(9, "xls");
            d.Add(10, "xlsx");
            d.Add(11, "zip");
        }

        public static Dictionary<int, string> Get()
        {
            return d;
        }

        public static string GetVal(int key)
        {
            return d.ContainsKey(key) ? d[key] : "";
        }
        public static int GetId(string val)
        {
            return d.ContainsValue(val) ? d.Where(_ => _.Value == val).FirstOrDefault().Key : 0;
        }
    }
}
