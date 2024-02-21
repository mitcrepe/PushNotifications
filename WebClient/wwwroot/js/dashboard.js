var swRegistration;
const serverPublicKey = document.getElementById('public-key').value;
var isSubscribed = false;

initializeUI();

if ('serviceWorker' in navigator && 'PushManager' in window) {
    window.addEventListener('load', async function () {
        swRegistration = await navigator.serviceWorker.register('/js/service-worker.js');
        await restoreUser(swRegistration);
        updateUI();
    });
}

function initializeUI() {
    document.getElementById('send-msg-btn').addEventListener('click', onSendMessageClick);
    document.getElementById('sub-btn').addEventListener('click', onSubscribeClick);
}

function updateUI() {
    let subBtn = document.getElementById('sub-btn');
    if (isSubscribed) {
        subBtn.innerText = 'Unsubscribe';
        document.getElementById('username').setAttribute('disabled', '');
    } else {
        subBtn.innerText = 'Subscribe';
        document.getElementById('username').removeAttribute('disabled');
    }
}

async function onSubscribeClick() {
    if (!await checkPermissions()) {
        return;
    }

    if (isSubscribed) {
        unsubscribe();
    } else {
        subscribe();
    }
}

async function onSendMessageClick() {
    let body = document.getElementById('Body').value;
    if (!body) {
        setError('Message body is mandatory.');
        return;
    } else {
        clearError();
    }

    let form = document.getElementById('msg-form');
    let data = new FormData(form);
    
    let response = await fetch('/api/push/sendMessageToUser', {
        method: 'POST',
        body: data
    })

    if (response.ok) {
        document.getElementById('Title').value = '';
        document.getElementById('Body').value = '';
    } else {
        setError('Error sending message.');
    }
}

async function checkPermissions() {
    let permissions = await Notification.requestPermission();

    if (permissions !== 'granted') {
        setError('Notifications are blocked');
        return false;
    } else {
        clearError();
        return true
    }
}

async function subscribe() {
    let options = {
        userVisibleOnly: true,
        applicationServerKey: urlB64ToUint8Array(serverPublicKey)
    };

    let subscription = await swRegistration.pushManager.subscribe(options);
    subscription = subscription.toJSON();
    let username = document.getElementById('username').value;

    if (username === '') {
        return;
    }

    let request = {
        username: username,
        endpoint: subscription.endpoint,
        authenticationKey: subscription.keys.auth,
        sharedSecret: subscription.keys.p256dh
    }

    let response = await fetch('/api/push/subscribe', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(request)
    });

    if (response.ok) {
        isSubscribed = true;
        saveUser(username);
        updateUI();
    }
}

async function unsubscribe() {
    const subscription = await swRegistration.pushManager.getSubscription();

    if (subscription !== null) {
        subscription.unsubscribe();
    }

    let username = document.getElementById('username').value;

    let params = new URLSearchParams({
        username: username
    });

    await fetch('/api/push/unsubscribe?' + params, {
        method: 'POST'
    });

    isSubscribed = false;
    document.getElementById('username').value = '';
    deleteUser();
    updateUI();
}


function urlB64ToUint8Array(base64String) {
    const padding = '='.repeat((4 - base64String.length % 4) % 4);
    const base64 = (base64String + padding)
        .replace(/\-/g, '+')
        .replace(/_/g, '/');

    const rawData = window.atob(base64);
    const outputArray = new Uint8Array(rawData.length);

    for (let i = 0; i < rawData.length; ++i) {
        outputArray[i] = rawData.charCodeAt(i);
    }
    return outputArray;
}

async function restoreUser(swRegistration) {
    let user = localStorage.getItem('user');
    if (user) {
        const subscription = await swRegistration.pushManager.getSubscription();
        if (subscription !== null) {
            isSubscribed = true;
            document.getElementById('username').value = user;
        } else {
            deleteUser();
        }
    }
}

const UserKey = 'user';
function saveUser(username) {
    localStorage.setItem(UserKey, username)
}

function deleteUser() {
    localStorage.removeItem(UserKey)
}

function setError(error) {
    document.getElementById('error-message').textContent = error;
}

function clearError() {
    setError('');
}