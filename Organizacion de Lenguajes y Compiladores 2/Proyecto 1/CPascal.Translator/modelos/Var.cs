
public class Var
{
    private string id;
    private string tipo;

    public Var(string id, string tipo){
        this.id = id;
        this.tipo = tipo;
    }

    public void setTipo(string tipo){
        this.tipo = tipo;
    }

    public string GetId(){
        return this.id;
    }

    public string GetTipo(){
        return this.tipo;
    }
}