program OrdenamientoRapido;
type
    v_ = array [0..6] of integer;
var
	v : array[0 .. 6] of integer;


(* paso el arreglo por referencia *)
procedure quick_sort(a : v_; izq, der : integer);
var
	pivote	: integer;
	i		: integer;		(* i realiza la búsqueda de izquierda a derecha *)
	d		: integer;		(* d realiza la búsqueda de derecha a izquierda *)
	aux		: integer;
begin
    pivote := a[izq];
    i := izq;
    d := der;
	(* WHILEs anidados *)
	while i < d do
	begin
		(* Whiles de una sentencia. no necesita begind/end *)
		while (a[i] <= pivote) AND (i < d) do i := i+1;
		WHILE a[D] > pivote DO d := d-1;

		(* parentesis innecesarions *)
		if ((((i < d)))) then		(* sino se han cruzado *)
		begin
			aux := a[i];				(* los intercambia *)
			a[i] := a[d];
			a[D] := aux;
		end;
	end;


	A[izq] := A[D];					(* se colocal el pivote en su lugar de forma que tendremos *)
	A[D] := pivote;				(* los menores a la izquierda y los mayores a su derecha *)

	(* IFs sin begin/end *)
	if izq < d-1 then quick_sort(A, izq, d-1);
	if d+1 < der then quick_sort(A, d+1, der);
end;


(* paso por valor el arreglo :O *)
procedure imprimirArreglo(arrayValor : v_);
var 
	indiceArregloLocal : integer = 12345; (* asignación intencional *)
begin
	for indiceArregloLocal := 0 to 5 do
	begin
		if indiceArregloLocal < 5 then
			wRite(arrayValor[indiceArregloLocal], ', ')
		else
			wRiteLn(arrayValor[indiceArregloLocal]);
	end;
end;

begin
	v[0] := 40;
	v[1] := 21;
	v[2] := 1;
	v[3] := 3;
	v[4] := 12;
	v[5] := 4;

	wRiteLn('Antes del sort: ');
	imprimirArreglo(v);
	quick_sort(v, 0, 5);
	wRiteLn('Despues del sort: ');
	imprimirArreglo(v);
end.