import { useEffect, useState } from 'react'
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import Home from './pages/Home'
import Login from './pages/Login'
import Tasks from './pages/Tasks'
import NewTask from './pages/NewTask'
import TaskDetails from './pages/TaskDetails'
import Users from './pages/Users'
import NavBar from './components/NavBar'
import { logout } from './services/authService'
import './App.css'

function App() {
  const [authenticated, setAuthenticated] = useState(false)
  const [theme, setTheme] = useState(localStorage.getItem('theme') || 'light')

  useEffect(() => {
    setAuthenticated(!!localStorage.getItem('authToken'))
    document.documentElement.setAttribute('data-theme', theme)
  }, [theme])

  const handleLogout = () => {
    logout()
    setAuthenticated(false)
  }

  const toggleTheme = () => {
    const newTheme = theme === 'light' ? 'dark' : 'light'
    setTheme(newTheme)
    localStorage.setItem('theme', newTheme)
  }

  return (
    <BrowserRouter>
      <div className="app-shell">
        <NavBar authenticated={authenticated} onLogout={handleLogout} theme={theme} onToggleTheme={toggleTheme} />
        <main className="page-content">
          <Routes>
            <Route path="/" element={<Home authenticated={authenticated} />} />
            <Route path="/login" element={<Login onLogin={() => setAuthenticated(true)} />} />
            <Route
              path="/tasks"
              element={authenticated ? <Tasks /> : <Navigate to="/login" replace />}
            />
            <Route
              path="/tasks/new"
              element={authenticated ? <NewTask /> : <Navigate to="/login" replace />}
            />
            <Route
              path="/tasks/:id"
              element={authenticated ? <TaskDetails /> : <Navigate to="/login" replace />}
            />
            <Route
              path="/users"
              element={authenticated ? <Users /> : <Navigate to="/login" replace />}
            />
            <Route path="*" element={<Navigate to="/" replace />} />
          </Routes>
        </main>
      </div>
    </BrowserRouter>
  )
}

export default App
