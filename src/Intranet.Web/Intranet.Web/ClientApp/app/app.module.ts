import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { HttpModule, Http, XHRBackend, RequestOptions } from '@angular/http'
import { FormsModule } from '@angular/forms'
import { APP_BASE_HREF } from '@angular/common'

import { TruncatePipe }   from './shared/pipes/truncate'
import { SafeHtmlPipe } from './shared/pipes/safehtml'

// Imports for loading & configuring the in-memory web api

import { AppComponent } from './components/app/app.component'
import { NavMenuComponent } from './components/navmenu/navmenu.component'

import { NewsComponent } from './components/news/news.component'
import { NewsNewComponent } from './components/news/news-new.component'
import { NewsDetailComponent } from './components/news/news-detail.component'
import { NewsEditComponent } from './components/news/news-edit.component'

import {
  DeleteNewsButtonComponent,
  NewsInfoStripComponent,
  TogglePublishedComponent,
  NewsKeywordsStripComponent,
} from './_directives'

import { ProfilesComponent } from './components/profile/profiles.component'
import { ProfileDetailComponent } from './components/profile/profile-detail.component'
import { ProfileChecklistComponent } from './components/profile/profile-checklist.component'
import { ProfileNewComponent } from './components/profile/profile-new.component'
import { ProfileEditComponent } from './components/profile/profile-edit.component'
import { ProfileSingleComponent } from './components/profile/profile-single.component'

import { TextEditorComponent } from './components/texteditor/texteditor.component'

// Services
import { DataService } from './shared/data_services/data.service'
import { ConfigService } from './shared/api_settings/config.service'
import { AuthenticationService, SecureHttpService } from './_services'
import { SecureHttpFactory } from './_factories'

export const sharedConfig: NgModule = {
    bootstrap: [ AppComponent ],
    declarations: [
        AppComponent,
        NavMenuComponent,
        NewsComponent, NewsNewComponent, NewsDetailComponent, NewsEditComponent,
        ProfilesComponent, ProfileDetailComponent, ProfileNewComponent, ProfileEditComponent,
        ProfileChecklistComponent, ProfileSingleComponent,
        TruncatePipe,
        TextEditorComponent,
        SafeHtmlPipe,
        DeleteNewsButtonComponent,
        NewsInfoStripComponent,
        TogglePublishedComponent,
        NewsKeywordsStripComponent,
    ],
    providers: [
        DataService,
        ConfigService,
        AuthenticationService,
        { provide: APP_BASE_HREF, useValue: '/' },
        { provide: SecureHttpService, useFactory: SecureHttpFactory, deps: [XHRBackend, RequestOptions, ConfigService, AuthenticationService] },
    ],
    imports: [
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'news', pathMatch: 'full' },
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
