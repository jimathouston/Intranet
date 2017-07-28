import { Component, Input } from '@angular/core'
import { RouterModule, Router, ActivatedRoute } from '@angular/router'
import { Location } from '@angular/common'
import { RequestOptions, Request, RequestMethod, Http, Headers } from '@angular/http'

import { News, Image } from '../../models'
import { AuthenticationService, ConfigService, NewsService } from '../../_services'

@Component({
    selector: 'news-new',
    templateUrl: 'news-new.component.html',
    styleUrls: ['./news.component.css']

})

export class NewsNewComponent {
    newsItem: News
    success: string
    error: string
    newsitems: News[]
    file: File
    apiUrl: string

    constructor(
        private newsService: NewsService,
        private authenticationService: AuthenticationService,
        private configService: ConfigService,
        private route: ActivatedRoute,
        private location: Location,
        private http: Http) {
        this.newsItem = new News()
        this.apiUrl = this.configService.getApiUrl()
    }

    goBack(): void {
        this.location.back()
    }

    handleEditorContentChange(content: string) {
        this.newsItem.text = content
    }

    async publishNewsItem() {
      this.newsItem.published = true
      const successMessage = 'News was created successfully!'
      this.addNewsItemInternal(successMessage)
    }

    async saveDraft() {
      this.newsItem.published = false
      const successMessage = 'News was saved successfully but is not yet published!'
      this.addNewsItemInternal(successMessage)
    }

    private async addNewsItemInternal(successMessage: string) {
        const fileName = await this.newsService.uploadFile(this.file)
        const jwt = await this.authenticationService.getJwtDecoded()

        if (fileName) {
            this.newsItem.headerImage = new Image()
            this.newsItem.headerImage.fileName = fileName
        }

        this.newsItem['userId'] = jwt.displayName

        this.newsService.postItem(this.newsItem).subscribe(
            newsitem => {
                this.success = successMessage
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
