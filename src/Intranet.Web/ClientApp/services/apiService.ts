import tokenService from './tokenService'

const apiService = {
    getNewsItem<T>() {
        return tokenService.getToken()
            .then(token => getData(token))
            .then(result => result.json() as Promise<T>)
    }
}

const getData = (token: string) => {
    return fetch('http://localhost:50590/api/news', {
        method: 'get',
        mode: 'cors',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    })
}

export default apiService;

