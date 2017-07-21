import { Component, OnInit } from '@angular/core'
import { RouterModule, Router, ActivatedRoute } from '@angular/router'
import { DataService } from '../../_services'

import { Profile } from '../../models'

@Component({
    selector: 'profiles',
    templateUrl: './profiles.component.html',
    styleUrls: ['./profile.component.css']
})

export class ProfilesComponent implements OnInit {
    profiles: Profile[]
    selectedProfile: Profile

    constructor(private dataService: DataService,
                private route: ActivatedRoute) { }

    ngOnInit() {
      this.dataService.getAllProfiles().subscribe((profiles: Profile[]) => {
        this.profiles = profiles
      })
    }

    removeProfile(profile: Profile): void {
        this.selectedProfile = profile
        this.dataService.deleteProfile(this.selectedProfile.id)
            .subscribe(() => {
                this.dataService.getAllProfiles().subscribe((profiles: Profile[]) => {
                    this.profiles = profiles
                },
                error => {
                   console.log('Failed to load profile ' + error)
                })
            })
    }
}
