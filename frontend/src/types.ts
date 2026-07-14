export type Category = {
  id: number
  name: string
}

export type CreateCategoryRequest = {
  name: string
}

export type UpdateCategoryRequest = {
  name: string
}

export type Task = {
  id: number
  title: string
  isCompleted: boolean
  createdAt: string
  categoryId: number | null
  categoryName: string | null
}

export type CreateTaskRequest = {
  title: string
  categoryId: number | null
}

export type UpdateTaskRequest = {
  title: string
  isCompleted: boolean
  categoryId: number | null
}
