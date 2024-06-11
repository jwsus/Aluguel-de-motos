using System;
using System.Collections.Generic;
using Mottu.Domain.Entities;

public interface INotificationRepository
{    Task AddNotificationsAsync(IEnumerable<Notification> notifications);
}
