import {
    Component,
    AfterViewInit,
    EventEmitter,
    OnDestroy,
    Input,
    Output
} from '@angular/core'

declare const tinymce: any

@Component({
    selector: 'text-editor',
    templateUrl: './texteditor.component.html',
    styleUrls: ['./texteditor.component.css']
})
export class TextEditorComponent implements AfterViewInit, OnDestroy {
    ClientSide: boolean
    TinyMCELoaded: boolean
    Editor: any
    Saved: boolean

    @Input() elementId: string
    @Input() text: string
    @Output() onSubmit = new EventEmitter<string>()
    @Output() onEditorContentChange = new EventEmitter<string>()

    constructor() {
        this.ClientSide = typeof window !== 'undefined'
        this.Saved = true
    }

    handleSubmit() {
        this.Saved = true
        const content = this.Editor.getContent()
        this.onSubmit.emit(content)
    }

    ngAfterViewInit() {
        if (this.ClientSide) {
            require.ensure([
                'tinymce'
            ], require => {
                require('tinymce')
                require('tinymce/themes/modern')
                require('tinymce/plugins/table')
                require('tinymce/plugins/link')
                require('tinymce/plugins/lists')
                require('tinymce/plugins/image')
                require('tinymce/plugins/imagetools')
                require('tinymce/plugins/save')
                require('tinymce/plugins/wordcount')

                this.TinyMCELoaded = true

                tinymce.init({
                    selector: '#' + this.elementId,
                    plugins: [ 'link', 'table', 'lists', 'image', 'imagetools', 'save', 'wordcount' ],
                    toolbar: 'undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | save media | codesample help',
                    menubar: false,
                    save_onsavecallback: () => this.handleSubmit(),
                    skin_url: '/assets/tinymce/skins/lightgray', // Skins is in the wwwroot
                    setup: editor => {
                        this.Editor = editor
                        editor.on('keyup change undo redo', () => {
                            const content = editor.getContent()
                            this.Saved = false
                            this.onEditorContentChange.emit(content)
                        })
                    }
                })
            })
        }
    }

    ngOnDestroy() {
        if (this.ClientSide) {
            tinymce.remove(this.Editor)
        }
    }
}
