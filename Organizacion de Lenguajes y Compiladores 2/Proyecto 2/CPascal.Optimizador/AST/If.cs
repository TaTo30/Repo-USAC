
namespace Optimizador
{
    public class If : IC3D
    {
        public Condicion Condicion {get; set;}
        public string Goto {get; set;}
        public int Linea {get; set;}
        public If(Condicion condicion, string Goto, int Linea){
            this.Condicion = condicion;
            this.Goto = Goto;
            this.Linea = Linea;
        }
        public string ParseString(){
            return $"if ({this.Condicion.ToString()}) goto {this.Goto};";
        }
    }
}