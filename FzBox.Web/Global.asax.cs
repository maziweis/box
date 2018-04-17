using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;

namespace FzBox.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);

            GetBooks();
            BookCatalogs _bookcatalog = new BookCatalogs();
            GetData(_bookcatalog, "BookCata", "bookcatalog.xml");
            Resources _res = new Resources();
            GetData(_res, "ResourcesFile", "resources.xml");
            C_E _bookedition = new C_E();
            CacheData(Server.MapPath("/Data/BookEdition.xml"), _bookedition, "BookEdition");
            C_E _course = new C_E();
            CacheData(Server.MapPath("/Data/Course.xml"), _course, "Course");
            C_E _dn = new C_E();
            CacheData(Server.MapPath("/Data/DirectoryName.xml"), _dn, "DirectoryName");
        }

        #region "获取全部教材资源"
        /// <summary>
        /// 获取全部教材资源
        /// </summary>
        private void GetBooks()
        {
            List<Book> books = new List<Book>();
            string URL_YKT_Folder = string.Format("{0}Resources\\Course", System.Web.Hosting.HostingEnvironment.MapPath("~/"));
            DirectoryInfo YKTFolders = new DirectoryInfo(URL_YKT_Folder);
            foreach (DirectoryInfo folder in YKTFolders.GetDirectories())
            {
                string fileUrl = string.Format("{0}\\{1}\\Course.kino", URL_YKT_Folder, folder.Name);
                if (File.Exists(fileUrl))
                {
                    using (StreamReader sr = new StreamReader(fileUrl, System.Text.Encoding.Default))
                    {
                        string json = sr.ReadToEnd();
                        if (!string.IsNullOrEmpty(json))
                        {
                            Book book = Implement.JsonHelper.DecodeJson<Book>(json);
                            book.DirectoryName = folder.Name;
                            books.Add(book);
                        }
                    }
                }
            }

            Fz.Common.Caches.SetCache("Books", books);
        }
        #endregion

        public void GetData<T>(T t, string name, string xmlName)
        {
            List<T> _list = new List<T>();
            string URL_YKT_Folder = string.Format("{0}Resources\\Course", System.Web.Hosting.HostingEnvironment.MapPath("~/"));
            DirectoryInfo YKTFolders = new DirectoryInfo(URL_YKT_Folder);
            foreach (DirectoryInfo folder in YKTFolders.GetDirectories())
            {
                string fileUrl = string.Format("{0}\\{1}\\Applications\\Data\\{2}", URL_YKT_Folder, folder.Name, xmlName);
                if (File.Exists(fileUrl))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(fileUrl);
                    Type type = typeof(T);
                    XmlNode datas = doc.SelectSingleNode("Datas");
                    var rows = datas.SelectNodes("Row");

                    if (rows != null && rows.Count > 0)
                    {
                        foreach (XmlNode row in rows)
                        {

                            if (t != null)
                                t = System.Activator.CreateInstance<T>();
                            PropertyInfo[] pi = type.GetProperties();
                            XmlNodeList childs = row.ChildNodes;
                            for (int i = 0; i < childs.Count; i++)
                            {
                                XmlNode n = childs[i];
                                //查找属性名称和xml文档中一致的节点名称，并且设置属性值
                                var list = pi.Where(_ => _.Name == n.Name).ToList();
                                //注释掉的这种写法可以不必拘束于所有的变量都是string类型
                                // if (list[0].GetValue(t, null) == null)
                                list[0].SetValue(t, n.InnerText.ToString(), null);
                                //else
                                // list[0].SetValue(t, null, null);
                            }
                            _list.Add(t);

                        }
                    }
                }
            }
            if (_list.Count > 0)
                Fz.Common.Caches.SetCache(name, _list);
        }

        public static void CacheData<T>(string url, T t, string name)
        {
            List<T> _list = new List<T>();

            var fileUrl = url;
            if (File.Exists(fileUrl))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileUrl);
                Type type = typeof(T);
                XmlNode datas = doc.SelectSingleNode("Datas");
                var rows = datas.SelectNodes("Row");

                if (rows != null && rows.Count > 0)
                {
                    foreach (XmlNode row in rows)
                    {

                        if (t != null)
                            t = System.Activator.CreateInstance<T>();
                        PropertyInfo[] pi = type.GetProperties();
                        XmlNodeList childs = row.ChildNodes;
                        for (int i = 0; i < childs.Count; i++)
                        {
                            XmlNode n = childs[i];
                            //查找属性名称和xml文档中一致的节点名称，并且设置属性值
                            var list = pi.Where(_ => _.Name == n.Name).ToList();
                            //注释掉的这种写法可以不必拘束于所有的变量都是string类型
                            //if (list[0].GetValue(t, null) == null)
                            list[0].SetValue(t, n.InnerText.ToString(), null);
                            //else
                            //    list[0].SetValue(t, null, null);
                        }
                        _list.Add(t);

                    }
                }
            }
            if (_list.Count > 0)
                Fz.Common.Caches.SetCache(name, _list);
        }

    }

    public class C_E
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class Book
    {
        public string CourseKey { get; set; }
        public string ClassName { get; set; }
        public int EditionID { get; set; }
        public int SubjectID { get; set; }
        public string Version { get; set; }
        public List<BookApp> Applications { get; set; }
        public string DirectoryName { get; set; }
    }

    public class BookApp
    {
        public string AppKey { get; set; }
        public string AppName { get; set; }
        public string Version { get; set; }
        public string Folder { get; set; }
        public int AppType { get; set; }
    }
    public class BookCatalogs
    {
        public string CatalogId { get; set; }

        public string BookId { get; set; }

        public string PId { get; set; }

        public string CatalogName { get; set; }

        public string PageStart { get; set; }

        public string PageEnd { get; set; }

        public string Sort { get; set; }
    }

    public class Resources
    {
        public string ResId { get; set; }
        public string BookId { get; set; }
        public string Catalog1 { get; set; }
        public string ResType { get; set; }
        public string ResBigType { get; set; }
        public string TzResType { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string SchoolStage { get; set; }
        public string Grade { get; set; }
        public string Subject { get; set; }
        public string Edition { get; set; }
        public string BookReel { get; set; }
        public string ResourceClass { get; set; }
        public string ResourceStyle { get; set; }
        public string ResourceType { get; set; }
        public string ComeFrom { get; set; }

        public string Cover { get; set; }
        public string KeyWords { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ParentID { get; set; }
    }
}
