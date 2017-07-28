import { Injectable } from '@angular/core'
import { Response, Headers, RequestOptions } from '@angular/http'
import * as _ from 'lodash'

// Grab everything with import 'rxjs/Rx'
import { Observable } from 'rxjs/Observable'
import { Observer } from 'rxjs/Observer'
import 'rxjs/add/observable/throw'
import 'rxjs/add/operator/map'
import 'rxjs/add/operator/catch'
import 'rxjs/add/operator/toPromise'

import { SecureHttpService } from './'
import { RestService } from './rest.service'

import { Faq, FaqByCategory, Category } from '../models'

@Injectable()
export class FaqService extends RestService<Faq> {
  constructor(
    http: SecureHttpService,
  ) {
    super(http, 'faq')
  }

  getFaqsByCategory(): Observable<FaqByCategory[]> {
    return this.http.get(this.url)
      .map((res: Response) => {
        return res.json()
      })
      .map((faqs: Faq[]) => {
        return (_
          .chain(faqs)
          .flatMap(faq => faq.category) as any) // Not pretty but it works: https://github.com/DefinitelyTyped/DefinitelyTyped/issues/6586#issuecomment-292532489
          .uniqWith((arrVal: Category, othVal: Category) => {
            return arrVal.id === othVal.id
          })
          .map((category: Category) => {
            const faqByCategory = new FaqByCategory()
            faqByCategory.category = category
            faqByCategory.faqs = _.filter(faqs, faq => faq.category.id === category.id)
            return faqByCategory
          })
          .sortBy(fc => fc.category.title)
          .value()
      })
      .catch(this.handleError)
  }
}
