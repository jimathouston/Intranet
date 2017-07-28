import { Component, Input } from '@angular/core'

import { News } from '../../../models'

@Component({
    selector: 'news-info-strip',
    templateUrl: 'news-info-strip.component.html',
    styleUrls: ['./news-info-strip.component.css']
})

export class NewsInfoStripComponent {
    @Input() newsItem: News

    hasBeenUpdated(newsItem: News) {
      return newsItem.created !== newsItem.updated
    }
}
