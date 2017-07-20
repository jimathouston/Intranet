import { Component, OnInit } from '@angular/core'
import { RouterModule, Router, ActivatedRoute } from '@angular/router'
import { Location } from '@angular/common'
import { DataService } from '../../shared/data_services/data.service'
import { AuthenticationService } from '../../_services'

import NewsItem from '../../models/newsItem.model'

@Component({
    selector: 'news-detail',
    templateUrl: 'news-detail.component.html',
    styleUrls: ['./news.component.css']
})

export class NewsDetailComponent implements OnInit {
    id: number
    newsItem: NewsItem
    selectedNewsItem: NewsItem
    info: string
    isAuthorised: boolean

    constructor(
        private dataService: DataService,
        private authenticationService: AuthenticationService,
        private route: ActivatedRoute,
        private location: Location,
    ) {
        this.newsItem = new NewsItem()
    }

    goBack(): void {
        this.location.back()
    }

    async ngOnInit() {
        this.id = +this.route.snapshot.params['id']
        const jwt = await this.authenticationService.getJwtDecoded()

        this.dataService.getNewsItem(this.id).subscribe(
            (newsitem: NewsItem) => {
                this.newsItem = newsitem
                if (jwt.role === 'Admin' || this.newsItem.user.username === jwt.username) {
                  this.isAuthorised = true
                }
            },
            error => {
                console.log('Failed while trying to load specific newsitem of news' + error)
            }
        )
    }

    onSelect(newsitem: NewsItem): void {
        this.selectedNewsItem = newsitem
    }

    handleChangePublishState(info: string) {
      this.info = info
    }

    handleOnDelete(info: string) {
      this.info = info
    }
}
