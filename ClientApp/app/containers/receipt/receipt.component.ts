import {
    Component, OnInit,
    // animation imports
    trigger, state, style, transition, animate,
    Inject, ElementRef, 
} from '@angular/core';
import { FileService } from '../../shared/file.service';
import { ReceiptService } from '../../shared/receipt.service';
import { DragnDropDirective } from '../../directives/dragndrop.directive'

@Component({
    selector: 'app-receipt',
    templateUrl: './receipt.component.html',
    styleUrls: ['./receipt.component.scss']

})
export class ReceiptComponent {

    title: string = 'Receipts';
    private fileList: any = [];
    private invalidFiles: any = [];


    // Use "constructor"s only for dependency injection
    constructor(
        private receiptService: ReceiptService,
        private fileService: FileService,
        private el: ElementRef
    ) { }

    onFileAdded(event: any) {
        let files = event.srcElement.files;
        let allowed_extensions = ['png', 'jpg', 'bmp'];
        if (files.length > 0) {
            let file = files[0];
            let ext = file.name.split('.')[file.name.split('.').length - 1];
            if (allowed_extensions.lastIndexOf(ext) != -1) {
                this.fileList.push(file);
            } else {
                this.invalidFiles.push(file);
            }
        }
        //this.fileList.push(files[0]);
        //console.log(files);
        
    }

    onFilesChange(fileList: Array<File>) {
        this.fileList = fileList;
    }

    onInvalidFilesAdded(invalidFiles: Array<File>) {
        this.invalidFiles = invalidFiles;
    }

    

    upload() {
        let inputEl: HTMLInputElement = this.el.nativeElement.querySelector('#photo');
        //get the total amount of files attached to the file input.
        //create a new fromdata instance
        let formData = new FormData();
        if (this.fileList.length > 0) { // a file was selected
            //append the key name 'file' with the file  element
            this.fileList.forEach(function (elem) {
                formData.append('file', elem, elem.name);
            });
            this.fileService.addFile(formData).subscribe(r => {
                if (r.ok) {
                    this.fileList = [];
                    this.invalidFiles = [];
                    alert("All Receipts where uploaded!");
                }

            }, error => {
                console.log(`There was an issue. ${error._body}.`);
            });



        }

    }


    deleteValidItem(i: number) {
        this.fileList.splice(i, 1);
    }

}
