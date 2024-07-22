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
connection.start().then(function () {
    console.log("SignalR connected");
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("JoinGroup", function (groupName) {
    connection.invoke("JoinGroup", groupName).catch(function (err) {
        return console.error(err.toString());
    });
});

connection.on("ReceiveMessage", function (fromUser, message) {
    // Handle receiving messages
});
document.getElementById("sendButton").addEventListener("click", function (event) {
    const message = document.getElementById("messageInput").value;
    const groupName = /* Logic to get the current group name */;
    connection.invoke("SendMessage", currentUser, groupName, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

