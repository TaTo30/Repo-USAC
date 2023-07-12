using System;
public class C3D
{

public enum Print
{
    DIGITO,
    CARACTER,
    DECIMAL
}
public enum Unario {
    CALL,
    GOTO,
    LABEL
}
public enum Operador {
    CONDICION,
    ADICION,
    SUSTRACCION,
    MULTIPLICACION,
    DIVISION,
    MODULO,
    NEGACION,
    AND,
    OR,
    NOT,
    MENOR,
    MENORIGUAL,
    MAYOR,
    MAYORIGUAL,
    IGUAL,
    DIFERENTE,
    NONE
}

    private Operador operador;
    private string operandoIzq;
    private string operandoDer;
    private string resultado;
    private bool condicion;
    private bool salto;

    private string verdadero;
    private string falso;


    private Print print;
    private string value;

    public C3D(Operador operador, string operandoIzq, string operandoDer, string resultado){
        this.operador = operador;
        this.operandoIzq = operandoIzq;
        this.operandoDer = operandoDer;
        this.resultado = resultado;
    }

    public C3D(Operador operador, string operandoIzq, string operandoDer, string verdadero, string falso){
        this.operador = operador;
        this.operandoIzq = operandoIzq;
        this.operandoDer = operandoDer;
        this.condicion = true;
        this.verdadero = verdadero;
        this.falso = falso;
    }
    public C3D( Unario tipo, string value){
        this.salto = true;
        switch (tipo)
        {
            case Unario.GOTO:
                this.operandoDer = $"goto {value};";
            break;
            case Unario.LABEL:
                this.operandoDer = $"{value}:";
            break;
            default:
                this.operandoDer = $"{value};";
            break;
        }  
    }
    public C3D(Print print, string value){
        this.print = print;
        this.value = value;
    }
    
    override
    public string ToString(){
        if (!String.IsNullOrEmpty(value))
            return $"printf(\"{(this.print == Print.DIGITO? "%d": this.print == Print.DECIMAL? "%f" : "%c")}\", {value});";        
        if (salto)
            return $"{operandoDer}";        
        string strOperador = "";
        switch (operador)
        {
            case Operador.ADICION:
                strOperador = " + ";
                break;
            case Operador.SUSTRACCION:
                strOperador = " - ";
                break;
            case Operador.MULTIPLICACION:
                strOperador = " * ";
                break;
            case Operador.DIVISION:
                strOperador = " / ";
                break;
            case Operador.MODULO:
                strOperador = " % ";
                break;
            case Operador.NEGACION:
                strOperador = " -";
                break;
            case Operador.MAYORIGUAL:
                strOperador = " >= ";
                break;
            case Operador.MAYOR:
                strOperador = " > ";
                break;
            case Operador.MENOR:
                strOperador = " < ";
                break;
            case Operador.MENORIGUAL:
                strOperador = " <= ";
                break;
            case Operador.IGUAL:
                strOperador = " == ";
                break;
            case Operador.DIFERENTE:
                strOperador = " != ";
                break;
        }
        if (condicion)            
            return $"if ({operandoIzq}{strOperador}{operandoDer}) goto {verdadero};\ngoto {falso};";
        return $"{resultado} = {operandoIzq}{strOperador}{operandoDer};";
    }

}