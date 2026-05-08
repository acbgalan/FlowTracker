import { MovementType } from "../enums/movementType.enum";

export interface UpdateSavingLogRequest {
  id: number;
  savingGoalId: number;
  amount: number;
  type: MovementType;
}
    