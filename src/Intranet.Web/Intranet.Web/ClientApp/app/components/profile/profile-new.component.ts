import { Component } from '@angular/core'
import { RouterModule, Router, ActivatedRoute } from '@angular/router'
import { IProfile } from '../../shared/interfaces'
import { DataService } from '../../shared/data_services/data.service'
import { Location } from '@angular/common'

import Profile from '../../models/profile'

@Component({
    selector: 'profile-new',
    templateUrl: 'profile-new.component.html'
})

export class ProfileNewComponent {
    profile: IProfile
    info: string = ''
    profileCreated: boolean = false

    constructor(private dataService: DataService,
                private route: ActivatedRoute,
                private location: Location) {
                  this.profile = new Profile()
                }

    goBack(): void {
        this.location.back()
    }

    newProfile(firstName: string, lastName: string, description: string, email: string, phoneNumber: number, mobile: number, streetAdress: string, postalCode: number, city: string) {
        this.dataService.createProfile(firstName, lastName, description, email, phoneNumber, mobile, streetAdress, postalCode, city).then((profile) => {
            this.profileCreated = true
            console.log(profile)
             this.info = 'Profile was created successfully!'
          },
          (error) => console.log(error))
    }
}