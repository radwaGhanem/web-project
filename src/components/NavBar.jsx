import { Link } from 'react-router-dom'
import { FaHome, FaTasks, FaPlus, FaUsers, FaSignInAlt, FaSignOutAlt, FaMoon, FaSun } from 'react-icons/fa'

export default function NavBar({ authenticated, onLogout, theme, onToggleTheme }) {
  return (
    <header className="navbar">
      <div>
        <Link className="nav-link" to="/">
          <FaTasks style={{ marginRight: '0.5rem' }} />
          Task Manager
        </Link>
      </div>
      <nav>
        <ul className="nav-list">
          <li>
            <Link className="nav-link" to="/">
              <FaHome style={{ marginRight: '0.5rem' }} />
              Home
            </Link>
          </li>
          {authenticated ? (
            <>
              <li>
                <Link className="nav-link" to="/tasks">
                  <FaTasks style={{ marginRight: '0.5rem' }} />
                  Tasks
                </Link>
              </li>
              <li>
                <Link className="nav-link" to="/tasks/new">
                  <FaPlus style={{ marginRight: '0.5rem' }} />
                  New Task
                </Link>
              </li>
              <li>
                <Link className="nav-link" to="/users">
                  <FaUsers style={{ marginRight: '0.5rem' }} />
                  Users
                </Link>
              </li>
              <li>
                <button className="nav-action" type="button" onClick={onLogout}>
                  <FaSignOutAlt style={{ marginRight: '0.5rem' }} />
                  Logout
                </button>
              </li>
            </>
          ) : (
            <li>
              <Link className="nav-link" to="/login">
                <FaSignInAlt style={{ marginRight: '0.5rem' }} />
                Login
              </Link>
            </li>
          )}
          <li>
            <button className="nav-action" type="button" onClick={onToggleTheme}>
              {theme === 'light' ? <FaMoon style={{ marginRight: '0.5rem' }} /> : <FaSun style={{ marginRight: '0.5rem' }} />}
              {theme === 'light' ? 'Dark' : 'Light'}
            </button>
          </li>
        </ul>
      </nav>
    </header>
  )
}
