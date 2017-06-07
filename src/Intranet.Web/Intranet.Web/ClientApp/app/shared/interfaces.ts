export interface INewsItem {
  id?: number
  title: string
  text: string
  author: string
  date: Date
}

export interface IChecklist {
  toDoId?: number
  description: string
  todos: boolean
}

export interface IProfile {
  id?: number
  firstName: string
  lastName: string
  description: string
  email: string
  phoneNumber: number
  mobile: number
  streetAdress: string
  postalCode: number
  city: string
  toDos?: number[]
}