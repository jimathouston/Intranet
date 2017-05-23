import { Injectable } from '@angular/core'

@Injectable()
export class ConfigService {

    _apiURI: string
    _mockURI: string

    constructor() {
        this._apiURI = 'http://localhost:3000/api/v1/'
        this._mockURI = 'http://localhost:3001/'
    }

    getApiURI() {
        return this._apiURI
    }

    getMockURI() {
        return this._mockURI
    }

    getApiHost() {
        return this._apiURI.replace('api', '')
    }
}


