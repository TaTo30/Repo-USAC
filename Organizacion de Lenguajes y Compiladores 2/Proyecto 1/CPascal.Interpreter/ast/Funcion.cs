using System.Collections.Generic;
using System.Collections;
using System.Linq;
class Funcion
{
    public LinkedList<Instruccion> Declaraciones;
    public LinkedList<Instruccion> Instrucciones;
    public LinkedList<Declaracion> Parametros;
    public Simbolo.Tipo ValorRetorno;
    public string identificador;

    public Funcion(string identificador, Simbolo.Tipo valorRetorno, LinkedList<Declaracion> parametros, LinkedList<Instruccion> declaraciones, LinkedList<Instruccion> instrucciones){
        this.Declaraciones = declaraciones;
        this.Instrucciones = instrucciones;
        this.Parametros = parametros;
        this.ValorRetorno = valorRetorno;
        this.identificador = identificador;
    }

    public object Ejecutar(Entorno env, LinkedList<Operacion> parametrosEnv){
        //obtenemos los nombres del los parametros que se declararon
        LinkedList<string> parametrosDec = new LinkedList<string>();
        //obtenemos el tipo de datoo con el que se declaro el parametro
        LinkedList<Simbolo.Tipo> parametrosDecTipo = new LinkedList<Simbolo.Tipo>();
        //obtenemos el booleano si es por referencia de cada parametro
        LinkedList<bool> parametrosDecRef = new LinkedList<bool>();
        foreach (var pdec in this.Parametros)
        {
            foreach (string var in pdec.Listavariables)
            {
                //insertamos el nombre del parametro
                parametrosDec.AddLast(var);  
                //insertamos el tipo del parametro
                parametrosDecTipo.AddLast(pdec.tipo);
                //insertamos si es referencia
                parametrosDecRef.AddLast(pdec.referencia); 
            }
        }
        if (parametrosEnv.Count == parametrosDec.Count)
        {
            //inicializamos la variable de retorno
            //env.SetValor(this.identificador, ValorTipo());
            if (this.ValorRetorno != Simbolo.Tipo.VOID)          
                env.NewReplace(new Simbolo(this.identificador, ValorTipo(), this.ValorRetorno, env.nombre));
            //inicializamos los parametros
            for (int i = 0; i < parametrosDec.Count; i++)
            {
                //es un parametro por valor
                env.NewReplace(new Simbolo(parametrosDec.ElementAt(i), parametrosEnv.ElementAt(i).ejecutar(env), parametrosDecTipo.ElementAt(i), env.nombre));                
            }            
            //ejecutamos las instrucciones de declaracion
            foreach (var ins in this.Declaraciones)
            {
                ins.ejecutar(env);
            }
            //ejecutamos las instrucciones de ejecucion
            foreach (var ins in this.Instrucciones)
            {
                var res = ins.ejecutar(env);
                if (res is Control.ControlSet)
                {
                    switch ((Control.ControlSet)res)
                    {
                        case Control.ControlSet.BREAK:
                            throw new SemanticException("Sentencia break debe estar en una sentencia de repeticion o de decision CASE");
                        case Control.ControlSet.CONTINUE:
                            throw new SemanticException("Sentencia continue debe estar en una sentencia de repeticion");
                        case Control.ControlSet.EXIT:
                            //funcion exit
                            env.SetValor(this.identificador, env.GetValor("exit"));
                            goto Findefuncion;                            
                    }
                }
                setReferencesByAsignacion(parametrosDec, parametrosDecRef, parametrosEnv, env);
            }
            Findefuncion:
            if (this.ValorRetorno != Simbolo.Tipo.VOID) 
                return env.GetValor(this.identificador);
            else
                return Control.ControlSet.NONE;
        } else {
            throw new SemanticException($"Se han enviado {parametrosEnv.Count} y se necesitan {parametrosDec.Count}");
        }
    }

    private void setReferencesByAsignacion(LinkedList<string> dec, LinkedList<bool> decRef, LinkedList<Operacion> send, Entorno env){
        for (int i = 0; i < dec.Count; i++)
            {
                if (decRef.ElementAt(i))
                {                    
                    switch (send.ElementAt(i).tipo)
                    {
                    case Operacion.TipoOperacion.IDENTIFICADOR:
                        //aqui tenemos una referencia
                        string idslave = dec.ElementAt(i);
                        string idmaster = (string)send.ElementAt(i).valor;
                        object slaveValue = env.GetValor(idslave);
                        env.SetValor(idmaster, slaveValue, true);
                        break;
                    case Operacion.TipoOperacion.ARRAY:
                        string arrep = dec.ElementAt(i);
                        OpArray arr = (OpArray)send.ElementAt(i).valor;
                        object arrvalue = env.GetValor(arrep);
                        arr.ejecutar(env, arrvalue);
                        break;
                    case Operacion.TipoOperacion.OBJETO:
                        string objrep = dec.ElementAt(i);
                        Objeto obj = (Objeto)send.ElementAt(i).valor;
                        object objvalue = env.GetValor(objrep);
                        obj.ejecutar_objeto(env, true, objvalue);
                        break;
                    default:
                        throw new SemanticException($"No se han pasado valores de referencia valido para la funcion {this.identificador}");
                            
                    }                    
                }                
            }
    }

    private object ValorTipo(){
        switch (this.ValorRetorno)
        {
            case Simbolo.Tipo.INTEGER:
                return 0; 
            case Simbolo.Tipo.REAL:
                return 0.0; 
            case Simbolo.Tipo.BOOLEAN:
                return true;
            case Simbolo.Tipo.STRING:
                return "";
            default:
                return 0;
        }
    }

}