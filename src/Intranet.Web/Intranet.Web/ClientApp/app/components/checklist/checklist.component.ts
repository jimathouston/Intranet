import { Component, OnInit } from '@angular/core'
import { RouterModule, Router, ActivatedRoute } from '@angular/router'
import { DataService } from '../../shared/data_services/data.service'
import {IChecklist} from '../../shared/interfaces'
import { Location } from '@angular/common'

@Component({
    selector: 'checklist',
    templateUrl: 'checklist.component.html'
})

export class ChecklistComponent implements OnInit {
    todos: IChecklist[]

    constructor(private dataService: DataService) { }

    ngOnInit() {
        this.dataService.getChecklist().subscribe((todos: IChecklist[]) => {
        this.todos = todos
            console.log('Checklist loaded')
            error => {
            console.log('Faild to load checklist ' + error)
            }
        })
    }
}