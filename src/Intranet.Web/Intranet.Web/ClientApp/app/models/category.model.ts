import { HasId, HasUrl } from '../contracts'
import { Faq } from './'

export class Category implements HasId, HasUrl {
  id: number | null
  title: string
  url: string
  faqs: Faq[]
}
