import { useEffect, useState } from 'react'
import { getUsers, getTags, createTask } from '../services/taskService'

export default function NewTask() {
  const [users, setUsers] = useState([])
  const [tags, setTags] = useState([])
  const [title, setTitle] = useState('')
  const [description, setDescription] = useState('')
  const [dueDate, setDueDate] = useState('')
  const [assignedUserId, setAssignedUserId] = useState('')
  const [selectedTagIds, setSelectedTagIds] = useState([])
  const [error, setError] = useState('')
  const [success, setSuccess] = useState('')
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const loadData = async () => {
      try {
        const [usersData, tagsData] = await Promise.all([getUsers(), getTags()])
        setUsers(usersData)
        setTags(tagsData)
      } catch (err) {
        setError('Unable to load users or tags. Admin login is required.')
      } finally {
        setLoading(false)
      }
    }

    loadData()
  }, [])

  const handleTagToggle = (tagId) => {
    setSelectedTagIds((current) =>
      current.includes(tagId) ? current.filter((id) => id !== tagId) : [...current, tagId],
    )
  }

  const handleSubmit = async (event) => {
    event.preventDefault()
    setError('')
    setSuccess('')

    try {
      await createTask({
        title,
        description,
        dueDate,
        assignedUserId: assignedUserId ? Number(assignedUserId) : null,
        tagIds: selectedTagIds,
      })
      setSuccess('Task created successfully.')
      setTitle('')
      setDescription('')
      setDueDate('')
      setAssignedUserId('')
      setSelectedTagIds([])
    } catch (err) {
      setError('Unable to create task. Confirm you have admin access.')
    }
  }

  return (
    <section className="page-content page-enter">
      <div className="card">
        <h1 className="page-title">Create Task</h1>
        {error && <div className="error-box">{error}</div>}
        {success && <div className="success-box">{success}</div>}
        <form onSubmit={handleSubmit}>
          <div className="form-row">
            <label htmlFor="title">Title</label>
            <input
              id="title"
              value={title}
              onChange={(event) => setTitle(event.target.value)}
              required
            />
          </div>
          <div className="form-row">
            <label htmlFor="description">Description</label>
            <textarea
              id="description"
              value={description}
              onChange={(event) => setDescription(event.target.value)}
              required
            />
          </div>
          <div className="form-row">
            <label htmlFor="dueDate">Due Date</label>
            <input
              id="dueDate"
              type="date"
              value={dueDate}
              onChange={(event) => setDueDate(event.target.value)}
              required
            />
          </div>
          <div className="form-row">
            <label htmlFor="assignedUser">Assign to user</label>
            {loading && <div>Loading users…</div>}
            {!loading && users.length === 0 && <div>No users available.</div>}
            <select
              id="assignedUser"
              value={assignedUserId}
              onChange={(event) => setAssignedUserId(event.target.value)}
            >
              <option value="">Unassigned</option>
              {users.map((user) => (
                <option key={user.id} value={user.id}>
                  {user.name || user.email}
                </option>
              ))}
            </select>
          </div>
          <div className="form-row">
            <label>Tags</label>
            {loading && <div>Loading tags…</div>}
            {!loading && tags.length === 0 && <div>No tags available.</div>}
            <div>
              {tags.map((tag) => (
                <label key={tag.id} style={{ display: 'block', marginBottom: '0.5rem' }}>
                  <input
                    type="checkbox"
                    checked={selectedTagIds.includes(tag.id)}
                    onChange={() => handleTagToggle(tag.id)}
                  />
                  {' '}
                  {tag.name}
                </label>
              ))}
            </div>
          </div>
          <button type="submit">Create Task</button>
        </form>
      </div>
    </section>
  )
}
