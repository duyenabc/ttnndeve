import {
  collection,
  query,
  where,
  onSnapshot,
  addDoc,
  updateDoc,
  doc,
  getDocs,
  writeBatch
} from 'firebase/firestore';
import { db } from './firebase';

const NOTIFICATIONS_COLLECTION = 'notifications';

// Default initial seed data by role (empty for fresh clean system)
const initialSeedByRole = {
  SinhVien: [],
  GiangVien: [],
  GiaoVu: [],
  Admin: []
};

/**
 * Subscribe to real-time notifications for a given role/userId
 * Ensures strict database-level scoping to prevent loading other users' notifications.
 */
export function subscribeNotifications(role, userId, onData, onError) {
  try {
    const colRef = collection(db, NOTIFICATIONS_COLLECTION);
    
    // Database-level query scoping: only fetch notifications destined for 'ALL' or the specific user
    const targetUserId = userId ? String(userId) : 'ALL';
    const q = query(
      colRef,
      where('userId', 'in', ['ALL', targetUserId])
    );

    return onSnapshot(
      q,
      (snapshot) => {
        let items = [];
        snapshot.forEach((docSnap) => {
          items.push({ id: docSnap.id, ...docSnap.data() });
        });

        // Filter items that match current role or 'ALL'
        const filtered = items.filter((item) => {
          if (item.role && item.role !== role && item.role !== 'ALL') return false;
          return true;
        });

        // Sort descending by createdAt
        filtered.sort((a, b) => new Date(b.createdAt || 0) - new Date(a.createdAt || 0));

        // If collection is completely empty for this snapshot, seed initial documents
        if (items.length === 0) {
          seedInitialNotifications(role, userId);
        } else {
          onData(filtered);
        }
      },
      (err) => {
        console.warn('Firebase notification subscription notice:', err.message);
        if (onError) onError(err);
      }
    );
  } catch (err) {
    console.error('Failed to subscribe notifications:', err);
    if (onError) onError(err);
    return () => {};
  }
}

/**
 * Send a real-time notification to Firebase Firestore
 */
export async function sendRealtimeNotification({
  role = 'SinhVien',
  userId = 'ALL',
  title,
  content,
  icon = 'notifications',
  bgClass = 'bg-blue-100 text-blue-700',
  link = ''
}) {
  try {
    const docData = {
      role,
      userId: String(userId),
      title,
      content,
      icon,
      bgClass,
      link,
      read: false,
      createdAt: new Date().toISOString()
    };
    const docRef = await addDoc(collection(db, NOTIFICATIONS_COLLECTION), docData);
    return docRef.id;
  } catch (err) {
    console.error('Failed to send notification to Firestore:', err);
  }
}

/**
 * Mark a single notification as read
 */
export async function markNotificationAsReadInDb(notificationId) {
  try {
    const docRef = doc(db, NOTIFICATIONS_COLLECTION, notificationId);
    await updateDoc(docRef, { read: true });
  } catch (err) {
    console.error('Failed to mark notification as read:', err);
  }
}

/**
 * Toggle notification read state
 */
export async function toggleNotificationReadInDb(notificationId, currentReadState) {
  try {
    const docRef = doc(db, NOTIFICATIONS_COLLECTION, notificationId);
    await updateDoc(docRef, { read: !currentReadState });
  } catch (err) {
    console.error('Failed to toggle notification read state:', err);
  }
}

/**
 * Mark all notifications as read for current role
 */
export async function markAllNotificationsAsReadInDb(role, userId) {
  try {
    const colRef = collection(db, NOTIFICATIONS_COLLECTION);
    const targetUserId = userId ? String(userId) : 'ALL';
    const q = query(colRef, where('userId', 'in', ['ALL', targetUserId]));
    const snapshot = await getDocs(q);
    const batch = writeBatch(db);

    snapshot.forEach((docSnap) => {
      const data = docSnap.data();
      if ((data.role === role || data.role === 'ALL') && !data.read) {
        batch.update(docSnap.ref, { read: true });
      }
    });

    await batch.commit();
  } catch (err) {
    console.error('Failed to mark all notifications as read:', err);
  }
}

/**
 * Clear all notifications for current role
 */
export async function clearAllNotificationsInDb(role, userId) {
  try {
    const colRef = collection(db, NOTIFICATIONS_COLLECTION);
    const targetUserId = userId ? String(userId) : 'ALL';
    const q = query(colRef, where('userId', 'in', ['ALL', targetUserId]));
    const snapshot = await getDocs(q);
    const batch = writeBatch(db);

    snapshot.forEach((docSnap) => {
      const data = docSnap.data();
      if (data.role === role || data.role === 'ALL') {
        batch.delete(docSnap.ref);
      }
    });

    await batch.commit();
  } catch (err) {
    console.error('Failed to clear notifications:', err);
  }
}

/**
 * Seed initial demo notifications into Firestore
 */
export async function seedInitialNotifications(role, userId) {
  try {
    const seeds = initialSeedByRole[role] || initialSeedByRole['SinhVien'];
    for (const seed of seeds) {
      await addDoc(collection(db, NOTIFICATIONS_COLLECTION), seed);
    }
  } catch (err) {
    console.warn('Could not seed initial notifications:', err);
  }
}
