using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1
{
    class Estado
    {
        public string nombre;
        public Estado siguientePrimero;
        public string transicionPrimero;
        public Estado siguienteSegundo;
        public string transicionSegundo;
        public int noEstado;

        public Estado()
        {
            this.nombre = "";
            this.siguientePrimero = null;
            this.transicionPrimero = "";
            this.siguienteSegundo = null;
            this.transicionSegundo = "";
            this.noEstado = 0;
        }
    }
}
