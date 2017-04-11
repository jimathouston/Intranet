import * as React from 'react'
import { Router, Route, HistoryBase } from 'react-router'
import { Layout } from './components/Layout'
import NewsItem from './components/News'
import FetchData from './components/FetchData'
import Counter from './components/Counter'
import MyCert from './components/MyCert'
import OurCert from './components/OurCert'
import ConsultMap from './components/ConsultMap'
import Costumer from './components/Costumer'
import Home from './components/Home'

export default <Route component={ Layout }>
    <Route path='/' components={{ body: NewsItem }} />
    <Route path='/counter' components={{ body: Counter }} />
    <Route path='/fetchdata' components={{ body: FetchData }} />
    <Route path='/mycert' components={{ body: MyCert }} />
    <Route path='/ourcert' components={{ body: OurCert }} />
    <Route path='/consultmap' components={{ body: ConsultMap }} />
    <Route path='/costumer' components={{ body: Costumer }}>
        <Route path='(:startDateIndex)' /> { /* Optional route segment that does not affect NavMenu highlighting */ }
    </Route>
</Route>

// Enable Hot Module Replacement (HMR)
if (module.hot) {
    module.hot.accept()
}
