import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { StoryService } from './story.service';
import { Story } from '../models/story';
import { environment } from '../../environments/environment';

describe('StoryService', () => {
  let service: StoryService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [StoryService]
    });
    service = TestBed.inject(StoryService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify(); // Ensure that there are no outstanding requests.
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should retrieve newest stories', () => {
    const dummyStories: Story[] = [{ id: 1, title: 'Test Story 1', link: 'http://test1.com' }];

    service.getNewestStories().subscribe(stories => {
      expect(stories.length).toBe(1);
      expect(stories).toEqual(dummyStories);
    });

    const req = httpMock.expectOne(`${environment.apiUrl}/api/stories/newest?pageNumber=1&pageSize=10`);
    expect(req.request.method).toBe('GET');
    req.flush(dummyStories);
  });

  it('should send search term and pagination params', () => {
    service.getNewestStories('angular', 2, 5).subscribe();

    const req = httpMock.expectOne(`${environment.apiUrl}/api/stories/newest?pageNumber=2&pageSize=5&searchTerm=angular`);
    expect(req.request.method).toBe('GET');
    req.flush([]);
  });
});