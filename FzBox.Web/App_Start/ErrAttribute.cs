using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FzBox.Web
{
    public class ErrAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// 全局异常处理
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            //导向友好错误界面
            //filterContext.Result = new RedirectResult("/Home/Index");
            filterContext.HttpContext.Response.Charset = "UTF-8";
            filterContext.HttpContext.Response.ContentEncoding = System.Text.Encoding.UTF8;

            filterContext.ExceptionHandled = true;//重要！！告诉系统异常已处理！！如果没有这个步骤，系统还是会按照正常的异常处理流程走

            Exception e = filterContext.Exception;
            BLL.Common.WriteLogError(e.ToString());

            base.OnException(filterContext);
        }
    }
}