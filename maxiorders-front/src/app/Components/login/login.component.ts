import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'public-login',
  templateUrl: './login.component.html'
})
export class PublicLoginComponent implements OnInit {
  title = 'Bienvenido a Maxi Orders ADMIN';

  ngOnInit() {
      console.log('Componente home cargado');
  }
}