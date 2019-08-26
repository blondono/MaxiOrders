import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseService } from '../../services/base.master';
import { UserService } from '../../services/user.service';
import { UploadService } from '../../services/upload.service';
import { Upload } from 'src/app/models/master/upload';
import { ADMINGLOBAL } from '../../services/admin.global';
import { Company } from 'src/app/models/master/company';

@Component({
  selector: 'app-admin-cliente',
  templateUrl: './clientes.component.html',
  styleUrls: ['../../../../assets/styles.css'],
  providers: [BaseService, UserService, UploadService]
})
export class ClienteComponent implements OnInit {
  public title: string;
  public companies: Company[];
  public busqueda: string;
  public mode: string;
  public controller: string;
  public company: Company;
  public status: string;
  public url: string;
  public filesToUpload: Array<Upload>;

  constructor(
    private _route: ActivatedRoute,
    private _router: Router,
    private _baseService: BaseService,
    private _uploadService: UploadService
  ){
    this.clearCompany();
    this.title = 'Administración de clientes';
    this.controller = 'company';
    this.url = ADMINGLOBAL.url;
  }

    fileChangeEvent(field: string, fileInput: any) {
      this.filesToUpload.forEach( (item, index) => {
        if (item.Field === field) {
          this.filesToUpload.splice(index, 1);
        } 
      });
      let file = new Upload(field, fileInput.target.files[0]);
      this.filesToUpload.push(file);
  }

  resetForm(newForm) {
    newForm.resetForm();
    this.clearCompany();
  }

  clearCompany() {
    this.company = new Company(0, '', '');
    this.filesToUpload = new Array<Upload>();
  }

  ngOnInit() {
    console.log('Componente cliente cargado');
    this.getCompany();
  }

  getCompany() {
    this._baseService.all(this.controller).subscribe(
      response => {
        if (response.list.length !== 0) {
          this.companies = response.list;
        } else {
          this.companies = null;
        }
      }, error => {
        console.log(<any>error);
      }
    );
  }

  onNew() {
    this.mode = 'Guardar';
  }
  onEdit(item: Company) {
    this.mode = 'Actualizar';
    this.company = new Company(item.idCompany, item.name, item.logo);
    $('#modalNew').modal('show');

  }

  onSubmit() {
    if (this.mode === 'Guardar') {
      this._baseService.add(this.company, this.controller).subscribe(
        response => {
          if (response.code !== 200) {
            this.status = 'error';
          } else {
            this.status = 'success';
            if (this.filesToUpload) {
              this._baseService.upload(response.content.idCompany, this.filesToUpload, this.controller)
              .then((result: any) => {
                $('#modalNew').modal('hide');
                this.clearCompany();
                this.getCompany();
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
      this._baseService.update(this.company.idCompany, this.company, this.controller).subscribe(
        response => {
          if (response.code !== 200) {
            this.status = 'error';
          } else {
            this.status = 'success';
            if (this.filesToUpload && this.filesToUpload.length !== 0) {
              this._baseService.upload(this.company.idCompany, this.filesToUpload, this.controller)
              .then((result: any) => {
              });
            }
            $('#modalNew').modal('hide');
            this.clearCompany();
            this.getCompany();
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
          this.clearCompany();
          this.getCompany();
        }
      }, error => {

      }
    );
  }
}