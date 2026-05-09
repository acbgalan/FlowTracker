export interface CreateTransactionRequest {
    amount: number;
    date: string;
    description: string | null;
    categoryId: number;
    savingGoalId: number | null;
}
