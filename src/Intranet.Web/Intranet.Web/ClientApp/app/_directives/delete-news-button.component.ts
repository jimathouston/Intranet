import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core'
import { DataService } from '../shared/data_services/data.service'
import { AuthenticationService } from '../_services'

import NewsItem from '../models/newsItem.model'

@Component({
    selector: 'delete-news-button',
    templateUrl: 'delete-news-button.component.html',
    styleUrls: ['./delete-news-button.component.css']
})

export class DeleteNewsButtonComponent implements OnInit {
    @Input() newsItem: NewsItem
    @Output() onDelete = new EventEmitter<string>()
    isAuthorised: boolean

    constructor(
        private dataService: DataService,
        private authenticationService: AuthenticationService,
    ) { }

    async ngOnInit() {
      const jwt = await this.authenticationService.getJwtDecoded()
      if (jwt.role === 'Admin' || this.newsItem.user.username === jwt.username) {
        this.isAuthorised = true
      }
    }

    removeNewsItem() {
        this.dataService.deleteNewsItem(this.newsItem.id)
            .subscribe(() => {
                this.onDelete.emit(`${this.newsItem.title} was deleted successfully!`)
            })
    }
}
