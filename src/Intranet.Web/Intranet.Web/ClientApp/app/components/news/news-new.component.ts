import { Component, Input } from '@angular/core'
import { RouterModule, Router, ActivatedRoute } from '@angular/router'
import { Location } from '@angular/common'
import { RequestOptions, Request, RequestMethod, Http, Headers } from '@angular/http'

import NewsItem from '../../models/newsItem.model'
import Image from '../../models/image.model'
import { DataService } from '../../shared/data_services/data.service'
import { ConfigService } from '../../shared/api_settings/config.service'
import { AuthenticationService } from '../../_services'

@Component({
    selector: 'news-new',
    templateUrl: 'news-new.component.html',
    styleUrls: ['./news.component.css']

})

export class NewsNewComponent {
    newsItem: NewsItem
    success: string
    error: string
    newsitems: NewsItem[]
    file: File
    apiUrl: string

    constructor(
        private dataService: DataService,
        private authenticationService: AuthenticationService,
        private configService: ConfigService,
        private route: ActivatedRoute,
        private location: Location,
        private http: Http) {
        this.newsItem = new NewsItem()
        this.apiUrl = this.configService.getApiUrl()
    }

    goBack(): void {
        this.location.back()
    }

    handleEditorContentChange(content: string) {
        this.newsItem.text = content
    }

    async addNewsItem(title: string) {
        const fileName = await this.dataService.uploadFile(this.file)
        const jwt = await this.authenticationService.getJwtDecoded()

        if (fileName) {
            this.newsItem.headerImage = new Image()
            this.newsItem.headerImage.fileName = fileName
        }

        this.newsItem.title = title
        this.newsItem.author = jwt['displayName']

        this.dataService.createNewsItem(this.newsItem).then(
            newsitem => {
                this.success = 'News was created successfully!'
                this.error = null
            },
            error => {
                console.log(error)
                this.success = null
                this.error = 'Something went wrong, please check all fields and try again.'
            }
        )
    }

    fileChange(event) {
        this.file = event.target.files[0]
    }
}