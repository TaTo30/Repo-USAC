program accesos;

type 
codigos = array[1..4] of integer;

const TOPE = 4;

// crear un objecto curso
type
curso = object
var codigo:integer;
var nombre:string;
var nota:integer;
var creditos:integer;
end;

// vector de cursos
type 
cursos = array[1..4] of curso;

// crear un objecto estudiante
type 
estudiante=object
var nombre:string;
var carnet:string;
var lista_cursos:cursos;
end;

// vector estudiantes
type
estudiantes = array[1..4] of estudiante;


function get_curso(codigo_curso:integer):curso;
    var retorno:curso;
    begin
    //****** USO DE CASE
    case codigo_curso of
        722:
            begin
                retorno.codigo := codigo_curso;
                retorno.nombre := 'Teoria de sistemas';
                retorno.nota := 90;
                retorno.creditos := 5;
                get_curso:= retorno;
                
            end;
        781:
            begin
                retorno.codigo := codigo_curso;
                retorno.nombre := 'Organizacion de lenguajes y compiladores 2';
                retorno.nota := 80;
                retorno.creditos := 5;
                get_curso:= retorno;
            end;
        778:
            begin
                retorno.codigo := codigo_curso;
                retorno.nombre := 'Arquitectura computadoras y ensambladores 1';
                retorno.nota := 85;
                retorno.creditos := 5;
                get_curso:= retorno;
            end;
        773:
            begin
                retorno.codigo := codigo_curso;
                retorno.nombre := 'Manejo e implementacion de archivos';
                retorno.nota := 61;
                retorno.creditos := 4;
                get_curso:= retorno;
            end;
    end;
end;


   
function get_cursos():cursos;
    var lista_cursos:cursos;
    lista_codigos:codigos;
    var i:integer;
    begin
        // leno el arreglo de codigos
        lista_codigos[1]:=781;
        lista_codigos[2]:=722;
        lista_codigos[3]:=778;
        lista_codigos[4]:=773;
        i:=1;
        // ******* Uso del while do, while
        while i <= TOPE do
        begin
            lista_cursos[i] := get_curso(lista_codigos[i]);
            i:= i+1;
        end;
    exit(lista_cursos);
    writeln('Mensaje dentro de la funcion get_cursos, si imprime esto no se maneja bien el exit');
end;

function get_estudiante(posicion:integer):estudiante;
var retorno:estudiante;
begin
    
    // ***** Uso del If then else 

    if posicion = 1 then 
        begin
            retorno.nombre := 'Erik Flores';
            retorno.carnet := '201712345';
            retorno.lista_cursos := get_cursos();
            get_estudiante:= retorno;
        end
    else if posicion = 2 then
        begin
            retorno.nombre := 'Manuel Miranda';
            retorno.carnet := '2020123456';
            retorno.lista_cursos := get_cursos();
            get_estudiante:= retorno;
        end
    else if posicion = 3 then 
        begin
            retorno.nombre := 'Gerardo Diaz';
            retorno.carnet := '201755443';
            retorno.lista_cursos := get_cursos();
            get_estudiante:= retorno;
        end
    else
        begin
            retorno.nombre := 'Angel Asturias';
            retorno.carnet := '2018554433';
            retorno.lista_cursos := get_cursos();
            get_estudiante:= retorno;
        end;

end;


procedure imprimir_datos_cursos(entrada:cursos);
var i:integer;
begin
    writeln('cursos');
    // ****** USO DEL FOR ascendente
    for i:=1 to TOPE do
    begin
        writeln('----------');
        writeln('Codigo: ',entrada[i].codigo);
        writeln('Nombre: ',entrada[i].nombre);
        writeln('Nota: ',entrada[i].nota);
        writeln('Creditos: ',entrada[i].creditos);
    end;
    // tabla de simbolos del procedimiento imprimir_datos_cursos
    //graficar_ts();
    exit();
    writeln('Mensaje dentro del procedimiento imprimir datos, si imprime esto no se maneja bien el exit');
end;

procedure imprimir_datos_estudiante(entrada:estudiante);
var mensaje:string;
begin
    mensaje:='Estudiante reconocido';
    writeln('***', mensaje, '***');
    writeln('Nombre del Estudiante: ',entrada.nombre);
    writeln('Carnet del estudiante: ',entrada.carnet);
    //-- graficar_ts();
    imprimir_datos_cursos(entrada.lista_cursos);
end;
    

var contador:integer;
var listado_general: estudiantes;


begin

    contador:=1;

    // ******* Uso del repeat
    repeat
        listado_general[contador]:= get_estudiante(contador);
        contador := contador +1;
    until (contador = 5);
    
    writeln('***** Imprimiendo datos del estudiante******');

    // ******** Uso del for descendente
    for contador:= 4 downto 1 do 
    begin
        imprimir_datos_estudiante(listado_general[contador]);
    end;

    // tabla de simbolos global
    //graficar_ts();
end.