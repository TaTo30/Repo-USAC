using System.Collections.Generic;
using System;
using System.Linq;
public class Tipicas : Instruccion, Definicion
{
    public Posicion posicion {get; set;}
    public bool mutable {get; set;}
    public List<Variable> variables {get; set;}

    public Tipicas(List<Variable> variables, bool mutable, Posicion posicion){
        this.variables = variables;
        this.mutable = mutable;
        this.posicion = posicion;
    }

    public void MapearGlobales(Tabla tabla, Ambito ambito){
        foreach (var item in variables)  
            tabla.Add(new Simbolo(item.identificador, item.tipo, ambito.Nombre, "Variable", ambito.Correlativo, posicion));
    }

    public List<C3D> GenerarC3D(Tabla tabla, string ambito){
        List<C3D> codigo = new List<C3D>();
        // T0 = HP              1.  Nueva variable que guarde el valor actual del heap
        // HP = HP + 1          2.  Aumentamos la posicion del heap en uno
        // Heap[T0] = value     3.  Asignamos el valor al heap
        
        foreach (var item in this.variables)
        {
            if (item.tipo.ToLower() == "integer" 
                || item.tipo.ToLower() == "real" 
                || item.tipo.ToLower() == "boolean" 
                || item.tipo.ToLower() == "string")
            {
                codigo = TiposPrimitivos(item, codigo, tabla, ambito);
            }else{
                //Structs o Objetos
                if (tabla.IsStruct(item.tipo))
                {
                    string temporalAsignacion = Temporales.Correlativo;
                    int pointer = (int)tabla.GetPointer(item.identificador, ambito);
                    string pointerAmbito = tabla.GetPointerAmbito(item.identificador, ambito);
                    if (pointerAmbito == "Global")
                    {
                        codigo = TipoStruct(item, codigo, tabla, ambito, temporalAsignacion, $"Heap[{pointer}]");
                        //codigo.Add(new C3D(C3D.Operador.NONE,"", temporalAsignacion, $"Heap[{pointer}]"));   // Heap[0] = T0
                    }
                    else{
                        //codigo.Add(new C3D(C3D.Operador.ADICION, "SP", $"{pointer}", temporalAsignacion));  // T0 = SP + <POINTER>
                        //codigo.Add(new C3D(C3D.Operador.NONE,"", "0", $"Stack[{temporalAsignacion}]"));     // Stack[T0] = T<RETURN>  
                        string stackp = Temporales.Correlativo;
                        codigo.Add(new C3D(C3D.Operador.ADICION, "SP", $"{pointer}", stackp));
                        codigo = TipoStruct(item, codigo, tabla, ambito, temporalAsignacion, $"Stack[{stackp}]");
                    }
                    //codigo = TipoStruct(item, codigo, tabla, ambito);
                } else if (tabla.IsArray(item.tipo)) {
                    string temporalAsignacion = Temporales.Correlativo; //Donde empieza el array
                    int pointer = (int)tabla.GetPointer(item.identificador, ambito); //apuntador en la tabla
                    string pointerAmbito = tabla.GetPointerAmbito(item.identificador, ambito);//ambito del apuntador
                    int total = tabla.GetArraySize(item.tipo);
                    codigo.Add(new C3D(C3D.Operador.NONE, "", "HP", temporalAsignacion)); //DONDE INICIA MI ARRAY //T0 = HP
                    string arraytype = tabla.ArrType(ambito, item.tipo);
                    for (int i = 0; i < total; i++)
                    {
                        Variable avar = new Variable("a1", 0, arraytype);
                        TipoArray(avar, codigo, tabla, ambito);
                    }
                    if (pointerAmbito == "Global")
                    {
                        codigo.Add(new C3D(C3D.Operador.NONE, "", temporalAsignacion, $"Heap[{pointer}]")); //Heap[T0] = T0
                    }else{
                        string stackp = Temporales.Correlativo; //T0
                        codigo.Add(new C3D(C3D.Operador.ADICION, "SP", $"{pointer}", stackp));//T0 = SP + 1
                        codigo.Add(new C3D(C3D.Operador.NONE, "", temporalAsignacion, $"Stack[{stackp}]")); //Heap[T0] = T1
                    }
                }
            }
            
        }
        return codigo;
    }

    private List<C3D> TipoArray(Variable item, List<C3D> codigo, Tabla tabla, string ambito){
        if (item.tipo.ToLower() == "integer" 
            || item.tipo.ToLower() == "real" 
            || item.tipo.ToLower() == "boolean" 
            || item.tipo.ToLower() == "string")
        {
            codigo.Add(new C3D(C3D.Operador.NONE,"", "0", $"Heap[HP]"));    //Heap[HP] = 0
            codigo.Add(new C3D(C3D.Operador.ADICION, "HP", "1", "HP"));     //HP = HP + 1
            
        } else {
            if (tabla.IsStruct(item.tipo))
            {
                //array de objetos
                string temporalAsignacion = Temporales.Correlativo; //T0
                codigo = TipoStruct(item, codigo, tabla, ambito, temporalAsignacion, $"Heap[{temporalAsignacion}]");
            } else if (tabla.IsArray(item.tipo)){
                string temporalAsignacion = Temporales.Correlativo; //T0 referencia el inicio del array
                int total = tabla.GetArraySize(item.tipo);
                string arraytype = tabla.ArrType(ambito, item.tipo);
                codigo.Add(new C3D(C3D.Operador.NONE, "", "HP", temporalAsignacion)); //DONDE INICIA MI ARRAY //T0 = HP
                for (int i = 0; i < total; i++)
                {
                    Variable avar = new Variable("", 0, arraytype);
                    codigo = TipoArray(avar, codigo, tabla, ambito);
                }
                codigo.Add(new C3D(C3D.Operador.NONE, "", temporalAsignacion, $"Heap[{temporalAsignacion}]")); //Heap[T0] = T0
            }
        }   
        return codigo;    
    }

    private List<C3D> TipoStruct(Variable item, List<C3D> codigo, Tabla tabla, string ambito, string referencia, string guardado){
        int varnumber = tabla.GetAmbitoSize(item.tipo);
        
        codigo.Add(new C3D(C3D.Operador.NONE, "", "HP", referencia));   // T0 = HP
        List<Simbolo> subsimbolos = tabla.GetAmbitoSymbols(item.tipo);
        foreach (var smb in subsimbolos)
        {
            if (smb.Tipo.ToLower() == "integer" 
                || smb.Tipo.ToLower() == "real" 
                || smb.Tipo.ToLower() == "boolean" 
                || smb.Tipo.ToLower() == "string")
            {
                codigo.Add(new C3D(C3D.Operador.NONE,"", "0", $"Heap[HP]"));    //Heap[HP] = 0
                codigo.Add(new C3D(C3D.Operador.ADICION, "HP", "1", "HP"));     //HP = HP + 1
            }else{
                if (tabla.IsStruct(smb.Tipo))
                {
                    Variable newvar = new Variable(smb.Nombre, 0, smb.Tipo);
                    string temporalAsignacion = Temporales.Correlativo;
                    codigo = TipoStruct(newvar, codigo, tabla, item.tipo, temporalAsignacion, $"Heap[{temporalAsignacion}]");
                    //codigo.Add(new C3D(C3D.Operador.ADICION, "HP", "1", "HP"));     //HP = HP + 1
                }else if (tabla.IsArray(smb.Tipo)){
                    string temporalAsignacion = Temporales.Correlativo; //T0 referencia el inicio del array
                    int total = tabla.GetArraySize(smb.Tipo);
                    string arraytype = tabla.ArrType(ambito, smb.Tipo);
                    codigo.Add(new C3D(C3D.Operador.NONE, "", "HP", temporalAsignacion)); //DONDE INICIA MI ARRAY //T0 = HP
                    for (int i = 0; i < total; i++)
                    {
                        Variable avar = new Variable("", 0, arraytype);
                        codigo = TipoArray(avar, codigo, tabla, ambito);
                    }
                    codigo.Add(new C3D(C3D.Operador.NONE, "", temporalAsignacion, $"Heap[{temporalAsignacion}]")); //Heap[T0] = T0
                }
                
            }//S O P O R T E   P A R A   A R R A Y S
        }
        codigo.Add(new C3D(C3D.Operador.NONE, "", referencia, guardado));
        return codigo;        
    }

    private List<C3D> TiposPrimitivos(Variable item, List<C3D> codigo, Tabla tabla, string ambito){
        if (item.expresion != null)
        {
            List<C3D> exp = item.expresion.GenerarC3D(tabla, ambito);
            codigo = codigo.Concat(exp).ToList();
            string temporalAsignacion = Temporales.Correlativo;
            int pointer = (int)tabla.GetPointer(item.identificador, ambito);
            string pointerAmbito = tabla.GetPointerAmbito(item.identificador, ambito);
            if (pointerAmbito == "Global")
            {
                codigo.Add(new C3D(C3D.Operador.NONE,"", item.expresion.ultimoTemporal, $"Heap[{pointer}]"));   // Heap[0] = T0
                codigo.Add(new C3D(C3D.Operador.ADICION, "HP", "1", "HP"));                                     // HP = HP + 1
            }else{
                codigo.Add(new C3D(C3D.Operador.ADICION, "SP", $"{pointer}", temporalAsignacion));                          // T0 = SP + <POINTER>
                codigo.Add(new C3D(C3D.Operador.NONE,"", item.expresion.ultimoTemporal, $"Stack[{temporalAsignacion}]"));   // Stack[T0] = T<RETURN>   
            }
        } else {
            string temporalAsignacion = Temporales.Correlativo;
            int pointer = (int)tabla.GetPointer(item.identificador, ambito);
            string pointerAmbito = tabla.GetPointerAmbito(item.identificador, ambito);
            if (pointerAmbito == "Global")
            {
                codigo.Add(new C3D(C3D.Operador.NONE,"", "0", $"Heap[{pointer}]"));   // Heap[0] = 0
                codigo.Add(new C3D(C3D.Operador.ADICION, "HP", "1", "HP"));           // HP = HP + HP
            }else{
                codigo.Add(new C3D(C3D.Operador.ADICION, "SP", $"{pointer}", temporalAsignacion));  // T0 = SP + <POINTER>
                codigo.Add(new C3D(C3D.Operador.NONE,"", "0", $"Stack[{temporalAsignacion}]"));     // Stack[T0] = T<RETURN>   
            }
        }
        return codigo;
    }
}