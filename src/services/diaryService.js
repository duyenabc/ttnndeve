import apiClient from '../api/axiosConfig';

export const OperationType = {
  CREATE: 'create',
  UPDATE: 'update',
  DELETE: 'delete',
  LIST: 'list',
  GET: 'get',
  WRITE: 'write',
};

export function handleApiError(error, operationType) {
  console.error(`API Error during ${operationType}: `, error.response?.data || error.message);
}

/**
 * Fetch all diary entries for a given student userId from .NET Backend
 */
export async function getStudentDiariesFromDb(userId) {
  if (!userId) return [];
  try {
    const response = await apiClient.get(`/diaries/${userId}`);
    return response.data;
  } catch (error) {
    handleApiError(error, OperationType.GET);
    return [];
  }
}

/**
 * Note: Real-time listeners are no longer natively supported via standard HTTP.
 * This is a fallback to standard polling or single fetch. 
 * For real-time, consider SignalR integration in the future.
 */
export function listenStudentDiaries(userId, onData, onError) {
  if (!userId) return () => {};
  
  // Immediately fetch data once. To simulate real-time, you could set an interval here.
  getStudentDiariesFromDb(userId)
    .then(data => onData(data))
    .catch(err => {
      if(onError) onError(err);
    });

  // Return a dummy unsubscribe function
  return () => {};
}

/**
 * Add a new diary entry to Database
 */
export async function addDiaryEntryToDb(entryData) {
  try {
    const response = await apiClient.post('/diaries', {
      ...entryData,
      userId: String(entryData.userId)
    });
    return response.data.id;
  } catch (error) {
    handleApiError(error, OperationType.CREATE);
  }
}

/**
 * Update an existing diary entry in Database
 */
export async function updateDiaryEntryInDb(docId, updateData) {
  try {
    // Merge id with updateData since PUT requires the full model usually
    await apiClient.put(`/diaries/${docId}`, {
      id: docId,
      ...updateData
    });
  } catch (error) {
    handleApiError(error, OperationType.UPDATE);
  }
}

/**
 * Note: No realtime listener for all diaries in standard HTTP.
 * Just doing a standard fetch.
 */
export function listenAllDiaries(onData, onError) {
  apiClient.get('/diaries')
    .then(response => onData(response.data))
    .catch(err => {
        handleApiError(err, OperationType.LIST);
        if (onError) onError(err);
    });
    
  return () => {};
}
