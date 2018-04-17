using FzBox.Implement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FzBox.Web
{
    /// <summary>
    /// FlashHandler 的摘要说明
    /// </summary>
    public class FlashHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string formData = context.Request.Form["Form"];
                if (string.IsNullOrEmpty(formData))
                {
                    KingResponse response = new KingResponse();
                    response.Success = false;
                    response.ErrorMsg = "没有找到相应的数据包!";
                    response.RequestID = "";
                    response.Data = null;
                    context.Response.Write(JsonHelper.EncodeJson(response));
                    context.Response.End();
                    return;
                }
                KingForm form = JsonHelper.DecodeJson<KingForm>(formData);
                if (form == null)
                {
                    KingResponse response = new KingResponse();
                    response.Success = false;
                    response.ErrorMsg = "没有找到相应的数据包!";
                    response.RequestID = "";
                    response.Data = null;
                    context.Response.Write(JsonHelper.EncodeJson(response));
                    context.Response.End();
                    return;
                }
                string package = form.Pack;
                string returnStr = null;

                if (String.IsNullOrEmpty(package))
                {
                    //返回错误信息
                    returnStr = KingResponse.GetErrorResponseString("无法找到参数包");
                }
                else
                {
                    KingRequest request = KingRequest.DecodeRequest(package);
                    if (request == null)
                    {
                        returnStr = KingResponse.GetErrorResponseString("参数包解析失败");
                    }
                    else
                    {
                        FlashImplement obj = new FlashImplement();
                        KingResponse response;
                        if (obj != null)
                        {
                            try
                            {
                                response = obj.ProcessRequest(request);
                            }
                            catch
                            {
                                response = KingResponse.GetErrorResponse("服务接口内部错误，请联系管理员", request);
                            }
                        }
                        else
                        {
                            response = KingResponse.GetErrorResponse("无法实例化服务接口！", request);
                        }
                        returnStr = JsonHelper.EncodeJson(response);
                    }
                }
                context.Response.Write(returnStr);
            }
            catch
            {
                KingResponse response = new KingResponse();
                response.Success = false;
                response.ErrorMsg = "后台处理异常。";
                response.RequestID = "";
                response.Data = null;
                context.Response.Write(JsonHelper.EncodeJson(response));
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }


}