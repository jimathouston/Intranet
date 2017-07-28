// import { Component, OnInit, Input } from '@angular/core'
// import { RouterModule, Router, ActivatedRoute, Params } from '@angular/router'
// import { Location } from '@angular/common'
// import { DataService } from '../../_services'

// import { Profile } from '../../models'

// @Component({
//     selector: 'profile-edit',
//     templateUrl: 'profile-edit.component.html',
//     styleUrls: ['./profile.component.css']
// })

// export class ProfileEditComponent implements OnInit {
//     profile: Profile
//     info: string = ''
//     profileEdited: boolean = false

//     constructor(private dataService: DataService,
//                 private route: ActivatedRoute,
//                 private location: Location) {
//         this.profile = new Profile()
//     }

//     goBack(): void {
//         this.location.back()
//     }

//     ngOnInit() {
//         const id = +this.route.snapshot.params['id']

//         this.dataService.getProfile(id).subscribe((profile: Profile) => {
//             this.profile = profile
//         },
//         error => {
//             console.log('Failed while trying to load specific profile of profiles' + error)
//         })
//     }

//     updateProfile() {
//          this.dataService.updateProfile(this.profile)
//          .subscribe(() => {
//                 this.profileEdited = true
//                 this.info = this.profile.firstName + this.profile.lastName + ' was edited successfully!'
//             },
//             error => {
//                 console.log('Failed while trying to update the profiles. ' + error)
//             })

//      }
// }
