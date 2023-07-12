program listaDoble;

type
Points = object
var right : integer;
var left : integer;
end;

type
Node = object
var idx : integer;
var val : integer;
var point : Points;
end;

type
DoubleList = array[1..20] of Node;

var actualDL : DoubleList;
var count : integer = 1;
var first : integer = -1;
var last : integer = -1;

procedure InsertFirst(val : integer);
var root : Node;
var aux : Node;
begin
    if (first <> -1) then
    begin
        aux := actualDL[first];
        
        first := count;

        root.idx := count;
        root.val := val;
        root.point.left := -1;
        root.point.right := aux.idx;

        aux.point.left := root.idx;
        actualDL[aux.idx] := aux;
    end
    else
    begin
        first := count;
        last := first;
        
        root.idx := count;
        root.val := val;
        root.point.left := -1;
        root.point.right := -1;
    end;
    actualDL[count] := root;
    count := count + 1;
end;



procedure PrintListNormal();
var actual : Node;
var i : integer;
begin
    if (first <> -1) then
    begin
        i := first;
        repeat
            actual := actualDL[i];
            write('Valor de nodo: ');
            writeln(actual.val);
            i := actual.point.right;
        until (actual.idx = last);
    end;
end;


begin
    writeln('---Insertando al inicio---');
    InsertFirst(5);
    InsertFirst(7);
    InsertFirst(10);
    PrintListNormal();
    writeln('---Insertando al final---');
end.