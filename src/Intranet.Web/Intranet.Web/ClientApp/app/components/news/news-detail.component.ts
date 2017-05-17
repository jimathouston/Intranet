import { Component, OnInit } from '@angular/core'
import { RouterModule, Router, ActivatedRoute } from '@angular/router'
import { Location } from '@angular/common'
import { DataService } from '../../shared/data_services/data.service'
import {INewsItem} from '../../shared/interfaces'
import { MaterialModule } from '@angular/material'


@Component({
    selector: 'news-detail',
    templateUrl: 'news-detail.component.html',
    styleUrls: ['./news.component.css']
})

export class NewsDetailComponent implements OnInit {
    id: number

    title: string
    date: Date
    text: string
    author: string

    newsitem: INewsItem


    constructor(private dataService: DataService,
                private route: ActivatedRoute,
                private location: Location) { }


    goBack(): void {
        this.location.back()
    }

    ngOnInit() {
        this.id = +this.route.snapshot.params['newsId']

        this.dataService.getNewsItem(this.id).subscribe((newsitem: INewsItem) => {
            this.title = newsitem.title
            this.date = newsitem.date
            this.text = newsitem.text
            this.author = newsitem.author
        },
        error => {
            console.log('Failed while trying to load specific newsitem of news' + error)
         })
    }
}