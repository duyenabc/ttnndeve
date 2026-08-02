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

/** One-shot fetch with unsubscribe stub (HTTP, not realtime). */
export function listenStudentDiaries(userId, onData, onError) {
  if (!userId) return () => {};

  getStudentDiariesFromDb(userId)
    .then((data) => onData(data))
    .catch((err) => {
      if (onError) onError(err);
    });

  return () => {};
}

export async function addDiaryEntryToDb(entryData) {
  try {
    const response = await apiClient.post('/diaries', {
      ...entryData,
      userId: String(entryData.userId),
    });
    return response.data.id;
  } catch (error) {
    handleApiError(error, OperationType.CREATE);
  }
}

export async function updateDiaryEntryInDb(docId, updateData) {
  try {
    await apiClient.put(`/diaries/${docId}`, {
      id: docId,
      ...updateData,
    });
  } catch (error) {
    handleApiError(error, OperationType.UPDATE);
  }
}

export function listenAllDiaries(onData, onError) {
  apiClient
    .get('/diaries')
    .then((response) => onData(response.data))
    .catch((err) => {
      handleApiError(err, OperationType.LIST);
      if (onError) onError(err);
    });

  return () => {};
}
