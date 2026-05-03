import { Type } from "../enums/type.enum";

export interface CategoryResponse {
    id:          number;
    name:        string;
    type:        Type;
    icon:        string;
    description: string;
    userId:      string | null;
}
