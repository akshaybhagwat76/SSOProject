
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.start().then(function () {
    console.log("connected");
    callUserOnline();
});
connection.on("ReceiveMessage", function (user, count, msg, ids) {

    //$.post("/Users/GetUsersByTenant", { tcode: $("#selectTenant").val() }, function (htmlData) {
    //    $("#gridBody").html(htmlData);

    //    if (ids != undefined) {
    //        $(ids).each(function (index, item) {
    //            var udiv = "." + item.userName;
    //            console.log(udiv);
    //            console.log(item.count);
    //            $(udiv).html("Logged In " + item.count);
    //        });
    //    }
    //});

});
connection.on("OfflineMessage", function (user, msg, count, ids) {

    if (ids != undefined) {
        $(ids).each(function (index, item) {
            var udiv = "." + item.userName;
            console.log(udiv);
            console.log(item.count);
            if (item.count > 0)
                $(udiv).html("Logged In " + item.count);
            else
                $(udiv).html("No");
        });
    }

});

if (document.getElementById("logout-btn") != undefined) {
    document.getElementById("logout-btn").addEventListener("click", function (event) {

        var user = document.getElementById("logoutUsername").value;
        connection.invoke("UserOffline", user);
    });
}
function callUserOnline() {

    var user = document.getElementById("hdnUserNameforSignalr").value;
    connection.invoke("CountUser", user);
}

//$(document).ready(function () {
//    getUsersBytenant();
//});
//function getUsersBytenant() {
//    $.post("/Users/GetUsersByTenant", { tcode: $("#selectTenant").val() }, function (htmlData) {
//        $("#gridBody").html(htmlData);
//        callUserOnline();
//    })
//}
function UpdateStatus(value, id, action) {
    var payLoad = '';
    
    if (action === "deleteUser") {
        var response = confirm('Are you sure to delete this tenant?')
        if (response == true) {
            payLoad = { 'Id': id, 'IsDelete': value, 'action': action };
            $.post("/changetenantStatus", payLoad, function (htmlData) {
                if (htmlData.status == 'Saved') {
                    $(".bg-success").html("Status updated.")
                    $(".bg-success").show();
                }
                else
                    $(".bg-danger").html("Error in updating.")
            })
        }
    }
    else {

        if (action === "statusUpdate") {
            payLoad = { 'Id': id, 'IsActive': value, 'action': action };
        }
        else if (action === "onHoldUpdate") {
            payLoad = { 'Id': id, 'IsOnHold': value, 'action': action };
        }

        $.post("/changetenantStatus", payLoad, function (htmlData) {
            if (htmlData.status == 'Saved') {
                $(".bg-success").html("Status updated.")
                $(".bg-success").show();
            }
            else
                $(".bg-danger").html("Error in updating.")
        })
    }
}