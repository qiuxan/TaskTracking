import { useEffect, useState } from "react";
import { fetchTaskApi } from "../api/FetchTaskApi";
import { fetchCategoryApi } from "../api/FetchCategoryApi";
import type { Category, Task } from "../types";

export function TasksPage() {

    const [tasks, setTasks] = useState<Task[]>([]);
    const [categories, setCategories] = useState<Category[]>([]);
    const [title, setTitle] = useState("");
    const [categoryId, setCategoryId] = useState("");
    const [isLoading, setIsLoading] = useState(true);
    const [isSaving, setIsSaving] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        Promise.all([
            fetchTaskApi.getAll(),
            fetchCategoryApi.getAll(),
        ])
            .then(([tasksResponse, categoriesResponse]) => {
                setTasks(tasksResponse);
                setCategories(categoriesResponse);
            })
            .catch(error => {
                setError(getErrorMessage(error));
            })
            .finally(() => {
                setIsLoading(false);
            });
    }, []);

    const loadPageData = async () => {
        try {
            const [tasksResponse, categoriesResponse] = await Promise.all([
                fetchTaskApi.getAll(),
                fetchCategoryApi.getAll(),
            ]);

            setTasks(tasksResponse);
            setCategories(categoriesResponse);
        } catch (error) {
            setError(getErrorMessage(error));
        } finally {
            setIsLoading(false);
        }
    };

    const handleRefresh = () => {
        setIsLoading(true);
        setError(null);
        loadPageData();
    };

    const handleCreateTask = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        const trimmedTitle = title.trim();
        if (!trimmedTitle) {
            setError("Task title is required.");
            return;
        }

        setIsSaving(true);
        setError(null);

        try {
            const createdTask = await fetchTaskApi.create({
                title: trimmedTitle,
                categoryId: categoryId ? Number(categoryId) : null,
            });

            setTasks(currentTasks => [createdTask, ...currentTasks]);
            setTitle("");
            setCategoryId("");
        } catch (error) {
            setError(getErrorMessage(error));
        } finally {
            setIsSaving(false);
        }
    };

    const handleToggleTask = async (task: Task) => {
        setError(null);

        try {
            await fetchTaskApi.update(task.id, {
                title: task.title,
                isCompleted: !task.isCompleted,
                categoryId: task.categoryId,
            });

            setTasks(currentTasks =>
                currentTasks.map(currentTask =>
                    currentTask.id === task.id
                        ? { ...currentTask, isCompleted: !currentTask.isCompleted }
                        : currentTask
                )
            );
        } catch (error) {
            setError(getErrorMessage(error));
        }
    };

    const handleDeleteTask = async (taskId: number) => {
        setError(null);

        try {
            await fetchTaskApi.delete(taskId);
            setTasks(currentTasks => currentTasks.filter(task => task.id !== taskId));
        } catch (error) {
            setError(getErrorMessage(error));
        }
    };

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

    const completedCount = tasks.filter(task => task.isCompleted).length;
    const activeCount = tasks.length - completedCount;

  return (
    <section className="workspace">
      <div className="page-heading">
        <div>
          <p className="eyebrow">Task workspace</p>
          <h1>Tasks</h1>
          <p className="page-copy">Plan, classify, and finish work backed by your SQL Server API.</p>
        </div>
        <button className="button button--secondary" type="button" onClick={handleRefresh}>
          Refresh
        </button>
      </div>

      {error && <div className="alert">{error}</div>}

      <div className="task-layout">
        <aside className="workspace-panel">
          <h2>Create task</h2>
          <form className="form-stack" onSubmit={handleCreateTask}>
            <label className="field">
              <span>Title</span>
              <input
                value={title}
                onChange={event => setTitle(event.target.value)}
                placeholder="Write API integration notes"
              />
            </label>

            <label className="field">
              <span>Category</span>
              <select value={categoryId} onChange={event => setCategoryId(event.target.value)}>
                <option value="">Uncategorized</option>
                {categories.map(category => (
                  <option key={category.id} value={category.id}>
                    {category.name}
                  </option>
                ))}
              </select>
            </label>

            <button className="button button--primary" type="submit" disabled={isSaving}>
              {isSaving ? "Creating..." : "Create task"}
            </button>
          </form>
        </aside>

        <div className="workspace-main">
          <div className="stats-grid">
            <div className="stat">
              <span>Total</span>
              <strong>{tasks.length}</strong>
            </div>
            <div className="stat">
              <span>Active</span>
              <strong>{activeCount}</strong>
            </div>
            <div className="stat">
              <span>Completed</span>
              <strong>{completedCount}</strong>
            </div>
          </div>

          {isLoading ? (
            <div className="empty-state">Loading tasks...</div>
          ) : tasks.length === 0 ? (
            <div className="empty-state">No tasks yet. Create one to start testing CRUD.</div>
        ) : (
          <div className="task-list">
            {tasks.map(task => (
              <article className="task-card" key={task.id}>
                <div className="task-card__top-row">
                  <button
                    className={`status-toggle ${task.isCompleted ? "status-toggle--completed" : ""}`}
                    type="button"
                    onClick={() => handleToggleTask(task)}
                    aria-label={task.isCompleted ? "Mark task active" : "Mark task completed"}
                  />
                  <div>
                    <h3 className="task-card__title">{task.title}</h3>
                    <div className="task-card__meta">
                      <span>{task.categoryName ?? "Uncategorized"}</span>
                      <span>Created {formatDate(task.createdAt)}</span>
                    </div>
                  </div>
                  <span
                    className={`task-card__status ${task.isCompleted ? "task-card__status--completed" : "task-card__status--active"}`}
                  >
                    {task.isCompleted ? "Completed" : "In progress"}
                  </span>
                </div>

                <div className="task-card__actions">
                  <button className="button button--ghost" type="button" onClick={() => handleDeleteTask(task.id)}>
                    Delete
                  </button>
                </div>
              </article>
            ))}
          </div>
        )}
        </div>
      </div>
    </section>
  )
}

function getErrorMessage(error: unknown): string {
    return error instanceof Error ? error.message : "Something went wrong.";
}
