import { Component, OnInit } from '@angular/core'
import { INewsItem } from '../../shared/interfaces'
import { DataService } from '../../shared/data_services/data.service'
import { MaterialModule } from '@angular/material'

@Component({
    selector: 'news',
    templateUrl: './news.component.html',
    styleUrls: ['./news.component.css']

})
export class NewsComponent implements OnInit {
    newsitems: INewsItem[]
    newsitemsFilter: INewsItem[]
    selectedNewsitem: INewsItem

    constructor(private dataService: DataService) { }

    ngOnInit() {
    this.dataService.getNewsItems().subscribe((newsitems: INewsItem[]) => {
        this.newsitems = newsitems
    })
}

    onSelect(newsitem: INewsItem): void {
        this.selectedNewsitem = newsitem
    }

    /*filterNewsItems(filter:string){
        console.log(filter)
        if (filter.length === 0) {
            this.newsitems = this.newsitemsFilter
        } else {
            this.newsitems = this.newsitemsFilter.filter(newsitem => newsitem.title.toLowerCase().indexOf(filter.toLowerCase()) > -1 )
        }
    }*/

    removeNewsItem(newsitem: INewsItem): void {
        this.selectedNewsitem = newsitem
        this.dataService.deleteNewsItem(this.selectedNewsitem.id)
            .subscribe(() => {
                console.log('News was deleted successfully!')
                this.dataService.getNewsItems().subscribe((newsitems:INewsItem[]) => {
                    this.newsitems = newsitems
                },
                error => {
                   console.log('Failed to load news '+error)
                })
            })     
    }
}
