import { Component, OnInit, Input } from '@angular/core'
import { RouterModule, Router, ActivatedRoute, Params } from '@angular/router'
import { Location } from '@angular/common'
import { DataService } from '../../shared/data_services/data.service'

import NewsItem from '../../models/newsItem.model'

@Component({
    selector: 'news-edit',
    templateUrl: 'news-edit.component.html',
    styleUrls: ['./news.component.css']
})

export class NewsEditComponent implements OnInit {
    newsitem: NewsItem
    info: string = ''
    newsItemEdited: boolean = false
    loadedNewsItem: boolean

    constructor(private dataService: DataService,
                private route: ActivatedRoute,
                private location: Location) {
                  this.newsitem = new NewsItem()
    }

    onEditorContentChange(content: string) {
        this.newsitem.text = content
    }

    goBack(): void {
        this.location.back()
    }

    ngOnInit() {
        const id = +this.route.snapshot.params['id']

        this.dataService.getNewsItem(id).subscribe((newsitem: NewsItem) => {
            this.newsitem = newsitem
            this.loadedNewsItem = true
        },
        error => {
            console.log('Failed while trying to load specific newsitem of news' + error)
        })
    }

    updateNewsItem() {
         this.dataService.updateNewsItem(this.newsitem)
         .subscribe(() => {
                this.newsItemEdited = true
                this.info = this.newsitem.title + ' was edited successfully!'
                console.log('News was updated successfully. ')
            },
            error => {
                console.log('Failed while trying to update the news. ' + error)
            })

     }
}
