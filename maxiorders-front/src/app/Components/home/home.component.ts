import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'public-home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {
  title = 'Home Public';

  ngOnInit() {
      console.log('Componente home cargado');
  }
}