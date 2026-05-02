import { Type } from "../type.enum";

export interface CreateCategoryRequest {
    name: string;
    type: Type;
    icon: string | null;
    description: string;
}