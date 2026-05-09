import { Type } from "../enums/type.enum";

export interface TransactionResponse {
    id: number;
    amount: number;
    date: Date;
    description: string | null;
    savingGoalId: number | null;
    categoryName: string;
    type: Type;
    icon: string;
    categoryDescription: string;
}
