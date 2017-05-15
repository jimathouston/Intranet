export interface INewsItem {
    id?: number
    title: string
    text: string
    author: string
    date: Date
}

export interface IChecklist {
    description: string
    todos: boolean
}
