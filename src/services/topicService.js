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

export async function saveTopicProposalToDb(studentId, topicData) {
  if (!studentId) return;
  try {
    const payload = {
      ...topicData,
      userId: String(studentId),
      mssv: String(studentId),
    };
    await apiClient.post('/topics', payload);
    return true;
  } catch (error) {
    handleApiError(error, OperationType.WRITE);
  }
}

export async function getStudentTopicFromDb(studentId) {
  if (!studentId) return null;
  try {
    const response = await apiClient.get(`/topics/${studentId}`);
    return response.data;
  } catch (error) {
    if (error.response && error.response.status === 404) {
      return null;
    }
    handleApiError(error, OperationType.GET);
    return null;
  }
}

export function listenStudentTopic(studentId, onData, onError) {
  if (!studentId) return () => {};

  getStudentTopicFromDb(studentId)
    .then((data) => onData(data))
    .catch((err) => {
      if (onError) onError(err);
    });

  return () => {};
}

export function listenAllTopics(onData, onError) {
  apiClient
    .get('/topics')
    .then((response) => onData(response.data))
    .catch((err) => {
      handleApiError(err, OperationType.LIST);
      if (onError) onError(err);
    });

  return () => {};
}

export async function updateTopicStatusInDb(studentId, updatePayload) {
  if (!studentId) return;
  try {
    const payload = {
      userId: String(studentId),
      ...updatePayload,
    };
    await apiClient.post('/topics', payload);
    return true;
  } catch (error) {
    handleApiError(error, OperationType.WRITE);
  }
}
