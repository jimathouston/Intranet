import { HasId, HasUrl } from '../contracts'
import { FaqKeyword, NewsKeyword } from './'

export class Keyword implements HasId, HasUrl {
  id: number | null
  name: string
  url: string
  faqKeywords: FaqKeyword[]
  newsKeywords: NewsKeyword[]
}
