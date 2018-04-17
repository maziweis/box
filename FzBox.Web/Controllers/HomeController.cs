using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace FzBox.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int? s)
        {
            ViewBag.Subject = s;

            if (s == null)
            {
                List<Book> books = Fz.Common.Caches.GetCache("Books") as List<Book>;
                List<int> subjectIds = null;
                if (books != null)
                {
                    subjectIds = books.Select(s1 => s1.SubjectID).Distinct().ToList();
                }

                foreach (var subject in Dict.Subject.Get())
                {
                    if (subjectIds.Contains(subject.Key))
                    {
                        ViewBag.Subject = subject.Key;
                        break;
                    }
                }
            }

            return View();
        }

        public ActionResult Book(int s, string id)
        {
            ViewBag.Subject = s;

            Models.VM_Book m = new Models.VM_Book();

            List<Book> books = Fz.Common.Caches.GetCache("Books") as List<Book>;
            if (books != null)
            {
                Book book = books.Where(w => w.CourseKey == id).FirstOrDefault();
                m.Edition = book.EditionID;
                m.Subject = book.SubjectID;
                m.EditionName = Dict.Edition.GetVal(book.EditionID);
                m.BookName = book.ClassName;
                m.Cover = string.Format("~/Resources/Course/{0}/Course.gif", book.DirectoryName);
                m.Url = string.Format("~/Resources/Course/{0}/Start.htm", book.DirectoryName);
                m.CourseKey = id;
                m.Apps = new List<Models.VM_Book_App>();
                Boolean IsHaveCs = false;
                List<C_E> _listcourse = Fz.Common.Caches.GetCache("Course") as List<C_E>;                
                if (_listcourse.Where(_ => _.Name.Contains(id)).Count() > 0)
                {
                    IsHaveCs = true;
                }
                if (book.Applications != null && book.Applications.Count > 0)
                {
                    foreach (var app in book.Applications)
                    {
                        if (app.AppName == "备课资源" && IsHaveCs) {
                            continue;
                        }
                        m.Apps.Add(new Models.VM_Book_App
                        {
                            AppType = app.AppType.ToString(),
                            Name = app.AppName,
                            Url = string.Format("~/Resources/Course/{0}/{1}/Start.htm", book.DirectoryName, app.Folder)
                        });
                    }
                }
                if (_listcourse.Where(_ => _.Name.Contains(id)).Count() > 0)
                {
                    m.Apps.Add(new Models.VM_Book_App
                    {
                        AppType = "8",
                        Name = "备课资源",
                        Url = ""
                    });
                }
                //else
                //{
                //    List<C_E> _listcourse = Fz.Common.Caches.GetCache("Course") as List<C_E>;
                //    if (_listcourse.Where(_ => _.Name.Contains(id)).Count() > 0)
                //    {
                //        m.Apps.Add(new Models.VM_Book_App
                //        {
                //            AppType = "8",
                //            Name = "备课资源",
                //            Url = ""
                //        });
                //    }
                //}
            }

            return View(m);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="app">操作功能类型</param>
        /// <param name="edition">操作功能所属版本</param>
        /// <param name="subject">操作功能所属学科</param>
        /// <param name="appName">操作功能类型名称</param>
        public void ResLog(string app, int? edition, int? subject, string appName)
        {
            var fileName = string.Format("{0}.xml", DateTime.Now.ToString("yyyy-MM-dd"));
            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/");
            path = string.Format("{0}\\Log", path);
            var pathAll = path + "\\" + fileName;

            if (System.IO.File.Exists(pathAll))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(pathAll);

                XmlNode logs = doc.SelectSingleNode("Logs");

                XmlElement datas = doc.CreateElement("Datas");
                logs.AppendChild(datas);

                XmlElement node_mac = doc.CreateElement("Mac");
                node_mac.InnerText = BLL.Common.GetMac();
                datas.AppendChild(node_mac);

                XmlElement node_time = doc.CreateElement("Time");
                node_time.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                datas.AppendChild(node_time);

                XmlElement node_subject = doc.CreateElement("Subject");
                node_subject.InnerText = subject == null ? "" : subject.ToString();
                datas.AppendChild(node_subject);

                XmlElement node_edition = doc.CreateElement("Edition");
                node_edition.InnerText = edition == null ? "" : edition.ToString();
                datas.AppendChild(node_edition);

                XmlElement node_type = doc.CreateElement("Type");
                node_type.InnerText = app;
                datas.AppendChild(node_type);

                XmlElement node_typeName = doc.CreateElement("TypeName");
                node_typeName.InnerText = appName;
                datas.AppendChild(node_typeName);

                doc.Save(pathAll);
            }
            else
            {
                System.IO.Directory.CreateDirectory(path);

                XmlDocument doc = new XmlDocument();
                XmlNode decl = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                doc.AppendChild(decl);

                XmlElement logs = doc.CreateElement("Logs");
                doc.AppendChild(logs);

                XmlElement datas = doc.CreateElement("Datas");
                logs.AppendChild(datas);

                XmlElement node_mac = doc.CreateElement("Mac");
                node_mac.InnerText = BLL.Common.GetMac();
                datas.AppendChild(node_mac);

                XmlElement node_time = doc.CreateElement("Time");
                node_time.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                datas.AppendChild(node_time);

                XmlElement node_subject = doc.CreateElement("Subject");
                node_subject.InnerText = subject == null ? "" : subject.ToString();
                datas.AppendChild(node_subject);

                XmlElement node_edition = doc.CreateElement("Edition");
                node_edition.InnerText = edition == null ? "" : edition.ToString();
                datas.AppendChild(node_edition);

                XmlElement node_type = doc.CreateElement("Type");
                node_type.InnerText = app;
                datas.AppendChild(node_type);

                XmlElement node_typeName = doc.CreateElement("TypeName");
                node_typeName.InnerText = appName;
                datas.AppendChild(node_typeName);

                doc.Save(pathAll);
            }
        }
    }
}