import { Injectable } from '@angular/core'
import { ConnectionBackend, RequestOptions, Request, RequestOptionsArgs, Response, Http, Headers } from '@angular/http'
import { Observable } from 'rxjs/Rx'
import 'rxjs/add/observable/fromPromise'
import 'rxjs/add/operator/map'
import 'rxjs/add/operator/mergeMap'

import { AuthenticationService } from './'
import { ConfigService } from '../shared/api_settings/config.service'

@Injectable()
export class SecureHttpService extends Http {
    constructor(
        backend: ConnectionBackend,
        defaultOptions: RequestOptions,
        private configService: ConfigService,
        private authenticationService: AuthenticationService,
    ) {
        super(backend, defaultOptions)
    }

    request(url: string | Request, options?: RequestOptionsArgs): Observable<Response> {
        return super.request(url, options)
    }

    get(url: string, options?: RequestOptionsArgs, baseCall = false): Observable<Response> {
        return Observable.fromPromise(this.getRequestOptionArgs(options)).mergeMap(requestOption => {
            url = this.updateUrl(url, baseCall)
            return super.get(url, requestOption)
        })
    }

    post(url: string, body: string | FormData, options?: RequestOptionsArgs, baseCall = false): Observable<Response> {
        return Observable.fromPromise(this.getRequestOptionArgs(options, typeof body === 'object')).mergeMap(requestOption => {
            url = this.updateUrl(url, baseCall)
            return super.post(url, body, requestOption)
        })
    }

    put(url: string, body: string | FormData, options?: RequestOptionsArgs, baseCall = false): Observable<Response> {
        return Observable.fromPromise(this.getRequestOptionArgs(options, typeof body === 'object')).mergeMap(requestOption => {
            url = this.updateUrl(url, baseCall)
            return super.put(url, body, requestOption)
        })
    }

    delete(url: string, options?: RequestOptionsArgs, baseCall = false): Observable<Response> {
        return Observable.fromPromise(this.getRequestOptionArgs(options)).mergeMap(requestOption => {
            url = this.updateUrl(url, baseCall)
            return super.delete(url, requestOption)
        })
    }

    private updateUrl(req: string, baseCall: boolean) {
        return baseCall ? req : this.configService.getApiUrl() + req
    }

    private async getRequestOptionArgs(options?: RequestOptionsArgs, dontSetContentType = false): Promise<RequestOptionsArgs> {
        const jwt = await this.authenticationService.getJwt()

        if (options == null) {
            options = new RequestOptions()
        }

        if (options.headers == null) {
            options.headers = new Headers()
        }

        if (!dontSetContentType) {
            options.headers.append('Content-Type', 'application/json')
        }

        options.headers.append('Authorization', `Bearer ${jwt}`)

        return options
    }
}