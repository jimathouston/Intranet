import { Component, OnInit } from '@angular/core'
import { RouterModule, Router, ActivatedRoute } from '@angular/router'
import { DataService } from '../../shared/data_services/data.service'
import { IProfile } from '../../shared/interfaces'

@Component({
    selector: 'profiles',
    templateUrl: './profiles.component.html',
    styleUrls: ['./profile.component.css']
})

export class ProfilesComponent implements OnInit {
    profiles: IProfile[]
    selectedProfile: IProfile

    constructor(private dataService: DataService,
                private route: ActivatedRoute) { }

    ngOnInit() {
      this.dataService.getAllProfiles().subscribe((profiles: IProfile[]) => {
        this.profiles = profiles
      })
    }

    removeProfile(profile: IProfile): void {
        this.selectedProfile = profile
        this.dataService.deleteProfile(this.selectedProfile.id)
            .subscribe(() => {
                console.log('Profile was deleted successfully!')
                this.dataService.getAllProfiles().subscribe((profiles: IProfile[]) => {
                    this.profiles = profiles
                },
                error => {
                   console.log('Failed to load profile ' + error)
                })
            })
    }
}
