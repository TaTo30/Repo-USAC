using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1
{
    class Conjunto
    {
        public string nombre;
        public List<int> Movedura;
        public List<int> Cerradura;

        public Conjunto()
        {
            Movedura = new List<int>();
            Cerradura = new List<int>();
            nombre = "";
        }

        
    }
}
