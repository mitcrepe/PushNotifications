using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using WebPush;

namespace Notifications.Infrastructure
{
    public class WebPushNotifier : INotifier
    {
        private readonly VapidDetails _vapid;
        public WebPushNotifier(string subject, string publicKey, string privateKey)
        {
            VapidHelper.ValidateSubject(subject);
            VapidHelper.ValidatePublicKey(publicKey);
            VapidHelper.ValidatePrivateKey(privateKey);

            _vapid = new VapidDetails(subject, publicKey, privateKey);
        }

        public async Task SendNotification(Subscription subscription, Message message)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));
            if (message == null || message.Body == null)
                throw new ArgumentException($"'Please specify a message to be sent.", nameof(message));

            string messageJson = JsonSerializer.Serialize(message);

            WebPushClient client = new();
            PushSubscription pushSubscription = new(subscription.Endpoint, subscription.SharedSecret, subscription.AuthenticationKey);

            try
            {
                await client.SendNotificationAsync(pushSubscription, messageJson, _vapid);
            }
            catch (WebPushException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound || ex.StatusCode == HttpStatusCode.Gone)
                {
                    throw new SubscriptionNotFoundException($"User {subscription.Username} has unsubscribed.", ex);
                }
            }
        }
    }
}
