import { Link, NavLink, Route, Routes } from 'react-router-dom'
import './App.css'

function App() {
  return (
    <div className="app-shell">
      <header className="app-header">
        <Link className="brand" to="/">
          TaskTracking
        </Link>

        <nav className="nav-links" aria-label="Main navigation">
          <NavLink to="/tasks">Tasks</NavLink>
          <NavLink to="/categories">Categories</NavLink>
        </nav>
      </header>

      <main className="page">
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/tasks" element={<TasksPage />} />
          <Route path="/categories" element={<CategoriesPage />} />
          <Route path="*" element={<NotFoundPage />} />
        </Routes>
      </main>
    </div>
  )
}

function HomePage() {
  return (
    <section className="panel">
      <p className="eyebrow">Full-stack practice</p>
      <h1>React routes are connected.</h1>
      <p>
        This frontend is served by ASP.NET Core from <code>wwwroot</code>. Try
        opening <code>/tasks</code> or <code>/categories</code> directly.
      </p>
    </section>
  )
}

function TasksPage() {
  return (
    <section className="panel">
      <p className="eyebrow">Tasks</p>
      <h1>Tasks page</h1>
      <p>Next step: load task data from <code>/api/tasks</code>.</p>
    </section>
  )
}

function CategoriesPage() {
  return (
    <section className="panel">
      <p className="eyebrow">Categories</p>
      <h1>Categories page</h1>
      <p>Next step: load category data from <code>/api/categories</code>.</p>
    </section>
  )
}

function NotFoundPage() {
  return (
    <section className="panel">
      <p className="eyebrow">404</p>
      <h1>Page not found</h1>
      <p>The route exists in React, but no page matches this path.</p>
    </section>
  )
}

export default App
