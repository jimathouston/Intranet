import { Injectable } from '@angular/core'
import { Response, Headers, RequestOptions } from '@angular/http'

// Grab everything with import 'rxjs/Rx'
import { Observable } from 'rxjs/Observable'
import { Observer } from 'rxjs/Observer'
import 'rxjs/add/observable/throw'
import 'rxjs/add/operator/map'
import 'rxjs/add/operator/catch'
import 'rxjs/add/operator/toPromise'

import { SecureHttpService } from './'
import { RestService } from './rest.service'

import { News } from '../models'

@Injectable()
export class NewsService extends RestService<News> {
  constructor(
    http: SecureHttpService,
  ) {
    super(http, 'news')
  }

  getItems(): Observable<News[]> {
    return this.http.get('news')
      .map((res: Response) => {
        return res.json()
      })
      .map((newsItems: News[]) => {
        newsItems.forEach(news => {
          news.created = new Date(news.created)
          news.updated = new Date(news.updated)
        })
        return newsItems.sort((a, b) => a.created < b.created ? 1 : -1)
      })
      .catch(this.handleError)
    }

  getNewsByDateAndUrl(date: Date, url: string): Observable<News> {
    return this.http.get(`news/${date.getUTCFullYear()}/${date.getUTCMonth() + 1}/${date.getUTCDate()}/${url}`)
      .map((res: Response) => {
        return res.json()
      })
      .catch(this.handleError)
  }
}
