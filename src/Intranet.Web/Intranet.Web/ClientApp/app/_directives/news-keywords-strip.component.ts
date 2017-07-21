import { Component, Input } from '@angular/core'

@Component({
    selector: 'news-keywords-strip',
    templateUrl: 'news-keywords-strip.component.html',
    styleUrls: ['./news-keywords-strip.component.css']
})

export class NewsKeywordsStripComponent {
    @Input() keywords: string

    formatedKeywords() {
      return this.keywords.replace(/,/g, ', ')
    }
}
