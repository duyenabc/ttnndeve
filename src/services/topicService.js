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
 * Save or submit a student's topic proposal
 */
export async function saveTopicProposalToDb(studentId, topicData) {
  if (!studentId) return;
  try {
    const payload = {
      ...topicData,
      userId: String(studentId),
      mssv: String(studentId)
    };
    await apiClient.post('/topics', payload);
    return true;
  } catch (error) {
    handleApiError(error, OperationType.WRITE);
  }
}

/**
 * Fetch a single student's topic proposal
 */
export async function getStudentTopicFromDb(studentId) {
  if (!studentId) return null;
  try {
    const response = await apiClient.get(`/topics/${studentId}`);
    return response.data;
  } catch (error) {
    // If 404 Not Found, just return null
    if (error.response && error.response.status === 404) {
        return null;
    }
    handleApiError(error, OperationType.GET);
    return null;
  }
}

/**
 * Real-time listeners replaced with standard fetch
 */
export function listenStudentTopic(studentId, onData, onError) {
  if (!studentId) return () => {};
  
  getStudentTopicFromDb(studentId)
    .then(data => onData(data))
    .catch(err => {
        if(onError) onError(err);
    });

  return () => {};
}

/**
 * Real-time listener for all topic proposals replaced with fetch
 */
export function listenAllTopics(onData, onError) {
  apiClient.get('/topics')
    .then(response => onData(response.data))
    .catch(err => {
        handleApiError(err, OperationType.LIST);
        if (onError) onError(err);
    });

  return () => {};
}

/**
 * Teacher updates status and/or feedback of a student's topic.
 */
export async function updateTopicStatusInDb(studentId, updatePayload) {
  if (!studentId) return;
  try {
    // Here we use POST because our API uses Upsert
    const payload = {
        userId: String(studentId),
        ...updatePayload
    };
    await apiClient.post('/topics', payload);
    return true;
  } catch (error) {
    handleApiError(error, OperationType.WRITE);
  }
}
