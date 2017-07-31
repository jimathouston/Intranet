import { Component, Input } from '@angular/core'
import { NgSwitch } from '@angular/common'

@Component({
  selector: 'flash-message',
  templateUrl: './flash-message.component.html',
  styleUrls: ['./flash-message.component.css'],
  providers: [NgSwitch]
})
export class FlashMessageComponent {
  @Input() flash: 'success' | 'info' | 'warning' | 'danger'
  @Input() message: string
}
