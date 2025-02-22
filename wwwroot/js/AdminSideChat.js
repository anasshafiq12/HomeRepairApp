

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
let email = "";

connection.start().then(function () {
    console.log("Connected to SignalR");
    connection.invoke("SetConnection").catch(console.error);
}).catch(console.error);

function openChat(userName, element) {
    email = userName;
    var messagesData = element.getAttribute('data-messages');
    var messages = JSON.parse(messagesData);

    document.getElementById("chatsSidebar").style.display = "none";
    document.getElementById("chatSection").innerHTML = `
            <div class="chat-main">
                <div class="chat-header">
                    <i class="bi bi-x-lg m-2" id="closeChat" onclick="closeChat()"></i>
                    <h6>Chat with ${userName}</h6>
                </div>
                <div class="chat-messages" id="chatMessages"></div>
                <div class="chat-input">
                    <div class="input-group">
                        <input type="text" class="form-control" id="messageInput" placeholder="Type your message...">
                        <button class="btn btn-success" id="sendMessageFromAdmin">Send</button>
                    </div>
                </div>
            </div>`;

    messages.forEach(messageObj => {
        if (messageObj.SenderEmail == userName) {
            messageObj.MessagesText.forEach(text => {
                if (messageObj.SenderEmail === "anas@admin.com") {
                    sendMessageByme(text);
                } else {
                    receiveMessageOfOther(text);
                }
            });
        }
    });
}

function closeChat() {
    document.getElementById("chatsSidebar").style.display = "block";
    document.getElementById("chatSection").innerHTML = `
            <div class="chat-main">
                <div class="chat-header">
                    <h6>Select a chat to start messaging</h6>
                </div>
            </div>`;
}

function addMessageAtAdmin(text, type) {
    let chatMessages = document.getElementById("chatMessages");
    let messageDiv = `<div class="message ${type}">
            <div class="message-content">${text}</div>
            <small class="text-muted">${new Date().toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" })}</small>
        </div>`;
    chatMessages.insertAdjacentHTML("beforeend", messageDiv);
    chatMessages.scrollTop = chatMessages.scrollHeight;
}

function receiveMessageOfOther(text) {
    addMessageAtAdmin(text, "received");
}

function sendMessageByme(text) {
    addMessageAtAdmin(text, "sent");
}

document.addEventListener("click", function (event) {
    if (event.target.id === "sendMessageFromAdmin") {
        let messageInput = document.getElementById("messageInput");
        let messageText = messageInput.value.trim();
        if (messageText) {
            connection.invoke("SendMessageToUser", email, messageText).catch(console.error);
            sendMessageByme(messageText);
            messageInput.value = "";
        }
    }
});

connection.on("ReceiveMessageAtAdmin", function (message) {
    receiveMessageOfOther(message);
});


connection.on("MySentMessage", function (message) {
    sendMessageByme(message);
})