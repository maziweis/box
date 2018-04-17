using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FzBox.Implement
{
    public class FlashManagement
    {
        public KingResponse Validatelesson(KingRequest request)
        {
            if (request != null)
            {
                string courseKey = request.Data.ToString();
                /////////////////////////////////////////////////////////////////////////
                /////////////去验证课程的有效性，并返回结果
                /////////////////////////////////////////////////////////////////////////
                return KingResponse.GetResponse(request, "12121||1");
            }
            return KingResponse.GetErrorResponse("请求无效", request);
        }

        public KingResponse GetExperience(KingRequest request)
        {
            if (request != null)
            {
                string courseKey = request.Data.ToString();
                /////////////////////////////////////////////////////////////////////////
                /////////////返回课程试用章节
                /////////////////////////////////////////////////////////////////////////
                return KingResponse.GetResponse(request, "1");
            }
            return KingResponse.GetErrorResponse("请求无效", request);
        }

        public KingResponse Validatelesson_Second(KingRequest request)
        {
            if (request != null)
            {
                SubmitData data = JsonHelper.DecodeJson<SubmitData>(request.Data.ToString());
                if (data == null)
                {
                    return KingResponse.GetErrorResponse("请求无效", request);
                }
                var decryptStr = xxtea.Decrypt(data.lessonKey, "szfzKINGsunsoft");
                /////////////////////////////////////////////////////////////////////////
                /////////////去验证课程的有效性，并返回结果
                /////////////////////////////////////////////////////////////////////////


                return KingResponse.GetResponse(request, "12121||1||" + decryptStr);
            }
            return KingResponse.GetErrorResponse("请求无效", request);
        }
        public KingResponse Isreactivated(KingRequest request)
        {
            if (request != null)
            {
                string courseKey = request.Data.ToString();
                /////////////////////////////////////////////////////////////////////////
                /////////////去验证课程的是否激活，并返回结果
                /////////////////////////////////////////////////////////////////////////
                return KingResponse.GetResponse(request, true);
            }
            return KingResponse.GetErrorResponse("请求无效", request);
        }
        public KingResponse Decrypt(KingRequest request)
        {
            if (request != null)
            {
                string encryptStr = request.Data.ToString();
                AESEncrypt encrypt = new AESEncrypt();                
                /////////////////////////////////////////////////////////////////////////
                /////////////解密
                /////////////////////////////////////////////////////////////////////////
                return KingResponse.GetResponse(request, encrypt.AESDecrypt(encryptStr));
            }
            return KingResponse.GetErrorResponse("请求无效", request);
        }

    }
    public class SubmitData
    {
        public string lessonID
        {
            get;
            set;
        }
        public string lessonKey
        {
            get;
            set;
        }
    }
        
}
