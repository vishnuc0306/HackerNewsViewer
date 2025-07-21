import { ApplicationConfig, importProvidersFrom } from '@angular/core'; // <-- Add importProvidersFrom here
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms'; // Import FormsModule

import { routes } from './app.routes'; // Your application's routes

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(), // Provide HttpClient
    importProvidersFrom(FormsModule) // Use importProvidersFrom for FormsModule
  ]
};