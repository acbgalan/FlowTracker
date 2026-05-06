import { TransactionResponse } from "./transactionResponse.interface";

export interface PagedTransactionResponse {
    data: TransactionResponse[];
    page: number;
    limit: number;
    total: number;
}
