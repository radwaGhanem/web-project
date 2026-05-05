import api from './api'

export async function login(email, password) {
  const response = await api.post('/auth/login', { email, password })
  localStorage.setItem('authToken', response.data.token)
  return response.data
}

export function logout() {
  localStorage.removeItem('authToken')
}

export function isAuthenticated() {
  return Boolean(localStorage.getItem('authToken'))
}
