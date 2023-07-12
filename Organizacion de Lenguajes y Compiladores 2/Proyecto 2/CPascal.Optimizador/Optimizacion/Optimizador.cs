using System.Collections.Generic;
using System.Linq;
using System;

namespace Optimizador
{
    public class Optimizar
    {
        public List<Bitacora> Reporte {get; set;}
        public Optimizar(List<IC3D> codigo){
            this.Reporte = new List<Bitacora>();
            Regla1(codigo);
            Regla2(codigo);
            Regla34(codigo);
            Regla5(codigo);
            Regla6789(codigo);
            Regla10111213(codigo);
            Regla141516(codigo);
        }
        #region ELIMINACION DE CODIGO MUERTO
        private List<IC3D> Regla1(List<IC3D> codigo){
            int indice = -1;            
            var gotoInfo = FindNextGotoOf((indice + 1), codigo);

            while (gotoInfo.index != -1)
            {
                //AHORA BUSCAMOS LA ETIQUETA
                var etiquetaInfo = IndexOfEtiqueta(gotoInfo.index, gotoInfo.ins.Etiqueta, codigo);
                if (etiquetaInfo.index - gotoInfo.index == 1 || etiquetaInfo.index == -1) //SI SON CONTINUOS O LA ETIQUETA NO ESTA DESPUES DEL GOTO NO SE MATCHEA
                    goto REASIGNAR;                
                if (!MatchBetween(gotoInfo.index, etiquetaInfo.index, typeof(Etiqueta), codigo))
                {
                    //No hay match, podemos eliminar el codigo muerto
                    var eliminado = ExtractC3D(gotoInfo.index, etiquetaInfo.index, codigo);
                    var bitacora = new Bitacora(nameof(Regla1), gotoInfo.ins.Linea, eliminado, new List<IC3D>());
                }
                REASIGNAR:
                gotoInfo = FindNextGotoOf(gotoInfo.index + 1, codigo);
            }
            return codigo;
        }
        private List<IC3D> Regla2(List<IC3D> codigo){
            int indice = -1;            
            var ifInfo = FindNextIfOf((indice + 1), codigo);

            while (ifInfo.index != -1)
            {
                //AHORA BUSCAMOS LA ETIQUETA A LA CUAL SE GOTEA SI ES TRUE
                var etiquetaInfo = IndexOfEtiqueta(ifInfo.index, ifInfo.ins.Goto, codigo);
                if (etiquetaInfo.index - ifInfo.index != 2 || !(codigo.ElementAt(ifInfo.index + 1) is Goto)) 
                //SI LA ETIQUETA VERDADERA NO ESTA LUEGO DEL SALTO FALSO Y LA INSTRUCCION SIGUIENTE NO ES UN SALTO SE REASIGNA
                    goto REASIGNAR;
                if (!UniqueGoto(ifInfo.ins.Goto, codigo))
                    goto REASIGNAR;
                //OPTIMIZAR
                var gotoFalse = codigo.ElementAt(ifInfo.index + 1); //obtenemos el salto falso
                var eliminados = new List<IC3D>() { new If(ifInfo.ins.Condicion, ifInfo.ins.Goto, ifInfo.ins.Linea), gotoFalse, etiquetaInfo.ins};
                ifInfo.ins.Condicion.Negar();
                ifInfo.ins.Goto = ((Goto)gotoFalse).Etiqueta;
                codigo.Remove(etiquetaInfo.ins);
                codigo.Remove(gotoFalse);
                var agregados = new List<IC3D>() {ifInfo.ins};
                Reporte.Add(new Bitacora(nameof(Regla2), ifInfo.ins.Linea, eliminados, agregados));
                REASIGNAR:
                ifInfo = FindNextIfOf(ifInfo.index + 1, codigo);
            }
            return codigo;
        }
        private List<IC3D> Regla34(List<IC3D> codigo){
            int indice = -1;            
            var ifInfo = FindNextIfOf((indice + 1), codigo);

            while (ifInfo.index != -1)
            {
                if (!(codigo.ElementAt(ifInfo.index + 1) is Goto) || !ifInfo.ins.Condicion.Evaluar().isconstant) 
                //NO SE OPTIMIZA SI EL SIGUIENTE NO ES GOTO Y LA CONDICION NO ES DE CONSTANTES
                    goto REASIGNAR;                
                //OPTIMIZAR
                var gotosiguiente = codigo.ElementAt(ifInfo.index + 1);
                if (ifInfo.ins.Condicion.Evaluar().value)
                {
                    //REGLA3
                    Goto newgoto = new Goto(ifInfo.ins.Goto, ifInfo.ins.Linea);
                    List<IC3D> eliminados = new List<IC3D>() {ifInfo.ins, gotosiguiente};
                    List<IC3D> agregados = new List<IC3D>() { newgoto }; 
                    ((Goto)gotosiguiente).Etiqueta = ifInfo.ins.Goto;
                    codigo.Remove(ifInfo.ins);
                    Reporte.Add(new Bitacora(nameof(Regla34), ifInfo.ins.Linea, eliminados, agregados));
                }else{
                    //REGLA4
                    List<IC3D> eliminados = new List<IC3D>() {ifInfo.ins};
                    List<IC3D> agregados = new List<IC3D>();
                    codigo.Remove(ifInfo.ins);
                    Reporte.Add(new Bitacora(nameof(Regla34), ifInfo.ins.Linea, eliminados, agregados));
                }
                REASIGNAR:
                ifInfo = FindNextIfOf(ifInfo.index + 1, codigo);
            }
            return codigo;
        }   
        #endregion 

        #region ELIMINACION REDUNDANTE DE CARGA
        private List<IC3D> Regla5(List<IC3D> codigo){
            int indice = -1;            
            var asigInfo = FindNextAsignacionOf((indice + 1), Asignacion.Tipo.EXEX, codigo);

            while (asigInfo.index != -1)
            {
                if (!(asigInfo.ins.Resultado is string)) 
                //SI EL RESULTADO NO ES UN ID REASIGNAMOS
                    goto REASIGNAR;                
                
                var asigTem = FindNextAsignacionOf(asigInfo.index + 1, Asignacion.Tipo.EXEX, codigo);
                VERIFICAR:
                if (asigInfo.ins.Asignado == asigTem.ins.Resultado && asigInfo.ins.Resultado == asigTem.ins.Asignado)
                {
                    if (ValueChange(asigInfo.index + 1, asigTem.index, asigInfo.ins.Asignado.ToString(), codigo) || ValueChange(asigInfo.index + 1, asigTem.index, asigInfo.ins.Resultado.ToString(), codigo) || MatchBetween(asigInfo.index + 1, asigTem.index, typeof(Etiqueta), codigo))
                        goto REASIGNAR;
                    //OPTIMIZAR
                    List<IC3D> eliminados = new List<IC3D>() { asigTem.ins };
                    codigo.Remove(asigTem.ins);
                    Reporte.Add(new Bitacora(nameof(Regla5), asigInfo.ins.Linea, eliminados, new List<IC3D>()));
                    goto REASIGNAR;
                } else {
                    asigTem = FindNextAsignacionOf(asigTem.index + 1, Asignacion.Tipo.EXEX, codigo);
                    goto VERIFICAR;
                }
                REASIGNAR:
                asigInfo = FindNextAsignacionOf(asigInfo.index + 1, Asignacion.Tipo.EXEX, codigo);
            }
            return codigo;
        }
        #endregion

        #region SIMPLIFICACION ALGEBRAICA Y REDUCCION POR FUERZA
        private List<IC3D> Regla6789(List<IC3D> codigo){
            int indice = -1;            
            var asigInfo = FindNextAsignacionOf((indice + 1), Asignacion.Tipo.EXOP, codigo);
            while (asigInfo.index != -1)
            {
                if (asigInfo.ins.Asignado.ToString() == ((Operacion)asigInfo.ins.Resultado).OperadorIzq.Valor.ToString() && ((Operacion)asigInfo.ins.Resultado).OperadorDer.TipoValor == Expresion.Tipo.NUMERO)
                {
                    var operacion = (Operacion)asigInfo.ins.Resultado;
                    switch (operacion.TipoOperacion)
                    {
                        case Operacion.Tipo.ADICION:
                        case Operacion.Tipo.SUSTRACCION:
                            if (Convert.ToDouble(operacion.OperadorDer.Valor) == 0)
                            {
                                List<IC3D> eliminados = new List<IC3D>() {asigInfo.ins};
                                codigo.Remove(asigInfo.ins);
                                Reporte.Add(new Bitacora(nameof(Regla6789), asigInfo.ins.Linea, eliminados, new List<IC3D>()));
                            }
                            break;
                        case Operacion.Tipo.MULTIPLICACION:
                        case Operacion.Tipo.DIVISION:
                            if (Convert.ToDouble(operacion.OperadorDer.Valor) == 1)
                            {
                                List<IC3D> eliminados = new List<IC3D>() {asigInfo.ins};
                                codigo.Remove(asigInfo.ins);
                                Reporte.Add(new Bitacora(nameof(Regla6789), asigInfo.ins.Linea, eliminados, new List<IC3D>()));
                            }
                            break;
                    }
                }
                asigInfo = FindNextAsignacionOf(asigInfo.index + 1, Asignacion.Tipo.EXOP, codigo);
            }
            return codigo;
        }
        private List<IC3D> Regla10111213(List<IC3D> codigo){
            int indice = -1;            
            var asigInfo = FindNextAsignacionOf((indice + 1), Asignacion.Tipo.EXOP, codigo);
            while (asigInfo.index != -1)
            {
                if (((Operacion)asigInfo.ins.Resultado).OperadorDer.TipoValor == Expresion.Tipo.NUMERO)
                {
                    var operacion = (Operacion)asigInfo.ins.Resultado;
                    switch (operacion.TipoOperacion)
                    {
                        case Operacion.Tipo.ADICION:
                        case Operacion.Tipo.SUSTRACCION:
                            if (Convert.ToDouble(operacion.OperadorDer.Valor) == 0)
                            {
                                var nuevaAsignacion = new Asignacion(asigInfo.ins.Asignado, new Expresion(operacion.OperadorIzq.Valor, Expresion.Tipo.IDENTIFICADOR), Asignacion.Tipo.EXEX, asigInfo.ins.Linea);
                                List<IC3D> eliminados = new List<IC3D>() {asigInfo.ins};
                                List<IC3D> agregados = new List<IC3D>() {nuevaAsignacion};
                                asigInfo.ins = nuevaAsignacion;
                                Reporte.Add(new Bitacora(nameof(Regla10111213), asigInfo.ins.Linea, eliminados, agregados));
                            }
                            break;
                        case Operacion.Tipo.MULTIPLICACION:
                        case Operacion.Tipo.DIVISION:
                            if (Convert.ToDouble(operacion.OperadorDer.Valor) == 1)
                            {
                                var nuevaAsignacion = new Asignacion(asigInfo.ins.Asignado, new Expresion(operacion.OperadorIzq.Valor, Expresion.Tipo.IDENTIFICADOR), Asignacion.Tipo.EXEX, asigInfo.ins.Linea);
                                List<IC3D> eliminados = new List<IC3D>() {asigInfo.ins};
                                List<IC3D> agregados = new List<IC3D>() {nuevaAsignacion};
                                asigInfo.ins = nuevaAsignacion;
                                Reporte.Add(new Bitacora(nameof(Regla10111213), asigInfo.ins.Linea, eliminados, new List<IC3D>()));
                            }
                            break;
                    }
                }
                asigInfo = FindNextAsignacionOf(asigInfo.index + 1, Asignacion.Tipo.EXOP, codigo);
            }
            return codigo;
        }
        private List<IC3D> Regla141516(List<IC3D> codigo){
            int indice = -1;            
            var asigInfo = FindNextAsignacionOf((indice + 1), Asignacion.Tipo.EXOP, codigo);
            while (asigInfo.index != -1)
            {
                if (((Operacion)asigInfo.ins.Resultado).OperadorDer.TipoValor == Expresion.Tipo.NUMERO)
                {
                    var operacion = (Operacion)asigInfo.ins.Resultado;
                    if (Convert.ToDouble(operacion.OperadorDer.Valor) == 2 && operacion.TipoOperacion == Operacion.Tipo.MULTIPLICACION)
                    {
                        var nuevaAsignacion = new Asignacion(asigInfo.ins.Asignado, new Operacion(operacion.OperadorIzq, operacion.OperadorIzq, Operacion.Tipo.ADICION), Asignacion.Tipo.EXOP, asigInfo.ins.Linea);
                        List<IC3D> eliminados = new List<IC3D>(){asigInfo.ins};
                        List<IC3D> agregados = new List<IC3D>(){nuevaAsignacion};
                        asigInfo.ins = nuevaAsignacion;
                        Reporte.Add(new Bitacora(nameof(Regla141516), asigInfo.ins.Linea, eliminados, agregados));
                    } else if (Convert.ToDouble(operacion.OperadorDer.Valor) == 0 && operacion.TipoOperacion == Operacion.Tipo.MULTIPLICACION){
                        var nuevaAsignacion = new Asignacion(asigInfo.ins.Asignado, new Expresion(0, Expresion.Tipo.NUMERO), Asignacion.Tipo.EXEX, asigInfo.ins.Linea);
                        List<IC3D> eliminados = new List<IC3D>(){asigInfo.ins};
                        List<IC3D> agregados = new List<IC3D>(){nuevaAsignacion};
                        asigInfo.ins = nuevaAsignacion;
                        Reporte.Add(new Bitacora(nameof(Regla141516), asigInfo.ins.Linea, eliminados, agregados));
                    }
                } else if(((Operacion)asigInfo.ins.Resultado).OperadorIzq.TipoValor == Expresion.Tipo.NUMERO) { 
                    var operacion = (Operacion)asigInfo.ins.Resultado;
                    if (Convert.ToDouble(operacion.OperadorIzq.Valor) == 0 && operacion.TipoOperacion == Operacion.Tipo.DIVISION)
                    {
                        var nuevaAsignacion = new Asignacion(asigInfo.ins.Asignado, new Expresion(0, Expresion.Tipo.NUMERO), Asignacion.Tipo.EXEX, asigInfo.ins.Linea);
                        List<IC3D> eliminados = new List<IC3D>(){asigInfo.ins};
                        List<IC3D> agregados = new List<IC3D>(){nuevaAsignacion};
                        asigInfo.ins = nuevaAsignacion;
                        Reporte.Add(new Bitacora(nameof(Regla141516), asigInfo.ins.Linea, eliminados, agregados));
                    }
                }
                asigInfo = FindNextAsignacionOf(asigInfo.index + 1, Asignacion.Tipo.EXOP, codigo);
            }
            return codigo;
        }
        #endregion


        #region UTILIDADES
        private bool UniqueGoto(string gotoVal, List<IC3D> codigo){
            int contador = 0;
            foreach (var ins in codigo)
            {
                if (ins is If)
                    if (((If)ins).Goto == gotoVal)
                        contador++;
            }
            if (contador > 1)
                return false;
            else
                return true;
        }
        private (int index, Asignacion ins) FindNextAsignacionOf(int start, Optimizador.Asignacion.Tipo type, List<IC3D> codigo){
            for (int i = start; i < codigo.Count; i++)
            {
                var ins = codigo.ElementAt(i);
                if (ins is Asignacion)
                    //La instruccion es de tipo ASIGNACION
                    if (((Asignacion)ins).TipoAsignacion == type)
                        return (i, (Asignacion)ins);
            }
            return (-1, null);
        }
        private (int index, If ins) FindNextIfOf(int start, List<IC3D> codigo){
            for (int i = start; i < codigo.Count; i++)
            {
                var ins = codigo.ElementAt(i);
                if (ins is If)
                    //La instruccion es de tipo GOTO
                    return (i, (If)ins);
            }
            return (-1, null);
        }
        private (int index, Goto ins) FindNextGotoOf(int start, List<IC3D> codigo){
            for (int i = start; i < codigo.Count; i++)
            {
                var ins = codigo.ElementAt(i);
                if (ins is Goto)
                    //La instruccion es de tipo GOTO
                    return (i, (Goto)ins);
            }
            return (-1, null);
        }
        private (int index, Etiqueta ins) FindNextEtiquetaOf(int start, List<IC3D> codigo){
            for (int i = start; i < codigo.Count; i++)
            {
                var ins = codigo.ElementAt(i);
                if (ins is Etiqueta)
                    //La instruccion es de tipo Etiqueta
                    return (i, (Etiqueta)ins);
            }
            return (-1, null);
        }
        private (int index, Etiqueta ins) IndexOfEtiqueta(int start, string name, List<IC3D> codigo){
            for (int i = start; i < codigo.Count; i++)
            {
                var ins = codigo.ElementAt(i);
                if (ins is Etiqueta)
                    if (((Etiqueta)ins).Id == name)
                        return (i, (Etiqueta)ins);
                        //La instruccion es de tipo Etiqueta
            }
            return (-1, null);
        }
        private bool ValueChange(int start, int end, string name,List<IC3D> codigo){
            for (int i = start; i < end; i++)
            {
                var ins = codigo.ElementAt(i);
                if (ins is Asignacion)  
                    if (((Asignacion)ins).Asignado.ToString() == name)
                        return true;
            }
            return false;
        }
        private bool MatchBetween(int start, int end, Type classType, List<IC3D> codigo){
            for (int i = start; i < end; i++)
            {
                var ins = codigo.ElementAt(i);
                if (ins.GetType() == classType)                
                    return true;
            }
            return false;
        }
        private List<IC3D> ExtractC3D(int start, int end, List<IC3D> codigo){
            List<IC3D> eliminado = new List<IC3D>();
            for (int i = 0; i < end - start - 1; i++)
            {
                var el = codigo.ElementAt(start + 1);
                eliminado.Add(el);
                codigo.Remove(el);
            }
            return eliminado;
        }
        #endregion
    }
}

// 0 1 2 3 4 5 6 7 8 9 10
//       g l     g   l