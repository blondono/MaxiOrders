import { Component } from '@angular/core';
import { UserService } from './services/user.service';
import { Auth } from './models/users/auth.upload';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css', '../assets/styles.css']
})
export class AppComponent {
  title = 'maxiorders-front';
  public _user;
  constructor(
    private _userService: UserService
  ){
    if (_userService.getIdentity() === null) {
      this._user = new Auth('', null);
    } else {
      this._user = _userService.getIdentity();
    }
  }

}
