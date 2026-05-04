import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SearchService {
  private searchTermSubject = new Subject<string | null>();
  
  public searchTerm$ = this.searchTermSubject.asObservable();

  public setSearchTerm(term: string | null): void {
    this.searchTermSubject.next(term);
  }

  public clearSearchTerm(): void {
    this.searchTermSubject.next(null);
  }
}
