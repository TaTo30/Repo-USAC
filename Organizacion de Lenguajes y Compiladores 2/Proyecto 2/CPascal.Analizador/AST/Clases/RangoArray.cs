
public class RangoArray
{
    public Expresion minimo {get; set;}
    public Expresion maximo {get; set;}
    public long Lenght {
        get
        {
            return this.maximo.ObtenerValorImplicito() - this.minimo.ObtenerValorImplicito();
        }
    }
    public long Minimo{
        get{
            return this.minimo.ObtenerValorImplicito();
        }
    }

    public RangoArray(Expresion min, Expresion max){
        this.minimo = min;
        this.maximo = max;
    }

}