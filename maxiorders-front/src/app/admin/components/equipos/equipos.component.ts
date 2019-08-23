import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Device } from 'src/app/models/master/device';
import { BaseService } from '../../services/base.master';
import { UserService } from '../../services/user.service';
import { UploadService } from '../../services/upload.service';
import { Upload } from 'src/app/models/master/upload';
import { ADMINGLOBAL } from '../../services/admin.global';

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
  public url: string;
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
    this.url = ADMINGLOBAL.url;
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
    this.device = new Device(0, '', '', '', '', '', true, null, null, null, '', '', '');
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
        } else {
          this.devices = null;
        }
      }, error => {
        console.log(<any>error);
      }
    );
  }

  onNew() {
    this.mode = 'Guardar';
  }
  onEdit(item: Device) {
    this.mode = 'Actualizar';
    this.device = new Device(item.idDevice, item.name, item.brand, item.model, item.serie, item.licenseNumber,
      item.state, item.manufacturingDate.toString().split('T')[0], item.purchaseDate.split('T')[0], item.instalationDate.split('T')[0],
      item.image, item.billImage, item.dataSheets);
    $('#modalNew').modal('show');

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
      );
    } else {
      this._baseService.update(this.device.idDevice, this.device, this.controller).subscribe(
        response => {
          if (response.code !== 200) {
            this.status = 'error';
          } else {
            this.status = 'success';
            if (this.filesToUpload && this.filesToUpload.length !== 0) {
              this._baseService.upload(this.device.idDevice, this.filesToUpload, this.controller)
              .then((result: any) => {
              });
            }
            $('#modalNew').modal('hide');
            this.clearDevice();
            this.getDevices();
          }
        }, error => {
          var errorMessage = <any>error;
          if (errorMessage != null) {
            this.status = 'error';
          }
        }
      );
    }
  }

  delete(id){
    this._baseService.delete(id, this.controller).subscribe(
      response => {
        if (response.code !== 200) {
          alert('Error en el servicio');
        } else {
          $('#myModal' + id).modal('hide');
          this.clearDevice();
          this.getDevices();
        }
      }, error => {

      }
    );
  }
}