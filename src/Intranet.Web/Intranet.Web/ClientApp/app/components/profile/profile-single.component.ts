import { Component, OnInit } from '@angular/core'
import { RouterModule, Router, ActivatedRoute } from '@angular/router'
import { Location } from '@angular/common'
import { DataService } from '../../shared/data_services/data.service'
import { IProfile } from '../../shared/interfaces'

import Profile from '../../models/profile'

@Component({
    selector: 'profile-single',
    templateUrl: 'profile-single.component.html'
})

export class ProfileSingleComponent implements OnInit {
    id: number
    firstName: string
    lastName: string
    description: string
    email: string
    phoneNumber: number
    mobile: number
    streetAdress: string
    postalCode: number
    city: string

    profile: IProfile
    profiles: IProfile[]
    selectedProfile: IProfile


    constructor(private dataService: DataService,
                private route: ActivatedRoute,
                private location: Location) {
        this.profile = new Profile()
    }

    goBack(): void {
        this.location.back()
    }

    ngOnInit() {
        this.id = 1 // +this.route.snapshot.params['id']
        this.dataService.getProfile(this.id).subscribe((profile: IProfile) => {
            this.firstName = profile.firstName
            this.lastName = profile.lastName
            this.description = profile.description
            this.email = profile.email
            this.phoneNumber = profile.phoneNumber
            this.mobile = profile.mobile
            this.streetAdress = profile.streetAdress
            this.postalCode = profile.postalCode
            this.city = profile.city
            this.profile = profile
        },
        error => {
            console.log('Failed while trying to load specific profile of profiles' + error)
         })
    }
}