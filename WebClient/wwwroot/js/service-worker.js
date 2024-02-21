self.addEventListener('push', function (event) {
    let data = {};
    if (event.data) {
        data = event.data.json();
    }

    let title = data.Title || "";
    let body = data.Body || "No message body";
    let icon = "/img/push-icon.jpg";

    self.registration.showNotification(title, {
        body: body,
        icon: icon
    });
});