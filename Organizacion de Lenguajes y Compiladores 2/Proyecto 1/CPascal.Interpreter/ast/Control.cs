class Control : Instruccion
{
    public int Linea {get; set;}
    public int Columna {get; set;}
    public enum ControlSet{
        BREAK,
        CONTINUE,
        EXIT,
        NONE
    }

    private ControlSet accion = ControlSet.NONE;

    public Control(ControlSet accion, int x, int y){
        this.accion = accion;
        this.Linea = x;
        this.Columna = y;
    }

    public object ejecutar(Entorno env){
        return this.accion;
    }
}