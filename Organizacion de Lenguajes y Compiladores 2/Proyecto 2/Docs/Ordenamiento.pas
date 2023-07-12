
program Ordenar;

const 
	maximo = 100; 
	maxVal = 30000;

var 
	datos: array[1..maximo] of integer;      
   i: integer;                            
 
procedure swap(var a,b: integer); 
var 
	tmp: integer; 
begin 
	tmp := a; 
   	a := b; 
   	b := tmp; 
end;
 
procedure generaNumeros();               { Genera números aleatorios } 
begin 
	writeln(); 
   	writeln('Generando números...'); 
   	for i := 1 to maximo do 
    	datos[i] := maximo - i * i ; 
end;
 
procedure muestraNumeros();              { Muestra los núms almacenados } 
begin 
	writeln(); 
   	writeln('Los números son...'); 
   	for i := 1 to maximo do 
    	write(datos[i], ' '); 
   	writeln(''); 
end; 
 
procedure Burbuja();                     { Ordena según burbuja } 
var 
	cambiado: boolean; 
begin 
	writeln(''); 
   	writeln('Ordenando mediante burbuja...'); 
   	repeat 
     	cambiado := false;                 	{ No cambia nada aún } 
     	for i := maximo downto 2 do        	{ De final a principio } 
       		if datos[i] < datos[i-1] then   { Si está colocado al revés } 
         	begin 
         		swap(datos[i], datos[i-1]);    { Le da la vuelta } 
         		cambiado := true;              { Y habrá que seguir mirando } 
         	end; 
	until not cambiado;                  { Hasta q nada se haya cambiado } 
 end;
 

procedure Sort(l, r: Integer);         { Esta es la parte recursiva } 
var 
	i, j, x, y: integer; 
begin 
	i := l; j := r;                      { Límites por los lados } 
   	x := datos[(l+r) / 2];             { Centro de la comparaciones } 
   	repeat 
     	while datos[i] < x do i := i + 1;  { Salta los ya colocados } 
     	while x < datos[j] do j := j - 1;  {   en ambos lados } 
     
	 	if i <= j then                     { Si queda alguno sin colocar } 
       	begin 
       		swap(datos[i], datos[j]);  	{ Los cambia de lado } 
       		i := i + 1; 
			j := j - 1;          		{ Y sigue acercándose al centro } 
       	end; 
   until i > j;                         { Hasta que lo pasemos } 
   
   if l < j then Sort(l, j);            { Llamadas recursivas por cada } 
   if i < r then Sort(i, r);            {   lado } 
 end; 
 
procedure QuickSort();                   { Ordena según Quicksort } 
begin
	writeln('');
	writeln('Ordenando mediante QuickSort...'); 
	Sort(1,Maximo);
end;
 
 
 begin 
   generaNumeros(); 
   muestraNumeros(); 
   Burbuja(); 
   muestraNumeros(); 

   generaNumeros(); 
   muestraNumeros(); 
   QuickSort(); 
   muestraNumeros(); 
end.