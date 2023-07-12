program arrglos;
{
type
notas = array [0..10] of integer;

type 
universidad = object
var codigo:integer;
var nombre:string;
end;
{
type
curso = object
var codigo:integer;
var nombre:string;
var uni: universidad;
var rangoNotas: notas;
end;*/}
{
type
cursos = array [0..2, 3..5] of universidad;

var miscursos : cursos;}
var nombres : array [0..10] of string;


procedure proc();
var n: array[0..10] of string;
var i: integer;
begin
for i := 0 to 8 do
    n[i] := nombres[i];

for i := 0 to 8 do
    writeln(n[i]);
    
end;


begin  
    nombres[0] := 'nombre1';
    nombres[1] := 'nombre2';
    nombres[2] := 'nombre3';
    nombres[3] := 'nombre4';
    nombres[4] := 'nombre5';
    nombres[5] := 'nombre6';
    nombres[6] := 'nombre7';
    nombres[7] := 'nombre8';
    nombres[8] := 'nombre9';
    proc();
end.