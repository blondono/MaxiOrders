import { Injectable } from "@angular/core";
import { Router, CanActivate } from '@angular/router';
import { UserService } from 'src/app/services/user.service';

@Injectable()
export class AdminGuard implements CanActivate {
    
    constructor(
        private _router: Router,
        private _userService: UserService
    ){}

    canActivate(){
        let identity = this._userService.getIdentity();
        
        if(identity !== null && identity.user && (identity.user.role == 'ROLE_ADMIN' || identity.user.role == 'ROLE_SUPERADMIN'))
            return true
        else{
            this._router.navigate(['/admin/login']);
            return false;
        }
    }
}