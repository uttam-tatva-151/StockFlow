import { inject } from '@angular/core';
import { Router, type CanActivateFn } from '@angular/router';
import { TokenHandlingService } from '../../shared/services/token-handling.service';
import { catchError, map, of, switchMap } from 'rxjs';

export const entryGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const tokenService = inject(TokenHandlingService);

  return tokenService.validateToken().pipe(
    switchMap(result => {
      if (result.isValid && result.isExpired === false) {
        router.navigateByUrl('/dashboard');
        return of(false);
      }

      // If access token expired but backend can refresh, let them in (and navigate)
      return tokenService.validateRefreshToken().pipe(
        map(res => {
          if (res?.succeeded && res.data?.refreshed) {
            router.navigateByUrl('/dashboard');
            return false;
          }
          // no refresh possible -> show entry (login)
          return true;
        }),
        catchError(() => {
          return of(true);
        })
      );
    }),
    catchError(() => of(true))
  );
};
