"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:59162/chatHub") // Use correct backend port
    .build();


document.getElementById("sendBookingBtn").disabled = true;
// receive message
//connection.on("ReceiveMessage", function (user, message) {
//    var li = document.createElement("li");
//    document.getElementById("messagesList").appendChild(li);
//    li.textContent = `${user} says ${message}`;
//});

connection.start().then(function () {
    console.log("Connection started");
}).catch(function (err) {
    console.error("Connection failed:", err.toString());
});

document.addEventListener("click", function () {
    connection.invoke("SetConnection").then(function () {
        document.getElementById("sendBookingBtn").disabled = false;
    }).catch(function (err) {
        console.error("Failed to register user:", err.toString());
    });
});

document.getElementById("sendBookingBtn").addEventListener("click", function (event) {
    var targetUser = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessageToUser", targetUser, message).catch(function (err) {
        console.error("Failed to send message:", err.toString());
    });
    event.preventDefault();
});

//connection.onclose(async () => {
//    console.error("Connection lost, attempting to reconnect...");
//    setTimeout(() => connection.start(), 5000); // Retry after 5 seconds
//});

