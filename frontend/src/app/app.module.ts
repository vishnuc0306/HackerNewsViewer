import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'; 
import { provideHttpClient,withFetch } from '@angular/common/http';
import { AppComponent } from './app.component';
import { StoryListComponent } from './components/story-list/story-list.component';

@NgModule({
  declarations: [],
  imports: [
    BrowserModule,
    FormsModule
    ],
  providers: [provideHttpClient(withFetch())

  ],
  bootstrap: []
})
export class AppModule { }