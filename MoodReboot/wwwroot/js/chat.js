"use strict";

function initializeConnection(connectionString) {
    $("#btnChatsSidenav").click(function () {
        $("#chatsSidenav").fadeToggle("fast");
        $("#chatsSidenav").height($("#chatWindow").height() - 50);
        $("#chatsSidenav").width($("#chatWindow").width() - 5);
    });

    const connection = new signalR.HubConnectionBuilder().withUrl(connectionString).build();
    //Disable the send button until connection is established.
    document.getElementById("sendButton").disabled = true;

    connection.on("ReceiveMessage", function (user, message) {
        var li = document.createElement("li");
        li.classList.add("dark:text-gray-200");
        document.getElementById("messagesList").appendChild(li);
        // We can assign user-supplied strings to an element's textContent because it
        // is not interpreted as markup. If you're assigning in any other way, you 
        // should be aware of possible script injection concerns.
        li.textContent = `${user} says ${message}`;
    });

    connection.on("ReceiveMessageGroup", function (userName, groupChatId, date, text) {
        var li = document.createElement("li");
        li.classList.add("dark:text-gray-200");
        document.getElementById("messagesList").appendChild(li);
        // We can assign user-supplied strings to an element's textContent because it
        // is not interpreted as markup. If you're assigning in any other way, you 
        // should be aware of possible script injection concerns.
        li.textContent = `in group ${groupChatId} user ${userName} says ${text} at ${date}`;
    });

    connection.on("GroupNotification", function (message) {
        var li = document.createElement("li");
        li.classList.add("dark:text-gray-200");
        document.getElementById("messagesList").appendChild(li);
        // We can assign user-supplied strings to an element's textContent because it
        // is not interpreted as markup. If you're assigning in any other way, you 
        // should be aware of possible script injection concerns.
        li.textContent = message;
    });

    connection.start().then(function () {
        document.getElementById("sendButton").disabled = false;
    }).catch(function (err) {
        return console.error(err.toString());
    });

    //document.getElementById("sendButton").addEventListener("click", function (event) {
    //    var user = document.getElementById("userInput").value;
    //    var text = document.getElementById("messageInput").value;
    //    connection.invoke("SendMessage", user, text).catch(function (err) {
    //        return console.error(err.toString());
    //    });
    //    event.preventDefault();
    //});

    document.getElementById("sendButton").addEventListener("click", function (event) {
        var groupChatId = document.getElementById("hiddenGroupId").value;
        var userId = document.getElementById("hidden-userid").value;
        var text = document.getElementById("messageInput").value;
        var fileAttach = document.getElementById("hidden-file").value;
        var seen = document.getElementById("hidden-seen").value;
        var userName = document.getElementById("hidden-username").value;

        //console.log(userId)
        //console.log(groupChatId)
        //console.log(userName)
        //console.log(text)

        connection.invoke("SendMessageToGroup", userId, groupChatId, userName, text, seen).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });

}