import { Component, OnInit } from '@angular/core'
import { RouterModule, Router, ActivatedRoute } from '@angular/router'
import { Location } from '@angular/common'
import { AuthenticationService, DataService } from '../../_services'

import { News } from '../../models'

@Component({
    selector: 'news-detail',
    templateUrl: 'news-detail.component.html',
    styleUrls: ['./news.component.css']
})

export class NewsDetailComponent implements OnInit {
    newsItem: News
    selectedNewsItem: News
    info: string
    isAuthorised: boolean

    constructor(
        private dataService: DataService,
        private authenticationService: AuthenticationService,
        private route: ActivatedRoute,
        private location: Location,
    ) {
        this.newsItem = new News()
    }

    goBack(): void {
        this.location.back()
    }

    async ngOnInit() {
        const year: number = this.route.snapshot.params['year']
        const month: number = this.route.snapshot.params['month']
        const day: number = this.route.snapshot.params['day']
        const url: string = this.route.snapshot.params['url']
        const date: Date = new Date(Date.UTC(year, month, day))

        const jwt = await this.authenticationService.getJwtDecoded()

        this.dataService.getNewsItemByDateAndUrl(date, url).subscribe(
            (newsitem: News) => {
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

    onSelect(newsitem: News): void {
        this.selectedNewsItem = newsitem
    }

    handleChangePublishState(info: string) {
      this.info = info
    }

    handleOnDelete(info: string) {
      this.info = info
    }
}
