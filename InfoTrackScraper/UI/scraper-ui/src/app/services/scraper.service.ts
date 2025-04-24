import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

// Define the interface for the request payload
export interface ScrapeRequest {
  keyword: string;
  url: string;
}

@Injectable({
  providedIn: 'root'
})
export class ScraperService {
  private apiUrl = 'https://localhost:7029/WebScraperContoller';

  constructor(private http: HttpClient) { }

  /**
   * Calls the backend API to scrape search results.
   * @param keyword The search keyword.
   * @param url The URL to find in the search results.
   * @returns An Observable containing the ranking positions as a string (e.g., "1, 11, 21") or "0".
   */
  scrapeUrl(keyword: string, url: string): Observable<string> {
    const requestBody: ScrapeRequest = { keyword, url };
    return this.http.post(`${this.apiUrl}/Scrape`, requestBody, { responseType: 'text' });
  }
}
