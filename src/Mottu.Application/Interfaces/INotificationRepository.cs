using System;
using System.Collections.Generic;
using Mottu.Domain.Entities;

public interface INotificationRepository
{
    List<Notification> GetNotificationsForDeliveryPerson(Guid deliveryPersonId);
    void AddNotification(Notification notification);
    void RemoveNotification(Guid notificationId);
}
