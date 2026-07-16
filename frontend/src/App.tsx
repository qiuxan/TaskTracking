import { Link, NavLink, Route, Routes } from 'react-router-dom'
import './App.css'
import { CategoriesPage } from './pages/CategoriesPage'
import { TasksPage } from './pages/TasksPage'

function App() {
  return (
    <div className="app-shell">
      <header className="app-header">
        <Link className="brand" to="/">
          TaskTracking
        </Link>

        <nav className="nav-links" aria-label="Main navigation">
          <NavLink to="/">Tasks</NavLink>
          <NavLink to="/categories">Categories</NavLink>
        </nav>
      </header>

      <main className="page">
        <Routes>
          <Route path="/" element={<TasksPage />} />
          <Route path="/categories" element={<CategoriesPage />} />
          <Route path="*" element={<NotFoundPage />} />
        </Routes>
      </main>
    </div>
  )
}




function NotFoundPage() {
  return (
    <section className="workspace">
      <p className="eyebrow">404</p>
      <h1>Page not found</h1>
      <p className="page-copy">The route exists in React, but no page matches this path.</p>
    </section>
  )
}

export default App
