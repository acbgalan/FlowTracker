export interface SavingGoalResponse {
    id: number;
    name: string;
    targetAmount: number;
    currentAmount: number;
    deadline: Date | null;
    progressPercentaje: number;
}
