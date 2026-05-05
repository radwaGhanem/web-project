# Task Manager Frontend

A React + Vite frontend for a task management system. The app provides login-based access to task lists, task creation, task detail management, and user administration.

## Features

- Email/password login using JWT authentication
- Protected routes for tasks and user management
- Task listing with status, assigned user, and tags
- Create new tasks with due date, assigned user, and tags
- View task details, update status, and delete tasks
- List users and delete users (admin-only behavior enforced by backend)
- Theme toggle between light and dark modes

## App structure

- `src/App.jsx` — routes, authentication state, theme support
- `src/components/NavBar.jsx` — main navigation menu
- `src/pages/Login.jsx` — login form
- `src/pages/Tasks.jsx` — task list view
- `src/pages/NewTask.jsx` — task creation form
- `src/pages/TaskDetails.jsx` — view and manage a single task
- `src/pages/Users.jsx` — user list and delete actions
- `src/services/api.js` — Axios API client with auth interceptor
- `src/services/authService.js` — login/logout helpers
- `src/services/taskService.js` — task/user/tag API calls

## Setup instructions

1. Open a terminal in `frontend/frontend`
2. Install dependencies:

   ```bash
   npm install
   ```

3. Start the development server:

   ```bash
   npm run dev
   ```

4. Open the app in your browser at the URL shown by Vite (usually `http://localhost:5173`).

## Backend requirements

- The frontend expects the backend API to run at `http://localhost:5031/api`
- Login and protected endpoints require a valid JWT token stored in `localStorage` as `authToken`

## API routes used

The frontend calls these backend endpoints:

- `POST /api/auth/login` — login and receive a JWT token
- `GET /api/tasks` — list all tasks
- `GET /api/tasks/{id}` — get details for a task
- `POST /api/tasks` — create a new task
- `PATCH /api/tasks/{id}/status?isDone={true|false}` — update task status
- `DELETE /api/tasks/{id}` — delete a task
- `GET /api/users` — list users
- `DELETE /api/users/{id}` — delete a user
- `GET /api/tags` — list available tags

## Notes

- Ensure the backend is running before using task and user pages.
- You may need valid admin credentials for task creation, user listing, or delete operations depending on backend authorization rules.
- To change the API base URL, update `src/services/api.js`.
