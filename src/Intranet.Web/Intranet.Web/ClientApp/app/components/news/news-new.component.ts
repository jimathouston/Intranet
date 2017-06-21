import { Component, Input } from '@angular/core'
import { RouterModule, Router, ActivatedRoute } from '@angular/router'
import { Location } from '@angular/common'
import { RequestOptions, Request, RequestMethod, Http, Headers } from '@angular/http'

import NewsItem from '../../models/newsItem.model'
import Image from '../../models/image.model'
import { DataService } from '../../shared/data_services/data.service'
import { ConfigService } from '../../shared/api_settings/config.service'

@Component({
    selector: 'news-new',
    templateUrl: 'news-new.component.html',
    styleUrls: ['./news.component.css']

})

export class NewsNewComponent {
    newsitem: NewsItem
    success: string
    error: string
    newsitems: NewsItem[]
    file: File
    apiUrl: string

    constructor(private dataService: DataService,
        private configService: ConfigService,
        private route: ActivatedRoute,
        private location: Location,
        private http: Http) {
        this.newsitem = new NewsItem()
        this.apiUrl = this.configService.getApiUrl()
    }

    goBack(): void {
        this.location.back()
    }

    handleEditorContentChange(content: string) {
        this.newsitem.text = content
    }

    async addNewsItem(title: string, author: string) {
        const fileName = await this.uploadImage()

        if (fileName) {
            this.newsitem.headerImage = new Image()
            this.newsitem.headerImage.fileName = fileName
        }

        this.newsitem.title = title
        this.newsitem.author = author

        this.dataService.createNewsItem(this.newsitem).then(
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

    async uploadImage(): Promise<string> {
            if (typeof this.file !== 'undefined') {

                const formData: FormData = new FormData()
                const headers = new Headers()

                formData.append(this.file.name, this.file)

                headers.append('Accept', 'application/json')
                // headers.append('Content-Type', 'multipart/form-data')
                // DON'T SET THE Content-Type to multipart/form-data
                // You'll get the Missing content-type boundary error
                const options = new RequestOptions({ headers: headers })

                const response = await this.http.post(this.apiUrl + '/upload', formData, options).toPromise()

                return response.json().fileName
            }

            return null
    }
}