using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;

namespace FzBox.Implement
{
    public class JsonHelper
    {

        /// <summary>
        /// 对象JSON序列化接口
        /// </summary>
        /// <param name="obj">序列化对象</param>
        /// <returns></returns>
        public static string EncodeJson(object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            return serializer.Serialize(obj);
        }
        /// <summary>
        /// 对象反序列化接口
        /// </summary>
        /// <typeparam name="T">反序列化对象类型</typeparam>
        /// <param name="json">序列化字符串</param>
        /// <returns></returns>
        public static T DecodeJson<T>(string json) where T : new()
        {
            T obj;
            if (!String.IsNullOrEmpty(json))
            {
                
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = int.MaxValue;
                obj = (T)serializer.Deserialize(json, typeof(T));
                
            }
            else
            {
                obj = default(T);
            }
            return obj;
        }

        /// <summary>
        /// 对象JSON序列化接口
        /// </summary>
        /// <param name="obj">序列化对象</param>
        /// <returns></returns>
        public static string DeepEncodeJson(object obj)
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());

            string szJson = "";

            //序列化

            using (MemoryStream stream = new MemoryStream())
            {

                json.WriteObject(stream, obj);

                szJson = Encoding.UTF8.GetString(stream.ToArray());

            }
            return szJson;
        }

        /// <summary>
        /// 对象反序列化接口
        /// </summary>
        /// <typeparam name="T">反序列化对象类型</typeparam>
        /// <param name="json">序列化字符串</param>
        /// <returns></returns>
        public static T DeepDecodeJson<T>(string json) where T : new()
        {
            T obj = default(T);
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

                obj = (T)serializer.ReadObject(ms);

            }
            return obj;
        }
    }

    public class DateTimeConverter : JavaScriptConverter
    {

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            if (string.IsNullOrEmpty(dictionary["Value"].ToString()))
                return null;

            return DateTime.Parse(dictionary["Value"].ToString());
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {

            IDictionary<string, object> result = new Dictionary<string, object>();
            if (obj == null)
                result["Value"] = string.Empty;
            else
                result["Value"] = ((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get { yield return typeof(DateTime); }
        }
    }
}
