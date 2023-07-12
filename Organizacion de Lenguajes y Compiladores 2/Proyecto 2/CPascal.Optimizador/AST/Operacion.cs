namespace Optimizador
{
    public class Operacion
    {
        public enum Tipo
        {
            ADICION,
            SUSTRACCION,
            MULTIPLICACION,
            DIVISION,
            RESTO
        }

        public Tipo TipoOperacion {get; set;}
        public Expresion OperadorIzq {get; set;}
        public Expresion OperadorDer{get; set;}

        public override string ToString()
        {
            switch (this.TipoOperacion)
            {
                case Tipo.ADICION:
                    return $"{this.OperadorIzq.ToString()} + {this.OperadorDer.ToString()}";
                case Tipo.SUSTRACCION:
                    return $"{this.OperadorIzq.ToString()} - {this.OperadorDer.ToString()}";
                case Tipo.MULTIPLICACION:
                    return $"{this.OperadorIzq.ToString()} * {this.OperadorDer.ToString()}";
                case Tipo.DIVISION:
                    return $"{this.OperadorIzq.ToString()} / {this.OperadorDer.ToString()}";
                default:
                    return $"{this.OperadorIzq.ToString()} % {this.OperadorDer.ToString()}";
            }
        }
        public Operacion(Expresion izq, Expresion der, Tipo tipo){
            this.OperadorDer = der;
            this.OperadorIzq = izq;
            this.TipoOperacion = tipo;
        }
    }
}

