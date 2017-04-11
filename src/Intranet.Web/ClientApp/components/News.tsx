import * as React from 'react'
import { INewsItems } from '../definitions/API'

import apiService  from '../services/apiService'
import tokenService  from '../services/tokenService'

export interface NewsItemState {
    newsitems: INewsItems[]
}

class NewsItem extends React.Component<void, NewsItemState> {
    constructor() {
        super()
        this.state = {
            newsitems: [],
        }
    }

    componentDidMount() {
       apiService.getNewsItem<INewsItems[]>()
       .then(newsitems => this.setState({newsitems}))
       .catch(error => console.log(error))
    }

    render() {
        return(
            <div>
                <h1>Certaincy news</h1>
                   {this.state.newsitems.map(newsitem =>
                       <div className='newsitem' key={newsitem.id}>
                        <h3>{newsitem.title}</h3>
                        <p><em>{newsitem.date}</em></p>
                        <p>{newsitem.text}</p>
                        <p><b>Författare:</b> {newsitem.author} </p>
                    </div>
                    )}
            </div>

        )
    }
}


export default NewsItem
