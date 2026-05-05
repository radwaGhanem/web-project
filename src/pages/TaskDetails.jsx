import { useEffect, useState } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { getTaskById, updateTaskStatus, deleteTask } from '../services/taskService'

export default function TaskDetails() {
  const { id } = useParams()
  const navigate = useNavigate()
  const [task, setTask] = useState(null)
  const [status, setStatus] = useState('false')
  const [loading, setLoading] = useState(true)
  const [message, setMessage] = useState('')
  const [error, setError] = useState('')

  useEffect(() => {
    const loadTask = async () => {
      setError('')
      try {
        const data = await getTaskById(id)
        setTask(data)
        setStatus(data.isDone ? 'true' : 'false')
      } catch (err) {
        setError('Unable to fetch task details. Check backend connectivity.')
      } finally {
        setLoading(false)
      }
    }

    loadTask()
  }, [id])

  const handleStatusUpdate = async (event) => {
    event.preventDefault()
    setMessage('')
    setError('')

    try {
      const isDone = status === 'true'
      await updateTaskStatus(id, isDone)
      setMessage('Task status updated successfully.')
      setTask((current) => ({ ...current, isDone }))
    } catch (err) {
      setError('Unable to update task status.')
    }
  }

  const handleDeleteTask = async () => {
    if (!window.confirm('Are you sure you want to delete this task? This action cannot be undone.')) {
      return
    }

    try {
      await deleteTask(id)
      navigate('/tasks')
    } catch (err) {
      setError('Unable to delete task. Please try again.')
    }
  }

  if (loading) {
    return (
      <section className="page-content">
        <div className="card">Loading task details…</div>
      </section>
    )
  }

  if (error) {
    return (
      <section className="page-content">
        <div className="card error-box">{error}</div>
      </section>
    )
  }

  if (!task) {
    return (
      <section className="page-content">
        <div className="card">Task not found.</div>
      </section>
    )
  }

  return (
    <section className="page-content page-enter">
      <div className="card detail-card">
        <h1 className="page-title">Task Details</h1>
        <div className="data-row">
          <p>
            <strong>Title:</strong> {task.title}
          </p>
          <p>
            <strong>Description:</strong> {task.description}
          </p>
          <p>
            <strong>Due Date:</strong> {new Date(task.dueDate).toLocaleDateString()}
          </p>
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
        </div>
        {message && <div className="success-box">{message}</div>}
        {error && <div className="error-box">{error}</div>}
        <form onSubmit={handleStatusUpdate} className="form-row">
          <label htmlFor="status">Change task status</label>
          <select id="status" value={status} onChange={(event) => setStatus(event.target.value)}>
            <option value="false">Open</option>
            <option value="true">Completed</option>
          </select>
          <button type="submit">Update Status</button>
        </form>
        <div className="action-row">
          <button type="button" onClick={handleDeleteTask} className="btn-danger">
            Delete Task
          </button>
        </div>
      </div>
    </section>
  )
}
