namespace Optimizador
{
    public class Print : IC3D
    {
        public string Modo {get; set;}
        public Expresion Expresion {get; set;}
        public int Linea {get; set;}
        public Print(Expresion expresion, string Modo, int Linea){
            this.Expresion = expresion;
            this.Modo = Modo;
            this.Linea = Linea;
        }
        public string ParseString(){
            return $"printf(\"{this.Modo}\", {this.Expresion.ToString()});";
        }
    }
}