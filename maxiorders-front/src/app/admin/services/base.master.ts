import { Injectable } from "@angular/core";
import { Http, Headers, RequestOptions } from '@angular/http';
import { Router } from '@angular/router';
import {map} from 'rxjs/operators';
import { ADMINGLOBAL } from './admin.global';
import { UserService } from 'src/app/admin/services/user.service';

@Injectable()
export class BaseService {
    public url: string;
    public identity;
    public token;
    constructor(
        private _http: Http,
        private _userService: UserService,
        private _router: Router
    ){
        this.url = ADMINGLOBAL.url;
        this.getToken();
    }

    add(object, controller){
        let params = JSON.stringify(object);
        let headers = new Headers({
            'Content-Type': 'application/json',
            'Authorization': this.token
        });

        return this._http.post(this.url + controller, params, { headers: headers })
        .pipe(map(res => res.json()));
    }

    all(controller){
        return this._http.get(this.url  + controller)
        .pipe(map(res => res.json()));
    }

    get(id, controller){
        return this._http.get(this.url + controller + '/' + id)
        .pipe(map(res => res.json()));
    }

    update(id, object, controller){
        let params = JSON.stringify(object);
        let headers = new Headers({
            'Content-Type': 'application/json',
            'Authorization': this.token
        });

        return this._http.put(this.url + controller + '/' + id, params, { headers: headers })
        .pipe(map(res => res.json()));
    }

    delete(id, controller){
        let headers = new Headers({
            'Content-Type': 'application/json',
            'Authorization': this.token
        });

        let options = new RequestOptions({headers: headers});

        return this._http.delete(this.url + controller + '/' + id, options)
        .pipe(map(res => res.json()));
    }

    getToken(){
        this.identity = this._userService.getIdentity();
        if(this.identity != undefined && this.identity.user != undefined && 
            this.identity.token != undefined && this.identity.token != ''){
                this.token = this.identity.token;
        } else {
            this._router.navigate(['/admin/login']);
        }
    }
}