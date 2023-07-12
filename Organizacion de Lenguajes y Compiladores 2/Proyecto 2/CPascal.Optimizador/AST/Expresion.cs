namespace Optimizador
{
    public class Expresion
    {
        public enum Tipo 
        {
            IDENTIFICADOR,
            NUMERO
        }
        public Tipo TipoValor {get; set;}
        public object Valor {get; set;}
        public Expresion(object valor, Tipo tipo){
            this.Valor = valor;
            this.TipoValor = tipo;
        }
        public override string ToString()
        {
            return Valor.ToString();
        }
    }
}