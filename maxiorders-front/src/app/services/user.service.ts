import { Injectable } from "@angular/core";
import { Http, Response, Headers } from '@angular/http';
import {map} from 'rxjs/operators';
import { Observable } from 'rxjs';
import { GLOBAL } from './global';
import { User } from '../models/users/user';

@Injectable()
export class UserService {
    public url: string;
    public identity: User;
    public token: string;
    
    constructor(
        private _http: Http
    ){
        this.url = GLOBAL.url;
    }

    

    getIdentity(){
        let identity = JSON.parse(localStorage.getItem('identity'));
        if(identity != undefined)
            this.identity = identity;
        else
            this.identity = null;
        return identity;
    }

    getToken(){
        let token = localStorage.getItem('token');
        if(token != undefined)
            this.token = token;
        else
            this.token = null;
        return token;
    }
}