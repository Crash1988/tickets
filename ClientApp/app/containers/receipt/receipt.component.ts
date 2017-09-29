import {
    Component, OnInit,
    // animation imports
    trigger, state, style, transition, animate, Inject, ElementRef
} from '@angular/core';
import { ReceiptService } from '../../shared/receipt.service';


@Component({
    selector: 'app-receipt',
    templateUrl: './receipt.component.html'
})
export class ReceiptComponent implements OnInit {

    title: string = 'Receipts Managements';

    // Use "constructor"s only for dependency injection
    constructor(
        private receiptService: ReceiptService,
        private el: ElementRef
    ) { }

    // Here you want to handle anything with @Input()'s @Output()'s
    // Data retrieval / etc - this is when the Component is "ready" and wired up
    ngOnInit() {

    }
    onSubmit(userName, age, lastName) {
        
        this.receiptService.addReceipt(userName, age, lastName).subscribe(result => {
            console.log('Post user result: ', result);
           /* if (result.ok) {
                this.users.push(result.json());
            }*/
        }, error => {
            console.log(`There was an issue. ${error._body}.`);
        });
    }

    upload() {
        let inputEl: HTMLInputElement = this.el.nativeElement.querySelector('#photo');
        //get the total amount of files attached to the file input.
        let fileCount: number = inputEl.files.length;
        //create a new fromdata instance
        let formData = new FormData();
        if (fileCount > 0) { // a file was selected
            //append the key name 'file' with the first file in the element
            formData.append('file', inputEl.files.item(0), inputEl.files.item(0).name);
            this.receiptService.addFile(formData).subscribe(result => {
                console.log('Post user result: ', result);
                /* if (result.ok) {
                     this.users.push(result.json());
                 }*/
            }, error => {
                //console.log(`There was an issue. ${error._body}.`);
            });

        }

    }
}
