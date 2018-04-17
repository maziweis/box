using FzBox.Implement;
using FzBox.Web.BLL;
using FzBox.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FzBox.Web.Controllers
{
    public class LessonResController : Controller
    {
        // GET: 备课资源
        public ActionResult Index(VM_Res m)
        {
            m.listType = new List<int>();
            List<BookCatalogs> _listcata = Fz.Common.Caches.GetCache("BookCata") as List<BookCatalogs>;
            List<Resources> _listres = Fz.Common.Caches.GetCache("ResourcesFile") as List<Resources>;
            var _listParent = _listcata.Where(_ => _.BookId == m.book_id && _.PId == "0").OrderBy(_ => Convert.ToInt32(_.Sort)).ToList();
            var kidcatalog = "";
            if (string.IsNullOrEmpty(m.catalog_id) && _listParent.Count > 0)
            {
                m.catalog_id = _listParent.FirstOrDefault().CatalogId;
            }
            BookCatalogs mcatalog = _listcata.Where(_ => _.BookId == m.book_id && _.CatalogId == m.catalog_id).FirstOrDefault();
            BookCatalogs kidlog = _listcata.Where(_ => _.BookId == m.book_id && _.PId == m.catalog_id).OrderBy(_ => Convert.ToInt32(_.Sort)).FirstOrDefault();
            if (kidlog != null)
            {
                kidcatalog = kidlog.CatalogId;
            }
            var _listfile = new List<Resources>();
            if (string.IsNullOrEmpty(m.kidcatalog_id) || m.kidcatalog_id.Equals("-1") || m.kidcatalog_id.Equals("undefined"))
            {
                List<string> _listUcata = _listcata.Where(_ => _.BookId == m.book_id && _.PId == m.catalog_id).Select(s => s.CatalogId).ToList();
                if (_listUcata.Count > 0)
                {
                    List<string> _listPcata = _listcata.Where(_ => _.BookId == m.book_id && _listUcata.Contains(_.PId)).Select(s => s.CatalogId).ToList();
                    if (_listPcata.Count > 0)
                    {
                        _listUcata.AddRange(_listPcata);
                    }
                    _listUcata.Add(mcatalog.CatalogId);
                    _listfile = _listres.Where(_ => _.BookId == m.book_id && _listUcata.Contains(_.Catalog1)).ToList();
                }
                else
                {
                    _listfile = _listres.Where(_ => _.BookId == m.book_id && _.Catalog1 == mcatalog.CatalogId).ToList();
                }
            }
            else
            {
                _listfile = _listres.Where(_ => _.BookId == m.book_id && (_.Catalog1 == m.kidcatalog_id || _.ParentID == m.kidcatalog_id)).ToList();
            }
            foreach (var item in _listfile)
            {
                if (item.ResourceType == "10")
                {
                    if (!m.listType.Contains(Convert.ToInt32(item.ResourceType)))
                        m.listType.Add(Convert.ToInt32(item.ResourceType));
                }
                else
                {
                    if (!m.listType.Contains(Convert.ToInt32(item.ResourceStyle)))
                        m.listType.Add(Convert.ToInt32(item.ResourceStyle));
                }

            }
            if (_listfile.Count > 0)
            {
                if (string.IsNullOrEmpty(m.type_id))
                {
                    Dictionary<int, string> _dic = FzBox.Dict.ResType.Get();
                    int _i = 0;
                    foreach (var item in _dic)
                    {
                        foreach (var it in m.listType)
                        {
                            if (item.Key == it)
                            {
                                m.type_id = it.ToString();
                                _i++;
                                break;
                            }
                        }
                        if (_i > 0)
                            break;
                    }
                }

                if (m.type_id == "10")
                    _listfile = _listfile.Where(_ => _.ResourceType == m.type_id).ToList();
                else
                    _listfile = _listfile.Where(_ => _.ResourceStyle == m.type_id).ToList();
            }
            ViewData["listParent"] = _listParent;
            ViewData["listfile"] = _listfile;
            return View(m);
        }

        public ActionResult QueryResult(VM_Res m)
        {
            List<BookCatalogs> _listcata = Fz.Common.Caches.GetCache("BookCata") as List<BookCatalogs>;
            List<Resources> _listres = Fz.Common.Caches.GetCache("ResourcesFile") as List<Resources>;
            var _listfile = new List<Resources>();
            if (!string.IsNullOrWhiteSpace(m.conditions))
            {
                _listfile = _listres.Where(_ => _.Title.ToLower().Contains(m.conditions.ToLower()) && _.BookId == m.book_id).ToList();
            }
            ViewData["listfile"] = _listfile;
            return View(m);
        }

        //public JsonResult BackUrl(string filename, string url)
        //{
        //    string _fileurl = Server.MapPath("/Resources/DecFile/");
        //    if (!Directory.Exists(_fileurl))
        //    {
        //        Directory.CreateDirectory(_fileurl);
        //    }
        //    bool _result = FileHelper.DecryptFile(Server.MapPath(url), _fileurl + filename);
        //    if (_result)
        //        return Json(new { result = _result, fileurl = "/Resources/DecFile/" + filename });
        //    else
        //        return Json(new { result = _result });
        //}

        //public JsonResult DeleteFile(string filename)
        //{
        //    FileHelper.deletefile(Server.MapPath(filename));
        //    return Json(new { result = true });
        //}
        /// <summary>
        /// FilePathResult方式下载
        /// </summary>
        /// <returns></returns>
        public FileResult GetFileByPath(string filename, string filepath)
        {
            filename += Path.GetExtension(filepath);
            filepath = Server.MapPath(filepath);
            //string outpath = Server.MapPath("/Resources/DecFile/" + filename);
            //FileHelper.DecryptFile(filepath, outpath);
            //return File(outpath, "application/octet-stream", filename);
            return File(filepath, "application/octet-stream", filename);
        }
        /// <summary>  
        /// 获取考试数据  
        /// </summary>  
        /// <returns></returns> 
        public string GetCata(string bookid, string cataid, string kidcataid)
        {
            string type_id = "";//显示资源类型
            List<int> listType = new List<int>();//资源类型列表
            string listType1 = "";//资源类型列表  字符串方式
            var _listfile = new List<Resources>();//所有文件
            Dictionary<int, string> _dic = FzBox.Dict.ResType.Get();
            List<BookCatalogs> _listcata = Fz.Common.Caches.GetCache("BookCata") as List<BookCatalogs>;
            List<Resources> _listres = Fz.Common.Caches.GetCache("ResourcesFile") as List<Resources>;
            List<C_E> _listdn = Fz.Common.Caches.GetCache("DirectoryName") as List<C_E>;
            string bookname = _listdn.Where(_ => _.ID == bookid).FirstOrDefault().Name;
            if (!string.IsNullOrEmpty(bookid) && !string.IsNullOrEmpty(cataid))
            {
                if (string.IsNullOrEmpty(kidcataid))
                {
                    List<string> _listUcata = _listcata.Where(_ => _.BookId == bookid && _.PId == cataid).Select(s => s.CatalogId).ToList();
                    if (_listUcata.Count > 0)
                    {
                        List<string> _listPcata = _listcata.Where(_ => _.BookId == bookid && _listUcata.Contains(_.PId)).Select(s => s.CatalogId).ToList();
                        if (_listPcata.Count > 0)
                        {
                            _listUcata.AddRange(_listPcata);
                        }
                        _listUcata.Add(cataid);
                        _listfile = _listres.Where(_ => _.BookId == bookid && _listUcata.Contains(_.Catalog1)).ToList();
                    }
                    else
                    {
                        _listfile = _listres.Where(_ => _.BookId == bookid && _.Catalog1 == cataid).ToList();
                    }
                }
                else
                {
                    _listfile = _listres.Where(_ => _.BookId == bookid && (_.Catalog1 == kidcataid || _.ParentID == kidcataid)).ToList();
                }
            }
            foreach (var item in _listfile)
            {
                if (item.ResourceType == "10")
                {
                    if (!listType.Contains(Convert.ToInt32(item.ResourceType)))
                        listType.Add(Convert.ToInt32(item.ResourceType));
                }
                else
                {
                    if (!listType.Contains(Convert.ToInt32(item.ResourceStyle)))
                        listType.Add(Convert.ToInt32(item.ResourceStyle));
                }
            }
            foreach (var item in _dic)
            {
                foreach (var type in listType)
                {
                    if (item.Key == type)
                    {
                        listType1 += item.Key +"_"+ item.Value + ",";
                        break;
                    }
                }
            }
            listType1 = listType1.Remove(listType1.Length - 1);
            if (_listfile.Count > 0)
            {
                int _i = 0;
                foreach (var item in _dic)
                {
                    foreach (var it in listType)
                    {
                        if (item.Key == it)
                        {
                            type_id = it.ToString();
                            _i++;
                            break;
                        }
                    }
                    if (_i > 0)
                        break;
                }
                if (type_id == "10")
                    _listfile = _listfile.Where(_ => _.ResourceType == type_id).ToList();
                else
                    _listfile = _listfile.Where(_ => _.ResourceStyle == type_id).ToList();
            }
            //直接返回此类型JSON类型  
            return JsonHelper.DeepEncodeJson(_listfile) + "|" + listType1 + "|" + bookname;
        }


    }
}