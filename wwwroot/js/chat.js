
var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub") // Use correct backend port
    .build();


document.getElementById("sendBookingBtn").disabled = true;
// receive message
    document.getElementById("sendMessageFromClient").addEventListener("click", function () {
        let messageInput = document.getElementById("messageInput");
        let messageText = messageInput.value.trim();

        if (messageText !== "") {
            connection.invoke("SendMessageToUser", message); // Uncomment if you have this function
            messageInput.value = ""; // Clear input
            console.log("Message sent: ", messageText); // Debugging
        }
    });


function addMessage(text, type) {
    let chatMessages = document.getElementById("chatMessages");

    let messageDiv = document.createElement("div");
    messageDiv.classList.add("message", type);

    let contentDiv = document.createElement("div");
    contentDiv.classList.add("message-content");
    if (type === "received") {
        contentDiv.classList.add("text-black");
    }

    let p = document.createElement("p");
    p.classList.add("mb-0");
    p.textContent = text;

    let timeStamp = document.createElement("small");
    timeStamp.classList.add("text-muted");
    timeStamp.textContent = new Date().toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" });

    contentDiv.appendChild(p);
    messageDiv.appendChild(contentDiv);
    messageDiv.appendChild(timeStamp);
    chatMessages.appendChild(messageDiv);

    chatMessages.scrollTop = chatMessages.scrollHeight; // Auto-scroll
}

// Simulate receiving a message
function receiveMessage(text) {
    addMessage(text, "received");
}
function sendMessage(text) {
    addMessage(text, "sent");
}
// Simulating message reception from server
connection.on("ReceiveMessage", function (user, message) {
    receiveMessage(message);
});

connection.on("SendMessageByUser", function (user, message) {
    sendMessage(message);
});
connection.start().then(function () {
    console.log("Connection started");
}).catch(function (err) {
    console.error("Connection failed:", err.toString());
});

//document.addEventListener("click", function () {
//    connection.invoke("SetConnection").then(function () {
//        document.getElementById("sendBookingBtn").disabled = false;
//    }).catch(function (err) {
//        console.error("Failed to register user:", err.toString());
//    });
//});

//document.getElementById("sendBookingBtn").addEventListener("click", function (event) {
//    var targetUser = document.getElementById("userInput").value;
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessageToUser", targetUser, message).catch(function (err) {
//        console.error("Failed to send message:", err.toString());
//    });
//    event.preventDefault();
//});

//connection.onclose(async () => {
//    console.error("Connection lost, attempting to reconnect...");
//    setTimeout(() => connection.start(), 5000); // Retry after 5 seconds
//});

