$(document).ready(function () {
    //Command: toastr["info"]('Page Loaded!')


    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-bottom-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

});


var convArrToObj = function (array) {
    var thisEleObj = new Object();
    if (typeof array == "object") {
        for (var i in array) {
            var thisEle = convArrToObj(array[i]);
            thisEleObj[i] = thisEle;
        }
    } else {
        thisEleObj = array;
    }
    return thisEleObj;
}

var applySecurity = function () {

    $.ajax({
        url: "/SecurityModule/Account/GetActionPermission?url=" + window.location.pathname,
        cache: true,
        success: function (data) {
            if (data !== "")
            {
                $(".pnlView").hide();
                $(".btnSave").hide();
                $(".btnEdit").hide();
                var strs = data.split("|");
                for (var i = 0; i < strs.length - 1; i++) {
                    if (strs[i] != null) {                        
                        $("." + strs[i]).show();
                    }
                }
            }            
        }
    });

}

