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
  return (
    <section className="panel">
      <p className="eyebrow">Full-stack practice</p>
      <h1>React routes are connected.</h1>
      <p>
        This frontend is served by ASP.NET Core from <code>wwwroot</code>. Try
        opening <code>/tasks</code> or <code>/categories</code> directly.
      </p>
      <ul>
        {tasks.map(task => (
          <li key={task.id}>
            {task.title} - {task.isCompleted ? "Completed" : "Not completed"}
          </li>
        ))}
      </ul>
    </section>
  )
}