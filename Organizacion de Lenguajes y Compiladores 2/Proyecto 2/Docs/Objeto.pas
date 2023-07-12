program accesos;

type 
universidad = object
var codigo:integer;
var nombre:string;
end;


type
curso = object
var codigo:integer;
var nombre:string;
var nota:integer;
var creditos:integer;
var uni: universidad;
end;


var micurso: curso;
var a, b: integer = 25;
var micurso2: curso;

procedure procedimiento();
    var micurso: curso;
begin
    micurso.nota := a + 75;
    writeln(micurso.nota);
end;


begin
    micurso.uni.nombre := 'USAC';
    micurso.nombre := micurso.uni.nombre;
    micurso.codigo := 10;
    micurso.nota := 10;
    micurso.creditos := 10;
    writeln(micurso.uni.nombre);
    writeln(micurso.nota);
    writeln(micurso.nombre);
    micurso2.nota := a;
    micurso2.codigo := b;
    writeln(micurso2.nota);
    writeln(micurso2.codigo);
    procedimiento();
    writeln(micurso.nota);
end.

{
    T0 = HP
    Heap[HP] = T<EXP RESULT>
    HP = HP + 1
    Heap[HP] = T<EXP RESULT>
    HP = HP + 1
    Heap[HP] = T<EXP RESULT>
    HP = HP + 1
    Heap[HP] = T<EXP RESULT>
    HP = HP + 1
    Stack[0] = T0 

}