using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FzBox.Web
{
    /// <summary>
    /// 访问权限
    /// </summary>
    public class AuthAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                string mac = BLL.Common.GetMac();
                string id = "";

                if (Fz.Common.Caches.GetCache("id") == null)
                {
                    var path = System.Web.Hosting.HostingEnvironment.MapPath("~/");
                    var pathAll = path + "sn.key";

                    if (File.Exists(pathAll))
                    {
                        using (StreamReader sr = new StreamReader(pathAll, System.Text.Encoding.Default))
                        {
                            id = sr.ReadToEnd();
                        }
                    }
                    else//激活码文件不存在
                    {
                        filterContext.Result = new RedirectResult("/Activ/Not");
                        return;
                    }
                }
                else
                {
                    id = Fz.Common.Caches.GetCache("id").ToString();
                }

                string data = FzBox.Web.BLL.Common.Decrypt(id, "a%k8h5.o");
                if (data != null)
                {
                    string[] strs = data.Split('|');
                    if (strs.Length > 1)
                    {
                        string extime = strs[1];
                        string _mac = strs[0];

                        if (_mac == mac && DateTime.Now < DateTime.Parse(extime))//mac地址对，并且没有过期
                        {
                            if (Fz.Common.Caches.GetCache("id") == null)//如果缓存是空
                            {
                                Fz.Common.Caches.SetCache("id", id);
                            }
                        }
                        else
                        {
                            filterContext.Result = new RedirectResult("/Activ/Not");
                            return;
                        }
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("/Activ/Not");
                        return;
                    }
                }
                else
                {
                    filterContext.Result = new RedirectResult("/Activ/Not");
                    return;
                }
            }
        }
    }
}