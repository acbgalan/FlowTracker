import { MovementType } from "../enums/movementType.enum";

export interface CreateSavingLogRequest {
    date: string;
    savingGoalId: number;
    amount: number;
    type: MovementType;
    transactionId: number;
}