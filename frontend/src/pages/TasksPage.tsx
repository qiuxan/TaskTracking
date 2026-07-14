import { useEffect, useState } from "react";
import { fetchTaskApi } from "../api/FetchTaskApi";
import type { Task } from "../types";

export function TasksPage() {

    const [tasks, setTasks] = useState<Task[]>([]);

    useEffect(() => {
        fetchTaskApi.getAll()
            .then(tasks => {
                setTasks(tasks);
            })
            .catch(error => {
                console.error("Error fetching tasks:", error);
            });
    }, []);

    const formatDate = (value: string): string => {
        const date = new Date(value);
        if (Number.isNaN(date.getTime())) {
            return "Unknown date";
        }

        return new Intl.DateTimeFormat("en-US", {
            month: "short",
            day: "numeric",
            year: "numeric",
        }).format(date);
    };

  return (
    <section className="panel">
      <p className="eyebrow">Full-stack practice</p>
      <h1>React routes are connected.</h1>
      <p>
        This frontend is served by ASP.NET Core from <code>wwwroot</code>. Try
        opening <code>/tasks</code> or <code>/categories</code> directly.
      </p>
      <div className="tasks-section">
        <div className="tasks-section__header">
          <h2 className="tasks-section__title">Your Tasks</h2>
          <span className="tasks-section__count">{tasks.length}</span>
        </div>

        {tasks.length === 0 ? (
          <div className="task-empty-state">No tasks yet. Add one to get started.</div>
        ) : (
          <div className="tasks-grid">
            {tasks.map(task => (
              <article className="task-card" key={task.id}>
                <div className="task-card__top-row">
                  <h3 className="task-card__title">{task.title}</h3>
                  <span
                    className={`task-card__status ${task.isCompleted ? "task-card__status--completed" : "task-card__status--active"}`}
                  >
                    {task.isCompleted ? "Completed" : "In progress"}
                  </span>
                </div>

                <div className="task-card__meta">
                  <span className="task-card__chip">{task.categoryName ?? "Uncategorized"}</span>
                  <span className="task-card__date">Created {formatDate(task.createdAt)}</span>
                </div>
              </article>
            ))}
          </div>
        )}
      </div>
    </section>
  )
}