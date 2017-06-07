import { Component, OnInit } from '@angular/core'
import { RouterModule, Router, ActivatedRoute } from '@angular/router'
import { DataService } from '../../shared/data_services/data.service'
import { IChecklist, IProfile } from '../../shared/interfaces'
import { Location } from '@angular/common'

@Component({
  selector: 'profile-checklist',
  templateUrl: 'profile-checklist.component.html'
})

export class ProfileChecklistComponent implements OnInit {
  todos: IChecklist[]
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
    this.dataService.getProfileChecklist(this.id).subscribe((todos: IChecklist[]) => {

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