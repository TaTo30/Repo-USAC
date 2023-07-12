using System.Collections.Generic;
using System;
class Operacion : Instruccion {
    public int Linea {get; set;}
    public int Columna {get; set;}
    public enum TipoOperacion
    {
        //Aritmeticas
        SUMA,
        RESTA,
        MULTIPLICACION,
        DIVISION,
        MODULO,
        NEGACION,
        //Logicas
        NOT,
        AND,
        OR,
        //Relacionales
        MENOR,
        MENORIGUAL,
        MAYORIGUAL,
        MAYOR,
        IGUAL,
        DIFERENTE,
        //CONSTANTES
        CADENA,
        NUMERO,
        BOOLEANO,
        IDENTIFICADOR,
        //OBJETOS
        OBJETO,
        ARRAY,
        FUNCION
    }

    private LinkedList<Operacion> parametros;
    private Operacion operadorIzq;
    private Operacion operadorDer;
    public TipoOperacion tipo;
    public object valor;

    //OPERACIONES BINARIAS
    public Operacion(Operacion izq, Operacion der, TipoOperacion tipo, int x, int y){
        this.operadorIzq = izq;
        this.operadorDer = der;
        this.tipo = tipo;
        this.Linea = x;
        this.Columna = y;
    }

    //OPERACIONES UNARIAS
    public Operacion(Operacion der, TipoOperacion tipo, int x, int y){
        this.operadorDer = der;
        this.tipo = tipo;
        this.Linea = x;
        this.Columna = y;
    }

    //IDENTIFIACORES
    public Operacion(object valor, TipoOperacion tipo, int x, int y){
        this.valor = valor;
        this.tipo = tipo;
        this.Linea = x;
        this.Columna = y;
    }

    //LLAMADAS A FUNCIONES
    public Operacion(object id, LinkedList<Operacion> parametros, int x, int y) {
        this.valor = id;
        this.parametros = parametros;
        this.tipo = TipoOperacion.FUNCION;
        this.Linea = x;
        this.Columna = y;
    }

    private LinkedList<object> ParametrosEnv(LinkedList<Operacion> parametros, Entorno env){
        LinkedList<object> lista = new LinkedList<object>();
        foreach (var item in parametros)
        {
            lista.AddLast(item.ejecutar(env));
        }
        return lista;
    }

    public object ejecutar(Entorno env){
        switch (this.tipo)
        {
            //FUNCIONES
            case TipoOperacion.FUNCION:
                var returnvalue = env.GetValor(this.valor.ToString());
                if (returnvalue is Simbolo.Tipo)
                    if ((Simbolo.Tipo)returnvalue == Simbolo.Tipo.ERROR)
                        throw new SemanticException($"La simbolo {this.valor} no se encuentra en el ambito actual {this.Linea}, {this.Columna}");
                Funcion func = (Funcion)returnvalue;
                //object value = func.Ejecutar(new Entorno(env), ParametrosEnv(this.parametros, env));
                object value = func.Ejecutar(new Entorno(env, this.valor.ToString(), env.padres), this.parametros);
                return value;
            //ARRAY
            case TipoOperacion.ARRAY:
                OpArray arr = (OpArray)this.valor;                
                return arr.ejecutar(env);
            //OBJETOS
            case TipoOperacion.OBJETO:
                Objeto var = (Objeto)this.valor;
                return var.ejecutar_objeto(env);
            //CONSTANTES
            case TipoOperacion.NUMERO:
                return (double)(double.Parse(this.valor.ToString()));
            case TipoOperacion.BOOLEANO:
                return (bool)(bool.Parse(this.valor.ToString()));
            case TipoOperacion.CADENA:
                return this.valor.ToString();
            case TipoOperacion.IDENTIFICADOR:
                var returnvalue2 = env.GetValor(this.valor.ToString());
                if (returnvalue2 is Simbolo.Tipo)
                    if ((Simbolo.Tipo)returnvalue2 == Simbolo.Tipo.ERROR)
                        throw new SemanticException($"La simbolo {this.valor} no se encuentra en el ambito actual {this.Linea}, {this.Columna}");
                return env.GetValor(this.valor.ToString());
            //RELACIONALES
            case TipoOperacion.MAYOR:
                return (double)(operadorIzq.ejecutar(env)) > (double)(operadorDer.ejecutar(env));
            case TipoOperacion.MAYORIGUAL:
                return (double)(operadorIzq.ejecutar(env)) >= (double)(operadorDer.ejecutar(env));
            case TipoOperacion.MENOR:
                return (double)(operadorIzq.ejecutar(env)) < (double)(operadorDer.ejecutar(env));
            case TipoOperacion.MENORIGUAL:
                return (double)(operadorIzq.ejecutar(env)) <= (double)(operadorDer.ejecutar(env));
            case TipoOperacion.IGUAL:
                return (double)(operadorIzq.ejecutar(env)) == (double)(operadorDer.ejecutar(env));
            case TipoOperacion.DIFERENTE:
                var a = operadorIzq.ejecutar(env).ToString();
                var b = operadorDer.ejecutar(env).ToString();
                System.Diagnostics.Debug.WriteLine(a + " " + b);
                return Convert.ToDouble(a) != Convert.ToDouble(b);
            //LOGICAS
            case TipoOperacion.AND:
                return (bool)(operadorIzq.ejecutar(env)) && (bool)(operadorDer.ejecutar(env));
            case TipoOperacion.OR:
                return (bool)(operadorIzq.ejecutar(env)) || (bool)(operadorDer.ejecutar(env));
            case TipoOperacion.NOT:
                return !(bool)(operadorDer.ejecutar(env));
            //ARITMETICAS
            case TipoOperacion.SUMA:
                return (double)(operadorIzq.ejecutar(env)) + (double)(operadorDer.ejecutar(env));
            case TipoOperacion.RESTA:
                return (double)(operadorIzq.ejecutar(env)) - (double)(operadorDer.ejecutar(env));
            case TipoOperacion.MULTIPLICACION:
                return (double)(operadorIzq.ejecutar(env)) * (double)(operadorDer.ejecutar(env));
            case TipoOperacion.DIVISION:
                return (double)(operadorIzq.ejecutar(env)) / (double)(operadorDer.ejecutar(env));
            case TipoOperacion.MODULO:
                return (double)(operadorIzq.ejecutar(env)) % (double)(operadorDer.ejecutar(env));
            case TipoOperacion.NEGACION:
                return -1 * (double)(operadorDer.ejecutar(env));
            default:
                throw new SemanticException("Operador invalido.");
        }
    }
}