import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'; // For *ngIf, *ngFor
import { FormsModule } from '@angular/forms';   // <--- IMPORT FormsModule here for ngModel

import { StoryService } from '../../services/story.service';
import { Story } from '../../models/story';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';

@Component({
  selector: 'app-story-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule // <--- Add FormsModule to the imports array
  ],
  templateUrl: './story-list.component.html',
  styleUrls: ['./story-list.component.css']
})
export class StoryListComponent implements OnInit {
  stories: Story[] = [];
  searchTerm: string = '';
  currentPage: number = 1;
  pageSize: number = 10;
  isLoading: boolean = false;
  hasMoreStories: boolean = true;
  private searchSubject = new Subject<string>();

  constructor(private storyService: StoryService) { }

  ngOnInit(): void {
    this.loadStories();

    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(() => {
      this.currentPage = 1;
      this.loadStories();
    });
  }

  onSearchTermChange(): void {
    this.searchSubject.next(this.searchTerm);
  }

  loadStories(): void {
    this.isLoading = true;
    this.storyService.getNewestStories(this.searchTerm, this.currentPage, this.pageSize)
      .subscribe({
        next: (data) => {
          this.stories = data;
          this.hasMoreStories = data.length === this.pageSize;
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Error fetching stories:', err);
          this.isLoading = false;
        }
      });
  }

  goToPage(page: number): void {
    if (page >= 1 && (page <= this.currentPage || this.hasMoreStories)) {
      this.currentPage = page;
      this.loadStories();
    }
  }

  nextPage(): void {
    if (this.hasMoreStories) {
      this.currentPage++;
      this.loadStories();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadStories();
    }
  }
}