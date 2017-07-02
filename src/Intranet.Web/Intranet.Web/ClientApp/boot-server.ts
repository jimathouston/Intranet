import 'reflect-metadata'
import 'zone.js'
import 'rxjs/add/operator/first'
import { enableProdMode, ApplicationRef, NgZone, ValueProvider } from '@angular/core'
import { platformDynamicServer, PlatformState, INITIAL_CONFIG } from '@angular/platform-server'
import { createServerRenderer, RenderResult } from 'aspnet-prerendering'
import { AppModule } from './app/app.module.server'

enableProdMode()

/* START - Hack to enable to set cookie header */
import * as xhr2 from 'xhr2'
xhr2.prototype._restrictedHeaders.cookie = false
/* END - Hack to enable to set cookie header */

export default createServerRenderer(params => {
    const cookies: string = params.data.cookies.map(c => `${c.key}=${c.value}; `).toString().trim()

    const providers = [
        { provide: INITIAL_CONFIG, useValue: { document: '<app></app>', url: params.url } },
        { provide: 'ORIGIN_URL', useValue: params.origin },
        { provide: 'COOKIES', useValue: cookies }
    ]

    return platformDynamicServer(providers).bootstrapModule(AppModule).then(moduleRef => {
        const appRef = moduleRef.injector.get(ApplicationRef)
        const state = moduleRef.injector.get(PlatformState)
        const zone = moduleRef.injector.get(NgZone)

        return new Promise<RenderResult>((resolve, reject) => {
            zone.onError.subscribe(errorInfo => reject(errorInfo))
            appRef.isStable.first(isStable => isStable).subscribe(() => {
                // Because 'onStable' fires before 'onError', we have to delay slightly before
                // completing the request in case there's an error to report
                setImmediate(() => {
                    resolve({
                        html: state.renderToString()
                    })
                    moduleRef.destroy()
                })
            })
        })
    })
})