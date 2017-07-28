import { HasId, HasUrl, HasKeywords } from '../contracts'
import { Image, User, NewsKeyword} from './'

export class News implements HasId, HasUrl, HasKeywords {
    id: number | null
    title: string
    text: string
    created: Date
    updated: Date
    headerImage: Image
    user: User
    keywords: string
    published: boolean
    hasEverBeenPublished: boolean
    newsKeywords: NewsKeyword[]
    url: string
}
