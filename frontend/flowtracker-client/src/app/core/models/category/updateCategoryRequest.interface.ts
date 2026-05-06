import { Type } from "../enums/type.enum";

export interface UpdateCategoryRequest {
        id: number;
        name: string;
        type: Type;
        icon: string | null;
        description: string;    
}
