import { Component, OnInit } from '@angular/core'
import NewsItem from '../../models/newsItem.model'
import { DataService } from '../../shared/data_services/data.service'
import { ConfigService } from '../../shared/api_settings/config.service'

@Component({
    selector: 'news',
    templateUrl: './news.component.html',
    styleUrls: ['./news.component.css']

})
export class NewsComponent implements OnInit {
    newsitems: NewsItem[]
    newsitemsFilter: NewsItem[]
    selectedNewsitem: NewsItem
    apiBaseUrl: string

    constructor(
        private dataService: DataService,
        private configService: ConfigService
    ) {
        this.apiBaseUrl = this.configService.getApiBaseUrl()
    }

    ngOnInit() {
        this.dataService.getNewsItems().subscribe((newsitems: NewsItem[]) => {
            this.newsitems = newsitems
        })
    }

    onSelect(newsitem: NewsItem) {
        this.selectedNewsitem = newsitem
    }

    getUrlToImage(urls: string[]) {
        const url = urls.find(value => {
            return value.includes('600/300')
        })

        return this.apiBaseUrl + url
    }
}
