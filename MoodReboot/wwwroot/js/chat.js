"use strict";

function initializeConnection(connectionString) {
    $("#btnChatsSidenav").click(function () {
        $("#chatsSidenav").fadeToggle("fast");
        $("#chatsSidenav").height($("#chatWindow").height() - 50);
        $("#chatsSidenav").width($("#chatWindow").width() - 5);
        $("#open-sidenav-icon").toggle("fast");
        $("#close-sidenav-icon").toggle("fast");
    });

    const connection = new signalR.HubConnectionBuilder().withUrl(connectionString).build();
    //Disable the send button until connection is established.
    document.getElementById("sendButton").disabled = true;

    //connection.on("ReceiveMessage", function (user, message) {
    //    var li = document.createElement("li");
    //    li.classList.add("dark:text-gray-200");
    //    document.getElementById("messagesList").appendChild(li);
    //    // We can assign user-supplied strings to an element's textContent because it
    //    // is not interpreted as markup. If you're assigning in any other way, you 
    //    // should be aware of possible script injection concerns.
    //    li.textContent = `${user} says ${message}`;
    //});

    connection.on("ReceiveMessageGroup", function (userName, groupChatId, date, text) {
        var li = document.createElement("li");
        li.classList.add("dark:text-gray-200");
        li.classList.add("my-4");
        li.classList.add("text-sm");
        document.getElementById("messagesList").appendChild(li);
        // We can assign user-supplied strings to an element's textContent because it
        // is not interpreted as markup. If you're assigning in any other way, you 
        // should be aware of possible script injection concerns.
        li.textContent = `in group ${groupChatId} user ${userName} says ${text} at ${date}`;
        // Scroll to bottom
        var d = $('#messagesList');
        d.scrollTop(d.prop("scrollHeight"));

        const id = "notification_" + $("#chat-notification-list").children().length + 1;

        const htmlNotification = `
                <div id="${id}" onclick="loadChatMessages(${groupChatId},'${id}')" class="flex py-3 px-4 cursor-pointer text-gray-900 dark:text-gray-200 dark:bg-gray-700 hover:bg-gray-100 dark:hover:bg-gray-600">
                    <div class="flex-shrink-0">
                        <img class="w-11 h-11 rounded-full" src="https://flowbite.s3.amazonaws.com/blocks/marketing-ui/avatars/bonnie-green.png" alt="${userName} avatar">
                    </div>
                    <div class="pl-3 w-full">
                        <div class="text-gray-500 font-normal text-sm mb-1.5 dark:text-gray-400">Nuevo mensaje de <span class="font-semibold text-gray-900 dark:text-white">${userName}</span>: "${text}"</div>
                        <div class="text-xs font-medium text-primary-700 dark:text-gray-200">${date}</div>
                    </div>
                </div>`;
        $("#no-notifications-text").hide();
        $("#chat-notification-list").append(htmlNotification);
        $("#btn-open-notifications").addClass("animate-pulse");
    });

    //connection.on("GroupNotification", function (message) {
    //    var li = document.createElement("li");
    //    li.classList.add("dark:text-gray-200");
    //    document.getElementById("messagesList").appendChild(li);
    //    // We can assign user-supplied strings to an element's textContent because it
    //    // is not interpreted as markup. If you're assigning in any other way, you 
    //    // should be aware of possible script injection concerns.
    //    li.textContent = message;
    //});

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
        var userName = document.getElementById("hidden-username").value;

        connection.invoke("SendMessageToGroup", userId, groupChatId, userName, text).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });
}
