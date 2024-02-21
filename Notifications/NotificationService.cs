using Notifications.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notifications
{
    public class NotificationService
    {
        private readonly ISubscriptionStorage _storage;
        private readonly INotifier _notifier;

        public NotificationService(ISubscriptionStorage storage, INotifier notifier)
        {
            this._storage = storage ?? throw new ArgumentNullException(nameof(storage));
            this._notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
        }

        public void Subscribe(Subscription subscription)
        {
            _storage.SaveSubscription(subscription);
        }

        public void Unsubscribe(string username)
        {
            _storage.DeleteSubscription(username);
        }

        public async Task Broadcast(Message message)
        {
            List<Subscription> allSubscriptions = _storage.GetAllSubscriptions();
            foreach (Subscription subscription in allSubscriptions)
            {
                try
                {
                    await _notifier.SendNotification(subscription, message);
                }
                catch (SubscriptionNotFoundException)
                {
                    _storage.DeleteSubscription(subscription.Username);
                }
            }
        }

        public async Task BroadcastFromUser(string username, Message message)
        {
            List<Subscription> allSubscriptions = _storage.GetAllSubscriptions();
            foreach (Subscription subscription in allSubscriptions)
            {
                if (subscription.Username == username)
                    continue;
                
                try
                {
                    await _notifier.SendNotification(subscription, message);
                }
                catch (SubscriptionNotFoundException)
                {
                    _storage.DeleteSubscription(subscription.Username);
                }
            }
        }

        public async Task NotifyUser(string username, Message message)
        {
            Subscription subscription = _storage.GetSubscription(username);
            try
            {
                await _notifier.SendNotification(subscription, message);
            }
            catch (SubscriptionNotFoundException)
            {
                _storage.DeleteSubscription(subscription.Username);
                throw;
            }
        }

        //public string IdentifyUserBySubscription(string subscriptionEndpoint)
        //{
        //    List<Subscription> allSubscriptions = _storage.GetAllSubscriptions();
        //    var subscription = allSubscriptions.FirstOrDefault(sub => sub.Endpoint == subscriptionEndpoint);
        //    if (subscription != null)
        //    {
        //        return subscription.Username;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public List<string> GetAllSubscribedUsers()
        {
            List<string> result = _storage.GetAllSubscriptions().Select(sub => sub.Username).ToList();
            return result;
        }
    }
}
