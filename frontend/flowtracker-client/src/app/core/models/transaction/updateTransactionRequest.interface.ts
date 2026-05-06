export interface UpdateTransactionRequest {
    id: number;
    amount: number;
    date: string;
    description: string | null;
    categoryId: number;
}
