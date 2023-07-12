namespace Optimizador
{
    public class Acceso
    {
        public enum Tipo
        {
            STACK,
            HEAP
        }
        public Tipo TipoAcceso {get; set;}
        public Expresion Expresion {get; set;}

        public Acceso(Expresion expresion, Tipo tipo){
            this.Expresion = expresion;
            this.TipoAcceso = tipo;
        }

        public override string ToString()
        {
            return $"{(this.TipoAcceso == Tipo.STACK? "Stack":"Heap")}[{this.Expresion.ToString()}]";
        }
    }    
}