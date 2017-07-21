import { Component, OnInit } from '@angular/core'
import { RouterModule, Router, ActivatedRoute } from '@angular/router'
import { DataService } from '../../_services'
import { Location } from '@angular/common'

import { Checklist, Profile } from '../../models'

@Component({
  selector: 'profile-checklist',
  templateUrl: 'profile-checklist.component.html',
  styleUrls: ['./profile.component.css']
})

export class ProfileChecklistComponent implements OnInit {
  todos: Checklist[]
  checklistEdited: boolean = false

  id: number

  constructor(private dataService: DataService,
              private route: ActivatedRoute,
              private location: Location) { }

  goBack(): void {
      this.location.back()
  }

  ngOnInit() {
    this.id =  1 // + this.route.snapshot.params['id']
    this.dataService.getProfileChecklist(this.id).subscribe((todos: Checklist[]) => {

    this.todos = todos
        error => {
      console.log('Faild to load checklist ' + error)
        }
    })
  }

  updateProfileChecklist() {
    this.todos.forEach(todo =>
      this.dataService.updateProfileChecklist(this.id, todo)
      .subscribe(() => {
          this.checklistEdited = true
      })
    )
  }
}
