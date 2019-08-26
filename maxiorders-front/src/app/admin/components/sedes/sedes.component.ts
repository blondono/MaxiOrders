import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseService } from '../../services/base.master';
import { UserService } from '../../services/user.service';
import { UploadService } from '../../services/upload.service';
import { Upload } from 'src/app/models/master/upload';
import { ADMINGLOBAL } from '../../services/admin.global';
import { HeadQuarter } from 'src/app/models/master/headquarter';
import { Company } from 'src/app/models/master/company';

@Component({
  selector: 'app-admin-sedes',
  templateUrl: './sedes.component.html',
  styleUrls: ['../../../../assets/styles.css'],
  providers: [BaseService, UserService, UploadService]
})
export class SedesComponent implements OnInit {
  public title: string;
  public list: HeadQuarter[];
  public listCompanies: Company[];
  public busqueda: string;
  public mode: string;
  public controller: string;
  public controllerCompany: string;
  public obj: HeadQuarter;
  public status: string;
  public url: string;
  public filesToUpload: Array<Upload>;

  constructor(
    private _route: ActivatedRoute,
    private _router: Router,
    private _baseService: BaseService,
    private _uploadService: UploadService
  ){
    this.clearHeadQuarter();
    this.title = 'Administración de Sedes';
    this.controller = 'headquarter';
    this.controllerCompany = 'company';
    this.url = ADMINGLOBAL.url;
  }

  resetForm(newForm) {
    newForm.resetForm();
    this.clearHeadQuarter();
  }

  clearHeadQuarter() {
    this.obj = new HeadQuarter(0, 0, '', '', '', '');
  }

  changeCompany(e) {
    this.obj.idCompany = e;
  }

  ngOnInit() {
    console.log('Componente sede cargado');
    this.getCompanies();
    this.getHeadQuarter();
  }

  searchByCompany(id) {
    if (id !== '' && id !== null) {
      this.list = this.list.filter(x => x.idCompany === parseInt(id));
    } else {
      this.getHeadQuarter();
    }
  }

  getHeadQuarter() {
    this._baseService.all(this.controller).subscribe(
      response => {
        if (response.list.length !== 0) {
          this.list = response.list;
        } else {
          this.list = null;
        }
      }, error => {
        console.log(<any>error);
      }
    );
  }

  getCompanies() {
    this._baseService.all(this.controllerCompany).subscribe(
      response => {
        if (response.list.length !== 0) {
          this.listCompanies = response.list;
        } else {
          this.listCompanies = null;
        }
      }, error => {
        console.log(<any>error);
      }
    );
  }

  onNew(newForm) {
    newForm.resetForm();
    this.clearHeadQuarter();
    this.mode = 'Guardar';
  }
  onMode(item: HeadQuarter, mode: string) {
    this.mode = mode;
    this.obj = new HeadQuarter(item.idHeadQuarter, item.idCompany, item.name, item.address, item.phone, item.email);
    $('#modalNew').modal('show');

  }

  onSubmit(newForm) {
    if(this.obj.idCompany === 0){
      newForm.valid = false;
    } else {
      if (this.mode === 'Guardar') {
        this._baseService.add(this.obj, this.controller).subscribe(
          response => {
            if (response.code !== 200) {
              this.status = 'error';
            } else {
              this.status = 'success';
              $('#modalNew').modal('hide');
              this.clearHeadQuarter();
              this.getHeadQuarter();
            }
          }, error => {
            var errorMessage = <any>error;
            if (errorMessage != null) {
              this.status = 'error';
            }
          }
        );
      } else {
        this._baseService.update(this.obj.idHeadQuarter, this.obj, this.controller).subscribe(
          response => {
            if (response.code !== 200) {
              this.status = 'error';
            } else {
              this.status = 'success';
              $('#modalNew').modal('hide');
              this.clearHeadQuarter();
              this.getHeadQuarter();
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
  }

  delete(id){
    this._baseService.delete(id, this.controller).subscribe(
      response => {
        if (response.code !== 200) {
          alert('Error en el servicio');
        } else {
          $('#myModal' + id).modal('hide');
          this.clearHeadQuarter();
          this.getHeadQuarter();
        }
      }, error => {

      }
    );
  }
}