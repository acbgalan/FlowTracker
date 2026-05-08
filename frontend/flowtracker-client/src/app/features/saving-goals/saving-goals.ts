import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  OnInit,
  inject,
} from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { SavingGoalService } from '../../core/services/savingGoal.service';
import { SavingGoalResponse } from '../../core/models/savingGoal/saving-goal-response.interface';
import { SavingGoalsModalsModule } from './saving-goals.modals';

@Component({
  selector: 'app-saving-goals',
  standalone: true,
  imports: [
    CurrencyPipe,
    DatePipe,
    SavingGoalsModalsModule,
  ],
  templateUrl: './saving-goals.html',
  styleUrl: './saving-goals.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SavingGoals implements OnInit {
  private savingGoalService = inject(SavingGoalService);
  private cdr = inject(ChangeDetectorRef);

  public savingGoals: SavingGoalResponse[] = [];
  public isLoading = false;
  public errorMessage = '';
  public savingGoalToEdit: SavingGoalResponse | null = null;
  public savingGoalToDelete: SavingGoalResponse | null = null;

  public ngOnInit(): void {
    this.getListData();
  }

  public getListData(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.cdr.markForCheck();

    this.savingGoalService.getSavingGoals().subscribe({
      next: response => {
        this.savingGoals = response;
        this.isLoading = false;
        this.cdr.markForCheck();
        this.notifyTooltipRefresh();
      },
      error: () => {
        this.savingGoals = [];
        this.isLoading = false;
        this.errorMessage = 'No se pudieron cargar las metas de ahorro.';
        this.cdr.markForCheck();
        this.notifyTooltipRefresh();
      },
    });
  }

  public prepareEdit(goal: SavingGoalResponse): void {
    this.savingGoalToEdit = { ...goal };
  }

  public prepareDelete(goal: SavingGoalResponse): void {
    this.savingGoalToDelete = { ...goal };
  }

  public getProgressColor(progress: number): string {
    const normalized = Math.max(0, Math.min(100, progress));
    const lightness = Math.round(70 - (normalized / 100) * 35);
    return `hsl(120, 60%, ${lightness}%)`;
  }

  private notifyTooltipRefresh(): void {
    window.dispatchEvent(new CustomEvent('bootstrap:refresh-tooltips'));
  }
}
