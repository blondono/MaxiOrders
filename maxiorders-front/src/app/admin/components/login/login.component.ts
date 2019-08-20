import { Component, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { UserService } from 'src/app/admin/services/user.service';
import { User } from 'src/app/models/users/user';

@Component({
  selector: 'admin-login',
  templateUrl: './login.component.html',
  styleUrls: [ '../assets/login.css' ],
  encapsulation: ViewEncapsulation.None,
  providers: [UserService]
})
export class LoginComponent {
  title = 'Inicio de sesión ADMIN';
  public user: User;
  public identity;
  public token;
  public status: string;
  public message: string;

  constructor(
    private _route: ActivatedRoute,
    private _router: Router,
    private _userService: UserService
  )
  {
      this.title = 'Login';
      this.user = new User(0, '', '', '', '', '', '', '');
  }

  public onSubmit() {
    //Lograr el usuario y  conseguir el usuario
    this._userService.login(this.user).subscribe(
    response => {
        if(response.code == 200) {
          this.status = 'success';
          this.identity = response.content.user;
          
          if(!this.identity || !this.identity.idUser){
            this.message = "El usuario no se ha logeado correctamente";
            this.status = 'error';
          } else {
              this.identity.password = '';
              localStorage.setItem('identity', JSON.stringify(response.content));
              this.status = "success";
              this._router.navigate(['/admin']);
          }
        } else if(response.code = 500){
          this.message = response.message;
          this.status = 'error';
        }
    },error => {
        var errorMessage= <any>error;
        if(errorMessage != null){
            var body = JSON.parse(error._body);
            this.message = "Error del sistema, por favor comunicarse con el administrador";
            this.status = 'error';
        }
    });
  }
}