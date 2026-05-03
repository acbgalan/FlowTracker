import { Type } from "../enums/type.enum";

export interface UpdateCategoryRequestInterface {
        id: number;
        name: string;
        type: Type;
        icon: string | null;
        description: string;    
}
