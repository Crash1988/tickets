import { Injectable, Inject } from '@angular/core';
import { Http, URLSearchParams, RequestOptions, RequestMethod, RequestOptionsArgs, Headers } from '@angular/http';
import { APP_BASE_HREF } from '@angular/common';
import { ORIGIN_URL } from './constants/baseurl.constants';
import { IUser } from '../models/User';
import { TransferHttp } from '../../modules/transfer-http/transfer-http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class ReceiptService {
    constructor(
        private transferHttp: TransferHttp, // Use only for GETS that you want re-used between Server render -> Client render
        private http: Http, // Use for everything else
        @Inject(ORIGIN_URL) private baseUrl: string) {

    }
    addReceipt(userName: string, age: number, lastName: string): Observable<any> {
        return this.http.post(`${this.baseUrl}/api/receipts`, { name: userName, age: age, lastname: lastName })
    }
    addFile(formData: FormData): Observable<any> {
        let headers = new Headers();
        //headers.append('Content-Type', 'multipart/form-data');
        headers.append('Accept', 'application/json');
        let opts: RequestOptionsArgs = { headers: headers };

        return this.http.post(`${this.baseUrl}/api/file`, formData, opts);
    }
}
