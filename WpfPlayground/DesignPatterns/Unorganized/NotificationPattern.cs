using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfPlayground.DesignPatterns.Unorganized
{
    public class NotificationPattern
    {
        public NotificationPattern()
        {

        }
    }

    public class Notification
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; }        
    }

    public enum NotificationType { Info, Warning, Error, Success }

    public interface INotificationService
    {
        void SendNotification(Notification notification);
        List<Notification> GetUnreadNotifications();
        void MarkAsRead(int notificationId);
    }

    public class NotificationService : INotificationService
    {
        private ConcurrentQueue<Notification> _notificationQueue;
        private readonly Timer _processingTimer;
        private int _batchSize;

        public NotificationService(int batchSize, int intervalSeconds)
        {
            _notificationQueue = new ConcurrentQueue<Notification>();
            _batchSize = batchSize;
            _processingTimer = new Timer(processingTimerCallback);
        }

        private void processingTimerCallback(object state) 
        {

        }
        public void SendNotification(Notification notification)
        {
            _notificationQueue.Enqueue(notification);
        }

        public List<Notification> GetUnreadNotifications()
        {
            throw new NotImplementedException();
        }

        public void MarkAsRead(int notificationId)
        {
            throw new NotImplementedException();
        }

        private void SendNotifications()
        {

        }        
    }
}
