import Image from './image.model'

export default class NewsItem {
    id: number | null
    title: string
    text: string
    author: string
    date: Date
    headerImage: Image
}