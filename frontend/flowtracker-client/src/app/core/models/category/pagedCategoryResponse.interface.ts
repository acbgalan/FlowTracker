import { CategoryResponse } from "./categoryResponse.interface";

export interface PagedCategoryResponseInterface {
    data: CategoryResponse[];
    page: number;
    limit: number;
    total: number;
}

