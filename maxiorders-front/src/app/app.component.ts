import { Component } from '@angular/core';
import { UserService } from './services/user.service';

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
    this._user = _userService.getIdentity();
  }

}
