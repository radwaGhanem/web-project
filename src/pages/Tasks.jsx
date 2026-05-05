import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { getTasks } from '../services/taskService'

export default function Tasks() {
  const [tasks, setTasks] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    const loadTasks = async () => {
      setError('')
      try {
        const data = await getTasks()
        setTasks(data)
      } catch (err) {
        setError('Unable to fetch tasks. Please make sure your backend is running.')
      } finally {
        setLoading(false)
      }
    }

    loadTasks()
  }, [])

  return (
    <section className="page-content page-enter">
      <div className="card">
        <h1 className="page-title">Task List</h1>
        <p>Browse all tasks from the backend API. Click a task to view details.</p>
        <div style={{ margin: '1rem 0' }}>
          <Link className="nav-link" to="/tasks/new">
            Create New Task
          </Link>
        </div>
        {loading && <div>Loading tasks...</div>}
        {error && <div className="error-box">{error}</div>}
        {!loading && !error && (
          <div className="task-list">
            {tasks.length === 0 ? (
              <div>No tasks found.</div>
            ) : (
              tasks.map((task) => (
                <article key={task.id} className="task-card">
                  <h3>{task.title}</h3>
                  <p>{task.description}</p>
                  <p>
                    <strong>Status:</strong> {task.isDone ? 'Completed' : 'Open'}
                  </p>
                  <p>
                    <strong>Assigned to:</strong> {task.assignedUserName || 'Unassigned'}
                  </p>
                  <p>
                    <strong>Tags:</strong>{' '}
                    {Array.isArray(task.tags) && task.tags.length > 0 ? task.tags.join(', ') : 'None'}
                  </p>
                  <Link className="nav-link" to={`/tasks/${task.id}`}>
                    View details
                  </Link>
                </article>
              ))
            )}
          </div>
        )}
      </div>
    </section>
  )
}
