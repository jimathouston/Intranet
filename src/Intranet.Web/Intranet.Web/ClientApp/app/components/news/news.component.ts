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

    constructor(
        private dataService: DataService,
    ) { }

    ngOnInit() {
        this.dataService.getNewsItems().subscribe(
            (newsItems: NewsItem[]) => {
                this.newsItems = newsItems
            }
        )
    }

    onSelect(newsitem: NewsItem) {
        this.selectedNewsitem = newsitem
    }
}
