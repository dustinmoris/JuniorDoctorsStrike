
var intervalInSeconds = 20;
var interval = intervalInSeconds * 1000;
var newMessages = new Array();
var messageIds = new Array();

function loadNewMessagesTicker() {
    var sinceId = messageIds[0];

    $.get("/api/messagesSince/" + sinceId, function (messages) {
        if (messages.length > 0) {
            for (var i = messages.length - 1; i >= 0; i--) {
                var message = messages[i];
                
                if ($.inArray(message.Id, messageIds) === -1) {
                    newMessages.unshift(message);
                    messageIds.unshift(message.Id);
                }
            }

            if (newMessages.length > 0) {
                $("#show-new-btn").text("Show " + newMessages.length + " new messages");
                $("#show-new-btn").show();
            }
        }
    });

    setTimeout(loadNewMessagesTicker, interval);
}

function loadOlderMessages() {
    var maxId = $(".message:last-child > input[name=id]").val();

    $.get("/api/messagesUntil/" + maxId, function (messages) {
        // skipping the first message, because it is a duplicate
        for (var i = 1; i < messages.length; i++) {
            var message = messages[i];
            var html = formatMessage(message);
            $("#message-stream").append(html);
        }
        $("#load-more-btn").show();
    });
}

function formatMessage(message) {
    var created = convertToDate(message.Created);

    var html =
        "<div class='message'>" +
        "<input type='hidden' name='id' value='" + message.Id + "'/>" +
        "<input type='hidden' name='created' value='" + created + "'/>" +
        "<img class='user-picture' src='" + message.User.ImageUrl + "' alt='" + message.User.Name + "'/>" +
        "<p class='message-header'>" +
        "<span class='user-name'>" + message.User.Name + "</span>" +
        "<span class='message-timeinfo'>" + created.toString().substring(0, 25) + "</span>" +
        "</p>" +
        "<p class='message-text'>" + message.Text + "</p>" +
        "</div>";
    return html;
}

function convertToDate(value) {
    var a = /\/Date\((\d*)\)\//.exec(value);
    return new Date(+a[1]);
}

$(function () {

    $("#show-new-btn").hide();

    $.get("/api/messages", function (messages) {
        for (var i = 0; i < messages.length; i++) {
            var message = messages[i];
            var html = formatMessage(message);
            $("#message-stream").append(html);
            messageIds.push(message.Id);
        }

        setTimeout(loadNewMessagesTicker, interval);
    });

    $("#load-more-btn").click(function (event) {
        event.preventDefault();
        $(this).hide();
        loadOlderMessages();
    });

    $("#show-new-btn").click(function (event) {
        event.preventDefault();
        for (var i = newMessages.length - 1; i >= 0; i--) {
            var message = newMessages[i];
            var html = formatMessage(message);
            $("#message-stream").prepend(html);
            newMessages.splice(i, 1);
        }
        $(this).hide();
    });
});