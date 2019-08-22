import { Injectable } from "@angular/core";
import { Http, Headers, RequestOptions } from '@angular/http';
import { Router } from '@angular/router';
import {map} from 'rxjs/operators';
import { ADMINGLOBAL } from './admin.global';
import { UserService } from 'src/app/admin/services/user.service';
import { Upload } from 'src/app/models/master/upload';

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

    getToken() {
        this.identity = this._userService.getIdentity();
        if(this.identity != undefined && this.identity.user != undefined && 
            this.identity.token != undefined && this.identity.token != ''){
                this.token = "bearer " + this.identity.token;
        } else {
            this._router.navigate(['/admin/login']);
        }
    }

    upload(id, files: Array<Upload>, controller) {
        let urlUpload = this.url;
        let tokenUpload = this.token;
        return new Promise(function(resolve, reject) {
            var formData: any = new FormData();
            var xhr = new XMLHttpRequest();

            for (var i = 0; i < files.length; i++) {
                formData.append(files[i].Field, files[i].File, files[i].File.name);
            }

            xhr.onreadystatechange = function() {
                if (xhr.readyState === 4) {
                    if (xhr.status === 200) {
                        resolve(JSON.parse(xhr.response));
                    } else {
                        reject(xhr.response);
                    }
                }
            }

            xhr.open('POST', urlUpload + controller + '/Upload/' + id, true);
            xhr.setRequestHeader('Authorization', tokenUpload);
            xhr.send(formData);
        });
    }
}