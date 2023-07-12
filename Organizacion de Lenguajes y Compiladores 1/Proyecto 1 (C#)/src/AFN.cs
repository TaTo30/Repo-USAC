using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1
{
    class AFN
    {

        public Estado inicio;
        public Tipo tipoNodo;
        public Estructura estructura;
        public Estado final;
        //public List<Estado> ListaEstados = new List<Estado>();
        
        public enum Tipo
        {
            OPERADOR,
            OPERADORUNITARIO,
            OPERANDO
        }

        public enum Estructura
        {
            CONCATENACION,
            DISYUNCION,
            KLEENE,
            POSITIVO,
            INTERROGATIVO,
            HOJA
        }
        public AFN(Tipo tipo, Estructura estructura)
        {
            inicio = null;
            final = null;
            this.tipoNodo = tipo;
            this.estructura = estructura;
        }
        public AFN(string transicion, int a, Tipo tipo, Estructura estructura, List<Estado> listado)
        {
            this.tipoNodo = tipo;
            this.estructura = estructura;
            Estado auxiliar1 = new Estado();
            Estado auxiliar2 = new Estado();
            auxiliar1.noEstado = a;
            auxiliar2.noEstado = a + 1;
            auxiliar1.nombre = "S"+ a;
            auxiliar1.siguientePrimero = auxiliar2;
            auxiliar1.transicionPrimero = transicion;
            auxiliar2.nombre = "S" + (a + 1);
            inicio = auxiliar1;
            final = auxiliar2;
            listado.Add(auxiliar1);
            listado.Add(auxiliar2);
        }

        public void Concatenar(AFN afn1, AFN afn2, int a)
        {            
            afn1.final.siguientePrimero = afn2.inicio;
            inicio = afn1.inicio;
            final = afn2.final;
        }

        public void Disyuncion(AFN afn1, AFN afn2, int a, List<Estado> listado)
        {
            Estado auxiliar1 = new Estado();
            Estado auxiliar2 = new Estado();
            auxiliar1.noEstado = a;
            auxiliar2.noEstado = a + 1;
            auxiliar1.nombre = "S" + a;
            auxiliar2.nombre = "S" + (a + 1);
            auxiliar1.siguientePrimero = afn1.inicio;
            auxiliar1.siguienteSegundo = afn2.inicio;
            afn1.final.siguientePrimero = auxiliar2;
            afn2.final.siguientePrimero = auxiliar2;
            inicio = auxiliar1;
            final = auxiliar2;
            listado.Add(auxiliar1);
            listado.Add(auxiliar2);
        }

        public void Indefinido(AFN afn, int a, List<Estado> listado)
        {
            Estado auxiliar1 = new Estado();
            Estado auxiliar2 = new Estado();
            auxiliar1.noEstado = a;
            auxiliar2.noEstado = a + 1;
            auxiliar1.nombre = "S" + a;
            auxiliar2.nombre = "S" + (a + 1);
            auxiliar1.siguientePrimero = afn.inicio;
            auxiliar1.siguienteSegundo = auxiliar2;
            afn.final.siguientePrimero = auxiliar2;            
            inicio = auxiliar1;
            final = auxiliar2;
            listado.Add(auxiliar1);
            listado.Add(auxiliar2);
        }

        public void Kleene(AFN afn, int a, List<Estado> listado)
        {
            Estado auxiliar1 = new Estado();
            Estado auxiliar2 = new Estado();
            auxiliar1.noEstado = a;
            auxiliar2.noEstado = a + 1;
            auxiliar1.nombre = "S" + a;
            auxiliar2.nombre = "S" + (a + 1);
            auxiliar1.siguientePrimero = afn.inicio;
            auxiliar1.siguienteSegundo = auxiliar2;
            afn.final.siguientePrimero = auxiliar2;
            afn.final.siguienteSegundo = afn.inicio;
            inicio = auxiliar1;
            final = auxiliar2;
            listado.Add(auxiliar1);
            listado.Add(auxiliar2);
        }

        public void Positiva(AFN afn, int a, List<Estado> listado)
        {
            Estado auxiliar1 = new Estado();
            Estado auxiliar2 = new Estado();
            auxiliar1.nombre = "S" + a;
            auxiliar2.nombre = "S" + (a + 1);
            auxiliar1.noEstado = a;
            auxiliar2.noEstado = a + 1;
            auxiliar1.siguientePrimero = afn.inicio;            
            afn.final.siguientePrimero = auxiliar2;
            afn.final.siguienteSegundo = afn.inicio;
            inicio = auxiliar1;
            final = auxiliar2;
            listado.Add(auxiliar1);
            listado.Add(auxiliar2);
        }


        public Estado getEstadoInicial()
        {
            return inicio;
        }

        public Estado getEstadoFinal()
        {
            return final;
        }
    }
}
