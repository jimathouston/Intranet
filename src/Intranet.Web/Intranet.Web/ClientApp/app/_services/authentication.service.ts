import { Injectable, Inject } from '@angular/core'
import { Http, Headers, Response, RequestOptions } from '@angular/http'
import * as jwtDecode from 'jwt-decode'
import { Jwt } from '../models'

@Injectable()
export class AuthenticationService {
    private jwtKey = 'jwt'

    private get hasLocalStorage() { return typeof localStorage !== 'undefined' }
    private get newTokenUrl() { return this.originUrl + '/GenerateToken' }

    constructor(
        private http: Http,
        @Inject('ORIGIN_URL') private originUrl: string,
        @Inject('COOKIES') private cookies
    ) { }

    async getJwt(): Promise<string> {
        const jwtFromLocalStorage = this.getJwtFromLocalStorage()
        const decodedCachedJwt = jwtFromLocalStorage ? jwtDecode(jwtFromLocalStorage) : null

        // 'exp' should be in sec but 'Date.now()' is in ms, so divide with 1000.
        // Subtract 30 sec to avoid that the JWT expires when other code is still using it.
        if (decodedCachedJwt !== null && decodedCachedJwt['exp'] - 30 > Date.now() / 1000) {
            return jwtFromLocalStorage
        }

        const newJwt = await this.getNewJwt()

        this.setJwtInLocalStorage(newJwt)
        return newJwt
    }

    async getJwtDecoded(): Promise<Jwt> {
        const jwt = await this.getJwt()
        return jwtDecode(jwt) as Jwt
    }

    async isAdmin() {
      const jwt = await this.getJwtDecoded()
      return jwt.role === 'Admin'
    }

    async isUser(username: string) {
      const jwt = await this.getJwtDecoded()
      return jwt.username.toLowerCase() === username.toLowerCase()
    }

    private getJwtFromLocalStorage() {
        if (!this.hasLocalStorage) {
            return null
        }

        return localStorage.getItem(this.jwtKey)
    }

    private setJwtInLocalStorage(jwt: string) {
        if (this.hasLocalStorage) {
            localStorage.setItem(this.jwtKey, jwt)
        }
    }

    private async getNewJwt(): Promise<string> {
        if (this.hasLocalStorage) {
            const response = await this.http.get(this.newTokenUrl).toPromise()
            return response.json().accessToken
        } else {
            // If localStorage is not avalible that means we are running this on
            // the server and need to attach the cookies on our own
            const headers = new Headers(
                {
                    'Content-Type': 'application/json',
                    'Cookie': this.cookies
                })

            const options = new RequestOptions({ headers: headers, withCredentials: true })
            const response = await this.http.get(this.newTokenUrl, options).toPromise()
            return response.json().accessToken
        }
    }
}


