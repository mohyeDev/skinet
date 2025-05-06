import { HttpInterceptorFn } from '@angular/common/http';



export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const closedRequest = req.clone({
    withCredentials:true,
  })
  return next(closedRequest);
};
