import { inject } from '@angular/core';
import { Router, type CanActivateFn } from '@angular/router';
import { TokenHandlingService } from '../../shared/services/token-handling.service';
import { catchError, map, of, switchMap } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const tokenService = inject(TokenHandlingService);


  return tokenService.validateToken().pipe(
    switchMap(result => {
      // access token valid => allow
      if (result.isValid && result.isExpired === false) return of(true);

      // access token expired (or invalid) -> ask backend to validate refresh token
      return tokenService.validateRefreshToken().pipe(
        map(res => {
          if (res?.succeeded && res.data?.refreshed) {
            // server refreshed tokens and set cookies; now allow route
            return true;
          }

          // server couldn't refresh -> force logout / login
          tokenService.clearAuthCookies();
          router.navigate(['/stackflow/login']);
          return false;
        }),
        catchError(() => {
          // backend call failed (network / 401), treat as unauthorized
          tokenService.clearAuthCookies();
          router.navigate(['/stackflow/login']);
          return of(false);
        })
      );
    }),
    catchError(() => {
      tokenService.clearAuthCookies();
      router.navigate(['/stackflow/login']);
      return of(false);
    })
  );
};
