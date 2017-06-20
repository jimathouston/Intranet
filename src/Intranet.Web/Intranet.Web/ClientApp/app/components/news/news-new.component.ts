import { Component, Input } from '@angular/core'
import { RouterModule, Router, ActivatedRoute } from '@angular/router'
import { INewsItem } from '../../shared/interfaces'
import { DataService } from '../../shared/data_services/data.service'
import { Location } from '@angular/common'

import NewsItem from '../../models/newsitem'

@Component({
    selector: 'news-new',
    templateUrl: 'news-new.component.html',
    styleUrls: ['./news.component.css']

})

export class NewsNewComponent {
    newsitem: INewsItem
    info: string = ''
    newsitems: INewsItem[]
    newsItemCreated: boolean = false

    constructor(private dataService: DataService,
                private route: ActivatedRoute,
                private location: Location) {
                  this.newsitem = new NewsItem()
                }

    goBack(): void {
        this.location.back()
    }

    handleEditorContentChange(content: string) {
        this.newsitem.text = content
    }

    addNewsItem(title: string, author: string) {
        this.newsitem.title = title
        this.newsitem.author = author

        this.dataService.createNewsItem(this.newsitem).then((newsitem) => {
            this.newsItemCreated = true
            console.log(newsitem)
             this.info = 'News was created successfully!'
          },
          (error) => console.log(error))
    }
}