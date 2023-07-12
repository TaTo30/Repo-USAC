var mysql = require('mysql'); //CARGA EL MODULO MYSQL
const express = require('express'); //CARGA EL MODULO EXPRESS
const app = express();//CREAR UNA INSTANCIA DE APLICACION EXPRESS
const bodyParser = require('body-parser');//BODY PARSER LEERA EL BODY DE LOS REQUEST
const fs = require('fs');

// recibir datos en formato json
app.use(bodyParser.json());

app.use(function (req, res, next) {
    res.header("Access-Control-Allow-Origin", "*");
    res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    next();
  });

//getAllEvaluaciones

//CONECTAR A BASE DE DATOS
const conexion = mysql.createConnection({
	host: "localhost",
	user: "root",
	password: "1234",
	database: "pvibd"
});




/*****************************************************************
******************QUERIES DE MANEJO DE USUARIOS*******************
*****************************************************************/
//---------------------REGISTRO------------------
app.post('/registrar', (request, response)=>{
	var nombre = request.body.nombre;
	var apellido = request.body.apellido;
	var	carnet = request.body.carnet;
	var email = request.body.email;
	var pass = request.body.pass;
	var miQuery = "insert into usuario(Nombre, Apellido, Carnet, Email, pass) VALUES("+"\'"+nombre+"\', \'"+apellido+"\',"+carnet+", \'"+email+"\', \'"+pass+"\'"+");";
    console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		} else{
			response.send("0");
		}
	});
});
//--------------------LOGEAR--------------------
app.post('/logear', (request, response) =>{
	var carnet = request.body.carnet;
	var pass = request.body.pass;
	var MyQuery = "select exists(select 1 from auxiliar A, usuario B where A.carnet = B.carnet and B.pass = \'"+pass+"\') as cod;";
	var MyQuery2 = "select exists(select 1 from usuario where carnet = "+carnet+" and pass = \'"+pass+"\') as cod2;";
	console.log(MyQuery);
	console.log(MyQuery2);
    conexion.query(MyQuery, function (err, result){
    	if (err) {
    		throw err;
    	}else{
    		if (result[0].cod == 1) {
    			response.send("1");
    		}else{
    			conexion.query(MyQuery2, function (err, result){
    				if (err) {
    					throw err;
    				} else{
    					if (result[0].cod2 == 1) {
    						response.send("0");
    					} else{
    						response.send("2");
    					}
    				}
    			});
    		}
    	}
    });
});
//-------------------RECUPERAR DATOS------------------
app.post('/recuperarUsuario', (request, response) => {
	var Carnet = request.body.carnet;
	var MyQuery = "select * from usuario where carnet= "+Carnet+";";
	console.log(MyQuery);
	conexion.query(MyQuery, function (err, result) {
		if (err) {
			throw err;
		} else{
			response.send(result);
		}
	});

});
//--------------------LISTAR USUARIOS------------------
app.get('/listarUsuarios', (request, response)=>{
	var MyQuery = "SELECT * FROM usuario";
	console.log(MyQuery);
	conexion.query(MyQuery, function (err, result){
    	if (err) {
        	throw err;
    	}else{
        	console.log(result);
        	response.send(result);
    	}
	});
});
//-------------------LISTAR AUXILIARES------------------
app.get('/listarAuxiliares', (request, response) =>{
	var MyQuery ="SELECT * FROM auxiliar A, usuario B WHERE A.carnet = B.carnet";
	console.log(MyQuery);
	conexion.query(MyQuery, function (err, result){
		if (err) {
			throw err;
		} else{
			response.send(result);
		}
	});
});
//--------------------AGREGAR NUEVO AUXILIAR------------
app.post('/agregarAuxiliar', (request, response) => {
	var carnet = request.body.carnet;
	var MyQuery = "INSERT INTO auxiliar(carnet) VALUES("+carnet+")";
    conexion.query(MyQuery, function (err, result){
    	if (err) {
    		throw err;
    	} else{
    		response.send(result);
    	}
    });
});
//---------------------ELIMINAR UN AUXILIAR-------------
app.post('/eliminarAuxiliar', (request, response)=>{
	var carnet =request.body.carnet;
	var miQuery = "delete from auxiliar where carnet = "+carnet;
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		} else{
			response.send(result);
		}
	})
});
//------------------ACTUALIZAR AUXILIAR------------------
app.post('/editarAuxiliar', (request, response) => {
	var carnet = request.body.carnet;
	var	nombre = request.body.nombre;
	var Apellido = request.body.apellido;
	var email = request.body.email;
	var MyQuery = "UPDATE usuario SET nombre = \'"+nombre+"\', apellido = \'"+Apellido+"\', email = \'"+email+"\' WHERE carnet="+carnet+"; ";
	conexion.query(MyQuery, function (err, result){
		if (err) {
			throw err;
		} else{
			response.send(result);
		}
	});

});




/*****************************************************************
*******************QUERIES DE MANEJO DE CURSOS********************
*****************************************************************/
//-------------------------AGREGAR CURSO------------------------//
app.post('/agregarCurso',  (request, response) =>{
	var codigo = request.body.codigo;
	var nombre = request.body.nombre;
	var MyQuery = "INSERT INTO Curso(IdCurso, Nombre) VALUES ("+codigo+", \'"+nombre+"\')";
	console.log(MyQuery);
    conexion.query(MyQuery, function (err, result){
    	if (err) {
    		throw err;
    	} else{
    		response.send("0");
    	}
    });
});
//------------------------LISTAR CURSOS-------------------------//
app.get('/listarCursos', (request, response) => {
	var MyQuery = "SELECT * FROM Curso";
	console.log(MyQuery);
	conexion.query(MyQuery, function (err, result){
		if (err) {
			throw err;
		} else{
			response.send(result);
		}
	});
});
//-----------------------EDITAR CURSO---------------------------//
app.post('/editarCurso', (request, response)=>{
	var codigo = request.body.codigo;
	var nombre = request.body.nombre;	
	var MyQuery = "UPDATE Curso SET nombre = \'"+nombre+"\' WHERE idCurso="+codigo;
	console.log(MyQuery);
    conexion.query(MyQuery, function (err, result){
    	if (err) {
    		throw err;
    	} else{    		
    		response.send(result);
       	}
    });
});
//-------------------ELIMINAR CURSO----------------------------//
app.post('/eliminarCurso', (request, response)=>{
	var codigo = request.body.codigo;
	var MyQuery="delete from curso where idCurso ="+codigo;
	conexion.query(MyQuery, function (err, result){
		if (err) {
			throw err;
		}else{
			response.send(result);
		}
	});	
});

/********************************************************************
*******************QUERIES DE MANEJO DE SEECIONES********************
********************************************************************/
//------------------AGREGAR SECCION---------------------------//
app.post('/agregarSeccion',  (request, response) =>{
	var codigo = request.body.codigo;
	var horario = request.body.horario;
	var seccion = request.body.seccion;
	var	semestre = request.body.semestre;
	var ano = request.body.ano;
	var sumaRepetidos = 0;
	var MyQuery = "INSERT INTO seccion(idCurso, horario, seccion, semestre, ciclo) VALUES ("+codigo+", \'"+horario+"\', \'"+seccion+"\', \'"+semestre+"\', \'"+ano+"\')";
    var MyQuery2 = "SELECT * FROM seccion WHERE idCurso ="+codigo;
    conexion.query(MyQuery2, function (err, result){
    	if (err) {
    		throw err;
    	}else{
    		for (var i = 0; i < result.length; i++) {
    			if (result[i].seccion == seccion && result[i].semestre == semestre && result[i].ciclo == ano) {
                    sumaRepetidos += 1;
    			}
    		};
    		console.log(sumaRepetidos);
    		if (sumaRepetidos == 0) {
                conexion.query(MyQuery, function (err, result){
    	            if (err) {
    		            throw err;
    	            } else{
    		            response.send(result);
    	            }
                });
    		} else{
    			response.send("0");
    		}
    	}
    });
});
//-----------------LISTAR SECCIONES---------------------------//
app.get('/listarSecciones', (request, response) => {
	var MyQuery = "select * from seccion A, curso B where A.idCurso=B.idCurso";
	conexion.query(MyQuery, function (err, result){
		if (err) {
			throw err;
		} else{
			response.send(result);
		}
	});
});
//----------------ELIMINAR SECCIONES---------------------------//
app.post('/eliminarSeccion', (request, response) =>{
	var codigo = request.body.codigo;
	var MyQuery = "DELETE FROM seccion WHERE idSeccion = "+codigo;
    conexion.query(MyQuery, function (err, result){
    	if (err) {
    		throw err;
    	} else{
    		response.send(result);
    	}
    });
});
//----------------LISTAR SECCIONES CON AUXILIAR------------------//
app.get('/listarSeccionesAuxiliar', (request, response) =>{
	var MyQuery = "select * from seccion_Auxiliar A, seccion B, curso C where A.idSeccion = B.idSeccion and B.idCurso=C.idCurso;";
	console.log(MyQuery);
	conexion.query(MyQuery, function (err, result){
		if (err) {
			throw err;
		} else{
			response.send(result);
		}
	});
});
//------------------LISTAR SECCIONES SIN AUXILIAR---------------//
app.get('/listarSeccionesSinAuxiliar', (request, response) => {
	var MyQuery = "Select * from seccion, curso where seccion.idCurso = curso.idCurso and not exists (select 1 from seccion_auxiliar where seccion.idSeccion = seccion_auxiliar.idSeccion);";
	console.log(MyQuery);
	conexion.query(MyQuery, function (err, result){
		if (err) {
			throw err;
		} else{
			response.send(result);
		}
	})
})
//---------------------AGREGAR AUXILIAR A SECCION---------------//
app.post('/agregarAuxiliarSeccion', (request, response) => {
	var Id = request.body.ID;
	var carnet = request.body.carnet;
	var MyQuery = "insert into seccion_Auxiliar(carnet, idSeccion) VALUES ("+carnet+", "+Id+")";
	conexion.query(MyQuery, function (err, result){
		if (err) {
			throw err;
		} else{
			response.send(result);
		}
	})
});
//---------------------DESASIGNAR AUXILIAR DE SECCION-----------//
app.post('/desasignarCurso', (request, response) => {
	var carnet = request.body.carnet;
	var idSeccion = request.body.iD;
	var miQuery = "delete from seccion_Auxiliar where carnet="+carnet+" and idSeccion = "+idSeccion;
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		} else{
			response.send(result)
		}
	})
});
//----------------------RECUPERAR DATOS DE SECCION---------------//
app.post('/recuperarSeccion', (request, response) => {
	var Id = request.body.Id;
	var miQuery = "select * from seccion, curso where seccion.idCurso=curso.idCurso and idSeccion = "+ Id;
	console.log(miQuery);
	conexion.query(miQuery, function (err, result) {
		if (err) {
			throw err;
		}else{
			response.send(result);
		}
	})
});
//---------------LISTADO DE SECCIONES SIN ASIGNAR---------------//
app.post('/listarSeccionesSinAsignar', (request, response) => {
	var carnet = request.body.carnet;
	var miQuery = "select * from seccion, curso where seccion.idCurso = curso.idCurso and not exists (select 1 from asignacion where asignacion.idSeccion = seccion.idSeccion and asignacion.carnet="+carnet+")";
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		}else{
			response.send(result);
		}
	})
});
app.get('/listarSeccionesAsginar', (request, response) => {
	var miQuery = "select * from seccion A, curso B, asignacion C where A.idCurso = B.idCurso and A.idSeccion = C.idSeccion";
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		}else{
			response.send(result);
		}
	})
})
//-----------------ASGINAR UN ESTUDIANTE A SECCION-------------//
app.post('/asignarCurso', (request, response) => {
	var idSeccion = request.body.idSeccion;
	var carnet = request.body.carnet;
	var miQuery = "insert into asignacion(carnet, idSeccion) values ("+carnet+", "+idSeccion+")";
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		}else{
			response.send(result);
		}
	})
})


/********************************************************************
*******************QUERIES DE MANEJO DE FOROS************************
********************************************************************/
//--------------------LISTADO DE FOROS CREADOS---------------------//
app.get('/listarForos', (request, response) => {
	var miQuery = "select * from foro";
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		} else{
			response.send(result);
		}
	});
});
//---------------------LISTADO DE MENSAJES DE FORO-----------------//
app.get('/listarMensajesForos', (request, response) => {
	var miQuery = "select * from mensaje_foro A, usuario B where A.carnet = B.carnet";
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		} else{
			response.send(result);
		}
	});
});
//---------------------AGREGAR UN NUEVO TEMA-FORMO----------------//
app.post('/agregarTemaForo', (request, response) => {
	var nombre = request.body.nombre;
	var idSeccion = request.body.idSeccion;
	var miQuery = "insert into foro(nombre, idSeccion) VALUES (\'"+nombre+"\', "+idSeccion+")";
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		}else{
			response.send(result);
		}
	});
});
app.post('/enviarMensajeForo', (request, response) => {
	var idForo = request.body.idForo;
	var mensaje = request.body.mensaje;
	var fecha = request.body.fecha;
	var carnet = request.body.carnet;
	var miQuery = "Insert into mensaje_foro (idForo, mensaje, fecha, carnet) VALUES ("+idForo+", \'"+mensaje+"\', \'"+fecha+"\', "+carnet+")";
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		}else{
			response.send(result);
		}
	});
});



/************************************************************************
*******************QUERIES DE MANEJO DE MENSAJES*************************
************************************************************************/
//--------------------LISTADO DE INSCRITOS CREADOS---------------------//
app.get('/listarInscritos', (request, response) => {
	var miQuery = "select * from usuario A, asignacion B where A.carnet = B.carnet";
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		} else{
			response.send(result);
		}
	});
});
//-------------------------LISTADO DE MENSAJES-------------------------//
app.get('/listarMensajes', (request, response) => {
	var miQuery = "select * from mensajes";
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		} else{
			response.send(result);
		}
	})
});
//---------------------------ENVIAR MENSAJE PRIVADO---------------------//
app.post('/enviarMensajePrivado', (request, response) => {
	var emisor = request.body.emisor;
	var receptor = request.body.receptor;
	var mensaje = request.body.mensaje;
	var fecha = request.body.fecha;
	var nombre = request.body.nombre;
	var miQuery = "insert into mensajes (emisor, receptor, mensaje, fecha, nombre) VALUES ("+emisor+", "+receptor+", \'"+mensaje+"\', \'"+fecha+"\', \'"+nombre+"\')"
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		} else{
			response.send(result);
		}
	});
});

/***************************************************************************
********************QUERIES DE MANEJO DE EVALUACIONES***********************
***************************************************************************/
//-------------------------LISTADO DE EVALUACIONES------------------------//
app.get('/listarEvaluaciones', (request, response) => {
	var miQuery = "select * from evaluacion";
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		} else{
			response.send(result);
		}
	})
});
//-------------------------LISTADO DE EVALUACIONES------------------------//
app.get('/listarPreguntas', (request, response) => {
	var miQuery = "select * from preguntas_evaluacion";
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		} else{
			response.send(result);
		}
	})
});
//-------------------------LISTADO DE EVALUACIONES------------------------//
app.get('/listarRespuestas', (request, response) => {
	var miQuery = "select * from respuestas_evaluacion";
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		} else{
			response.send(result);
		}
	})
});
//--------------------------AGREGAR EVALUACIONES-------------------------//
app.post('/agregarEvaluacion', (request, response) => {
	var nombre = request.body.nombre;
	var idSeccion = request.body.idSeccion;
	var miQuery = "insert into evaluacion (idSeccion, nombre, habilitado, aleatorio) values ("+idSeccion+", \'"+nombre+"\', 0,0)";
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		}else{
			response.send(result);
		}
	});
});
//----------------------------AGREGAR PREGUNTA-------------------------//
app.post('/agregarPregunta', (request, response) => {
	var no = request.body.no;
	var idEvaluacion = request.body.idEvaluacion;
	var enunciado = request.body.enunciado;
	var res = request.body.res;
	var tipo = request.body.tipo;
	var miQuery = "insert into preguntas_evaluacion (noPregunta, idEvaluacion, enunciado, respuesta, tipo) values ("+no+", "+idEvaluacion+", \'"+enunciado+"\', \'"+res+"\', \'"+tipo+"\')";
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		}else{
			response.send(result);
		}
	});
});
//-----------------------------AGREGAR RESPUESTA-------------------------//
app.post('/agregarRespuesta', (request, response) => {
	var no = request.body.no;
	var id = request.body.id;
	var res = request.body.res;
	var miQuery = "insert into respuestas_evaluacion (noPregunta, idEvaluacion, respuesta) values ("+no+", "+id+", \'"+res+"\')";
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		}else{
			response.send(result);
		}
	});
});
//-------------------------------SETEAR VALOR ALEATORIO------------------//
app.post('/setAleatorioValor', (request, response) =>{
	var valor = request.body.valor;
	var id = request.body.id;
	var miQuery = "update evaluacion set aleatorio="+valor+" where IdEvaluacion = "+id;
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		}else {
			response.send(result);
		}
	})
});
//--------------------------SETEAR VALOR HABILITADO----------------------//
app.post('/setHabilitadoValor', (request, response) =>{
	var valor = request.body.valor;
	var id = request.body.id;
	var miQuery = "update evaluacion set habilitado="+valor+" where IdEvaluacion = "+id;
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		}else {
			response.send(result);
		}
	})
});
//-----------------------CREAR Y ACTUALIZAR NOTA--------------------------//
app.post('/actualizarNota', (request, response) =>{
	var nota = request.body.nota;
	var carnet = request.body.carnet;
	var id = request.body.id;
	var miQuery = "select exists(select 1 from nota_evaluacion where carnet="+carnet+" and idEvaluacion = "+id+") as Cod";
	var miQuerySecond = "insert into nota_evaluacion(idEvaluacion, carnet, nota) values ("+id+", "+carnet+", "+nota+")";
	var miQueryThird = "update nota_evaluacion set nota = "+nota+" where idEvaluacion = "+id+" and carnet = "+carnet;
	console.log(miQuery);
	console.log(miQuerySecond);
	console.log(miQueryThird);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		}else{
			if (result[0].Cod == 0) {
				conexion.query(miQuerySecond, function (err, result){
					if (err) {
						throw err;
					}else{
						response.send(result);
					}
				});
			}else{
				conexion.query(miQueryThird, function (err, result){
					if (err) {
						throw err;
					}else{
						response.send(result);
					}
				});
			}
		}
	});
});
//---------------------------LISTAR NOTAS---------------------------------//
app.get('/listarNotas', (request, response) => {
	var miQuery = "select * from nota_evaluacion";
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		}else {
			response.send(result);
		}
	})
})

/***************************************************************************
********************QUERIES DE MANEJO DE ACTIVIDADES************************
***************************************************************************/
//-----------------------------LISTAR ACTIVIDADES-------------------------//
app.get('/listarActividades', (request, response) => {
	var miQuery = "select * from actividades";
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		}else{
			response.send(result);
		}
	})
});
//----------------------LISTAR RESPUESTA DE ACTIVIDADES--------------------//
app.get('/listarRespuestasActividades', (request, response) =>{
	var miQuery = "select * from actividades_respuestas A, usuario B where A.carnet = B.carnet";
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		}else{
			response.send(result);
		}
	});
});
//-------------------------AGREGAR NUEVA ACTIVIDAD-------------------------//
app.post('/agregarActividad', (request, response) => {
	var idSeccion = request.body.idSeccion;
	var titulo = request.body.titulo;
	var enunciado = request.body.enunciado;
	var fecha = request.body.fecha;
	var miQuery = "insert into actividades(idSeccion, titulo, enunciado, fecha) values ("+idSeccion+", \'"+titulo+"\', \'"+enunciado+"\', \'"+fecha+"\')";
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		}else{
			response.send(result);
		}
	});
});
app.post('/agregarActividadRespuesta', (request, response) => {
	var idActividad = request.body.idActivdad;
	var carnet = request.body.carnet;
	var respuesta = request.body.respuesta;
	var fecha = request.body.fecha;
	var miQuery = "insert into actividades_respuestas(idActividad, carnet, archivo, fecha, notas) values ("+idActividad+", "+carnet+", \'"+respuesta+"\', \'"+fecha+"\', 0)";
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		}else{
			response.send(result);
		}
	})
});
//---------------------------ACTUALIZAR NOTA--------------------------------//
app.post('/actualizarNotaActividad', (request, response) => {
	var nota = request.body.nota;
	var id = request.body.id;
	var carnet = request.body.carnet;
	var miQuery = "update actividades_respuestas set notas = "+nota+" where idActividad= "+id+" and carnet = "+carnet;
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		}else{
			response.send(result);
		}
	})
})

/*****************************************************************
******************QUERIES DE MANEJO DE TICKETS********************
*****************************************************************/
//-------------------------LISTAR TICKETS-----------------------//
app.get('/listarTickets', (request, response) => {
	var miQuery = "select * from ticket A, usuario B where A.carnet=B.carnet";
	console.log(miQuery);
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		}else{
			response.send(result);
		}
	});
});
//------------------------AGREGAR TICKETS----------------------//
app.post('/agregarTickets', (request, response) => {
	var asunto = request.body.asunto;
	var queja = request.body.queja;
	var carnet = request.body.carnet;
	var miQuery = "insert into ticket(asunto, queja, estado, carnet) values (\'"+asunto+"\', \'"+queja+"\', 'Pendiente', "+carnet+")";
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		}else{
			response.send(result);
		}
	})
});
//------------------------ACTUALIZAR TICKETS-------------------//
app.post('/acutalizarTickets', (request, response) => {
	var estado = request.body.estado;
	var id = request.body.id;
	var miQuery = "update ticket set estado = '"+estado+"' where idTicket="+id;
	conexion.query(miQuery, function (err, result){
		if (err) {
			throw err;
		}else{
			response.send(result);
		}
	})
});




app.listen(3000, () =>{
	console.log("Backend inicializado");
});



