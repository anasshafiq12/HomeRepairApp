


var connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

connection.start().then(function () {
    console.error("Connection started");
}).catch(function (err) {
    console.error("Connection failed:", err.toString());
});

let email = "";


function openChat(userName, element) {
    email = userName;
    const popup = document.getElementById('chatPopup');
    const formSection = document.querySelector('.form-section');
    const chatbtn = document.getElementById('chatButton');

    // Set the connection (if not already set)
    connection.invoke("SetConnection").catch(function (err) {
        console.error("Failed to set connection:", err.toString());
    });

    // Toggle display of the chat popup vs. booking form.
    if (popup.style.display === "none" || popup.style.display === "") {
        formSection.style.display = "none";
        popup.style.display = "block";
        chatbtn.textContent = "Book a technician";
    } else {
        formSection.style.display = "block";
        popup.style.display = "none";
        chatbtn.textContent = "Chat with us";
    }

    // Retrieve the JSON messages from the data attribute and parse them.
    var messagesData = element.getAttribute('data-messages');
    var messages = JSON.parse(messagesData);

    // If messages is an array, iterate over each message object.
    if (Array.isArray(messages)) {
        messages.forEach(messageObj => {
            // Check that MessagesText exists and is an array.
            if (messageObj && messageObj.MessagesText && Array.isArray(messageObj.MessagesText)) {
                messageObj.MessagesText.forEach(text => {
                    // Display as received if the message was sent by the client (i.e. if ReceiverEmail equals admin's email).
                    if (messageObj.ReceiverEmail === "anas@admin.com") {
                        sendMessageByMe(text);
                    } else {
                        receiveMessageOfOther(text);
                    }
                });
            }
        });
    }
}

// Listen for the client's Send button click.
document.getElementById("sendMessageFromClient").addEventListener("click", function () {
    let messageInput = document.getElementById("messageInput");
    let messageText = messageInput.value.trim();

    if (messageText !== "") {
        connection.invoke("SendMessageByUser", messageText).catch(function (err) {
            console.error("Failed to send message:", err.toString());
        });
        messageInput.value = ""; // Clear input after sending.
        console.log("Message sent: ", messageText);
    }
});

// Function to add a new message to the chat.
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

    // Auto-scroll to the bottom.
    chatMessages.scrollTop = chatMessages.scrollHeight;
}

function receiveMessageOfOther(text) {
    addMessage(text, "received");
}
function sendMessageByMe(text) {
    addMessage(text, "sent");
}

// Listen for messages from SignalR.
connection.on("ReceiveMessage", function (message) {
    receiveMessageOfOther(message);
});
connection.on("MySentMessage", function (message) {
    sendMessageByMe(message);
});