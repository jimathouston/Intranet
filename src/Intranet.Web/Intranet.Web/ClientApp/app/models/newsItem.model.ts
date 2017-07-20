import Image from './image.model'
import User from './user.model'

export default class NewsItem {
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
}