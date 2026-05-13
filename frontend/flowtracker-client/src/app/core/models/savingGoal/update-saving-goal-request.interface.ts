export interface UpdateSavingGoalRequest {
    id: number;
    name: string;
    targetAmount: number;
    deadline: string | null;
    completed: boolean;
}
