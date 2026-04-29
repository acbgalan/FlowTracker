import { Component, inject, OnInit, ChangeDetectorRef, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CategoryService } from '../../core/services/category.service';
import { CategoryResponse } from '../../core/models/categoryResponse.interface';

@Component({
  selector: 'app-categories',
  imports: [CommonModule],
  templateUrl: './categories.html',
  styleUrl: './categories.css',
})
export class Categories implements OnInit {
  private categoryService = inject(CategoryService);
  private cdr = inject(ChangeDetectorRef);
  public categories = signal<CategoryResponse[]>([]);
  
  public ngOnInit(){
    this.categoryService.getCategories().subscribe({
      next: (response) => {
        this.categories.set(response);
        console.log('Categorías cargadas:', this.categories());
        this.cdr.markForCheck();
      },
      error: (err) => {
        console.error('Error al cargar categorías:', err);
      }
    });
  }

}
