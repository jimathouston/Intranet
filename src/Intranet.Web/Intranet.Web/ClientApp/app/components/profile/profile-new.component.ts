// import { Component } from '@angular/core'
// import { RouterModule, Router, ActivatedRoute } from '@angular/router'
// import { DataService } from '../../_services'
// import { Location } from '@angular/common'

// import { Profile } from '../../models'

// @Component({
//     selector: 'profile-new',
//     templateUrl: 'profile-new.component.html',
//     styleUrls: ['./profile.component.css']
// })

// export class ProfileNewComponent {
//     profile: Profile
//     info: string = ''
//     profileCreated: boolean = false

//     constructor(private dataService: DataService,
//                 private route: ActivatedRoute,
//                 private location: Location) {
//                   this.profile = new Profile()
//                 }

//     goBack(): void {
//         this.location.back()
//     }

//     newProfile(firstName: string, lastName: string, description: string, email: string, phoneNumber: number, mobile: number, streetAdress: string, postalCode: number, city: string) {
//         //  this.dataService.createProfile(firstName, lastName, description, email, phoneNumber, mobile, streetAdress, postalCode, city).then((profile) => {
//         //    this.profileCreated = true
//         //     this.info = 'Profile was created successfully!'
//         //  },
//         //  (error) => console.log(error))
//     }
// }
