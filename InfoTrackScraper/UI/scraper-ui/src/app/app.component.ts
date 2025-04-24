import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms'; // Import Validators
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { CommonModule } from '@angular/common'; // Import CommonModule
import { ScraperService } from './services/scraper.service'; // Import ScraperService
import { HttpClientModule } from '@angular/common/http'; // Import HttpClientModule
import { tap, map, catchError, finalize } from 'rxjs/operators'; // Import tap, map, catchError, finalize operators
import { of } from 'rxjs'; // Import of

@Component({
  selector: 'app-root',
  standalone: true, // Add standalone: true
  imports: [ReactiveFormsModule, CommonModule, HttpClientModule], // Import ReactiveFormsModule, CommonModule, and HttpClientModule
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'scraper-ui';
  searchForm: FormGroup;
  iframeUrl: SafeResourceUrl;
  scrapeResult: string | null = null;
  scrapeError: string | null = null;
  isLoading: boolean = false;

  constructor(
    private sanitizer: DomSanitizer,
    private scraperService: ScraperService // Inject ScraperService
  ) {
    this.iframeUrl = this.sanitizer.bypassSecurityTrustResourceUrl('about:blank');
    this.searchForm = new FormGroup({
      targetUrl: new FormControl('', Validators.required),
      query: new FormControl('')
    });
  }

  onSubmit() {
    if (this.searchForm.valid) {
      const keywords = this.searchForm.value.query;
      const encodedKeywords = keywords.replace(/ /g, '+');
      const googleSearchUrl = `https://www.google.co.uk/search?num=100&q=${encodedKeywords}`;

      this.iframeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(googleSearchUrl);
    }
  }

  onScrape() {
    if (this.searchForm.valid) {
      this.isLoading = true;
      this.scrapeResult = null;
      this.scrapeError = null;
      const keyword = this.searchForm.value.query;
      const url = this.searchForm.value.targetUrl;

      this.scraperService.scrapeUrl(keyword, url).pipe(
        map(result => `Number of hits for URL: ${result}`),
        catchError(error => { 
          console.error('Error calling scraper API:', error);
          this.scrapeError = 'Failed to fetch scrape results. Please check the console.';
          return of(null);
        }),
        finalize(() => {
          this.isLoading = false;
        })
      ).subscribe(result => {
        if (result) {
          this.scrapeResult = result;
        }
      });
    }
  }
}
