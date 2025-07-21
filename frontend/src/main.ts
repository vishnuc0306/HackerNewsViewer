import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { provideHttpClient } from '@angular/common/http';
import { importProvidersFrom } from '@angular/core';
import { FormsModule } from '@angular/forms'; // Import FormsModule

bootstrapApplication(AppComponent, {
  providers: [
    provideHttpClient(), // Provide HttpClient
    importProvidersFrom(FormsModule) // Use importProvidersFrom for FormsModule
    // If you had routing, you'd also have provideRouter(routes) here
  ]
}).catch(err => console.error(err));
