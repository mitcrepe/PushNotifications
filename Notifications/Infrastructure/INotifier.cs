using System.Threading.Tasks;

namespace Notifications.Infrastructure
{
    public interface INotifier
    {
        Task SendNotification(Subscription subscription, Message message);
    }
}
