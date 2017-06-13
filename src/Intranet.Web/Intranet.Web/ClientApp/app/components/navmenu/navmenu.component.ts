import { Component } from '@angular/core'

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent {
    isIn = false

    toggleState() {
        const bool = this.isIn
        this.isIn = bool === false ? true : false
    }
}
