export interface SavingLogResponse {
    id: number;
    name: string;
    targetAmount: number;
    currentAmount: number;
    deadline: string | null;
    progressPercentaje: number;
    transactionId: number;
}
