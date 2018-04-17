var Common = Common || {};
Common.GUID = function () {
    var result, i, j;
    result = '';
    for (j = 0; j < 32; j++) {
        if (j == 8 || j == 12 || j == 16 || j == 20)
            result = result + '-';
        i = Math.floor(Math.random() * 16).toString(16).toUpperCase();
        result = result + i;
    }
    return result
}
Common.RequestCore = function (core) {
    if (core) {
        this.ID = core.ID;
        this.Function = core.Function;
        this.Data = core.Data;
    }
    else {
        this.ID = "";
        this.Function = "";
        this.Data = "";
    }
}
Common.Request = function (request) {
    if (request) {
        this.RID = request.RID;
        this.SKEY = request.SKEY;
        this.Pack = request.Pack;
        this.Ticket = Common.Cookie.getcookie(Constant.serviceKey);
    }
    else {
        this.RID = "";
        this.SKEY = "";
        this.Pack = new Common.RequestCore();
        this.Ticket = Common.Cookie.getcookie(Constant.serviceKey);
    }
}

Common.Ajax = function (serviceKey, funcName, data, callback) {
    var request = new Common.Request();
    request.RID = Common.GUID();
    request.SKEY = serviceKey;
    request.Pack.ID = request.RID;
    request.Pack.Function = funcName;
    request.Pack.Data = $.toJSON(data);
    request.Pack = $.toJSON(request.Pack);
    var sendValues = { Form: $.toJSON(request) };
    var url = Constant.do_Url + "?rand=" + Math.random();
    var async_Sign = true;
    if (typeof callback == "function") {
        async_Sign = true;
    } else {
        async_Sign = false;
    }
    var obj = null;
    Loading.show();
    $.ajax({
        type: "POST",
        url: url,
        data: sendValues,
        async: async_Sign,
        success: function (response) {
            Loading.hide();
            try {
                response = eval("(" + response + ")")
            }
            catch (exception) {
                //alert("返回数据格式错误！");
            }
            if (async_Sign) {
                callback(response);
            } else {
                obj = response;
            }
        },
        error: function (request, status, error) {
            Loading.hide();
            //alert("请求失败！提示：" + status + error);
        }
    });
    return obj;
}

Common.Ajax2 = function (serviceKey, funcName, data, callback) {
    var request = new Common.Request();
    request.RID = Common.GUID();
    request.SKEY = serviceKey;
    request.Pack.ID = request.RID;
    request.Pack.Function = funcName;
    request.Pack.Data = $.toJSON(data);
    request.Pack = $.toJSON(request.Pack);
    var sendValues = { Form: $.toJSON(request) };
    var url = Constant.do_Url + "?rand=" + Math.random();
    var async_Sign = true;
    if (typeof callback == "function") {
        async_Sign = true;
    } else {
        async_Sign = false;
    }
    var obj = null;
    $.ajax({
        type: "POST",
        url: url,
        data: sendValues,
        async: async_Sign,
        success: function (response) {
            Loading.hide();
            try {
                response = eval("(" + response + ")")
            }
            catch (exception) {
                //alert("返回数据格式错误！");
            }
            if (async_Sign) {
                callback(response);
            } else {
                obj = response;
            }
        },
        error: function (request, status, error) {
            Loading.hide();
            //alert("请求失败！提示：" + status + error);
        }
    });
    return obj;
}
Common.DirectAjax = function (directURL, funcName, data, callback) {
    var request = new Common.Request();
    request.RID = Common.GUID();
    request.SKEY = "";
    request.Pack.ID = request.RID;
    request.Pack.Function = funcName;
    request.Pack.Data = data;
    request.Pack = $.toJSON(request.Pack);
    var sendValues = { Form: $.toJSON(request) };
    var url = directURL + "?rand=" + Math.random();
    var async_Sign = true;
    if (typeof callback == "function") {
        async_Sign = true;
    } else {
        async_Sign = false;
    }
    var obj = null;
    $.ajax({
        type: "POST",
        url: url,
        data: sendValues,
        async: async_Sign,
        success: function (response) {
            try {
                response = eval("(" + response + ")")
            }
            catch (exception) {
                //alert("返回数据格式错误！");
            }
            if (async_Sign) {
                callback(response);
            } else {
                obj = response;
            }
        },
        error: function (request, status, error) {
            //alert("请求失败！提示：" + status + error);
        }
    });
    return obj;
}
Common.CodeAjax_City = function (sevice, data, callback) {
    var sendValues = { t: data };
    var url = Constant.code_mod_Url + sevice + "?rand=" + Math.random();
    var async_Sign = true;
    if (typeof callback == "function") {
    } else {
        //////不支持同步
        return null;
    }
    var obj = null;
    $.ajax({
        type: "POST",
        url: url,
        data: sendValues,
        dataType: "jsonp",
        async: true,
        success: function (response) {
            if (async_Sign) {
                callback(response);
            } else {
                obj = response;
            }
        },
        error: function (request, status, error) {
        }
    });
    return obj;
}

Common.District = Common.District || {};
/////////////通过条件获取学校列表/////////////////////
Common.District.GetSchool = function (districtID, townID, keyword, callback) {
    var obj = {
        DistrictID: districtID,
        TownsID: townID,
        SchoolName: keyword
    };
    Common.CodeAjax_City("do.school", obj, function (data) {
        if (typeof callback == "function") {
            callback(data);
            return;
        }
    });
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// Start ////Cookie 管理 Start 
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Common.Cookie = Common.Cookie || {};
Common.Cookie.getcookie = function (cookiename) {
    var strCookie = document.cookie;
    var arrCookie = strCookie.split("; ");
    var ck;
    for (var i = 0; i < arrCookie.length; i++) {
        var arr = arrCookie[i].split("=");
        if (cookiename == arr[0]) {
            return unescape(arr[1]);
            break;
        }
    }
    return "";
}
Common.Cookie.setcookie = function (cookiename, val, day) {
    if (day) { day = day; } else { day = 0; }
    if (day == 0) { document.cookie = cookiename + "=" + escape(val) + ";path=/"; }
    else {
        var expires = new Date();
        expires.setTime(expires.getTime() + (1000 * 24 * 3600 * day));
        document.cookie = cookiename + "=" + escape(val) + ";path=/;expires=" + expires.toGMTString();
    }
}

Common.Cookie.delcookie = function (cookiename) {
    var expires = new Date();
    expires.setTime(expires.getTime() - (1000 * 24 * 3600 * 365));
    document.cookie = cookiename + "=;path=/;expires=" + expires.toGMTString();
}
Common.Cookie.delallcookie = function () {
    var strCookie = document.cookie;
    var arrCookie = strCookie.split("; ");
    for (var i = 0; i < arrCookie.length; i++) {
        var arr = arrCookie[i].split("=");
        Common.delcookie(arr[0]);
    }
    return 0;
};
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// End ////Cookie 管理 Start 
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// start ////过滤文本框中输入的特殊字符
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Common.ValidateTxt = function (txtValue) {
    var forbidChar = new Array("@", "#", "$", "%", "^", "&", "*", "……", "￥", "×", "\"", "<", ">");
    for (var i = 0; i < forbidChar.length; i++) {
        if (txtValue.indexOf(forbidChar[i]) >= 0) {
            return "您输入的信息: " + txtValue + " 中含有非法字符: " + forbidChar[i] + " 请更正！";
        }
    }
    return "";
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// End ////过滤文本框中输入的特殊字符
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// start ////时间格式内容转换为字符型 eg：yyyy年MM月dd日 hh时mm分ss秒
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Common.FormatTime = function (time, format) {
    if (format == undefined || format == "") {
        format = "yyyy年MM月dd日 hh时mm分";
    }
    if (time) {
        var date = new Date(parseInt(time.substring(6, time.length - 2)))
        return date.format(format);
    }
}
Date.prototype.format = function (format) {
    var o =
    {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(),    //day
        "h+": this.getHours(),   //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format))
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(format))
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
return format;
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// end ////时间格式内容转换为字符型 eg：yyyy年MM月dd日 hh时mm分ss秒
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// start ////获取url参数
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Common.QueryString = {
    data: {},
    Initial: function () {
        var aPairs, aTmp;
        var queryString = new String(window.location.search);
        queryString = queryString.substr(1, queryString.length); //remove   "?"     
        aPairs = queryString.split("&");
        for (var i = 0; i < aPairs.length; i++) {
            aTmp = aPairs[i].split("=");
            this.data[aTmp[0]] = aTmp[1];
        }
    },
    GetValue: function (key) {
        return this.data[key];
    }
}
Common.QueryString.Initial();
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// end ////获取url参数
///////////////////////////////////////////////////////////////////////


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// start ////公共验证函数
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Common.Validate = Common.Validate || {};
////是否为数字
Common.Validate.IsNumber = function (str) {
    var reg = /^\d+(\.\d+)?$/;
    return reg.test(str);
};
//是否为整数
Common.Validate.IsInt = function (str) {
    var reg = /^-?\d+$/;
    return reg.test(str);
}
////是否版本号
Common.Validate.IsVersion = function (str) {
    var reg = /^[1-9]{1}\.\d{1,2}$/;
    return reg.test(str);
};
///正则验证是否是正确的URL地址
Common.Validate.IsURL = function (str_url) {
    var strRegex = "^((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?";
    var re = new RegExp(strRegex);
    //re.test()
    if (re.test(str_url)) {
        return (true);
    } else {
        return (false);
    }
}
Common.Validate.IsMobileNo = function (phone) {
    var regexp = /^1[3|4|5|8][0-9]\d{8}$/
    return regexp.test(phone);
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// end ////公共验证函数
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Common.showMsg = function(msg) {
    $.messager.show({
        title: '系统提示',
        msg: msg,
        showType: 'show'
    });
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// start ////用户心跳
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Common.AliveFaildCount = 0;//心跳失败次数
Common.KeepAlive = function () {
    var loginCookie = Common.Cookie.getcookie(".KingsunWisdomCampus");
    if (loginCookie == undefined || loginCookie == null || loginCookie.length == 0) {
        //尚未登录，取消心跳
        return;
    }
    var cookie = Common.Cookie.getcookie("KingsunGuid");
    if (cookie == undefined || cookie == null || cookie.length == 0) {
        //找不到对应的cookie
        return;
    }
    Common.Ajax("SessionImplement", "KeepAlive", cookie, function (data) {
        if (data.Data.Success) {
            if (typeof (data.Data.Result) != undefined && !data.Data.Result) {
                var DialogResult = alert("提示：您的帐号在别处登录，您已经被迫下线！");
                window.location = "/LoginOut.aspx";
                return;
            }
        } else {
            ////同一个页面允许失败3次，第四次失败则导航到登陆页
            if (Common.AliveFaildCount < 3) {
                Common.AliveFaildCount = Common.AliveFaildCount + 1;
            } else {
                window.location = "/LoginOut.aspx";
                return;
            }
        }
        window.setTimeout("Common.KeepAlive()", 10000);
    });
}
$(function () {
    //window.setTimeout("Common.KeepAlive()", 10000);
});
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//// end ////用户心跳
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Common.HtmlEncode = function (str) {
    if (str == null) return "";
    return str.toString().replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/\"/g, "&quot;").replace(/\'/g, "&#39;");
}
//把字符串进行HTML反编码
Common.HtmlDecode = function (str) {
    if (str == null) return "";
    return str.toString().replace(/\&amp\;/g, '\&').replace(/\&gt\;/g, '\>').replace(/\&lt\;/g, '\<').replace(/\&quot\;/g, '\"').replace(/\&\#39\;/g, '\'');
}



var Loading = {
    text: '操作正在执行中，请稍候。。。', //Loading默认显示的文字
    //获取滚动条距离上边顶部的距离
    getScrollTop: function () {
        var scrollTop = 0;
        if (document.documentElement && document.documentElement.scrollTop) {
            scrollTop = document.documentElement.scrollTop;
        } else if (document.body) {
            scrollTop = document.body.scrollTop;
        }
        return scrollTop;
    },
    //获取内部内容的总高度,
    getScrollHeight: function () {
        return Math.max(document.body.scrollHeight, document.documentElement.scrollHeight);
    },
    //是获取可见内容的高度
    getHeight: function () {
        if (window.innerHeight != undefined) {
            return window.innerHeight;
        } else {
            var B = document.body, D = document.documentElement;
            return Math.min(D.clientHeight, B.clientHeight)
        }
    },
    //显示阴影
    showShadow: function () {
        var maskHeight = this.getScrollHeight() + "px";
        var shadowDiv = document.createElement("div");
        shadowDiv.innerHTML = "";
        shadowDiv.setAttribute('id', 'shadowDiv_MASK');
        shadowDiv.setAttribute('style', 'position:fixed; position: absolute; z-index: 999;left:0;top:0;display:block;width:100%;height:' + maskHeight + '; opacity:0.3;filter: alpha(opacity=30);-moz-opacity: 0.6; background:#fff;');
        var body = document.getElementsByTagName("body")[0];
        body.appendChild(shadowDiv);
    },
    //关闭阴影
    hideShadow: function () {
        var body = document.getElementsByTagName("body")[0];
        var shadowDiv_MASK = document.getElementById('shadowDiv_MASK');
        if (body && shadowDiv_MASK) {
            body.removeChild(shadowDiv_MASK);
        }
    },
    //显示Loading
    show: function (txt) {
        var top = this.getScrollTop() + (this.getHeight() / 2) + "px";
        Loading.showShadow();
        var me = this;
        if (txt) {
            me.text = txt;
        }
        var loadingDiv = document.createElement("div");
        loadingDiv.innerHTML = me.text;
        loadingDiv.setAttribute('id', 'loadingDiv');
        loadingDiv.setAttribute('style', 'top:' + top + ';left:40%;z-index: 9999;position:absolute;background: #fff;width: 200px;height: 50px;line-height: 50px;text-align: center;');
        var body = document.getElementsByTagName("body")[0];
        body.appendChild(loadingDiv);
    },
    //显示Loading
    showload: function () {
        var html = '';
        html += '<div id="loadingDiv"></div>'
        $("body").append(html);
    },
    hideload: function () {
        $("#loadingDiv").remove();
    },
    //关闭Loading
    hide: function () {
        var body = document.getElementsByTagName("body")[0];
        var loadingDiv = document.getElementById('loadingDiv');
        if (body && loadingDiv) {
            body.removeChild(loadingDiv);
        }
        Loading.hideShadow();
    }
}

// 资源库数据调用方法
Common.ResourceAjax = function (sevice, data, callback) {
    var sendValues = { t: data };
    var url = Constant.resource_Url + sevice + "?rand=" + Math.random();

    var async_Sign = true;
    if (typeof callback == "function") {
    } else {
        //////不支持同步
        return null;
    }
    var obj = null;
    $.ajax({
        type: "POST",
        url: url,
        data: sendValues,
        dataType: "jsonp",
        async: true,
        beforeSend: function () {
            //alert("正在加载资源数据");
            //$("#loading").html("<img src='' />");
        },
        success: function (response) {
            $("#loading").empty();
            if (async_Sign) {
                callback(response);
            } else {
                obj = response;
            }
        },
        error: function (request, status, error) {
        }
    });
    return obj;
}

// 基础数据MOD数据调用方法
Common.CodeAjax = function (sevice, data, callback) {
    var sendValues = { t: data };
    var url = Constant.code_mod_Url + sevice + "?rand=" + Math.random();
    var async_Sign = true;
    if (typeof callback == "function") {
    } else {
        //////不支持同步
        return null;
    }
    var obj = null;
    $.ajax({
        type: "POST",
        url: url,
        data: sendValues,
        dataType: "jsonp",
        async: true,
        success: function (response) {
            if (async_Sign) {
                callback(response);
            } else {
                obj = response;
            }
        },
        error: function (request, status, error) {
        }
    });
    return obj;
}

////读取对应教材章节目录  //Stage Grade Subject Booklet Edition
Common.GetStandardCatalog = function (Stage, Grade, Subject, Booklet, Edition, callback) {
    var obj = {
        Stage: Stage,
        Grade: Grade,
        Subject: Subject,
        Booklet: Booklet,
        Edition: Edition
    };
    Common.CodeAjax("StandardCatalog.sun", obj, function (data) {
        if (typeof callback == "function") {
            callback(data);
            return;
        }
    });
}
////读取对应教材章节目录  //BookId
Common.GetCatalogByBookId = function (BookId, callback) {
    var obj = {
        BookId: BookId
    };
    Common.CodeAjax("GetCatalogByBookId.sun", obj, function (data) {
        if (typeof callback == "function") {
            callback(data);
            return;
        }
    });
}









