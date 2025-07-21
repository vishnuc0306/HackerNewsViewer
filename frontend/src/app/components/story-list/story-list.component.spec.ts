import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { StoryListComponent } from './story-list.component';
import { StoryService } from '../../services/story.service';
import { of } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';

describe('StoryListComponent', () => {
  let component: StoryListComponent;
  let fixture: ComponentFixture<StoryListComponent>;
  let mockStoryService: any;

  beforeEach(async () => {
    mockStoryService = jasmine.createSpyObj('StoryService', ['getNewestStories']);
    mockStoryService.getNewestStories.and.returnValue(of([
      { id: 1, title: 'Story 1', link: 'http://link1.com' },
      { id: 2, title: 'Story 2 (no link)', link: null }
    ]));

    await TestBed.configureTestingModule({
      declarations: [StoryListComponent],
      imports: [FormsModule],
      providers: [{ provide: StoryService, useValue: mockStoryService }]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StoryListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges(); // ngOnInit triggers loadStories
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load stories on init', () => {
    expect(mockStoryService.getNewestStories).toHaveBeenCalledWith('', 1, 10);
    expect(component.stories.length).toBe(2);
    expect(component.stories[0].title).toBe('Story 1');
  });

  it('should display stories with links', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    const links = compiled.querySelectorAll('ul.story-list li a');
    expect(links.length).toBe(1);
    expect(links[0].textContent).toContain('Story 1');
    expect(links[0].getAttribute('href')).toBe('http://link1.com');
  });

  it('should display stories without links correctly', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    const noLinkSpans = compiled.querySelectorAll('ul.story-list li span');
    expect(noLinkSpans.length).toBe(1);
    expect(noLinkSpans[0].textContent).toContain('Story 2 (No Link Available)');
  });

  it('should call loadStories with search term after debounce', fakeAsync(() => {
    const inputElement = fixture.debugElement.query(By.css('.search-bar input')).nativeElement;
    inputElement.value = 'test search';
    inputElement.dispatchEvent(new Event('input'));

    expect(mockStoryService.getNewestStories).toHaveBeenCalledTimes(1); // Not yet called due to debounce
    tick(300); // Advance time by debounceTime
    expect(mockStoryService.getNewestStories).toHaveBeenCalledTimes(2); // Now it should be called
    expect(component.searchTerm).toBe('test search');
    expect(component.currentPage).toBe(1); // Should reset page
  }));

  it('should navigate to next page', () => {
    component.hasMoreStories = true; // Simulate having more stories
    component.nextPage();
    expect(component.currentPage).toBe(2);
    expect(mockStoryService.getNewestStories).toHaveBeenCalledWith('', 2, 10);
  });

  it('should navigate to previous page', () => {
    component.currentPage = 2;
    component.previousPage();
    expect(component.currentPage).toBe(1);
    expect(mockStoryService.getNewestStories).toHaveBeenCalledWith('', 1, 10);
  });
});