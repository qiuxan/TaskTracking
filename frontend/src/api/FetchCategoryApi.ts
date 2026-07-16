import type { Category, CreateCategoryRequest, UpdateCategoryRequest } from "../types";
import type { CrudApi } from "./interface/crudApi";

class FetchCategoryApi implements CrudApi<
  Category,
  CreateCategoryRequest,
  UpdateCategoryRequest
> {
  async getAll(): Promise<Category[]> {
    const response = await fetch("/api/categories");

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    return await response.json() as Category[];
  }

  async getById(id: number): Promise<Category> {
    const response = await fetch(`/api/categories/${id}`);

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    return await response.json() as Category;
  }

  async create(request: CreateCategoryRequest): Promise<Category> {
    const response = await fetch("/api/categories", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(request),
    });

    if (!response.ok) {
      throw new Error(await readErrorMessage(response));
    }

    return await response.json() as Category;
  }

  async update(id: number, request: UpdateCategoryRequest): Promise<void> {
    const response = await fetch(`/api/categories/${id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(request),
    });

    if (!response.ok) {
      throw new Error(await readErrorMessage(response));
    }
  }

  async delete(id: number): Promise<void> {
    const response = await fetch(`/api/categories/${id}`, {
      method: "DELETE",
    });

    if (!response.ok) {
      throw new Error(await readErrorMessage(response));
    }
  }
}

async function readErrorMessage(response: Response): Promise<string> {
  const text = await response.text();
  return text || `HTTP error! status: ${response.status}`;
}

export const fetchCategoryApi = new FetchCategoryApi();
