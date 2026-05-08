import { MovementType } from "../enums/movementType.enum";

export interface CreateSavingLogRequest {

    savingGoalId: number;
    amount: number;
    type: MovementType;
}