import { Component, OnInit, Input } from '@angular/core'
import { RouterModule, Router, ActivatedRoute, Params } from '@angular/router'
import { Location } from '@angular/common'
import { NewsService } from '../../_services'

import { News } from '../../models'

@Component({
    selector: 'news-edit',
    templateUrl: 'news-edit.component.html',
    styleUrls: ['./news.component.css']
})

export class NewsEditComponent implements OnInit {
    newsItem: News
    info: string = ''
    newsItemEdited: boolean

    constructor(
      private newsService: NewsService,
      private route: ActivatedRoute,
      private location: Location
    ) {
      this.newsItem = new News()
    }

    onEditorContentChange(content: string) {
        this.newsItem.text = content
    }

    goBack(): void {
        this.location.back()
    }

    ngOnInit() {
        const id = +this.route.snapshot.params['id']

        this.newsService.getItem(id).subscribe(
            (newsitem: News) => {
                this.newsItem = newsitem
            },
            error => {
                console.log('Failed while trying to load specific newsitem of news' + error)
            }
        )
    }

    updateNewsItem() {
        this.newsService.putItem(this.newsItem).subscribe(
            () => {
                this.newsItemEdited = true
                this.info = this.newsItem.title + ' was edited successfully!'
            },
            error => {
                console.log('Failed while trying to update the news. ' + error)
            }
        )
    }
}
