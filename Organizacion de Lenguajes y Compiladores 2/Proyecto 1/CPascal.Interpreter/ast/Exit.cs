
class Exit : Instruccion
{
    public int Linea {get; set;}
    public int Columna {get; set;}
    private Operacion expresion;
    public Exit(Operacion expresion, int x, int y){
        this.Linea = x;
        this.Columna = y;
        this.expresion = expresion;
    }

    public object ejecutar(Entorno env){
        env.NewReplace(new Simbolo("exit", expresion.ejecutar(env), Simbolo.Tipo.TYPE, env.nombre));
        return Control.ControlSet.EXIT;
    }
}