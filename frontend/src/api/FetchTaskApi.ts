import type { Task, CreateTaskRequest, UpdateTaskRequest } from "../types";
import type { CrudApi } from "./interface/crudApi";

class FetchTaskApi implements CrudApi<
  Task,
  CreateTaskRequest,
  UpdateTaskRequest
> {
  async getAll(): Promise<Task[]> {
    const response = await fetch("/api/tasks");

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    const data = await response.json();
    return data as Task[];
  }
  getById(id: number): Promise<Task> {
    console.log("getById called with id:", id);
    throw new Error("Method not implemented.");
  }
  create(request: CreateTaskRequest): Promise<Task> {
    console.log("create called with request:", request);
    throw new Error("Method not implemented.");
  }
  update(id: number, request: UpdateTaskRequest): Promise<void> {
    console.log("update called with id:", id, "and request:", request);
    console.log("getById called with id:", id);

    throw new Error("Method not implemented.");
  }
  delete(id: number): Promise<void> {
    console.log("getById called with id:", id);

    throw new Error("Method not implemented.");
  }
}

export const fetchTaskApi = new FetchTaskApi();
