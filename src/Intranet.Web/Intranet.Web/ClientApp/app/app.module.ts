import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { HttpModule } from '@angular/http'
import { FormsModule } from '@angular/forms'

// Imports for loading & configuring the in-memory web api

import { AppComponent } from './components/app/app.component'
import { NavMenuComponent } from './components/navmenu/navmenu.component'
import { HomeComponent } from './components/home/home.component'
import { FetchDataComponent } from './components/fetchdata/fetchdata.component'
import { CounterComponent } from './components/counter/counter.component'

import { NewsComponent } from './components/news/news.component'
import { NewsNewComponent } from './components/news/news-new.component'
import { NewsDetailComponent } from './components/news/news-detail.component'
import { NewsEditComponent } from './components/news/news-edit.component'

import { ProfilesComponent } from './components/profile/profiles.component'
import { ProfileDetailComponent } from './components/profile/profile-detail.component'
import { ProfileChecklistComponent } from './components/profile/profile-checklist.component'
import { ProfileNewComponent } from './components/profile/profile-new.component'
import { ProfileEditComponent } from './components/profile/profile-edit.component'
import { ProfileSingleComponent } from './components/profile/profile-single.component'

// Services
import { DataService } from './shared/data_services/data.service'
import { ConfigService } from './shared/api_settings/config.service'
import { TokenService } from './shared/data_services/jwt-token.service'


export const sharedConfig: NgModule = {
    bootstrap: [ AppComponent ],
    declarations: [
        AppComponent,
        NavMenuComponent,
        CounterComponent,
        FetchDataComponent,
        HomeComponent,
        NewsComponent, NewsNewComponent, NewsDetailComponent, NewsEditComponent,
        ProfilesComponent, ProfileDetailComponent, ProfileNewComponent, ProfileEditComponent,
        ProfileChecklistComponent, ProfileSingleComponent
    ],
    providers: [
        DataService,
        ConfigService,
        TokenService,
    ],
    imports: [
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'counter', component: CounterComponent },
            { path: 'fetch-data', component: FetchDataComponent },
            { path: 'news', component: NewsComponent },
            { path: 'news-new', component: NewsNewComponent },
            { path: 'news-detail/:id', component: NewsDetailComponent },
            { path: 'news-edit/:id', component: NewsEditComponent },
            { path: 'checklist', component: ProfileChecklistComponent },
            { path: 'profiles', component: ProfilesComponent },
            { path: 'profile-single', component: ProfileSingleComponent },
            { path: 'profile-new', component: ProfileNewComponent },
            { path: 'profile-detail/:id', component: ProfileDetailComponent },
            { path: 'profile-edit/:id', component: ProfileEditComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ]
}
