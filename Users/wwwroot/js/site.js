var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.on("ReceiveMessage", function (formUser, message) {
    var msg = formUser + ": " + message;
    var li = document.createElement("li");
    li.textContent = msg;
    $("#list").prepend(li);
});
connection.start();
$("#btnSend").on("click", function () {
    var formUser = $("#txtUser").val();
    var message = $("#txtMsg").val();

    connection.invoke("SendMessage", formUser, message);
});

$(document).ready(function () {

    $.ajax({
        url: '/api/GetCurrentUserName',
        method: 'GET',
        success: function (response) {
            $('#txtUser').val(response); 
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });
});
