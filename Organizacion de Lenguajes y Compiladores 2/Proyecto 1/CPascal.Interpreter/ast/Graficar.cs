
class Graficar: Instruccion
{
    public int Linea {get; set;}
    public int Columna {get; set;}
    public Graficar(){ }
    public object ejecutar(Entorno env){
        Graficador gast = new Graficador(env);
        gast.Print(Graficador.Graph.TS);
        return Control.ControlSet.NONE;
    }
}