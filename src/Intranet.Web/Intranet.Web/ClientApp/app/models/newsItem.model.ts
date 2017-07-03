import Image from './image.model'
import User from './user.model'

export default class NewsItem {
    id: number | null
    title: string
    text: string
    date: Date
    headerImage: Image
    user: User
}