export class Jwt {
    jti: string
    iat: Date
    displayName: string
    username: string
    role: 'Developer' | 'Admin'
    nbf: Date
    exp: Date
    iss: string
    aud: string
}
