using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1
{
    class Transicion
    {
        public Conjunto Inicial;
        public Conjunto Final;
        public string transicion;

        public Transicion(Conjunto salida, Conjunto entrada, string transicion)
        {
            this.Inicial = salida;
            this.Final = entrada;
            this.transicion = transicion;
        }


    }
}
