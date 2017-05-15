import { Component } from '@angular/core'
import { RouterModule, Router, ActivatedRoute } from '@angular/router'
import { INewsItem } from '../../shared/interfaces'
import { DataService } from '../../shared/data_services/data.service'
import { Location } from '@angular/common'
import { MaterialModule } from '@angular/material'


@Component({
    selector: 'news-new',
    templateUrl: 'news-new.component.html',
    styleUrls: ['./news.component.css']

})

export class NewsNewComponent {
    newsitems: INewsItem[]

    constructor(private dataService: DataService,
                private route: ActivatedRoute,
                private location: Location) { }

    goBack(): void {
        this.location.back()
    }

    addNewsItem(title: string, text: string, author: string) {
        this.dataService.createNewsItem(title, text, author).then(
            (news) => (news)
        )
    }
}