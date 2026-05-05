import api from './api'

export const getTasks = () => api.get('/tasks').then((response) => response.data)
export const getTaskById = (id) => api.get(`/tasks/${id}`).then((response) => response.data)
export const createTask = (task) => api.post('/tasks', task).then((response) => response.data)
export const updateTaskStatus = (id, isDone) =>
  api.patch(`/tasks/${id}/status?isDone=${isDone}`)
export const deleteTask = (id) => api.delete(`/tasks/${id}`)
export const getUsers = () => api.get('/users').then((response) => response.data)
export const deleteUser = (id) => api.delete(`/users/${id}`)
export const getTags = () => api.get('/tags').then((response) => response.data)
