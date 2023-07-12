using System.Collections.Generic;
using System;
class Declaracion : Instruccion
{
    public int Linea {get; set;}
    public int Columna {get; set;}
    public LinkedList<string> Listavariables;
    public Simbolo.Tipo tipo;
    public Operacion expresion;
    public object valor;

    public bool referencia = false;

    //CONSTRUCTOR PARA VARIABLES DECLARADAS SIN VALOR DE ASIGNACION SE ASIGNA EL
    //VALOR POR DEFECTO
    public Declaracion(LinkedList<string> lst, Simbolo.Tipo tipo, object valor, int x, int y){
        this.Listavariables = lst;
        this.tipo = tipo;
        this.valor = valor;
        this.expresion = null;
        this.Linea = x;
        this.Columna = y;
    }

    //CONSTRUCTOR PARA VARIABLES DECLARADAS E INICIALIZADAS SE ASIGNA LA EVALUACION DE
    //LA EXPRESION ENVIADA
    public Declaracion(LinkedList<string> lst, Simbolo.Tipo tipo, Operacion expresion, int x, int y){
        this.Listavariables = lst;
        this.tipo = tipo;
        this.expresion = expresion;
        this.valor = null;
        this.Linea = x;
        this.Columna = y;
    }

    //CONSTRUCTOR PARA CONSTANTES, SOLO SE ENVIA EL NOMBRE Y LA EXPRESION EL VALOR Y EL TIPO
    //SE ASIGNA EN EJECUCION
    public Declaracion(LinkedList<string> lst, Operacion expresion, int x, int y){
        this.Listavariables = lst;
        this.tipo = Simbolo.Tipo.ERROR;
        this.expresion = expresion;
        this.valor = null;
        this.Linea = x;
        this.Columna = y;
    }

    public object ejecutar(Entorno env){
        //============================================================
        //============DECLARACION SIN VALOR DE ASIGNACION=============
        //============================================================
        if (this.expresion == null)
        {
            //DECLARACION DE TYPES 
            if (this.tipo == Simbolo.Tipo.TYPE)
            {
                //DECLARACION DE INTERFAZ OBJECTS
                if (this.valor is Variables)                
                    foreach (var id in this.Listavariables)                    
                        env.AddLast(new Simbolo(id, this.valor, Simbolo.Tipo.IOBJECT, env.nombre));           
                //DECLARACION DE INTERFAZ ARRAYS
                else if (this.valor is Array)
                    foreach (var id in this.Listavariables)
                        env.AddLast(new Simbolo(id, this.valor, Simbolo.Tipo.IARRAY, env.nombre)); 
                //DECLARACION DE TIPOS DEFINIDOS
                else {
                    foreach (var id in this.Listavariables)
                    {
                        var tempv = this.valor;
                        switch (env.GetTipo(this.valor.ToString()))
                        {
                            case Simbolo.Tipo.ERROR:
                                throw new SemanticException($"No se ha declarado ninguna variable de tipo {this.valor}", this.Linea, this.Columna);
                            case Simbolo.Tipo.IOBJECT:  
                            //CREACION DEL OBJETO       
                                Variables vars = (Variables)env.GetValor(this.valor.ToString());
                                Entorno localobject = new Entorno(env, this.valor.ToString(), env.padres,false);
                                vars.ejecutar(localobject);
                                this.valor = localobject.DeleteInterfaces();
                                env.AddLast(new Simbolo(id, this.valor, Simbolo.Tipo.OBJECT, env.nombre));
                                this.valor = tempv;
                                break;
                            case Simbolo.Tipo.IARRAY:
                            //CREACION DEL ARRAY
                                Array arr = (Array)env.GetValor(this.valor.ToString());
                                Array tarr = (Array)arr.Clone();
                                tarr.setup(env);                                
                                env.AddLast(new Simbolo(id, tarr, Simbolo.Tipo.ARRAY, env.nombre));
                                this.valor = tempv;
                                break;                    
                        }
                    }
                }
            //DECLARACION POR DEFECTO
            } else {
                foreach(var id in this.Listavariables){
                    if (env.GetTipo(id,true) != Simbolo.Tipo.ERROR)                    
                        throw new SemanticException($"La variable {id} ya existe en el ambito", this.Linea, this.Columna);    
                    env.AddLast(new Simbolo(id, valor, tipo, env.nombre));
                }
            }
        //============================================================
        //============DECLARACION CON VALOR DE ASIGNACION=============
        //============================================================
        } else {
            //PARA CONSTANTES
            if (this.tipo == Simbolo.Tipo.ERROR)
            {
                foreach (var id in this.Listavariables)
                {
                    if (env.GetTipo(id, true) != Simbolo.Tipo.ERROR)          
                        throw new SemanticException($"La constante {id} ya existe en el ambito", this.Linea, this.Columna);
                    this.valor = this.expresion.ejecutar(env);
                    if (this.valor is double)
                    {
                        env.AddLast(new Simbolo(id, this.valor, Simbolo.Tipo.REAL, env.nombre, true));
                    }else if (this.valor is bool){
                        env.AddLast(new Simbolo(id, this.valor, Simbolo.Tipo.BOOLEAN, env.nombre, true));
                    }else if (this.valor is string){
                        env.AddLast(new Simbolo(id, this.valor, Simbolo.Tipo.STRING, env.nombre, true));
                    }else{
                        throw new SemanticException($"No se puede asignar este valor a una constante", this.Linea, this.Columna);
                    }
                }
            //DECLARACION CON VALOR
            } else {
                foreach(var id in this.Listavariables){
                    if (env.GetTipo(id, true) != Simbolo.Tipo.ERROR)                    
                        throw new SemanticException($"La variable {id} ya existe en el ambito", this.Linea, this.Columna);
                    this.valor = this.expresion.ejecutar(env);
                    env.AddLast(new Simbolo(id, valor, tipo, env.nombre));
                }
            }
        }        
        return Control.ControlSet.NONE;
    }    
}