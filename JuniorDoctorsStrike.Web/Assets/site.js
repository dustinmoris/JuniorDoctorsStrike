
var intervalInSeconds = 30;
var interval = intervalInSeconds * 1000;
var newMessages = new Array();

function loadNewMessagesTicker() {
    var sinceId = $(".message:first-child > input[name=id]").val();

    $.get("/api/messagesSince/" + sinceId, function (messages) {
        if (messages.length > 0) {
            newMessages = messages;
            $("#show-new-btn").text("Show " + newMessages.length + " new messages");
            $("#show-new-btn").show();
        }
    });

    setTimeout(loadNewMessagesTicker, interval);
}

function loadOlderMessages() {
    var maxId = $(".message:last-child > input[name=id]").val();

    $.get("/api/messagesUntil/" + maxId, function (messages) {
        for (var i = 0; i < messages.length; i++) {
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
        "<span class='message-timeinfo'>" + created + "</span>" +
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

    /* ---------------------
     * Load initial messages
     * ---------------------*/

    $.get("/api/messages", function (messages) {
        for (var i = 0; i < messages.length; i++) {
            var message = messages[i];
            var html = formatMessage(message);
            $("#message-stream").append(html);
        }
    });

    /* ---------------------
     * Poll messages
     * ---------------------*/

    setTimeout(loadNewMessagesTicker, interval);
    
    /* ---------------------
     * Register Button events
     * ---------------------*/

    $("#load-more-btn").click(function (event) {
        event.preventDefault();
        $(this).hide();
        loadOlderMessages();
    });

    $("#show-new-btn").click(function (event) {
        event.preventDefault();
        for (var i = 0; i < newMessages.length; i++) {
            var message = newMessages[i];
            var html = formatMessage(message);
            $("#message-stream").prepend(html);
        }
        $(this).hide();
    });
});