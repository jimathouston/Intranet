import { Component, OnInit, Input } from '@angular/core'
import { RouterModule, Router, ActivatedRoute, Params } from '@angular/router'
import { Location } from '@angular/common'
import { DataService } from '../../shared/data_services/data.service'
import { INewsItem } from '../../shared/interfaces'

import NewsItem from '../../models/newsitem'

@Component({
    selector: 'news-edit',
    templateUrl: 'news-edit.component.html',
    styleUrls: ['./news.component.css']
})

export class NewsEditComponent implements OnInit {
    newsitem: INewsItem
    info: string = ''
    newsItemEdited: boolean = false

    constructor(private dataService: DataService,
                private route: ActivatedRoute,
                private location: Location) {
                  this.newsitem = new NewsItem()
                }

    goBack(): void {
        this.location.back()
    }

    ngOnInit() {
        const id = +this.route.snapshot.params['id']

        this.dataService.getNewsItem(id).subscribe((newsitem: INewsItem) => {
            this.newsitem = newsitem
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
