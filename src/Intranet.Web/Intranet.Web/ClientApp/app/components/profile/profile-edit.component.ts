import { Component, OnInit, Input } from '@angular/core'
import { RouterModule, Router, ActivatedRoute, Params } from '@angular/router'
import { Location } from '@angular/common'
import { DataService } from '../../shared/data_services/data.service'
import { IProfile } from '../../shared/interfaces'

import Profile from '../../models/profile'

@Component({
    selector: 'profile-edit',
    templateUrl: 'profile-edit.component.html'
})

export class ProfileEditComponent implements OnInit {
    profile: IProfile
    info: string = ''
    profileEdited: boolean = false

    constructor(private dataService: DataService,
                private route: ActivatedRoute,
                private location: Location) {
        this.profile = new Profile()
    }

    goBack(): void {
        this.location.back()
    }

    ngOnInit() {
        const id = +this.route.snapshot.params['id']

        this.dataService.getProfile(id).subscribe((profile: IProfile) => {
            this.profile = profile
        },
        error => {
            console.log('Failed while trying to load specific profile of profiles' + error)
        })
    }

    updateProfile() {
         this.dataService.updateProfile(this.profile)
         .subscribe(() => {
                this.profileEdited = true
                this.info = this.profile.firstName + this.profile.lastName + ' was edited successfully!'
                console.log('Profiles was updated successfully. ')
            },
            error => {
                console.log('Failed while trying to update the profiles. ' + error)
            })

     }
}