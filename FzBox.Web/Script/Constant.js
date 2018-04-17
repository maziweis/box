var Constant = Constant || {};
Constant.do_Url = '/Handler.ashx';
Constant.flash_Url = '/FlashHandler.ashx';
Constant.serviceKey = 'KingsunCampusService';
Constant.code_mod_Url = "http://119.145.5.77:8027/"; // MOD基础数据地址
Constant.resource_Url = "http://reslib.kingsun.cn/"; //资源池地址 开发
/////评价统计///////////
Constant.classreport_Url = '/page/classratereport.aspx?Csstype=cloudHeader';   // 老师评价
Constant.gradereport_Url = '/page/GradeRateReport.aspx?Csstype=cloudHeader';   // 年级组长评价
Constant.schoolreport_Url = '/page/SchoolRateReport.aspx?Csstype=cloudHeader';   // 校长评价
////////////////////////////////////////////////
//////////服务定义
////////////////////////////////////////////////
Constant.UserManageService = "6512F1BC-8DEF-41A1-A5EF-5191075890A5";
Constant.ApplicationService = "B37A9E6E-4825-4F2F-93C6-42BDD9CC71E6";
///////////////////////////////////////////////
///////////////////////////////////////////////
////start///分页所用常亮参数
///////////////////////////////////////////////
Constant.PageSize = 15;
Constant.PageSizeList = [10, 15, 20, 30];
///////////////////////////////////////////////
////end///分页所用常亮参数
///////////////////////////////////////////////
//子窗体通知父窗体iframe框架自动适应内容高度
function autoParentHeight() {
    var iframeid = window.parent.document.getElementById("iframe1"); //iframe id
    if (document.getElementById) {
        if (iframeid && !window.opera) {
            var trueH, aH, tH, BH, maxH;
            if (iframeid.contentDocument && iframeid.contentDocument.body.offsetHeight) {
                //                var n = parseInt($("#liNum").val());                
                //                if (n > 0) {//针对课例集加载延迟...
                //                    var ulH = 0;
                //                    var rows = Math.ceil(n / 3);
                //                    //alert(rows);
                //                    if (rows > 0) ulH = rows * 248;
                //                    trueH = 200 + ulH;
                //                    //alert(trueH);
                //                }
                //                else {
                trueH = iframeid.contentDocument.body.offsetHeight;
                //                }               

            } else if (iframeid.Document && iframeid.Document.body.scrollHeight) {
                trueH = iframeid.Document.body.scrollHeight;
            }
            aH = window.parent.document.body.offsetHeight;
            tH = window.parent.document.getElementById("head").offsetHeight;
            bH = window.parent.document.getElementById("footer").offsetHeight;
            maxH = aH - tH - bH;
            if (maxH <= trueH) {
                window.parent.document.getElementById("iframe1").style.height = trueH + "px";
                //                window.parent.document.getElementById("left").style.height = (trueH + 24) + "px";
            }
            else {
                window.parent.document.getElementById("iframe1").style.height = (maxH - 24) + "px";
                //                window.parent.document.getElementById("left").style.height = maxH + "px";
            }
        }
    }
}