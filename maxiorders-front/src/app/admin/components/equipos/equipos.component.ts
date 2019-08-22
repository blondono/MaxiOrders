import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Device } from 'src/app/models/master/device';
import { BaseService } from '../../services/base.master';
import { UserService } from '../../services/user.service';
import { UploadService } from '../../services/upload.service';
import { Upload } from 'src/app/models/master/upload';


@Component({
  selector: 'app-admin-equipo',
  templateUrl: './equipos.component.html',
  styleUrls: ['../../../../assets/styles.css'],
  providers: [BaseService, UserService, UploadService]
})
export class EquiposComponent implements OnInit {
  public title: string;
  public devices: Device[];
  public busqueda: string;
  public mode: string;
  public controller: string;
  public device: Device;
  public status: string;
  public filesToUpload: Array<Upload>;

  constructor(
    private _route: ActivatedRoute,
    private _router: Router,
    private _baseService: BaseService,
    private _uploadService: UploadService
  ){
    this.clearDevice();
    this.title = 'Listado de equipos';
    this.controller = 'device';
  }

    fileChangeEvent(field: string, fileInput: any) {
      this.filesToUpload.forEach( (item, index) => {
        if (item.Field === field) 
          this.filesToUpload.splice(index, 1);
      });
      let file = new Upload(field, fileInput.target.files[0]);
      this.filesToUpload.push(file);
  }

  resetForm(newForm) {
    newForm.resetForm();
    this.clearDevice();
  }

  clearDevice() {
    this.device = new Device(0, '', '', '', '', '', true, new Date(), new Date(), new Date(), '', '', '');
    this.filesToUpload = new Array<Upload>();
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
    if (this.mode === 'Guardar') {
      this._baseService.add(this.device, this.controller).subscribe(
        response => {
          if (response.code !== 200) {
            this.status = 'error';
          } else {
            this.status = 'success';
            if (this.filesToUpload) {
              this._baseService.upload(response.content.idDevice, this.filesToUpload, this.controller)
              .then((result: any) => {
                $('#modalNew').modal('hide');
                this.clearDevice();
                this.getDevices();
              });
            }
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