using System.Collections.Generic;
public class CaseValue
{
    public List<Expresion> expresions {get; set;}
    public List<Instruccion> bloque {get; set;}

    public CaseValue(List<Expresion> expresions, List<Instruccion> bloque){
        this.expresions = expresions;
        this.bloque = bloque;
    }
}