using System.Collections.Generic;
namespace Optimizador
{
    public class Bitacora
    {
        public string Regla {get; set;}
        public List<IC3D> Eliminados {get; set;}
        public List<IC3D> Agregados {get; set;}
        public int Linea {get; set;}
        public Bitacora(string Regla, int Linea, List<IC3D> Eliminados, List<IC3D> Agregados){
            this.Regla = Regla;
            this.Eliminados = Eliminados;
            this.Agregados = Agregados;
            this.Linea = Linea;
        }
    }
}