using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _LFP_Proyecto
{
    class Simbolo
    {
        private string Var;
        private string Valor;
        private Tipo VarTipo;

        public enum Tipo
        {
            VAR_INT,
            VAR_FLOAR,
            VAR_CHAR,
            VAR_STRING,
            VAR_BOOL
        }
        public Simbolo(string nombreVariable, string valorVariable, Tipo tipoVarialbe)
        {
            this.Var = nombreVariable;
            this.Valor = valorVariable;
            this.VarTipo = tipoVarialbe;
        }

        public string getNombreVar()
        {
            return this.Var;
        }

        public string getValorVar()
        {
            return this.Valor;
        }

        public string getTipo()
        {
            return this.VarTipo.ToString();
        }
    }

    
}
