import { Component, Input } from '@angular/core'

import NewsItem from '../models/newsItem.model'

@Component({
    selector: 'news-info-strip',
    templateUrl: 'news-info-strip.component.html',
    styleUrls: ['./news-info-strip.component.css']
})

export class NewsInfoStripComponent {
    @Input() newsItem: NewsItem

    hasBeenUpdated(newsItem: NewsItem) {
      return newsItem.created !== newsItem.updated
    }
}
