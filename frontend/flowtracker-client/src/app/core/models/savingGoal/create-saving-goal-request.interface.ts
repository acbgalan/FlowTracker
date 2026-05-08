export interface CreateSavingGoalRequest {
    name: string;
    targetAmount: number;
    deadline: string | null;
}
