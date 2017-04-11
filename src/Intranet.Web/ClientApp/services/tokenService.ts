import { default as jwt } from 'jwt-decode'

const tokenKey = 'token'
const tokenUrl = '/generateToken'

const tokenService = {
	getToken() {
		return new Promise<string>((resolve, reject) => {
			const token = getStoredToken()
			if (token) return resolve(token)

			fetch(tokenUrl, {
				credentials: 'same-origin'
			})
			.then(result => result.json() as any)
			.then(result => {
				const token = result.accessToken
				if (token) {
					localStorage.setItem('token', token)
					return resolve(token as string)
				} else {
					return reject('Something went wrong.')
				}
			})
		})
	}
}

const getStoredToken = (): string => {
	const tokenFromStorage = localStorage.getItem('token')
	if (!tokenFromStorage) return ''

	const tokenDecoded = jwt(tokenFromStorage)
	if (tokenFromStorage && tokenDecoded.exp < Date.now()) {
		return tokenFromStorage
	} else {
		localStorage.removeItem('token')
		return ''
	}
}

export default tokenService;