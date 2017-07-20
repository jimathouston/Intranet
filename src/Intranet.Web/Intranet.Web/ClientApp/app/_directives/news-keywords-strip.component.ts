import { Component, Input } from '@angular/core'

import NewsItem from '../models/newsItem.model'

@Component({
    selector: 'news-keywords-strip',
    templateUrl: 'news-keywords-strip.component.html',
    styleUrls: ['./news-keywords-strip.component.css']
})

export class NewsKeywordsStripComponent {
    @Input() keywords: string

    formatedKeywords() {
      console.log(this.keywords)
      return this.keywords.replace(/,/g, ', ')
    }
}
