import { Type } from "../enums/type.enum";

export interface CreateCategoryRequest {
    name: string;
    type: Type;
    icon: string | null;
    description: string;
}