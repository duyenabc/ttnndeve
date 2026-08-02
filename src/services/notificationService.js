import api from '@/api/api';

/** Poll notifications; returns unsubscribe for MainLayout. */
export function subscribeNotifications(role, userId, onData, onError) {
  let stopped = false;

  const load = async () => {
    if (stopped) return;
    try {
      const res = await api.get('/notifications', {
        params: { role, userId: userId || 'ALL' },
      });
      if (!stopped) onData(Array.isArray(res.data) ? res.data : []);
    } catch (err) {
      console.warn('Notification fetch failed:', err?.message || err);
      if (!stopped) {
        onData([]);
        if (onError) onError(err);
      }
    }
  };

  load();
  const timer = setInterval(load, 15000);

  return () => {
    stopped = true;
    clearInterval(timer);
  };
}

export async function sendRealtimeNotification({
  role = 'SinhVien',
  userId = 'ALL',
  title,
  content,
  icon = 'notifications',
  bgClass = 'bg-blue-100 text-blue-700',
  link = '',
}) {
  try {
    const res = await api.post('/notifications', {
      title,
      content,
      role,
      userId: String(userId),
      icon,
      bgClass,
      link,
    });
    return res.data?.id;
  } catch (err) {
    console.error('Failed to send notification:', err);
  }
}

export async function markNotificationAsReadInDb(notificationId) {
  try {
    await api.put(`/notifications/${notificationId}/read`, { read: true });
  } catch (err) {
    console.error('Failed to mark notification as read:', err);
  }
}

export async function toggleNotificationReadInDb(notificationId, currentReadState) {
  try {
    await api.put(`/notifications/${notificationId}/read`, { read: !currentReadState });
  } catch (err) {
    console.error('Failed to toggle notification read state:', err);
  }
}

export async function markAllNotificationsAsReadInDb(role, userId) {
  try {
    await api.put('/notifications/read-all', null, {
      params: { role, userId: userId || 'ALL' },
    });
  } catch (err) {
    console.error('Failed to mark all notifications as read:', err);
  }
}

export async function clearAllNotificationsInDb(role, userId) {
  try {
    await api.delete('/notifications', {
      params: { role, userId: userId || 'ALL' },
    });
  } catch (err) {
    console.error('Failed to clear notifications:', err);
  }
}
