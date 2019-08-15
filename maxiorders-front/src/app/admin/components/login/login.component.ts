import { Component, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'admin-login',
  templateUrl: './login.component.html',
  styleUrls: [ '../assets/login.css' ],
  encapsulation: ViewEncapsulation.None,
})
export class LoginComponent {
  title = 'Inicio de sesión ADMIN';
}