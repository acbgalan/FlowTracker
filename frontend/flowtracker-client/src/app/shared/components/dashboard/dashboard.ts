import { Component, inject, OnDestroy, OnInit, AfterViewInit, ViewChild, ElementRef, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardService } from '../../../core/services/dashboard.service';
import { Chart, registerables } from 'chart.js';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit, AfterViewInit, OnDestroy {
  private dashboardService = inject(DashboardService);
  private cdr = inject(ChangeDetectorRef);

  @ViewChild('balanceCanvas', { static: false }) balanceCanvas!: ElementRef<HTMLCanvasElement>;
  @ViewChild('expensesCanvas', { static: false }) expensesCanvas!: ElementRef<HTMLCanvasElement>;

  private balanceChart: Chart | null = null;
  private expensesChart: Chart | null = null;

  months: { label: string; year: number; month: number }[] = [];
  selectedYear = new Date().getFullYear();
  selectedMonth = new Date().getMonth() + 1; // 1-12

  availableBalance: number | null = null;
  incomes: number | null = null;
  expenses: number | null = null;
  savings: number | null = null;
  balanceHasData = true;
  expensesHasData = true;
  authRequired = false;

  get selectedLabel(): string {
    const s = this.months.find((m) => m.year === this.selectedYear && m.month === this.selectedMonth);
    return s ? s.label : '';
  }

  constructor() {
    Chart.register(...registerables);
    this.buildMonthsList();
  }

  ngOnInit(): void {}

  ngAfterViewInit(): void {
    // Ensure view children (canvases) are available before rendering charts
    this.loadData(this.selectedYear, this.selectedMonth);
  }

  ngOnDestroy(): void {
    this.destroyCharts();
  }

  private buildMonthsList(): void {
    const now = new Date();
    for (let i = 0; i < 12; i++) {
      const d = new Date(now.getFullYear(), now.getMonth() - i, 1);
      const rawLabel = d.toLocaleString(undefined, { month: 'long', year: 'numeric' });
      const label = this.capitalizeLabel(rawLabel);
      this.months.push({ label, year: d.getFullYear(), month: d.getMonth() + 1 });
    }
  }

  onMonthChange(value: string): void {
    const [yearStr, monthStr] = value.split('-');
    const year = parseInt(yearStr, 10);
    const month = parseInt(monthStr, 10);
    if (!isNaN(year) && !isNaN(month)) {
      this.selectedYear = year;
      this.selectedMonth = month;
      this.loadData(year, month);
    }
  }

  private loadData(year: number, month: number): void {
    const date = this.buildDateParam(year, month);
    this.dashboardService.getMonthlySummary(date).subscribe({
      next: (res) => {
        console.debug('monthlySummary', res);
        const summary = this.normalizeSummary(res);
        this.availableBalance = summary.balance;
        this.incomes = summary.incomes;
        this.expenses = summary.expenses;
        this.savings = summary.savings;
        const values = [summary.incomes, summary.expenses, summary.savings, summary.balance];
        const hasData = values.some((value) => value !== 0);
        if (!hasData) {
          this.balanceHasData = false;
          this.destroyBalanceChart();
          this.cdr.detectChanges();
          return;
        }
        this.balanceHasData = true;
        this.cdr.detectChanges();
        const labels = ['Ingresos', 'Gastos', 'Ahorros', 'Saldo'];
        this.renderBalanceChart(labels, values);
      },
      error: (err) => {
        console.error('monthlySummary error', err);
        if (err?.status === 401) {
          this.authRequired = true;
        }
        this.incomes = 0;
        this.expenses = 0;
        this.savings = 0;
        this.availableBalance = 0;
        this.balanceHasData = false;
        this.destroyBalanceChart();
        this.cdr.detectChanges();
      },
    });

    this.dashboardService.getMonthlyExpensesByCategory(date).subscribe({
      next: (items) => {
        console.debug('monthlyExpensesByCategory', items);
        const mapped = this.normalizeExpenses(items);
        if (!mapped.length) {
          this.expensesHasData = false;
          this.destroyExpensesChart();
          this.cdr.detectChanges();
          return;
        }
        this.expensesHasData = true;
        this.cdr.detectChanges();
        const labels = mapped.map((i) => i.label);
        const values = mapped.map((i) => i.amount);
        this.renderExpensesChart(labels, values);
      },
      error: (err) => {
        console.error('monthlyExpensesByCategory error', err);
        if (err?.status === 401) {
          this.authRequired = true;
        }
        this.expensesHasData = false;
        this.destroyExpensesChart();
        this.cdr.detectChanges();
      },
    });
  }

  private normalizeSummary(raw: any): { incomes: number; expenses: number; savings: number; balance: number } {
    const income = this.toNumber(
      raw?.incomes ?? raw?.income ?? raw?.totalIncome ?? raw?.totalIncomes ?? raw?.ingresos
    );
    const expenses = this.toNumber(
      raw?.expenses ?? raw?.expense ?? raw?.totalExpense ?? raw?.totalExpenses ?? raw?.gastos
    );
    const savings = this.toNumber(raw?.savings ?? raw?.saving ?? raw?.ahorros ?? raw?.savingsAmount);
    const balance = this.toNumber(raw?.balance ?? raw?.saldo ?? raw?.availableBalance ?? raw?.netBalance);

    return {
      incomes: income,
      expenses,
      savings,
      balance,
    };
  }

  private normalizeExpenses(items: any[]): { label: string; amount: number }[] {
    if (!Array.isArray(items)) return [];
    return items
      .map((i) => {
        const label = (i?.category ?? i?.categoryName ?? i?.name ?? i?.descripcion ?? '').toString().trim();
        const amount = this.toNumber(i?.amount ?? i?.total ?? i?.value ?? i?.importe ?? i?.expense);
        return { label, amount };
      })
      .filter((i) => i.label.length > 0);
  }

  private toNumber(value: any): number {
    const num = typeof value === 'string' ? parseFloat(value) : Number(value);
    return Number.isFinite(num) ? num : 0;
  }

  private capitalizeLabel(value: string): string {
    if (!value) return value;
    return value.charAt(0).toUpperCase() + value.slice(1);
  }

  formatValue(value: number): string {
    return new Intl.NumberFormat('es-ES', {
      style: 'currency',
      currency: 'EUR',
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    }).format(value);
  }

  private generateLegendWithValues(chart: Chart): any[] {
    const data = chart.data;
    if (!data.labels || !data.datasets.length) return [];
    const values = (data.datasets[0].data as number[]) || [];
    const bgColors = (data.datasets[0].backgroundColor as string[]) || [];
    return (data.labels as string[]).map((label, index) => {
      const value = values[index] ?? 0;
      return {
        text: `${label}: ${this.formatValue(Number(value))}`,
        fillStyle: bgColors[index] || '#6c757d',
        strokeStyle: '#fff',
        lineWidth: 1,
        hidden: !chart.getDataVisibility(index),
        index,
      };
    });
  }

  private buildDateParam(year: number, month: number): string {
    const mm = month.toString().padStart(2, '0');
    return `${year}-${mm}-01`;
  }

  private renderBalanceChart(labels: string[], data: number[]): void {
    this.destroyBalanceChart();
    const canvasEl = this.balanceCanvas?.nativeElement;
    if (!canvasEl) return;
    const ctx = canvasEl.getContext('2d');
    if (!ctx) return;
    this.balanceChart = new Chart(ctx, {
      type: 'doughnut',
      data: {
        labels,
        datasets: [
          {
            data,
            backgroundColor: ['#198754', '#dc3545', '#0d6efd', '#6c757d'],
          },
        ],
      },
      options: {
        plugins: {
          legend: {
            position: 'bottom',
            labels: {
              generateLabels: (chart) => this.generateLegendWithValues(chart),
            },
          },
        },
      },
    });
  }

  private renderExpensesChart(labels: string[], data: number[]): void {
    this.destroyExpensesChart();
    const canvasEl = this.expensesCanvas?.nativeElement;
    if (!canvasEl) return;
    const ctx = canvasEl.getContext('2d');
    if (!ctx) return;
    const bg = labels.map((_, i) => `hsl(${(i * 55) % 360} 70% 50%)`);
    this.expensesChart = new Chart(ctx, {
      type: 'doughnut',
      data: {
        labels,
        datasets: [{ data, backgroundColor: bg }],
      },
      options: {
        plugins: {
          legend: {
            position: 'bottom',
            labels: {
              generateLabels: (chart) => this.generateLegendWithValues(chart),
            },
          },
        },
      },
    });
  }

  private destroyBalanceChart(): void {
    if (this.balanceChart) {
      this.balanceChart.destroy();
      this.balanceChart = null;
    }
  }

  private destroyExpensesChart(): void {
    if (this.expensesChart) {
      this.expensesChart.destroy();
      this.expensesChart = null;
    }
  }

  private destroyCharts(): void {
    this.destroyBalanceChart();
    this.destroyExpensesChart();
  }
}
