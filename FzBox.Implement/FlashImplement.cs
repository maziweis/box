using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FzBox.Implement
{
    public class FlashImplement : BaseImplement
    {
        public override KingResponse ProcessRequest(KingRequest request)
        {
            if (string.IsNullOrEmpty(request.Function))
            {
                return KingResponse.GetErrorResponse("无法确定接口信息！", request);
            }
            if (string.IsNullOrEmpty(request.Data))
            {
                return KingResponse.GetErrorResponse("提交的数据不能为空！", request);
            }
            FlashManagement manage = new FlashManagement();
            KingResponse response = null;
            switch (request.Function.Trim())
            {
                case "validatelesson"://///
                    response = manage.Validatelesson(request);
                    break;
                case "isreactivated"://///
                    response = manage.Isreactivated(request);
                    break;
                case "decrypt"://///
                    response = manage.Decrypt(request);
                    break;
                case "validatelesson_Second"://///
                    response = manage.Validatelesson_Second(request);
                    break;
                case "getexperience"://///
                    response = manage.GetExperience(request);
                    break;
                default:
                    response = KingResponse.GetErrorResponse("未找到相应的接口!", request);
                    break;
            }
            return response;
        }
    }
}
