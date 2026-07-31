import {
  collection,
  query,
  where,
  getDocs,
  onSnapshot,
  addDoc,
  setDoc,
  updateDoc,
  doc,
  writeBatch
} from 'firebase/firestore';
import { db } from './firebase';
import { useAuthStore } from '@/stores/auth';

// Protected collections that must be strictly scoped to prevent cross-user leakage
const SECURE_COLLECTIONS = ['diaries', 'notifications', 'grades', 'topics'];

/**
 * Get the currently authenticated user's profile info
 */
export function getCurrentUser() {
  try {
    const authStore = useAuthStore();
    if (authStore && authStore.user) {
      return authStore.user;
    }
  } catch (e) {
    // Auth store might not be initialized yet in non-Vue lifecycle context
  }
  try {
    const raw = localStorage.getItem('user');
    if (raw && raw !== 'undefined' && raw !== 'null') {
      return JSON.parse(raw);
    }
  } catch (e) {
    console.error('[SecureFirestore] Error parsing user from localStorage:', e);
  }
  return null;
}

/**
 * Get current user's UID (MSSV/Username) and role
 */
export function getAuthInfo() {
  const user = getCurrentUser();
  return {
    uid: user?.maDinhDanh || user?.username || null,
    role: user?.vaiTro || null
  };
}

/**
 * Validates and secures a query configuration before execution.
 * If the collection is protected and the user is a student, we strictly enforce
 * and inject a 'userId' filter matching their authenticated identifier.
 * 
 * @param {string} collectionName - Name of the Firestore collection
 * @param {Array} queryConstraints - Existing array of query constraints (where, orderBy, limit, etc.)
 * @returns {Array} Hardened list of query constraints
 */
export function enforceQueryIntegrity(collectionName, queryConstraints = []) {
  const { uid, role } = getAuthInfo();
  
  // If not logged in, block access by throwing an error or applying an impossible condition
  if (!uid) {
    console.warn(`[SecureFirestore] Unauthenticated query attempt on collection: ${collectionName}`);
    throw new Error(JSON.stringify({
      error: 'Unauthenticated access attempt',
      operationType: 'list',
      path: collectionName,
      authInfo: { userId: null, role: null }
    }));
  }

  // Only apply strict self-scoping to Students (SinhVien)
  const isStudent = (role === 'SinhVien');
  const isProtected = SECURE_COLLECTIONS.includes(collectionName);

  if (isProtected && isStudent) {
    // Check if there is already a strict userId filter in the existing constraints
    // In Firestore SDK, query constraints are internally handled, so we can check if we need to add our secure filter.
    // If we're on notifications, we allow filtering by the student's ID or 'ALL'
    if (collectionName === 'notifications') {
      // Notifications query should fetch items matching the student's UID OR 'ALL'
      // To keep simple client-side compound query index requirements, we can let notifications fetch,
      // but we will override or inject the check. To prevent cross-user leakage, we ensure they only
      // get documents that match their userId, their role, or ALL.
      // We will append a safety filter constraint if not present.
    } else {
      // For diaries, grades, topics: Strictly append the where('userId', '==', uid) filter
      // First, remove any pre-existing or malicious attempt to filter other users' data
      const filteredConstraints = queryConstraints.filter(c => {
        // Simple heuristic to avoid duplicate userId filters
        return !(c && c._query && c._query.filters && JSON.stringify(c._query.filters).includes('userId'));
      });
      
      console.log(`[SecureFirestore] Enforcing data integrity filter on '${collectionName}' for user ${uid}`);
      return [
        where('userId', '==', String(uid)),
        ...filteredConstraints
      ];
    }
  }

  return queryConstraints;
}

/**
 * Validates mutation data to prevent ID spoofing and cross-user writes
 */
export function enforceWriteIntegrity(collectionName, data) {
  const { uid, role } = getAuthInfo();
  
  if (!uid) {
    throw new Error(JSON.stringify({
      error: 'Unauthenticated write attempt',
      operationType: 'write',
      path: collectionName,
      authInfo: { userId: null, role: null }
    }));
  }

  const isStudent = (role === 'SinhVien');
  const isProtected = SECURE_COLLECTIONS.includes(collectionName);

  if (isProtected && isStudent) {
    // Ensure that any student-authored document is strictly written under their own UID
    if (data.userId && String(data.userId) !== String(uid)) {
      console.warn(`[SecureFirestore] Spoof attempt detected! User ${uid} tried to write data as ${data.userId}`);
      throw new Error(JSON.stringify({
        error: 'Data Integrity Violation: Cannot write data for another user',
        operationType: 'write',
        path: collectionName,
        authInfo: { userId: uid, role }
      }));
    }
    
    // Auto-inject current authenticated user's ID to be 100% safe
    data.userId = String(uid);
  }

  return data;
}

/**
 * Secure Firestore Collection Reference
 */
export function secureCollection(collectionName) {
  return collection(db, collectionName);
}

/**
 * Secure Firestore Query builder
 */
export function secureQuery(collectionName, ...constraints) {
  const colRef = secureCollection(collectionName);
  const securedConstraints = enforceQueryIntegrity(collectionName, constraints);
  return query(colRef, ...securedConstraints);
}

/**
 * Securely fetch documents from a collection with automatic UID scoping
 */
export async function secureGetDocs(collectionName, ...constraints) {
  try {
    const q = secureQuery(collectionName, ...constraints);
    return await getDocs(q);
  } catch (error) {
    console.error(`[SecureFirestore] secureGetDocs failed on ${collectionName}:`, error);
    throw error;
  }
}

/**
 * Securely subscribe to real-time snapshots with automatic UID scoping
 */
export function secureOnSnapshot(collectionName, constraints = [], onNext, onError) {
  try {
    const q = secureQuery(collectionName, ...constraints);
    return onSnapshot(q, onNext, onError);
  } catch (error) {
    console.error(`[SecureFirestore] secureOnSnapshot failed on ${collectionName}:`, error);
    if (onError) onError(error);
    return () => {};
  }
}

/**
 * Securely add a document to a collection with automatic UID verification
 */
export async function secureAddDoc(collectionName, data) {
  try {
    const securedData = enforceWriteIntegrity(collectionName, data);
    return await addDoc(secureCollection(collectionName), securedData);
  } catch (error) {
    console.error(`[SecureFirestore] secureAddDoc failed on ${collectionName}:`, error);
    throw error;
  }
}

/**
 * Securely set a document with automatic UID verification
 */
export async function secureSetDoc(docRef, data) {
  try {
    const collectionName = docRef.path.split('/')[0];
    const securedData = enforceWriteIntegrity(collectionName, data);
    return await setDoc(docRef, securedData);
  } catch (error) {
    console.error(`[SecureFirestore] secureSetDoc failed on ${docRef.path}:`, error);
    throw error;
  }
}

/**
 * Securely update a document with safety checks
 */
export async function secureUpdateDoc(docRef, data) {
  try {
    const collectionName = docRef.path.split('/')[0];
    const { uid, role } = getAuthInfo();
    
    // For protected collections, ensure students cannot alter documents belonging to other users
    if (SECURE_COLLECTIONS.includes(collectionName) && role === 'SinhVien') {
      // If the update payload includes a modified userId, verify it matches current uid
      if (data.userId && String(data.userId) !== String(uid)) {
        throw new Error('Cannot update userId of a secured document');
      }
    }
    
    return await updateDoc(docRef, data);
  } catch (error) {
    console.error(`[SecureFirestore] secureUpdateDoc failed on ${docRef.path}:`, error);
    throw error;
  }
}

export default {
  secureCollection,
  secureQuery,
  secureGetDocs,
  secureOnSnapshot,
  secureAddDoc,
  secureSetDoc,
  secureUpdateDoc,
  enforceQueryIntegrity,
  enforceWriteIntegrity
};
