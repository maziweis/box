function ResLog(appType, _edition, _subject, _appName) {
    $.ajax({
        type: 'post',
        url: '/Home/ResLog',
        data: { app: appType, edition: _edition, subject: _subject, appName: _appName },
        dataType: 'json'
    });
}

function fixedFoot() {
    var allH = $(window).height();
    var footH = $(".footer").height() + 1;
    $(".mainBody").css("min-height", allH - footH - 30);
}