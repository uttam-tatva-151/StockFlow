import type { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { LoaderService } from '../../shared/services/loader.service';
import { Router } from '@angular/router';
import { EndPoints } from '../../shared/constants/end-points';
import { catchError, finalize, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const loader = inject(LoaderService);

  // skip modifying the validate-refresh-token endpoint to avoid edge cases
  if (req.url === EndPoints.AUTH.VALIDATE_REFRESH) {
    // still ensure withCredentials true so cookies are sent
    const plainReq = req.clone({ withCredentials: true });
    return next(plainReq);
  }
   // for all other requests, send cookies so server-side auth middleware can check them when needed
   const modified = req.clone({ withCredentials: true });
   loader.show();
  return next(modified).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        // redirect to login or let guards handle
        router.navigate([EndPoints.AUTH.LOGIN]);
      }
      return throwError(() => error);
    }),
    finalize(() => {
      setTimeout(() => loader.hide(), 400);
    })
  );
};
