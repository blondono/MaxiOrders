import { Injectable } from "@angular/core";
import { Http, Response, Headers } from '@angular/http';
import {map} from 'rxjs/operators';
import { Observable } from 'rxjs';
import { ADMINGLOBAL } from './admin.global';

@Injectable()
export class UserService {
    public url: string;
    public identity;
    public token;
    
    constructor(
        private _http: Http
    ){
        this.url = ADMINGLOBAL.url;
    }

    login(user_to_login, gettoken = null){
        if(gettoken != null){
            user_to_login.gettoken = gettoken;
        }
        let params = JSON.stringify(user_to_login);
        let headers = new Headers({'Content-Type': 'application/json'});
        return this._http.post(this.url + 'login', params, {headers: headers})
            .pipe(map(res => res.json()));
    }

    getIdentity(){
        let identity = JSON.parse(localStorage.getItem('identity'));
        if(identity != undefined)
            this.identity = identity;
        else
            this.identity = null;
        return identity;
    }
}