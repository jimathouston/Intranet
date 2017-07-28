import { HasId, HasUrl, HasKeywords } from '../contracts'
import { Category, FaqKeyword } from './'

export class Faq implements HasId, HasUrl, HasKeywords {
  id: number | null
  answer: string
  category: Category
  question: string
  faqKeywords: FaqKeyword[]
  keywords: string
  url: string
}
