class Simbolo
{
    public enum Tipo
    {
        //TIPOS VARIABLES
        STRING,
        INTEGER,
        REAL,
        BOOLEAN,
        //OTROS TIPOS
        IOBJECT,
        IARRAY,
        TYPE,
        OBJECT,
        ARRAY,
        VOID,
        ERROR
    }
    private Tipo tipo;
    private string id;
    private bool constante = false;
    private string entorno;
    public object valor;

    public Simbolo(string id, object valor, Tipo tipo, string entorno, bool constante = false){
        this.id = id;
        this.valor = valor;
        this.tipo = tipo;
        this.entorno = entorno;
        this.constante = constante;

    }


    public void SetValor(object valor){
        this.valor = valor;
    }
    public object GetValor(){
        return valor;
    }
    public string GetId(){
        return id;
    }
    public Tipo GetTipo(){
        return tipo;
    }
    public bool GetConst(){
        return constante;
    }
    public string GetEnv(){
        return this.entorno;
    }
}