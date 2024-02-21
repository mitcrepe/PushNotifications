using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Notifications.Infrastructure.Storage
{
    public class FileSubscriptionStorage : ISubscriptionStorage
    {
        private const string Filename = "Subscriptions.dat";
        private readonly string _filePath;
        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(List<Subscription>));

        public FileSubscriptionStorage(string basePath)
        {
            _filePath = Path.Combine(basePath, Filename);
        }

        public void DeleteSubscription(string username)
        {
            if (!File.Exists(_filePath))
            {
                return;
            }

            List<Subscription> subscriptions = GetFileContent();
            Subscription subscription = subscriptions.FirstOrDefault(sub => sub.Username == username);
            if (subscription != null)
            {
                subscriptions.Remove(subscription);
                SaveToFile(subscriptions);
            }
        }

        public List<Subscription> GetAllSubscriptions()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Subscription>();
            }

            return GetFileContent();
        }

        public Subscription GetSubscription(string username)
        {
            if (!File.Exists(_filePath))
            {
                throw new SubscriptionNotFoundException($"Subscription for user {username} does not exist");
            }

            List<Subscription> subscriptions = GetFileContent();
            Subscription result = subscriptions.FirstOrDefault(sub => sub.Username == username);

            if (result == null)
            {
                throw new SubscriptionNotFoundException($"Subscription for user {username} does not exist");
            }
            else
            {
                return result;
            }
        }

        public void SaveSubscription(Subscription subscription)
        {
            if (subscription == null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }
            
            List<Subscription> subscriptions = GetFileContent();
            subscriptions.Add(subscription);
            SaveToFile(subscriptions);
        }

        private List<Subscription> GetFileContent()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Subscription>();
            }
            using Stream file = File.OpenRead(_filePath);
            return (List<Subscription>)_serializer.Deserialize(file);
        }

        private void SaveToFile(List<Subscription> subscriptions)
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }

            using Stream file = File.OpenWrite(_filePath);
            _serializer.Serialize(file, subscriptions);
        }
    }
}
