namespace Optimizador
{
    public class Goto : IC3D
    {
        public string Etiqueta {get; set;}
        public int Linea {get; set;}
        public Goto(string Goto, int Linea){
            this.Etiqueta = Goto;
            this.Linea = Linea;
        }
        public string ParseString(){
            return $"goto {this.Etiqueta};";
        }
    }
}
