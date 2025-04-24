import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { AppComponent } from './app/app.component';
import { appConfig } from './app/app.config';
import { MessageService } from 'primeng/api';

// Required default export for Angular SSR (prerendering support)
export default function () {
  return bootstrapApplication(AppComponent, {
    ...appConfig,
    providers: [
      ...(appConfig.providers || []),
      provideHttpClient(withInterceptorsFromDi()),
      MessageService
    ]
  });
}
