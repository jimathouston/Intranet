import { Injectable } from '@angular/core'
import { Http, Response, Headers } from '@angular/http'

// Grab everything with import 'rxjs/Rx'
import { Observable } from 'rxjs/Observable'
import { Observer } from 'rxjs/Observer'
import 'rxjs/add/observable/throw'
import 'rxjs/add/operator/map'
import 'rxjs/add/operator/catch'
import 'rxjs/add/operator/toPromise'

import { INewsItem, IChecklist, IProfile } from '../interfaces'
import { ConfigService } from '../api_settings/config.service'
import { TokenService } from '../data_services/jwt-token.service'

@Injectable()
export class DataService {

    private headers = new Headers({'Content-Type': 'application/json'})

    _baseUrl: string = ''

    constructor(private http: Http,
                private configService: ConfigService) {
                this._baseUrl = configService.getApiURI()
    }

// NEWS SERVICES ************************************************************  /

    // get all News
    getNewsItems(): Observable<INewsItem[]> {
        return this.http.get(this._baseUrl + 'news')
            .map((res: Response) => {
                return res.json()
            })
            .catch(this.handleError)
    }

    // get a specific news by Id
    getNewsItem(id: number): Observable<INewsItem> {
        return this.http.get(this._baseUrl + 'news/' + id)
            .map((res: Response) => {
                return res.json()
            })
            .catch(this.handleError)
    }

    // create a new NewsItem
    createNewsItem(title: string, text: string, author: string): Promise< INewsItem> {
      const body = JSON.stringify({title: title, text: text, author: author})

        return this.http.post(this._baseUrl + 'news/', body, { headers: this.headers })
                .toPromise()
                .then(res => res.json().data as INewsItem)
                .catch(this.handleError)
    }

    // updates a NewsItem
    updateNewsItem(newsitem: INewsItem): Observable<void> {
        const headers = new Headers()
        headers.append('Content-Type', 'application/json')
        headers.append('Access-Control-Allow-Origin', '*')

        return this.http.put(this._baseUrl + 'news/' + newsitem.id, JSON.stringify(newsitem), {
            headers: headers
        })
            .map((res: Response) => {
                return
            })
            .catch(this.handleError)
    }


    // delete newsitem
    deleteNewsItem(id: number): Observable<void> {
        return this.http.delete(this._baseUrl + 'news/' + id)
            .map((res: Response) => {
                return
            })
            .catch(this.handleError)
    }

// NEW AT CERTAINCY SERVICE ************************************************  /

     // get all Profiles
    getAllProfiles(): Observable<IProfile[]> {
        return this.http.get(this._baseUrl + 'profile')
            .map((res: Response) => {
                return res.json()
            })
            .catch(this.handleError)
    }

    // get a specific profile by Id
    getProfile(id: number): Observable<IProfile> {
        return this.http.get(this._baseUrl + 'profile/' + id)
            .map((res: Response) => {
                return res.json()
        })
        .catch(this.handleError)
    }

    // get profiles checklist
    getProfileChecklist(id: number): Observable<IChecklist[]> {
        return this.http.get(this._baseUrl + 'profile/' + id + '/checklist')
            .map((res: Response) => {
                console.log(res)
                return res.json()
            })
            .catch(this.handleError)
    }

     // create new Profile
    createProfile(firstName: string, lastName: string, description: string, email: string, phoneNumber: number, mobile: number, streetAdress: string, postalCode: number, city: string): Promise< IProfile> {
      const body = JSON.stringify({firstName: firstName, lastName: lastName, description: description, email: email, phoneNumber: phoneNumber, mobile: mobile, streetAdress: streetAdress, postalCode: postalCode, city: city})
        return this.http.post(this._baseUrl + 'profile/', body, { headers: this.headers })
                .toPromise()
                .then(res => res.json().data as IProfile)
                .catch(this.handleError)
    }

    // update a Profile
    updateProfile(profile: IProfile): Observable<void> {
        const headers = new Headers()
        headers.append('Content-Type', 'application/json')
        headers.append('Access-Control-Allow-Origin', '*')
        return this.http.put(this._baseUrl + 'profile/' + profile.id, JSON.stringify(profile), {
            headers: headers
        })
            .map((res: Response) => {
                return
            })
            .catch(this.handleError)
    }

    // delete Profile
    deleteProfile(id: number): Observable<void> {
        return this.http.delete(this._baseUrl + 'profile/' + id)
            .map((res: Response) => {
                return
            })
            .catch(this.handleError)
    }


    private handleError (error: Response | any) {
        const applicationError = error.headers.get('constlication-Error')
        const serverError = error.json()
        let modelStateErrors: string = ''

        if (!serverError.type) {
            console.log(serverError)
            for (const key in serverError) {
                if (serverError[key])
                    modelStateErrors += serverError[key] + '\n'
            }
        }

        modelStateErrors = modelStateErrors = '' ? null : modelStateErrors

        return Observable.throw(applicationError || modelStateErrors || 'Server error')
    }
}
