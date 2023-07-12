using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto1
{
    class Expresion
    {        
        private string nombre;
        private string expresion;
        private AFN raizAFN;
        private List<Conjunto> conjuntos;
        private List<string> Terminales;
        private List<Transicion> transiciones;
        private DataGridView TablaTransiciones;
        private string Dot;
        private string image;
        private string AFNDot;
        private string AFNimage;
        


		//CONSTRUCTOR
        public Expresion(string expresion, string nombre)
        {
            this.expresion = expresion;
            this.nombre = nombre;
            this.Dot = "";
            conjuntos = new List<Conjunto>();
            Terminales = new List<string>();
            transiciones = new List<Transicion>();
            Conjunto conjuntoInicial = new Conjunto();
            conjuntoInicial.nombre = "S0";
            conjuntoInicial.Movedura.Add(0);
            conjuntoInicial.Cerradura.Add(0);
            conjuntos.Add(conjuntoInicial);
        }


		//GETTERS Y SETTERS
		public string getExpresion()
        {
            return expresion;
        }
		public void setExpresion(string expresion)
        {
            this.expresion = expresion;
        }
        public string getNombre()
        {
            return nombre;
        }
        public void setNombre(string nombre)
        {
            this.nombre = nombre;
        }

        public void setAFN(AFN afn)
        {
            this.raizAFN = afn;
        }

        public AFN GetAFN()
        {
            return raizAFN;
        }

        public List<string> getList()
        {
            return Terminales;
        }

        public List<Conjunto> GetConjuntos()
        {
            return conjuntos;
        }

        public void SetConjuntos(List<Conjunto> lista)
        {
            this.conjuntos = lista;
        }
        public List<Transicion> GetTransicions()
        {
            return transiciones;
        }
        
        public void SetTablaTransiciones(DataGridView tabla)
        {
            this.TablaTransiciones = tabla;
        }

        public DataGridView GetTablaTransiciones()
        {
            return TablaTransiciones;
        }

        public void SetDot(string path)
        {
            this.Dot = path;
        }

        public string GetDot()
        {
            return Dot;
        }

        public string GetImage()
        {
            return image;
        }

        public void SetImage(string image)
        {
            this.image = image;
        }

        public string GetAFNImage()
        {
            return AFNimage;
        }

        public void SetAFNimage(string image)
        {
            this.AFNimage = image;
        }

        public void SetAFNDot(string path)
        {
            this.AFNDot = path;
        }

        public string GetAFNADot()
        {
            return AFNDot;
        }


    }

    
}
