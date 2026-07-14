export interface CrudApi<
  TResponse,
  TCreateRequest,
  TUpdateRequest
> {
  getAll(): Promise<TResponse[]>
  getById(id: number): Promise<TResponse>
  create(request: TCreateRequest): Promise<TResponse>
  update(id: number, request: TUpdateRequest): Promise<void>
  delete(id: number): Promise<void>
}

