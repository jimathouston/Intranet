import { Injectable } from '@angular/core'

@Injectable()
export class ConfigService {

    _apiUrl: string
    _apiBaseUrl: string

    constructor() {
        this._apiUrl = '/api/v1/'
        this._apiBaseUrl = 'http://localhost:50590'
    }

    getApiUrl() {
        return this._apiBaseUrl + this._apiUrl
    }

    getApiBaseUrl() {
        return this._apiBaseUrl
    }
}


