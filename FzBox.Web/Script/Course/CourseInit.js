/// <reference path="../Common.js" />

var Kingsun = Kingsun || {}
Kingsun.WisdomCampus = Kingsun.WisdomCampus || {}
Kingsun.WisdomCampus.CourseView = Kingsun.WisdomCampus.CourseView || {}
Kingsun.WisdomCampus.CourseView = function () {
    this.VLesson = function (lessonID, Event) {
        var data = lessonID;
        return Common.DirectAjax(Constant.flash_Url, "validatelesson", data, Event);
    };
    this.VLesson_Second = function (lessonID, lessonKey, Event) {
        var data = {
            lessonID: lessonID,
            lessonKey:lessonKey
        };
        return Common.DirectAjax(Constant.flash_Url, "validatelesson_Second", $.toJSON(data), Event);
    };
    this.VActivation = function (lessonID, Event) {
        var data = lessonID;
        return Common.DirectAjax(Constant.flash_Url, "isreactivated", data, Event);
    };
    this.GetExperience = function (lessonID, Event) {
        var data = lessonID;
        return Common.DirectAjax(Constant.flash_Url, "getexperience", data, Event);
    };
    this.CDecrypt = function (encryptStr, Event) {
        var data = encryptStr;
        return Common.DirectAjax(Constant.flash_Url, "decrypt", data, Event);
    };
}
var courseViewObj = new Kingsun.WisdomCampus.CourseView();
var current_Course_Key = "";
var isInternetExplorer = navigator.appName.indexOf("Microsoft") != -1;

var originalTitle = document.title;
function pageLoad() {
    document.getElementById("start").style.width = document.body.clientWidth + "px";
    document.getElementById("start").style.height = document.body.clientHeight + "px";
    if (('onhashchange' in window) && ((typeof document.documentMode === 'undefined') || document.documentMode == 8)) {
        // 浏览器支持onhashchange事件  
        if (document.title != originalTitle) {
            document.title = originalTitle;
        }
    } else {
        // 不支持则用定时器检测的办法  
        setInterval(function () {
            if (document.title != originalTitle) {
                document.title = originalTitle;
            }
        }, 250);
    }
}

function validatelesson(lessonID) {
    if (arguments.length == 1) {
        current_Course_Key = lessonID;
        var response = courseViewObj.VLesson(lessonID);
        if (response && response.Success) {
            return response.Data;
        } else {
            return "";
        }
    } else {
        if (arguments.length == 2) {
            var lessonKey = arguments[1];
            var response = courseViewObj.VLesson_Second(lessonID, lessonKey);
            if (response && response.Success) {
                return response.Data;
            } else {
                return "";
            }
        } else {
            return "";
        }
    }
}
function getTrialUnit(obj) {
    var response = courseViewObj.GetExperience(current_Course_Key);
    if (response && response.Success) {
        return response.Data;
    } else {
        return false;
    }

}
function goPurchase(obj) {
    var response = courseViewObj.GetExperience(current_Course_Key);
    alert("去购买相应的课程：" + obj);

}
function isreactivated(obj) {
    var response = courseViewObj.VActivation(current_Course_Key);
    if (response && response.Success) {
        return response.Data;
    } else {
        return false;
    }

}
function decrypt(encryptStr) {
    var response = courseViewObj.CDecrypt(encryptStr);
    if (response && response.Success) {
        return response.Data;
    } else {
        return "";
    }
}
function startrecord() {
    window.document.start.realRecord();
    return "startrecord";
}
function stoprecord() {
    window.document.start.stoprecord();
    return "stoprecord";
}
function playrecord() {
    window.document.start.playrecord();
    return "playrecord";
}
function stopplayer() {
    window.document.start.stopPlaySound();
    return "stopplayer";
}
function nomicrophone() {
    alert("没有找到麦克风！");
    return "nomicrophone";
}

function start_DoFSCommand(command, args) {
    var startObj = isInternetExplorer ? document.all.start : document.start;
    if (command == "finalquit" || command == "quit") {
        window.close();
    } else
        if (command == "downloadpep") {
            args = args.substring(0, args.indexOf("|"));
            var fileURL = window.open(args, "_blank", "height=0,width=0,toolbar=no,menubar=no,scrollbars=no,resizable=on,location=no,status=no");
            fileURL.document.execCommand("SaveAs");
            fileURL.window.close();
            fileURL.close();
            window.open(args)
        } else if (command == "startrecord") {
            return startrecord();
        } else if (command == "stoprecord") {
            return stoprecord();
        } else if (command == "playrecord") {
            return playrecord();
        } else if (command == "nomicrophone") {
            return nomicrophone();
        } else if (command == "stopplayer") {
            return stopplayer();
        } 
        else {
            //alert(command);
        }
    //
    // 代码放在此处。
    //
}
function start_FSCommand(command, args) {
    return start_DoFSCommand(command, args);
}

$(function () {
    pageLoad();
});