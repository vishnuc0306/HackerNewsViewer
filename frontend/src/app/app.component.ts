import { Component } from '@angular/core';
import { CommonModule } from '@angular/common'; // Often needed for ngIf, ngFor
import { StoryListComponent } from './components/story-list/story-list.component';

@Component({
  selector: 'app-root',
  standalone: true, // Confirms it's a standalone component
  imports: [
    CommonModule,
    StoryListComponent
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'HackerNewsViewer.Client';
}