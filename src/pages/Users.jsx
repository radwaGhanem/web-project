import { useEffect, useState } from 'react'
import { getUsers, deleteUser } from '../services/taskService'

export default function Users() {
  const [users, setUsers] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [message, setMessage] = useState('')

  useEffect(() => {
    const loadUsers = async () => {
      setError('')
      try {
        const data = await getUsers()
        setUsers(data)
      } catch (err) {
        setError('Unable to fetch users. Admin access is required.')
      } finally {
        setLoading(false)
      }
    }

    loadUsers()
  }, [])

  const handleDeleteUser = async (userId) => {
    const user = users.find((u) => u.id === userId)
    if (!window.confirm(`Are you sure you want to delete ${user.name}? This action cannot be undone.`)) {
      return
    }

    try {
      await deleteUser(userId)
      setUsers((current) => current.filter((u) => u.id !== userId))
      setMessage(`User ${user.name} deleted successfully.`)
    } catch (err) {
      setError(`Unable to delete user. Please try again.`)
    }
  }

  return (
    <section className="page-content page-enter">
      <div className="card">
        <h1 className="page-title">Users Management</h1>
        <p>Manage all system users. Only admins can delete users.</p>
        {loading && <div>Loading users...</div>}
        {error && <div className="error-box">{error}</div>}
        {message && <div className="success-box">{message}</div>}
        {!loading && !error && (
          <div className="task-list">
            {users.length === 0 ? (
              <div>No users found.</div>
            ) : (
              users.map((user) => (
                <div key={user.id} className="task-card">
                  <div className="user-info">
                    <h3>{user.name}</h3>
                    <p>
                      <strong>Email:</strong> {user.email}
                    </p>
                    <p>
                      <strong>Role:</strong> {user.role}
                    </p>
                    {user.bio && (
                      <p>
                        <strong>Bio:</strong> {user.bio}
                      </p>
                    )}
                    {user.phoneNumber && (
                      <p>
                        <strong>Phone:</strong> {user.phoneNumber}
                      </p>
                    )}
                  </div>
                  <div className="user-actions">
                    <button
                      type="button"
                      onClick={() => handleDeleteUser(user.id)}
                      className="btn-danger"
                    >
                      Delete
                    </button>
                  </div>
                </div>
              ))
            )}
          </div>
        )}
      </div>
    </section>
  )
}
