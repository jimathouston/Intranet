import {
  Component,
  EventEmitter,
  Input,
  Output,
  OnInit,
} from '@angular/core'

@Component({
    selector: 'keywords-strip',
    templateUrl: 'keywords-strip.component.html',
    styleUrls: ['./keywords-strip.component.css']
})
export class KeywordsStripComponent {
  @Input() keywords: string
  @Input() elementId: string
  @Input() edit: boolean
  @Output() onEditorContentChange = new EventEmitter<string>()

  formatedKeywords() {
    return this.keywords.replace(/,/g, ', ')
  }

  handleEditorContentChange(content: string) {
    this.onEditorContentChange.emit(content)
  }
}
