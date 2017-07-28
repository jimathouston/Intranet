import { Injectable } from '@angular/core'
import { Response, Headers, RequestOptions } from '@angular/http'

// Grab everything with import 'rxjs/Rx'
import { Observable } from 'rxjs/Observable'
import { Observer } from 'rxjs/Observer'
import 'rxjs/add/observable/throw'
import 'rxjs/add/operator/map'
import 'rxjs/add/operator/catch'
import 'rxjs/add/operator/toPromise'

import {
  AuthenticationService,
  ConfigService,
  SecureHttpService
} from './'

import { HasId } from '../contracts'

export abstract class RestService<T extends HasId> {
  constructor(
    protected http: SecureHttpService,
    protected url: string
  ) {}

  getItems(): Observable<T[]> {
    return this.http.get(this.url)
      .map((res: Response) => {
        return res.json()
      })
      .catch(this.handleError)
  }

  // get a specific item by Id
  getItem(id: number): Observable<T> {
    return this.http.get(`${this.url}/${id}`)
      .map((res: Response) => {
        return res.json()
      })
      .catch(this.handleError)
  }

  postItem(item: T): Observable<T> {
    const body = JSON.stringify(item)

    return this.http.post(this.url, body)
      .map((res: Response) => {
        return res.json() as T
      })
      .catch(this.handleError)
  }

  putItem(item: T): Observable<T> {
    const headers = new Headers()
    headers.append('Content-Type', 'application/json')
    headers.append('Access-Control-Allow-Origin', '*')

    const options = new RequestOptions({
      headers: headers
    })

    return this.http.put(`${this.url}/${item.id}`, JSON.stringify(item), options)
      .map((res: Response) => {
        return res.json() as T
      })
      .catch(this.handleError)
  }

  deleteItem(id: number): Observable<boolean> {
    return this.http.delete(`${this.url}/${id}`)
      .map((res: Response) => {
        return true
      })
      .catch(this.handleError)
  }

  async uploadFile(file): Promise<string> {
    if (typeof file !== 'undefined') {
      const headers = new Headers(
      {
          'Accept': 'application/json'
      })

      const formData: FormData = new FormData()

      formData.append(file.name, file)
      // headers.append('Content-Type', 'multipart/form-data')
      // DON'T SET THE Content-Type to multipart/form-data
      // You'll get the Missing content-type boundary error
      const options = new RequestOptions({ headers: headers })

      const response = await this.http.post('upload', formData, options, true).toPromise()

      return response.json().fileName
    }

    return null
  }

  protected handleError(error: Response | any) {
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
