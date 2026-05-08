
import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { SearchService } from '../../services/search.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-header',
  imports: [RouterLink, RouterLinkActive, FormsModule],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header {
  private authService = inject(AuthService);
  private router = inject(Router);
  private searchService = inject(SearchService);

  public searchQuery = '';

  // Disable search on specific routes (saving-goals view)
  public isSearchDisabled(): boolean {
    try {
      const url = this.router.url || '';
      // Enabled only on categories and transactions
      const enabled = url.startsWith('/categories') || url.startsWith('/transactions');
      return !enabled;
    } catch {
      return false;
    }
  }

  onLogout(): void {
    this.authService.clearToken();
    this.router.navigate(['/login']);
  }

  isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }

  onSearch(event: Event): void {
    event.preventDefault();
    if (this.isSearchDisabled()) {
      return;
    }
    this.searchService.setSearchTerm(this.searchQuery || null);
  }

  clearSearch(): void {
    this.searchQuery = '';
    this.searchService.clearSearchTerm();
  }
}
