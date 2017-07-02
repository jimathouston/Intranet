import { NgModule } from '@angular/core'
import { ServerModule } from '@angular/platform-server'
import { sharedConfig } from './app.module'

@NgModule({
    bootstrap: sharedConfig.bootstrap,
    declarations: sharedConfig.declarations,
    imports: [
        ServerModule,
        ...sharedConfig.imports
    ],
    providers: [
        { provide: 'ServerSide', useValue: true },
        ...sharedConfig.providers,
    ],
})
export class AppModule {
}