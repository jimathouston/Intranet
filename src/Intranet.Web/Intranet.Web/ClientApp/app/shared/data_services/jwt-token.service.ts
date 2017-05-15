import { Injectable } from '@angular/core'
import { Http, Headers, Response } from '@angular/http'
import { Observable } from 'rxjs'
import 'rxjs/add/operator/map'

@Injectable()
export class TokenService {
    public token: string

    constructor(private http: Http) {
		// set token if saved in local storage
        const tokenFromStorage = JSON.parse(localStorage.getItem('Token'))
        if (tokenFromStorage  && tokenFromStorage.Exp < Date.now)
        return tokenFromStorage
    }
}
