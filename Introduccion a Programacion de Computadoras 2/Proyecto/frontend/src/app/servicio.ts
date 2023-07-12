import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Usuario } from './modelos/auxiliar-model';


@Injectable()

export class Servicio{
	constructor(private http: HttpClient){
}


/***********************************************************
************************************************************
******* METODOS PARA EL LOGIN Y REGISTRO DE USUARIOS *******
************************************************************
***********************************************************/
//----------------------------LOGIN-------------------------
	login(carnet:string, pass:string) {
		return this.http.post('http://localhost:3000/logear',{
			carnet:carnet,
			pass:pass,
		});		  
    }
//--------------------------REGISTRO-----------------------------------
    registro(carnet:string, nombre:string, apellido:string, email:string, pass:string){
        return this.http.post('http://localhost:3000/registrar',{
            carnet:carnet,
            nombre:nombre,
            apellido:apellido,
            email:email,
            pass:pass
        });
    }
//------------------------LISTAR USUARIOS------------------------------
    listarUsuarios(){
        return this.http.get('http://localhost:3000/listarUsuarios');
    }
//------------------------LISTAR AUXILIARES----------------------------
    listarAuxiliares(){
        return this.http.get('http://localhost:3000/listarAuxiliares');
    }
//------------------------AGREGAR AUXILIAR-----------------------------
    agregarAuxiliar(carnet:string){
        return this.http.post('http://localhost:3000/agregarAuxiliar',{
            carnet: carnet,
        })
    }
//------------------------ELIMINAR AUXILIAR-----------------------------
    eliminarAuxiliar(carnet:string){
        return this.http.post('http://localhost:3000/eliminarAuxiliar',{
            carnet: carnet,
        });
    }
//-------------------------ELIMINAR AUXILIAR----------------------------
    editarAuxiliar(carnet:string, nombre:string, apellido:string, email:string){
        return this.http.post('http://localhost:3000/editarAuxiliar',{
            carnet: carnet,
            nombre: nombre,
            apellido: apellido,
            email: email,
        });
    }
//------------------------GUARDADO DE DATOS-----------------------------
    private storage: string = "carnet"; 
    SaveData(Carnet:string, pass: string){
       localStorage.setItem(this.storage, JSON.stringify(Carnet));
    }
//----------------------RECUPERACION DE DATOS---------------------------
    GetData(){
       let Carnet = localStorage.getItem(this.storage);
       console.log(JSON.parse(Carnet));
       return this.http.post('http://localhost:3000/recuperarUsuario', {
       carnet:JSON.parse(Carnet),
       });
    }


/***********************************************************
************************************************************
**************** METODOS PARA MANEJO DE CURSOS *************
************************************************************
***********************************************************/
//--------------------AGREGAR NUEVO CURSO-----------------//
    AgregarCurso(codigo:string, nombre:string){
        return this.http.post('http://localhost:3000/agregarCurso',{
            codigo:codigo,
            nombre:nombre,
        });
    }
//-------------------LISTAR TODOS LOS CURSOS--------------//
    listarCursos(){
        return this.http.get('http://localhost:3000/listarCursos');
    }
//---------------------EDITAR CURSO-----------------------//
    editarCurso(codigo:string, nombre:string){
        return this.http.post('http://localhost:3000/editarCurso',{
            codigo:codigo,
            nombre:nombre,      
        });
    }
//--------------------ELIMINAR CURSO----------------------//
    eliminarCurso(codigo:string){
        return this.http.post('http://localhost:3000/eliminarCurso',{
            codigo:codigo,
        }); 
    }



/***********************************************************
************************************************************
*************** METODOS PARA MANEJO DE SECCIONES ***********
************************************************************
***********************************************************/
//--------------------AGREGAR SECCION---------------------//
    agregarSeccion(codigo:string, nombre:string, horario:string, seccion:string, semestre:string, ano:string){
        console.log(codigo+nombre+horario+seccion+semestre+ano);
        return this.http.post('http://localhost:3000/agregarSeccion',{
            codigo:codigo,
            nombre:nombre,
            horario:horario,
            seccion:seccion,
            semestre:semestre,
            ano:ano,
        });
    }
//----------------LISTAR TODAS LAS SECCIONES--------------//
    listarSecciones(){
        return this.http.get('http://localhost:3000/listarSecciones');
    }
//-----------------ELIMINAR UNA SECCION-------------------//
    eliminarSeccion(codigo:string){
        return this.http.post('http://localhost:3000/eliminarSeccion',{
            codigo:codigo,
        });
    }
//--------------------LISTAR SECCIONES CON AUXILIAR-------//
    listarSeccionesAuxiliar(){
        return this.http.get('http://localhost:3000/listarSeccionesAuxiliar');
    }
//--------------------LISTAR SECCIONES SIN AUXILIAR-------//
    listarSeccionesSinAuxiliar(){
        return this.http.get('http://localhost:3000/listarSeccionesSinAuxiliar');
    }
//------------------AGREGAR AUXILIAR A SECCION-----------//
    agregarAuxiliarSeccion(id: string, carnet: string){
        return this.http.post('http://localhost:3000/agregarAuxiliarSeccion',{
            ID: id,
            carnet: carnet,
        })
    }
//------------------DESASINGAR AUXILIAR DE SECCION--------//
    desasignarCurso(carnet: string, ID: string){
        return this.http.post('http://localhost:3000/desasignarCurso',{
            carnet: carnet,
            iD: ID,
        });
    }
//------------------GUARADADO DE DATOS SECCION--------------//    
    private storageSeccion: string = "idSeccion";
    SaveSeccionData(Id: string){
        localStorage.setItem(this.storageSeccion, JSON.stringify(Id));
    }
//------------------RECUPERACION DE DATOS SECCION-----------//
    GetSeccionData(){
        let idSeccion = localStorage.getItem(this.storageSeccion);
        console.log(JSON.parse(idSeccion));
        return this.http.post('http://localhost:3000/recuperarSeccion',{
            Id: JSON.parse(idSeccion),
        });
    }
//------------------LISTAR SECCIONES SIN ASIGNAR-------------//
    listarSeccionesSinAsignar(carnet: string){
        return this.http.post('http://localhost:3000/listarSeccionesSinAsignar',{
            carnet: carnet,
        });
    }
//------------------LISTAR SECCIONES ASIGNADAS---------------//
    listarSeccionesAsginar(){
        return this.http.get('http://localhost:3000/listarSeccionesAsginar');
    }
//--------------------ASGINARSE UNA SECCION------------------//
    asignarCurso(idSeccion, carnet){
        return this.http.post('http://localhost:3000/asignarCurso',{
            idSeccion: idSeccion,
            carnet: carnet,
        })
    }

/***********************************************************
************************************************************
*************** METODOS PARA MANEJO DE FOROS ***************
************************************************************
***********************************************************/
//-------------------LISTADO DE FOROS---------------------//
    listarForos(){
        return this.http.get('http://localhost:3000/listarForos');
    }
//-----------------LISTADO DE MENSAJES FORO---------------//
    listarMensajesForos(){
        return this.http.get('http://localhost:3000/listarMensajesForos');
    }
//-----------------CREACION DE UN TEMA FORO---------------//
    agregarTemaForo(nombre: string, id: string){
        return this.http.post('http://localhost:3000/agregarTemaForo', {
            nombre:nombre,
            idSeccion: id,
        });
    }
//-----------------ENVIO DE MENSAJE A UN FORO-------------//
    enviarMensajeForo(idForo: string, mensaje: string, fecha: string, carnet: string){
        return this.http.post('http://localhost:3000/enviarMensajeForo',{
            idForo: idForo,
            mensaje: mensaje,
            fecha: fecha,
            carnet: carnet,
        });
    }

/****************************************************************
*****************************************************************
*************** METODOS PARA MANEJO DE MENSAJERIA ***************
*****************************************************************
****************************************************************/
//--------------------LISTADO DE INSCRITOS---------------------//
    listarInscritos(){
        return this.http.get('http://localhost:3000/listarInscritos');
    }
//-------------------LISTADO DE MENSAJES-----------------------//
    listarMensajes(){
        return this.http.get('http://localhost:3000/listarMensajes');
    }
//---------------------ENVIAR MENSAJES-------------------------//
    enviarMensajePrivado(emisor:string, receptor:string, mensaje:string, fecha:string, nombre:string){
        return this.http.post('http://localhost:3000/enviarMensajePrivado',{
            emisor: emisor,
            receptor: receptor,
            mensaje: mensaje,
            fecha: fecha,
            nombre: nombre
        })
    }


/******************************************************************
*******************************************************************
*************** METODOS PARA MANEJO DE EVALUACIONES ***************
*******************************************************************
******************************************************************/
//--------------------LISTADO DE EVALUACIONES----------------------
    listarEvaluaciones(){
        return this.http.get('http://localhost:3000/listarEvaluaciones');
    }
//----------------------LISTADO DE PREGUNTAS-----------------------
    listarPreguntas(){
        return this.http.get('http://localhost:3000/listarPreguntas');
    }
//----------------------LISTADO DE RESPUESTAS----------------------
    listarRespuestas(){
        return this.http.get('http://localhost:3000/listarRespuestas');
    }
//---------------------AGREGAR NUEVA EVALUACION--------------------
    agregarEvaluacion(nombre: string, idSeccion:string){
        return this.http.post('http://localhost:3000/agregarEvaluacion',{
            nombre: nombre,
            idSeccion: idSeccion,
        })
    }
//--------------------AGREGAR NUEVA PREGUNTA-----------------------
    agregarPregunta(no, idEvaluacion, enunciado, res, tipo){
        return this.http.post('http://localhost:3000/agregarPregunta',{
            no: no,
            idEvaluacion: idEvaluacion,
            enunciado: enunciado,
            res: res,
            tipo: tipo
        });
    }
//------------------AGREGAR POSIBLE RESPUESTA----------------------
    agregarRespuesta(no, id, res){
        return this.http.post('http://localhost:3000/agregarRespuesta',{
            no: no,
            id: id,
            res: res,
        })
    }
//---------------------SETEAR VALOR ALEATORIO----------------------
    setAleatorioValor(valor: string, id:string){
        console.log("se llego aqui ptm")
        return this.http.post('http://localhost:3000/setAleatorioValor',{
            valor: valor,
            id: id,
        });
    }
//---------------------SETEAR VALOR ALEATORIO----------------------
    setHabilitadoValor(valor: string, id:string){
        return this.http.post('http://localhost:3000/setHabilitadoValor',{
            valor: valor,
            id: id,
        });
    }
//---------------------ACTUALIZAR NOTA DE ESTUDIANTE---------------
    actualizarNota(nota:number, carnet:string, iD: string){
        return this.http.post('http://localhost:3000/actualizarNota',{
            nota:nota,
            carnet:carnet,
            id:iD,
        });
    }
//------------------------LISTAR TODAS LAS NOTAS-------------------
    listarNotas(){
        return this.http.get('http://localhost:3000/listarNotas');
    }

/******************************************************************
*******************************************************************
*************** METODOS PARA MANEJO DE ACTIVIDADES ****************
*******************************************************************
******************************************************************/
//-------------------LISTADO DE ACTIVIDADES----------------------//
    listarActividades(){
        return this.http.get('http://localhost:3000/listarActividades');
    }
//--------------LISTADO DE RESPUESTA DE ACTIVIDADES--------------//
    listarRespuestasActividades(){
        console.log("listandoRespuesta")
        return this.http.get('http://localhost:3000/listarRespuestasActividades');
    }
//-----------------AGREGAR NUEVA ACTIVIDAD-----------------------//
    agregarActividad(idSeccion, titulo, enunciado, fecha){
        return this.http.post('http://localhost:3000/agregarActividad',{
            idSeccion: idSeccion,
            titulo: titulo,
            enunciado: enunciado,
            fecha: fecha,
        });
    }
//--------------------AÑADIR RESPUESTA ACTIVIDAD-----------------//
    enviarRespuesta(idActivdad:string, carnet: string, respuesta: string, fecha: string){
        console.log("se llego aqui uwu");
        return this.http.post('http://localhost:3000/agregarActividadRespuesta',{
            idActivdad: idActivdad,
            carnet: carnet,
            respuesta: respuesta,
            fecha: fecha
        });
    }
//----------------------ACTUALIZAR NOTA--------------------------//
    actualizarNotaActividad(nota:string, id:string, carnet:string){
        return this.http.post('http://localhost:3000/actualizarNotaActividad',{
            nota: nota,
            id: id,
            carnet: carnet,
        })
    }

/****************************************************************
*****************************************************************
*************** METODOS PARA MANEJO DE TICKETS ******************
*****************************************************************
****************************************************************/
    listarTickets(){
        return this.http.get('http://localhost:3000/listarTickets');
    }
//------------------------AGREGAR TICKET-----------------------//
    agregarTickets(asunto: string, queja: string, carnet: string){
        return this.http.post('http://localhost:3000/agregarTickets',{
            asunto: asunto,
            queja: queja,
            carnet: carnet,
        });
    }
//-----------------------ACTUALIZAR TICKET---------------------//
    acutalizarTickets(estado: string, id:string){
        return this.http.post('http://localhost:3000/acutalizarTickets',{
            estado: estado,
            id: id,
        })
    }
}


