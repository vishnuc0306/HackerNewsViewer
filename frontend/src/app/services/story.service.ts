import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Story } from '../models/story';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class StoryService {
  private apiUrl = environment.apiUrl + '/api/stories'; // Define in environment

  constructor(private http: HttpClient) { }

  getNewestStories(searchTerm: string = '', pageNumber: number = 1, pageSize: number = 10): Observable<Story[]> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (searchTerm) {
      params = params.set('searchTerm', searchTerm);
    }

    return this.http.get<Story[]>(`${this.apiUrl}/newest`, { params });
  }
}