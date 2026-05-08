import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { CreateSavingGoalModal } from './create-saving-goal-modal/create-saving-goal-modal';
import { EditSavingGoalModal } from './edit-saving-goal-modal/edit-saving-goal-modal';
import { DeleteSavingGoalModal } from './delete-saving-goal-modal/delete-saving-goal-modal';

@NgModule({
  imports: [CommonModule, ReactiveFormsModule],
  declarations: [CreateSavingGoalModal, EditSavingGoalModal, DeleteSavingGoalModal],
  exports: [CreateSavingGoalModal, EditSavingGoalModal, DeleteSavingGoalModal],
})
export class SavingGoalsModalsModule {}
