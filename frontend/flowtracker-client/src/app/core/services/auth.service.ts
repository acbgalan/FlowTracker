import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly TOKEN_KEY = 'auth_token';
  private readonly EXPIRATION_KEY = 'auth_expiration';

  public saveToken(token: string, expiration: string | Date, remember = true): void {
    const storage = remember ? localStorage : sessionStorage;
    const expDate = typeof expiration === 'string' ? new Date(expiration) : expiration;
    storage.setItem(this.TOKEN_KEY, token);
    storage.setItem(this.EXPIRATION_KEY, expDate.toISOString());
  }

  public getToken(): string | null {
    // Prefer token in localStorage, fallback to sessionStorage
    const sources = [localStorage, sessionStorage];

    for (const src of sources) {
      const token = src.getItem(this.TOKEN_KEY);
      const expiration = src.getItem(this.EXPIRATION_KEY);

      if (token && expiration) {
        const expirationDate = new Date(expiration);
        if (new Date() < expirationDate) {
          return token;
        } else {
          this.clearToken();
          return null;
        }
      }
    }

    return null;
  }

  public clearToken(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.EXPIRATION_KEY);
    sessionStorage.removeItem(this.TOKEN_KEY);
    sessionStorage.removeItem(this.EXPIRATION_KEY);
  }

  public isAuthenticated(): boolean {
    return this.getToken() !== null;
  }
}
