import { Injectable, Inject } from '@angular/core';
import { Http, URLSearchParams, RequestOptions, RequestMethod, RequestOptionsArgs, Headers } from '@angular/http';
import { APP_BASE_HREF } from '@angular/common';
import { ORIGIN_URL } from './constants/baseurl.constants';
import { TransferHttp } from '../../modules/transfer-http/transfer-http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class FileService {
    constructor(
        private transferHttp: TransferHttp, // Use only for GETS that you want re-used between Server render -> Client render
        private http: Http, // Use for everything else
        @Inject(ORIGIN_URL) private baseUrl: string) {

    }
    addFile(formData: FormData): Observable<any> {
        var i = localStorage.getItem("auth");
        console.log(i );
        return this.http.post(`${this.baseUrl}/api/files`, formData);
    }
}
