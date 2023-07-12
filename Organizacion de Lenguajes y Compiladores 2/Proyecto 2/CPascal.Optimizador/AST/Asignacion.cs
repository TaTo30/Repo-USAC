namespace Optimizador
{
    public class Asignacion : IC3D
    {
        public enum Tipo
        {
            EXEX,
            EXOP,
            EXAC,
            ACEX,
            ACOP
        }
        public object Asignado {get; set;}
        public object Resultado {get; set;}
        public Tipo TipoAsignacion {get; set;}
        public int Linea {get; set;}
        public Asignacion(object Asignado, object resultado, Tipo tipo, int Linea){
            this.Asignado = Asignado;
            this.Resultado = resultado;
            this.TipoAsignacion = tipo;
            this.Linea = Linea;
        }
        public string ParseString(){
            switch (this.TipoAsignacion)
            {
                case Tipo.EXEX:
                    return $"{(this.Asignado).ToString()} = {((Expresion)this.Resultado).ToString()};";
                case Tipo.EXAC:
                    return $"{(this.Asignado).ToString()} = {((Acceso)this.Resultado).ToString()};";
                case Tipo.EXOP:
                    return $"{(this.Asignado).ToString()} = {((Operacion)this.Resultado).ToString()};";
                case Tipo.ACEX:
                    return $"{((Acceso)this.Asignado).ToString()} = {((Expresion)this.Resultado).ToString()};";
                default:
                    return $"{((Acceso)this.Asignado).ToString()} = {((Operacion)this.Resultado).ToString()};";
            }
        }
    }
}