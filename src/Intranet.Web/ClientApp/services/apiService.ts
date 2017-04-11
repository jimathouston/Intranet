import tokenService from './tokenService'

const apiService = {
    getNewsItem<T>() {
        return tokenService.getToken()
        .then(token => getData(token))
        .then(result => result.json() as Promise<T>)
    }
}

const getData = (token: string) => {
    return fetch('http://localhost/api/news', { 
            method: 'get', 
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
}

export default apiService;

