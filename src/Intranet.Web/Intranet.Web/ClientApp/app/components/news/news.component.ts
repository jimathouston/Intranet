import { Component, OnInit } from '@angular/core'
import NewsItem from '../../models/newsItem.model'
import { DataService } from '../../shared/data_services/data.service'

@Component({
    selector: 'news',
    templateUrl: './news.component.html',
    styleUrls: ['./news.component.css']

})
export class NewsComponent implements OnInit {
    newsItems: NewsItem[]
    newsitemsFilter: NewsItem[]
    selectedNewsitem: NewsItem
    info: string

    constructor(
        private dataService: DataService,
    ) { }

    ngOnInit() {
        this.updateData()
    }

    onSelect(newsitem: NewsItem) {
        this.selectedNewsitem = newsitem
    }

    handleOnDelete(info: string) {
      this.updateData()
    }

    updateData() {
      this.dataService.getNewsItems().subscribe(
            (newsItems: NewsItem[]) => {
                this.newsItems = newsItems
            }
        )
    }

    generateUrl(newsItem: NewsItem) {
      return `/news/${newsItem.created.getUTCFullYear()}/${newsItem.created.getUTCMonth()}/${newsItem.created.getUTCDate()}/${newsItem.url}`
    }
}
