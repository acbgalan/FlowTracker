import { CategoryResponse } from "./categoryResponse.interface";

export interface PagedCategoryResponse {
    data: CategoryResponse[];
    page: number;
    limit: number;
    total: number;
}

