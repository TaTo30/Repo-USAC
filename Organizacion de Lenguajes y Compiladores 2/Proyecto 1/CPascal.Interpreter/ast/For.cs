using System.Collections.Generic;
using System;
class For : Instruccion
{
    public int Linea {get; set;}
    public int Columna {get; set;}
    private string init;
    private Operacion init_expresion;
    private Operacion to_expresion;
    private LinkedList<Instruccion> instrucciones;
    private bool downto = false;

    public For(string init, Operacion init_exp, Operacion to_exp, LinkedList<Instruccion> instrucciones, bool downto, int x, int y){
        this.init = init;
        this.init_expresion = init_exp;
        this.to_expresion = to_exp;
        this.instrucciones = instrucciones;
        this.downto = downto;
        this.Linea = x;
        this.Columna = y;
    }

    public object ejecutar(Entorno env){
        //asignamos el valor inicial
        var valorInicial = this.init_expresion.ejecutar(env);
        var valorFinal = this.to_expresion.ejecutar(env);
        if (valorInicial is double && valorFinal is double)
        {
            //solo se va a ejecutar valores enteros
            valorInicial = Convert.ToInt32(valorInicial);
            valorFinal = Convert.ToInt32(valorFinal);
            //SE ASIGNA A LA VARIABLE EL VALOR INICIAL            
            new Asignacion(this.init, new Operacion(valorInicial, Operacion.TipoOperacion.NUMERO, 0, 0), 0, 0).ejecutar(env);
            if (this.downto)
            {
                //MAYOR O IGUAL QUE
                //CREAMOS NUESTRO BOOLEANO
                var op = new Operacion(new Operacion(this.init, Operacion.TipoOperacion.IDENTIFICADOR, 0, 0), new Operacion(valorFinal, Operacion.TipoOperacion.NUMERO, 0, 0), Operacion.TipoOperacion.MAYORIGUAL, 0, 0);
                bool condicional = (bool)op.ejecutar(env);
                while(condicional){
                    foreach (var ins in this.instrucciones)
                    {
                        var res = ins.ejecutar(env);
                        if (res is Control.ControlSet)
                        {
                            switch ((Control.ControlSet)res)
                            {
                                case Control.ControlSet.BREAK:
                                    return Control.ControlSet.NONE;
                                case Control.ControlSet.EXIT:
                                    return Control.ControlSet.EXIT;
                                case Control.ControlSet.CONTINUE:
                                    goto Reasignacion;
                            }
                        }
                    }
                    //VOLVEMOS A ASINGAR
                    Reasignacion:
                    valorInicial = (int)valorInicial - 1;
                    new Asignacion(this.init, new Operacion(valorInicial, Operacion.TipoOperacion.NUMERO, 0, 0), 0, 0).ejecutar(env);
                    condicional = (bool)op.ejecutar(env);
                }
            }else{
                //MENOR O IGUAL QUE
                //CREAMOS NUESTRO BOOLEANO
                var op = new Operacion(new Operacion(this.init, Operacion.TipoOperacion.IDENTIFICADOR, 0, 0), new Operacion(valorFinal, Operacion.TipoOperacion.NUMERO, 0, 0), Operacion.TipoOperacion.MENORIGUAL, 0, 0);
                bool condicional = (bool)op.ejecutar(env);
                while(condicional){
                    foreach (var ins in this.instrucciones)
                    {
                        var res = ins.ejecutar(env);
                        if (res is Control.ControlSet)
                        {
                            switch ((Control.ControlSet)res)
                            {
                                case Control.ControlSet.BREAK:
                                    return Control.ControlSet.NONE;
                                case Control.ControlSet.EXIT:
                                    return Control.ControlSet.EXIT;
                                case Control.ControlSet.CONTINUE:
                                    goto Reasignacion;
                            }
                        }
                    }
                    //VOLVEMOS A ASINGAR
                    Reasignacion:
                    valorInicial = (int)valorInicial + 1;
                    new Asignacion(this.init, new Operacion(valorInicial, Operacion.TipoOperacion.NUMERO, 0, 0), 0, 0).ejecutar(env);
                    condicional = (bool)op.ejecutar(env);
                }
            }
        }else{
            throw new SemanticException($"La variable {this.init} no es una variable de tipo ordinal numerica", this.Linea, this.Columna);
        }
        return Control.ControlSet.NONE;        
    }
}