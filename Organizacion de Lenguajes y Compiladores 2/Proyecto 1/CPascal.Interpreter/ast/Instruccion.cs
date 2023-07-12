
interface Instruccion
{
    int Linea {get; set;}
    int Columna {get; set;}
    object ejecutar(Entorno env);
}