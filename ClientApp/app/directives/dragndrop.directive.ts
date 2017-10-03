
import { Directive, Input, Output, EventEmitter, HostListener, HostBinding,  } from '@angular/core';
//import { forEach } from "@angular/router/src/utils/collection";

@Directive({
    selector: "[dragnDrop]"
})
export class DragnDropDirective {
    
    @Output() private filesChangeEmiter: EventEmitter<File[]> = new EventEmitter();
    @Output() private filesInvalidEmiter: EventEmitter<File[]> = new EventEmitter();
    @Input() private allowed_extensions: Array<string> = [];

    @HostBinding('style.background') private background = '#eee';

    @HostListener('dragover', ['$event']) onDragOver(evt) {
        evt.preventDefault();
        evt.stopPropagation();
        this.background = '#999';


    }


    @HostListener('dragLeave', ['$event']) onDragLeave(evt) {
        evt.preventDefault();
        evt.stopPropagation();
        let files = evt.dataTransfer.files;
        this.background = '#eee';

    }

    @HostListener('drop', ['$event']) onDrop(evt) {
            evt.preventDefault();
            evt.stopPropagation();
            this.background = '#eee';
            let files = evt.dataTransfer.files;
            let valid_files: Array<File> = [];
            let invalid_files: Array<File> = [];
            if (files.length > 0) {
                

                for(let file of files) {
                    let ext = file.name.split('.')[file.name.split('.').length - 1];
                    if (this.allowed_extensions.lastIndexOf(ext) != -1) {
                        valid_files.push(file);
                    } else {
                        invalid_files.push(file);
                    }
                    }
                
                this.filesChangeEmiter.emit(valid_files);
                this.filesInvalidEmiter.emit(invalid_files);
            }
        




    }

}
