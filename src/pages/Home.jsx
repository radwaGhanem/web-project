import { Link } from 'react-router-dom'

export default function Home({ authenticated }) {
  return (
    <section className="page-content">
      <div className="card hero-card">
        <div>
          <h1 className="page-title">React Task Manager</h1>
          <p className="hero-copy">
            Connect to the existing backend API and manage tasks directly from the browser with a clean,
            modern interface.
          </p>
          <p className="footer-note">
            Use <strong>admin@taskmanager.com</strong> / <strong>Admin123!</strong> to test the full task creation and user endpoints.
          </p>
        </div>
        <div className="form-row" style={{ marginTop: '1.25rem', maxWidth: '320px' }}>
          {authenticated ? (
            <Link className="nav-link" to="/tasks">
              View Tasks
            </Link>
          ) : (
            <Link className="nav-link" to="/login">
              Sign In to Continue
            </Link>
          )}
        </div>
      </div>
    </section>
  )
}
