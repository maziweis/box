using FzBox.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FzBox.Web.Controllers
{
    [AllowAnonymous]
    public class ActivController : Controller
    {
        /// <summary>
        /// 没激活
        /// </summary>
        /// <returns></returns>
        public ActionResult Not()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Not(VM_Activ m)
        {
            string mac = BLL.Common.GetMac();
            string data = FzBox.Web.BLL.Common.Decrypt(m.Code, "a%k8h5.o");
            if (data != null)
            {
                string[] strs = data.Split('|');
                if (strs.Length > 1)
                {
                    string extime = strs[1];
                    string _mac = strs[0];

                    if (_mac == mac && DateTime.Now < DateTime.Parse(extime))//mac地址对，并且没有过期
                    {
                        var path = System.Web.Hosting.HostingEnvironment.MapPath("~/");
                        var pathAll = path + "sn.key";

                        using (FileStream fs = new FileStream(pathAll, FileMode.Create, FileAccess.Write))
                        {
                            byte[] Bt = new byte[m.Code.Length * 2];
                            Bt = Encoding.UTF8.GetBytes(m.Code);
                            fs.Position = fs.Length;
                            fs.Write(Bt, 0, Bt.Length);
                            fs.Close();
                        }

                        Fz.Common.Caches.SetCache("id", m.Code);

                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("Code", "激活码无效或过期了！");
                    return View(m);
                }
            }
            else
            {
                ModelState.AddModelError("Code", "激活码无效！");
            }

            return View(m);
        }
    }
}