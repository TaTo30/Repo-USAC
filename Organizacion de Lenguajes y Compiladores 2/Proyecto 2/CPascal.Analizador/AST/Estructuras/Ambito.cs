
public class Ambito
{
    public string Nombre{get; set;}
    int correlativo;
    public int Correlativo{
        get { return correlativo++; }
        set { correlativo = value; }
    }
    public Ambito(string Nombre){
        this.Nombre = Nombre;
        this.Correlativo = 0;
    }
}