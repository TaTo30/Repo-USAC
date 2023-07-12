using Irony.Parsing;
using System.Collections.Generic;
public static class Utilities
{
    private static int arrayCount = 0;
    private static int globalVarCount = 0;
    private static List<Instruccion> saveDeclaracion = new List<Instruccion>();
    public static Posicion GetPosicion(ParseTreeNode node){
        return new Posicion(node.Token.Location.Line, node.Token.Location.Column, node.Token.Location.Position);
    }
    public static int GetArrayDefinicion(){
        return arrayCount++;
    }
    public static void SaveInstruccion(Instruccion ins){
        saveDeclaracion.Add(ins);
    }
    public static List<Instruccion> GetSaveInstruccions(){
        return saveDeclaracion;
    }
    public static void ClearInstruccion(){
        saveDeclaracion.Clear();
    }
    public static int GetVarSet(){
        return globalVarCount++;
    }
}