import { Image, User} from './'

export class News {
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
    url: string
}
