
public class Posicion
{
    public int Linea {get; set;}
    public int Columna {get; set;}
    public int Indice {get; set;}

    public Posicion(int linea, int columna, int indice){
        this.Columna = columna;
        this.Linea = linea;
        this.Indice = indice;
    }

    public Posicion(){
        this.Columna = 0;
        this.Linea = 0;
        this.Indice = 0;
    }
}