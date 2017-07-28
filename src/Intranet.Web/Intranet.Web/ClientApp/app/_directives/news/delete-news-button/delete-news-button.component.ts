import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core'
import { AuthenticationService, NewsService } from '../../../_services'

import { News } from '../../../models'

@Component({
    selector: 'delete-news-button',
    templateUrl: 'delete-news-button.component.html',
    styleUrls: ['./delete-news-button.component.css']
})

export class DeleteNewsButtonComponent implements OnInit {
    @Input() newsItem: News
    @Output() onDelete = new EventEmitter<string>()
    isAuthorised: boolean

    constructor(
        private newsService: NewsService,
        private authenticationService: AuthenticationService,
    ) { }

    async ngOnInit() {
      const jwt = await this.authenticationService.getJwtDecoded()
      if (jwt.role === 'Admin' || this.newsItem.user.username === jwt.username) {
        this.isAuthorised = true
      }
    }

    removeNewsItem() {
        this.newsService.deleteItem(this.newsItem.id)
            .subscribe(() => {
                this.onDelete.emit(`${this.newsItem.title} was deleted successfully!`)
            })
    }
}
