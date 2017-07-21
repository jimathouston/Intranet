import { XHRBackend, Http, RequestOptions } from '@angular/http'
import { AuthenticationService, SecureHttpService } from '../_services'
import { ConfigService } from '../_services'

export function SecureHttpFactory(
    xhrBackend: XHRBackend,
    requestOptions: RequestOptions,
    configService: ConfigService,
    authenticationService: AuthenticationService,
): SecureHttpService {
    return new SecureHttpService(xhrBackend, requestOptions, configService, authenticationService)
}
