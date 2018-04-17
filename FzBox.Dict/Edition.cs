using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzBox.Dict
{
    public class Edition
    {
        private static Dictionary<int, string> d = null;

        static Edition()
        {
            d = new Dictionary<int, string>();
            d.Add(1, "人教PEP版");
            d.Add(3, "北京版");
            d.Add(4, "牛津上海版");
            d.Add(5, "牛津上海全国版");
            d.Add(6, "牛津少儿英语Let's go");
            d.Add(7, "新牛津幼儿英语 New English First!");
            d.Add(8, "北师大版(一起)");
            d.Add(9, "外研新标准(一起)");
            d.Add(10, "人教版新起点");
            d.Add(11, "冀教版(一起)");
            d.Add(12, "科普版");
            d.Add(13, "江苏牛津");
            d.Add(14, "湘少版");
            d.Add(15, "陕旅版");
            d.Add(16, "人教版精通");
            d.Add(17, "外研新交际");
            d.Add(18, "朗文新派少儿英语");
            d.Add(19, "译林新版");
            d.Add(20, "湘教版");
            d.Add(21, "牛津深圳版");
            d.Add(22, "江苏译林");
            d.Add(24, "广州版");
            d.Add(25, "牛津上海本地版");
            d.Add(27, "人教版");
            d.Add(29, "语文S版");
            d.Add(30, "山东版");
            d.Add(31, "闽教版");
            d.Add(32, "人教精通版");
            d.Add(33, "人教新目标");
            d.Add(34, "剑桥英语青少版");
            d.Add(36, "牛津译林版");
            d.Add(37, "仁爱版");
            d.Add(39, "广东版");
            d.Add(43, "深港朗文版");
            d.Add(44, "广州口语");
            d.Add(45, "儿童英语");
            d.Add(46, "广西人教版");
            d.Add(47, "教科版");
            d.Add(48, "鲁科版");
            d.Add(49, "语文社必修");
            d.Add(50, "语文A版");
            d.Add(51, "西师大版");
            d.Add(52, "浙教版");
            d.Add(53, "鄂教版");
            d.Add(54, "沪教版");
            d.Add(55, "长春版");
            d.Add(56, "鲁教版");
            d.Add(57, "新课标标准实验版");
            d.Add(58, "青岛版");
            d.Add(59, "鲁人版");
            d.Add(60, "粤教版");
            d.Add(61, "外研新标准(三起)");
            d.Add(62, "北师大版(三起)");
            d.Add(63, "冀教版(三起)");
            d.Add(64, "北师大版");
            d.Add(65, "苏教版");
            d.Add(66, "部编本");
            d.Add(67, "小学英语拼读教程(全国通用版)");
            d.Add(999, "电影课");
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
