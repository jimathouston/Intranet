import { Component, OnInit } from '@angular/core'
import { News } from '../../models'
import { NewsService } from '../../_services'

@Component({
    selector: 'news',
    templateUrl: './news.component.html',
    styleUrls: ['./news.component.css']
})
export class NewsComponent implements OnInit {
    newsItems: News[]
    newsitemsFilter: News[]
    selectedNewsitem: News
    info: string

    constructor(
        private newsService: NewsService,
    ) { }

    ngOnInit() {
        this.updateData()
    }

    onSelect(newsitem: News) {
        this.selectedNewsitem = newsitem
    }

    handleOnDelete(info: string) {
      this.updateData()
    }

    updateData() {
      this.newsService.getItems().subscribe(
            (newsItems: News[]) => {
                this.newsItems = newsItems
            }
        )
    }

    generateUrl(newsItem: News) {
      return `/news/${newsItem.created.getUTCFullYear()}/${newsItem.created.getUTCMonth()}/${newsItem.created.getUTCDate()}/${newsItem.url}`
    }
}
