import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core'
import { AuthenticationService, NewsService } from '../../../_services'

import { News } from '../../../models'

@Component({
    selector: 'toggle-published',
    templateUrl: 'toggle-published.component.html',
    styleUrls: ['./toggle-published.component.css']
})

export class TogglePublishedComponent implements OnInit {
    @Input() newsitem: News
    @Output() onChangePublishState = new EventEmitter<string>()
    isAuthorised: boolean

    constructor(
        private newsService: NewsService,
        private authenticationService: AuthenticationService,
    ) { }

    async ngOnInit() {
      const jwt = await this.authenticationService.getJwtDecoded()
      if (jwt.role === 'Admin' || this.newsitem.user.username === jwt.username) {
        this.isAuthorised = true
      }
    }

    publishNewsItem(newsitem: News) {
        newsitem.published = true
        this.newsService.putItem(newsitem)
            .subscribe((savedNewsItem) => {
                newsitem = savedNewsItem
                if (this.onChangePublishState) {
                  this.onChangePublishState.emit(`${newsitem.title} was published successfully!`)
                }
            })
    }

    unpublishNewsItem(newsitem: News) {
        newsitem.published = false
        this.newsService.putItem(newsitem)
            .subscribe((savedNewsItem) => {
                newsitem = savedNewsItem
                if (this.onChangePublishState) {
                  this.onChangePublishState.emit(`${newsitem.title} was unpublished successfully!`)
                }
            })
    }
}
