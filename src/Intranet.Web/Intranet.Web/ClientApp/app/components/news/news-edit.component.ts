import { Component, OnInit, Input } from '@angular/core'
import { RouterModule, Router, ActivatedRoute, Params } from '@angular/router'
import { Location } from '@angular/common'
import { DataService } from '../../shared/data_services/data.service'
import { INewsItem } from '../../shared/interfaces'
import { MaterialModule } from '@angular/material'


@Component({
    selector: 'news-edit',
    templateUrl: 'news-edit.component.html'
})

export class NewsEditComponent implements OnInit {
    newsItem: INewsItem
    info: string = ''
    newsItemEdited: boolean = false

    constructor(private dataService: DataService,
                private route: ActivatedRoute,
                private location: Location) {
                  this.newsItem = {
                        id: null,
                        title: null,
                        text: null,
                        author: null,
                        date: null,
                  }
                }

    goBack(): void {
        this.location.back()
    }

    ngOnInit() {
        const id = +this.route.snapshot.params['newsId']

        this.dataService.getNewsItem(id).subscribe((newsitem: INewsItem) => {
            this.newsItem = newsitem
        },
        error => {
            console.log('Failed while trying to load specific newsitem of news' + error)
        })
    }

    updateNewsItem() {
         this.dataService.updateNewsItem(this.newsItem)
         .subscribe(() => {
                this.newsItemEdited = true
                console.log('News was updated successfully. ')
                this.info = 'News ' + this.newsItem.title + ' was edited successfully!'
            },
            error => {
                console.log('Failed while trying to update the news. ' + error)
            })

     }
}
