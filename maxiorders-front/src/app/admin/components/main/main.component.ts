import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'admin-main',
  templateUrl: './main.component.html'
})
export class MainComponent implements OnInit {
  title = 'Home Admin';

  ngOnInit() {
      console.log('Componente home cargado');
  }
}