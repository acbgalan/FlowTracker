import { Component, inject } from '@angular/core';
import { CategoryService } from '../../core/services/category.service';
import { CategoryResponse } from '../../core/models/category/categoryResponse.interface';


@Component({
  selector: 'app-categories',
  imports: [],
  templateUrl: './categories.html',
  styleUrl: './categories.css',
})
export class Categories {
  private categoryService = inject(CategoryService);
  public categories: CategoryResponse[] = [];
  

  public getListData():void{
    this.categoryService.getCategories().subscribe(response => {
      this.categories = response;
    });
  }
  

}
