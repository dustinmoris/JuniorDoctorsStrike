$(function () {

    var messageStream = $("#messages");

    /* ---------------------
     * Load initial messages
     * ---------------------*/

    $.get("/api/messages", function (messages) {
        for (var i = 0; i < messages.length; i++) {
            var message = messages[i];
            var html = formatMessage(message);
            messageStream.append(html);
        }
    });

    /* ---------------------
     * Poll messages
     * ---------------------*/

    var intervalInSeconds = 5;
    var interval = intervalInSeconds * 1000;

    setTimeout(updateMessages, interval);
});

function updateMessages() {
    var sinceId = 0; // ToDo

    $.get("/api/messagesSince/" + sinceId, function (messages) {
        for (var i = 0; i < messages.length; i++) {
            var message = messages[i];
            var html = formatMessage(message);
            window.messageStream.prepend(html);
        }
    });

    setTimeout(updateMessages, interval);
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