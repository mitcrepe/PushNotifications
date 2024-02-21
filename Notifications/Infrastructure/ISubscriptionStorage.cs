using System.Collections.Generic;

namespace Notifications.Infrastructure
{
    public interface ISubscriptionStorage
    {
        void SaveSubscription(Subscription subscription);

        Subscription GetSubscription(string username);

        List<Subscription> GetAllSubscriptions();

        void DeleteSubscription(string username);
    }
}
