import { MovementType } from "../enums/movementType.enum";

export interface UpdateSavingLogRequest {
  id: number;
  date: string;
  savingGoalId: number;
  amount: number;
  type: MovementType;
  transactionId: number;
}
    