export interface CategoryResponse {
    id:          number;
    name:        string;
    type:        Type;
    icon:        string;
    description: string;
    userId:      string | null;
}

export enum Type {
    Expense = "Expense",
    Income = "Income",
    Saving = "Saving",
}
