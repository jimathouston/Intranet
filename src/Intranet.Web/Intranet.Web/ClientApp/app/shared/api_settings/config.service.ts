import { Injectable, Inject } from '@angular/core'

@Injectable()
export class ConfigService {

    _apiUrl: string
    _apiBaseUrl: string

    constructor(@Inject('ORIGIN_URL') private originUrl: string) {
        this._apiUrl = '/api/v1/'
        this._apiBaseUrl = originUrl
    }

    getApiUrl() {
        return this._apiBaseUrl + this._apiUrl
    }

    getApiBaseUrl() {
        return this._apiBaseUrl
    }
}


