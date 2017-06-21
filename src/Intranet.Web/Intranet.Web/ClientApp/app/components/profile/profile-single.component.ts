import { Component, OnInit } from '@angular/core'
import { RouterModule, Router, ActivatedRoute } from '@angular/router'
import { Location } from '@angular/common'
import { DataService } from '../../shared/data_services/data.service'

import Profile from '../../models/profile.model'

@Component({
    selector: 'profile-single',
    templateUrl: 'profile-single.component.html',
    styleUrls: ['./profile.component.css']
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

    profile: Profile
    profiles: Profile[]
    selectedProfile: Profile

    isIn = false

    constructor(private dataService: DataService,
                private route: ActivatedRoute,
                private location: Location) {
        this.profile = new Profile()
    }

    goBack(): void {
        this.location.back()
    }

    toggleState() {
        const bool = this.isIn
        this.isIn = bool === false ? true : false
    }

    ngOnInit() {
        this.id = 1 // +this.route.snapshot.params['id']
        this.dataService.getProfile(this.id).subscribe((profile: Profile) => {
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