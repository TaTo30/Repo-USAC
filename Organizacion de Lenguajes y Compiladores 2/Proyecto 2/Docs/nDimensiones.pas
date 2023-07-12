
program nDimensiones;

type
    Reporte_ = object
        var vino:string;
        var revision:string;
        var botella:string;
        var rechazos:integer;
    end;
var
    vinos : array[0..4] of String;
    botellas : array[0..3] of String;
    revisiones : array[0..1] of String;
    reporte : array[0..1, 0..4, 0..3] of Reporte_;

procedure crearArray();
var
    i, j, k : integer;
begin
    for i := 0 to 1 do
    begin
        for j := 0 to 4 do
        begin
            for k := 0 to 3 do
            begin
                reporte[i,j,k].vino := vinos[j];
                reporte[i,j,k].revision := revisiones[i];
                reporte[i,j,k].botella := botellas[k];
                reporte[i,j,k].rechazos := ((i)*4*3) + ((j)*4) + (k + 1);
            end;
        end;
    end;
end;

procedure printStruct(reporte : Reporte_);
begin
    writeln(reporte.revision, '  ', reporte.vino, '  ', reporte.botella, '  ', reporte.rechazos);
end;

procedure printArreglo();
var
    i,j,k : integer;
begin
    for i := 0 to 1 do
    begin
        for j := 0 to 4 do
        begin
            for k := 0 to 3 do
            begin
                printStruct(reporte[i,j,k]);
            end;
        end;
    end;
end;


procedure llenarArreglo();
begin
    vinos[0] := 'Blanco'; 
    vinos[1] := 'Tinto'; 
    vinos[2] := 'Jerez'; 
    vinos[3] := 'Oporto'; 
    vinos[4] := 'Rosado';
    botellas[0] := 'Magnum'; 
    botellas[1] := 'DMagnum'; 
    botellas[2] := 'Estandar'; 
    botellas[3] := 'Imperial';
    revisiones[0] := 'Oxigeno'; 
    revisiones[1] := 'Envases';
end;

begin
    llenarArreglo();
    crearArray();
    writeln('***************************************');
    writeln('***************************************');
    printArreglo();
    writeln('***************************************');
end.