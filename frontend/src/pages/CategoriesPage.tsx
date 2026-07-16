import { useEffect, useState } from "react";
import { fetchCategoryApi } from "../api/FetchCategoryApi";
import type { Category } from "../types";

export function CategoriesPage() {
  const [categories, setCategories] = useState<Category[]>([]);
  const [name, setName] = useState("");
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetchCategoryApi.getAll()
      .then(categoriesResponse => {
        setCategories(categoriesResponse);
      })
      .catch(error => {
        setError(getErrorMessage(error));
      })
      .finally(() => {
        setIsLoading(false);
      });
  }, []);

  const loadCategories = async () => {
    try {
      setCategories(await fetchCategoryApi.getAll());
    } catch (error) {
      setError(getErrorMessage(error));
    } finally {
      setIsLoading(false);
    }
  };

  const handleRefresh = () => {
    setIsLoading(true);
    setError(null);
    loadCategories();
  };

  const handleCreateCategory = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    const trimmedName = name.trim();
    if (!trimmedName) {
      setError("Category name is required.");
      return;
    }

    setIsSaving(true);
    setError(null);

    try {
      const createdCategory = await fetchCategoryApi.create({ name: trimmedName });
      setCategories(currentCategories => [...currentCategories, createdCategory]);
      setName("");
    } catch (error) {
      setError(getErrorMessage(error));
    } finally {
      setIsSaving(false);
    }
  };

  const handleDeleteCategory = async (categoryId: number) => {
    setError(null);

    try {
      await fetchCategoryApi.delete(categoryId);
      setCategories(currentCategories =>
        currentCategories.filter(category => category.id !== categoryId)
      );
    } catch (error) {
      setError(getErrorMessage(error));
    }
  };

  return (
    <section className="workspace">
      <div className="page-heading">
        <div>
          <p className="eyebrow">Category management</p>
          <h1>Categories</h1>
          <p className="page-copy">Create labels that keep your task list readable.</p>
        </div>
        <button className="button button--secondary" type="button" onClick={handleRefresh}>
          Refresh
        </button>
      </div>

      {error && <div className="alert">{error}</div>}

      <div className="category-layout">
        <section className="workspace-panel">
          <h2>Create category</h2>
          <form className="form-stack" onSubmit={handleCreateCategory}>
            <label className="field">
              <span>Name</span>
              <input
                value={name}
                onChange={event => setName(event.target.value)}
                placeholder="Study"
              />
            </label>

            <button className="button button--primary" type="submit" disabled={isSaving}>
              {isSaving ? "Creating..." : "Create category"}
            </button>
          </form>
        </section>

        <section className="workspace-main">
          <div className="section-header">
            <h2>All categories</h2>
            <span className="count-pill">{categories.length}</span>
          </div>

          {isLoading ? (
            <div className="empty-state">Loading categories...</div>
          ) : categories.length === 0 ? (
            <div className="empty-state">No categories yet. Add one before creating categorized tasks.</div>
          ) : (
            <div className="category-list">
              {categories.map(category => (
                <article className="category-row" key={category.id}>
                  <div>
                    <h3>{category.name}</h3>
                    <span>Category ID {category.id}</span>
                  </div>
                  <button
                    className="button button--ghost"
                    type="button"
                    onClick={() => handleDeleteCategory(category.id)}
                  >
                    Delete
                  </button>
                </article>
              ))}
            </div>
          )}
        </section>
      </div>
    </section>
  );
}

function getErrorMessage(error: unknown): string {
  return error instanceof Error ? error.message : "Something went wrong.";
}
