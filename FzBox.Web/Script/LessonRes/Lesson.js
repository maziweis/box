function isFullScreen() {
    return (document.getElementById("layoutBody").clientHeight === window.screen.height);
}
function downFile(filename, filepath) {
    var pattern = new RegExp("[`~!#$^&()=|{}':;',\\[\\].<>/?~！%#￥……&（）&;|{}【】‘；：”“'。，、？]");
    var rs = "";
    for (var i = 0; i < filename.length; i++) {
        rs = rs + filename.substr(i, 1).replace(pattern, '');
    }
    filename = rs;
    window.location.href = "/LessonRes/GetFileByPath?filename=" + filename.replace("*", "'").replace("*", "'") + "&filepath=" + filepath;
}
function requestFullScreen(element) {
    if (element.requestFullscreen)
        element.requestFullscreen();
        //else if (window.ActiveXObject) {
        //    var WsShell = new ActiveXObject('WScript.Shell')
        //    WsShell.SendKeys('{F11}');
        //}
    else if (element.msRequestFullscreen)
        element.msRequestFullscreen();
    else if (element.mozRequestFullScreen)
        element.mozRequestFullScreen();
    else if (element.webkitRequestFullscreen)
        element.webkitRequestFullscreen();
}

function cancelFullScreen() {
    if (document.exitFullscreen)
        document.exitFullscreen();
        //else if (window.ActiveXObject) {
        //    var WsShell = new ActiveXObject('WScript.Shell')
        //    WsShell.SendKeys('{F11}');
        //}
    else if (document.msExitFullscreen)
        document.msExitFullscreen();
    else if (document.mozCancelFullScreen)
        document.mozCancelFullScreen();
    else if (document.webkitExitFullscreen)
        document.webkitExitFullscreen();
}

function toggleFullScreen(element) {
    if (isFullScreen()) {
        cancelFullScreen();
    }
    else {
        requestFullScreen(element || document.documentElement);
    }
    $("html").toggleClass("fullscreen");
}
function openDialog(type, src) {
    $("body").find(".backshadow").remove();
    //var payObj = null;
    //function InitPay() {
    //    payObj = $("#jquery_jplayer_1").jPlayer({
    //        ready: function () {
    //            StartPay();
    //        },
    //        supplied: "mp3",
    //        wmode: "window",
    //        swfPath: "../Scripts/"
    //    });
    //}
    //function StartPay() {
    //    payObj.jPlayer("setMedia", {
    //        mp3: src
    //    }).jPlayer("play");
    //}
    var htmlStr = "<div class=\"backshadow\">";
    if (type === "mp3" || type === "wav" || type === "wma") {
        htmlStr += "<div class=\"audioBox\" style=\"display:none\">";
    }
    else {
        htmlStr += "<div class=\"dialogBox\">";
    }
    htmlStr += "<button class=\"close\" onclick=\"closeDialog()\"></button>";
    htmlStr += "<div class=\"dialogContent\">";
    if (type === "png" || type === "jpg" || type === "ipeg" || type === "gif" || type === "bmp") {
        htmlStr += "<img src=\"" + src + "\" />";
    }
    else if (type === "ogg") {//.MP4 .ogg
        htmlStr += "<div class=\"video\" ><video controls=\"controls\" autoplay=\"autoplay\" width=\"100%\" controlsList=\"nodownload\">";
        htmlStr += "<source src=\"" + src + "\" type=\"video/ogg\" />";
        htmlStr += "<source src=\"" + src + "\" type=\"video/mp4\" />";
        htmlStr += "Your browser does not support the video tag.</video></div>"
    }
    else if (type === "swf") {
        htmlStr += '<div class=\"video\" >';
        htmlStr += '<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=11,0,0,0" id="start" align="middle" width="100%" height="100%">';
        htmlStr += '<param name="allowScriptAccess" value="sameDomain">';
        htmlStr += '<param name="allowFullScreen" value="true">';
        htmlStr += '<param name="movie" value="' + src + '">';
        htmlStr += '<param name="quality" value="high">';
        htmlStr += '<param name="scale" value= "scale">';
        htmlStr += '<param name="bgcolor" value="#ffffff">';
        htmlStr += '<embed src="' + src + '" quality="high" scale="scale" wmode="transparent"  bgcolor="#ffffff" width="100%" height="100%" swliveconnect="true" id="start" name="start" align="middle" allowscriptaccess="sameDomain" allowfullscreen="true" type="application/x-shockwave-flash" pluginspage="http://www.adobe.com/go/getflashplayer_cn">';
        htmlStr += '</object>';
        htmlStr += '</div>';
    }
    else if (type === "mp4" || type === "flv") {
        htmlStr += '<div class="video" id="videoplayer">';
        htmlStr += '</div>';
    }
    else if (type === "mp3" || type === "wav" || type === "wma") {
        if (!isIElt8) {
            htmlStr += '<audio preload="auto" controls id="audio1" controlsList="nodownload">';
            htmlStr += '<source src="' + src + '">';
            htmlStr += '</audio>';
        }
        else {
            htmlStr += '<embed id="audio" width="300" height="40" autostart="true" loop="false" type="application/mp3" controls="smallconsole" src="' + src + '" />';
        }
    }
    htmlStr += "</div></div></div>";
    $("body").append(htmlStr);
    if (type === "mp4" || type === "flv") {
        jwplayer("videoplayer").setup({
            autostart: false,
            skin: "/Content/Tools/jwplayer/glow.zip",
            stretching: "fill",
            flashplayer: "/Content/Tools/jwplayer/player.swf",
            width: '100%',
            height: '100%',
            levels: [{ file: src }]
        });
    }
    else if (type === "mp3" || type === "wav" || type === "wma") {
        $(".audioBox").show();
        $('#audio1').audioPlayer(); //InitPay(); 
        var audio = document.getElementById("audio1");
        $("#audio1").on('canplay', function () {
            duration = audio.duration;
            var other = duration % 3600;
            var minute = Math.round(other / 60);
            var second = Math.round(other % 60);
            if (minute < 10) minute = '0' + minute;
            if (second < 10) second = '0' + second;
            $(".audioplayer-time-duration").text(minute + ":" + second);
            $(".audioplayer .audioplayer-playpause").click();//autoplay属性不起作用
        })        
    }
}
function closeDialog() {
    $("body").find(".backshadow").remove();

}