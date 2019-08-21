import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Device } from 'src/app/models/master/device';
import { BaseService } from '../../services/base.master';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-admin-equipo',
  templateUrl: './equipos.component.html',
  styleUrls: ['../../../../assets/styles.css'],
  providers: [BaseService, UserService]
})
export class EquiposComponent implements OnInit {
  public title: string;
  public devices: Device[];
  public busqueda: string;
  public mode: string;
  public controller: string;
  public device: Device;
  public status: string;

  constructor(
    private _route: ActivatedRoute,
    private _router: Router,
    private _baseService: BaseService,
  ){
    this.clearDevice();
    this.title = 'Listado de equipos';
    this.controller = 'device';
  }

  resetForm(newForm) {
    newForm.resetForm();
    this.clearDevice();
  }

  clearDevice() {
    this.device = new Device(0, '', '', '', '', '', true, new Date(), new Date(), new Date(), '', '', '');
  }

  ngOnInit() {
    console.log('Componente equipo cargado');
    this.getDevices();
  }

  getDevices() {
    this._baseService.all(this.controller).subscribe(
      response => {
        if (response.list.length !== 0) {
          this.devices = response.list;
        }
      }, error => {
        console.log(<any>error);
      }
    );
  }

  onNew() {
    this.mode = 'Guardar';
  }
  onEdit() {
    this.mode = 'Actualizar';
  }

  onSubmit() {
    if (this.mode == 'Guardar') {
      this._baseService.add(this.device, this.controller).subscribe(
        response => {
          if (response.animal !== 200){
            this.status = 'error';
          } else {
            this.status = 'success';
            
            /*
            this.animal = response.animal;

            //subir imagen del animal
            if(!this.filesToUpload)
              this._router.navigate(['/admin-panel/listado']);
            else {
              this._uploadService.makeFileRequest(
                this.url + 'animal/uploadFile/' + this.animal._id,
                [],
                this.filesToUpload,
                this.token,
                'image'
            ).then((result: any) => {
                this.animal.image = result.image;
                console.log(this.animal);
                localStorage.setItem('identity', JSON.stringify(this.animal));
                console.log(this.animal);
            });
            }*/
          }
        }, error => {
          var errorMessage = <any>error;
          if (errorMessage != null) {
            this.status = 'error';
          }
        }
      )
    }
  }

  delete(id){
    $('#myModal' + id).modal('hide');
    this._baseService.delete(id, this.controller).subscribe(
      response => {
        if (response.code !== 200) {
          alert('Error en el servicio');
        } else {
          this.getDevices();
        }
      }, error => {

      }
    );
  }
}