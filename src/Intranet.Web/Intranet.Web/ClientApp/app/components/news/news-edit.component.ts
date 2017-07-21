import { Component, OnInit, Input } from '@angular/core'
import { RouterModule, Router, ActivatedRoute, Params } from '@angular/router'
import { Location } from '@angular/common'
import { DataService } from '../../_services'

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
      private dataService: DataService,
      private route: ActivatedRoute,
      private location: Location
    ) { }

    onEditorContentChange(content: string) {
        this.newsItem.text = content
    }

    goBack(): void {
        this.location.back()
    }

    ngOnInit() {
        const id = +this.route.snapshot.params['id']

        this.dataService.getNewsItem(id).subscribe(
            (newsitem: News) => {
                this.newsItem = newsitem
            },
            error => {
                console.log('Failed while trying to load specific newsitem of news' + error)
            }
        )
    }

    updateNewsItem() {
        this.dataService.updateNewsItem(this.newsItem).subscribe(
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
