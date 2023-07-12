class Objeto : Instruccion
{
    public int Linea {get; set;}
    public int Columna {get; set;}
    public string id;
    private Objeto propiedad;


    public Objeto(string id, Objeto propiedad, int x, int y){
        this.id = id;
        this.propiedad = propiedad;
        this.Linea = x;
        this.Columna = y;
    }

    //LA ULTIMA PROPIEDAD
    public Objeto(string id, int x, int y){
        this.id = id;
        this.propiedad = null;
        this.Linea = x;
        this.Columna = y;
    }

    public void SetChild(Objeto propiedad){
        this.propiedad = propiedad;
    }

    public Objeto GetChild(){
        return this.propiedad;
    }
    public object ejecutar_objeto(Entorno env, bool asignacion = false, object valor = null){
        if (this.propiedad == null)
        {
            if (env.GetTipo(this.id) == Simbolo.Tipo.ERROR)
                throw new SemanticException($"La {this.id} propiedad no existe en el contexto", this.Linea, this.Columna);
            if (asignacion)
            {
                env.SetValor(this.id, valor);
                return 0;
            } else {
                //esta es la ultima propiedad
                return env.GetValor(this.id);
            }            
        } else {
            //esta tiene mas propiedades
            if (env.GetTipo(this.id) != Simbolo.Tipo.ERROR)
            {
                return this.propiedad.ejecutar_objeto((Entorno)env.GetValor(this.id), asignacion, valor);            
            } else {
                throw new SemanticException($"La {this.id} propiedad no existe en el contexto", this.Linea, this.Columna);
            }            
        }
    }
    public object ejecutar(Entorno env){
        return Control.ControlSet.NONE;
    }
}