using System.Collections.Generic;
using System.Linq;
using System;

public class Acceso : Expresion
{
    public Posicion posicion {get; set;}
    public string ultimoTemporal {get; set;}
    public string PadreAmbito {get; set;}
    public C3D.Print tipoPrint {get; set;}
    public enum Tipo
    {
        IDENTIFICADOR,
        STRUCT,
        ARRAY
    }
    public enum Mode
    {
        GET,
        SET
    }
    public Mode modo {get; set;}
    private Acceso precedente;
    private Tipo tipo;
    private string identificador;
    private List<Expresion> indices;

    public Acceso(string identificador, Tipo tipo, Posicion posicion){
        this.identificador = identificador;
        this.tipo = tipo;
        this.posicion = posicion;
    }
    public Acceso(Acceso precedente, string identificador, Tipo tipo, Posicion posicion){
        this.precedente = precedente;
        this.identificador = identificador;
        this.tipo = tipo;
        this.posicion = posicion;
    }
    public Acceso(Acceso precedente, List<Expresion> indice, Tipo tipo, Posicion posicion){
        this.precedente = precedente;
        this.indices = indice;
        this.tipo = tipo;
        this.posicion = posicion;
    }
    public long ObtenerValorImplicito(){
        return 1;
    }

    /*
    
    HAY QUE ASIGNARLE TIPO PRINTS
    
    */


    //ACCESANDO A VARIABLES
    public List<C3D> GenerarC3D(Tabla tabla, string ambito){
        List<C3D> codigo = new List<C3D>();
        this.ultimoTemporal = Temporales.Correlativo;
        this.tipoPrint = C3D.Print.DECIMAL;
        if (tipo == Tipo.IDENTIFICADOR)
        {
            // T0 = Stack[<POINTER>]
            if (tabla.VarType(ambito, identificador).ToLower() == "string")
                this.tipoPrint = C3D.Print.CARACTER;
            string temporalAsignacion = Temporales.Correlativo;
            int pointer = (int)tabla.GetPointer(this.identificador, ambito);
            string pointerAmbito = tabla.GetPointerAmbito(this.identificador, ambito);
            this.PadreAmbito = pointerAmbito;
            if (pointerAmbito == "Global")
            {
                codigo.Add(new C3D(C3D.Operador.NONE,"", $"Heap[{pointer}]", this.ultimoTemporal));
                return codigo;
            }
            codigo.Add(new C3D(C3D.Operador.ADICION, "SP", $"{pointer}", temporalAsignacion));              // T0 = SP + <POINTER>
            codigo.Add(new C3D(C3D.Operador.NONE,"", $"Stack[{temporalAsignacion}]", this.ultimoTemporal)); // T1 = Stack[T0]
            return codigo;            
        } else if (tipo == Tipo.STRUCT) {
            codigo = codigo.Concat(this.precedente.GenerarC3D(tabla, ambito)).ToList(); //T0 = guarda una posicion del heap
            string tipoPadre = tabla.VarType(precedente.PadreAmbito, precedente.identificador);
            if (tipoPadre == "")            
                tipoPadre = tabla.ArrType(precedente.PadreAmbito, precedente.identificador);
            this.PadreAmbito = tipoPadre;
            if (tabla.VarType(tipoPadre, this.identificador).ToLower() == "string")
                this.tipoPrint = C3D.Print.CARACTER;
            int pointer = (int)tabla.GetPointer(this.identificador, tipoPadre);
            string pointerAmbito = tabla.GetPointerAmbito(this.identificador, ambito);
            //T1 = T0 + <POINTER>
            string posicionVariable = Temporales.Correlativo;
            codigo.Add(new C3D(C3D.Operador.ADICION, this.precedente.ultimoTemporal, $"{pointer}", posicionVariable));
            //T<ULTIMO> = Heap[T1]
            codigo.Add(new C3D(C3D.Operador.NONE, "", $"Heap[{posicionVariable}]", this.ultimoTemporal));
            
            return codigo;
        } else if (tipo == Tipo.ARRAY){
            codigo = codigo.Concat(this.precedente.GenerarC3D(tabla, ambito)).ToList(); //T0 = guarda la posicion de inicio del array
            string tipoPadre = tabla.VarType(precedente.PadreAmbito, precedente.identificador);
            string arrayType = tabla.ArrType(ambito, tipoPadre);
            this.identificador = tipoPadre;
            this.PadreAmbito = precedente.PadreAmbito;
            if (arrayType.ToLower() == "string" )
            {
                this.tipoPrint = C3D.Print.CARACTER;
            }
            int sizeType = 1;
            if (arrayType.ToLower() == "integer" 
                || arrayType.ToLower() == "real" 
                || arrayType.ToLower() == "boolean" 
                || arrayType.ToLower() == "string"){
                    sizeType = 1;
            } else if (tabla.IsStruct(arrayType)){
                sizeType = tabla.GetObjectSize(arrayType);
            } else if (tabla.IsArray(arrayType)){
                sizeType = tabla.GetArraySizeType(arrayType, ambito);
            }

            //DIMENSIONES
            int[] dimensiones = tabla.GetDimensiones(tabla.VarType(ambito, precedente.identificador));
            int[] minimos = tabla.GetMinimos(tabla.VarType(ambito, precedente.identificador));
            if (dimensiones.Length == 1)
            {
                codigo = codigo.Concat(this.indices.First().GenerarC3D(tabla, ambito)).ToList();
                string indice = this.indices.First().ultimoTemporal;
                string indiceTemporal = Temporales.Correlativo;
                string posicion = Temporales.Correlativo;
                string apoyo = Temporales.Correlativo;
                codigo.Add(new C3D(C3D.Operador.SUSTRACCION, indice, $"{minimos[0]}", apoyo));
                codigo.Add(new C3D(C3D.Operador.MULTIPLICACION, apoyo, $"{sizeType}", indiceTemporal));
                codigo.Add(new C3D(C3D.Operador.ADICION, this.precedente.ultimoTemporal, indiceTemporal, posicion));
                codigo.Add(new C3D(C3D.Operador.NONE, "", $"Heap[{posicion}]", this.ultimoTemporal));
                return codigo;
            }else{
                //N dimensiones
                string carga = Temporales.Correlativo;
                codigo = codigo.Concat(this.indices.First().GenerarC3D(tabla, ambito)).ToList();
                codigo.Add(new C3D(C3D.Operador.SUSTRACCION, this.indices.First().ultimoTemporal, $"{minimos[0]}", carga));
                
                for (int i = 1; i < dimensiones.Length; i++)
                {
                    string cargatemp = Temporales.Correlativo;
                    codigo.Add(new C3D(C3D.Operador.MULTIPLICACION, carga, $"{dimensiones[i]}", cargatemp));
                    carga = cargatemp;
                    cargatemp = Temporales.Correlativo;
                    codigo = codigo.Concat(this.indices.ElementAt(i).GenerarC3D(tabla, ambito)).ToList();
                    string apoyo = Temporales.Correlativo;
                    codigo.Add(new C3D(C3D.Operador.SUSTRACCION, this.indices.ElementAt(i).ultimoTemporal, $"{minimos[i]}", apoyo));
                    codigo.Add(new C3D(C3D.Operador.ADICION, carga, apoyo, cargatemp));
                    carga = cargatemp;
                }
                string indiceTemporal = Temporales.Correlativo;
                string posicion = Temporales.Correlativo;
                codigo.Add(new C3D(C3D.Operador.MULTIPLICACION, carga, $"{sizeType}", indiceTemporal));
                codigo.Add(new C3D(C3D.Operador.ADICION, this.precedente.ultimoTemporal, indiceTemporal, posicion));
                codigo.Add(new C3D(C3D.Operador.NONE, "", $"Heap[{posicion}]", this.ultimoTemporal));
                return codigo;
            }
        }
        return codigo;
    }
    
    //ASIGNANDO VARIABLES
    public List<C3D> GenerarC3D(Tabla tabla, string ambito, string result){
        // Heap[<POINTER>] = <RESULT>
        List<C3D> codigo = new List<C3D>();
        if (tipo == Tipo.IDENTIFICADOR)
        {
            string temporalAsignacion = Temporales.Correlativo;
            int pointer = (int)tabla.GetPointer(this.identificador, ambito);
            string pointerAmbito = tabla.GetPointerAmbito(this.identificador, ambito);
            this.PadreAmbito = pointerAmbito;
            if (pointerAmbito == "Global")
            {
                codigo.Add(new C3D(C3D.Operador.NONE,"", result, $"Heap[{pointer}]"));  //Heap[0] = T0
                return codigo;
            }
            codigo.Add(new C3D(C3D.Operador.ADICION, "SP", $"{pointer}", temporalAsignacion)); // T0 = SP + <POINTER>
            codigo.Add(new C3D(C3D.Operador.NONE,"", result, $"Stack[{temporalAsignacion}]")); //Stack[T0] = T<RESULT>
            return codigo;
        } else if (tipo == Tipo.STRUCT) {
            codigo = codigo.Concat(this.precedente.GenerarC3D(tabla, ambito)).ToList(); //T0 = guarda una posicion del heap
            string tipoPadre = tabla.VarType(precedente.PadreAmbito, precedente.identificador);
            if (tipoPadre == "")            
                tipoPadre = tabla.ArrType(precedente.PadreAmbito, precedente.identificador);
            this.PadreAmbito = tipoPadre;
            int pointer = (int)tabla.GetPointer(this.identificador, tipoPadre);
            string pointerAmbito = tabla.GetPointerAmbito(this.identificador, ambito);
            //T1 = T0 + <POINTER>
            string posicionVariable = Temporales.Correlativo;
            codigo.Add(new C3D(C3D.Operador.ADICION, this.precedente.ultimoTemporal, $"{pointer}", posicionVariable));
            //Heap[T1] = T<RESULT>
            codigo.Add(new C3D(C3D.Operador.NONE, "", result , $"Heap[{posicionVariable}]"));
            return codigo;
        } else if (tipo == Tipo.ARRAY){
            codigo = codigo.Concat(this.precedente.GenerarC3D(tabla, ambito)).ToList(); //T0 = guarda la posicion de inicio del array
            string tipoPadre = tabla.VarType(precedente.PadreAmbito, precedente.identificador);
            string arrayType = tabla.ArrType(ambito, tipoPadre);
            this.identificador = tipoPadre;
            this.PadreAmbito = precedente.PadreAmbito;
            int sizeType = 1;
            if (arrayType.ToLower() == "integer" 
                || arrayType.ToLower() == "real" 
                || arrayType.ToLower() == "boolean" 
                || arrayType.ToLower() == "string"){
                    sizeType = 1;
            } else if (tabla.IsStruct(arrayType)){
                sizeType = tabla.GetObjectSize(arrayType);
            } else if (tabla.IsArray(arrayType)){
                sizeType = tabla.GetArraySizeType(arrayType, ambito);
            }

            //DIMENSIONES
            int[] dimensiones = tabla.GetDimensiones(tabla.VarType(ambito, precedente.identificador));
            int[] minimos = tabla.GetMinimos(tabla.VarType(ambito, precedente.identificador));
            if (dimensiones.Length == 1)
            {
                codigo = codigo.Concat(this.indices.First().GenerarC3D(tabla, ambito)).ToList();
                string indice = this.indices.First().ultimoTemporal;
                string indiceTemporal = Temporales.Correlativo;
                string posicion = Temporales.Correlativo;
                string apoyo = Temporales.Correlativo;
                codigo.Add(new C3D(C3D.Operador.SUSTRACCION, indice, $"{minimos[0]}", apoyo));
                codigo.Add(new C3D(C3D.Operador.MULTIPLICACION, apoyo, $"{sizeType}", indiceTemporal));
                codigo.Add(new C3D(C3D.Operador.ADICION, this.precedente.ultimoTemporal, indiceTemporal, posicion));

                //codigo.Add(new C3D(C3D.Operador.NONE, "", $"Heap[{posicion}]", this.ultimoTemporal)); //TU = Heap[POS]
                codigo.Add(new C3D(C3D.Operador.NONE, "", result, $"Heap[{posicion}]"));    //Heap[PS] = TR
                return codigo;
            }else{
                //N dimensiones
                string carga = Temporales.Correlativo;
                codigo = codigo.Concat(this.indices.First().GenerarC3D(tabla, ambito)).ToList();
                codigo.Add(new C3D(C3D.Operador.SUSTRACCION, this.indices.First().ultimoTemporal, $"{minimos[0]}", carga));
                
                for (int i = 1; i < dimensiones.Length; i++)
                {
                    string cargatemp = Temporales.Correlativo;
                    codigo.Add(new C3D(C3D.Operador.MULTIPLICACION, carga, $"{dimensiones[i]}", cargatemp));
                    carga = cargatemp;
                    cargatemp = Temporales.Correlativo;
                    codigo = codigo.Concat(this.indices.ElementAt(i).GenerarC3D(tabla, ambito)).ToList();
                    string apoyo = Temporales.Correlativo;
                    codigo.Add(new C3D(C3D.Operador.SUSTRACCION, this.indices.ElementAt(i).ultimoTemporal, $"{minimos[i]}", apoyo));
                    codigo.Add(new C3D(C3D.Operador.ADICION, carga, apoyo, cargatemp));
                    carga = cargatemp;
                }
                string indiceTemporal = Temporales.Correlativo;
                string posicion = Temporales.Correlativo;
                codigo.Add(new C3D(C3D.Operador.MULTIPLICACION, carga, $"{sizeType}", indiceTemporal));
                codigo.Add(new C3D(C3D.Operador.ADICION, this.precedente.ultimoTemporal, indiceTemporal, posicion));

                //codigo.Add(new C3D(C3D.Operador.NONE, "", $"Heap[{posicion}]", this.ultimoTemporal));
                codigo.Add(new C3D(C3D.Operador.NONE, "", result, $"Heap[{posicion}]"));    //Heap[PS] = TR
                return codigo;
            }
        }
        return codigo;
    }

    public List<C3D> GenerarC3D(Tabla tabla, string ambito, string verdadero, string falso){
        List<C3D> codigo = new List<C3D>();
        this.ultimoTemporal = Temporales.Correlativo;
        if (tipo == Tipo.IDENTIFICADOR)
        {
            // T0 = Stack[<POINTER>]
            string temporalAsignacion = Temporales.Correlativo;
            int pointer = (int)tabla.GetPointer(this.identificador, ambito);
            string pointerAmbito = tabla.GetPointerAmbito(this.identificador, ambito);
            this.PadreAmbito = pointerAmbito;
            if (pointerAmbito == "Global")
            {
                codigo.Add(new C3D(C3D.Operador.NONE,"", $"Heap[{pointer}]", this.ultimoTemporal));
                return codigo;
            }
            codigo.Add(new C3D(C3D.Operador.ADICION, "SP", $"{pointer}", temporalAsignacion));              // T0 = SP + <POINTER>
            codigo.Add(new C3D(C3D.Operador.NONE,"", $"Stack[{temporalAsignacion}]", this.ultimoTemporal)); // T1 = Stack[T0]
            return codigo;            
        } else if (tipo == Tipo.STRUCT) {
            codigo = codigo.Concat(this.precedente.GenerarC3D(tabla, ambito)).ToList(); //T0 = guarda una posicion del heap
            string tipoPadre = tabla.VarType(precedente.PadreAmbito, precedente.identificador);
            this.PadreAmbito = tipoPadre;
            if (tabla.VarType(tipoPadre, this.identificador).ToLower() == "string")
                this.tipoPrint = C3D.Print.CARACTER;
            int pointer = (int)tabla.GetPointer(this.identificador, tipoPadre);
            string pointerAmbito = tabla.GetPointerAmbito(this.identificador, ambito);
            //T1 = T0 + <POINTER>
            string posicionVariable = Temporales.Correlativo;
            codigo.Add(new C3D(C3D.Operador.ADICION, this.precedente.ultimoTemporal, $"{pointer}", posicionVariable));
            //T<ULTIMO> = Heap[T1]
            codigo.Add(new C3D(C3D.Operador.NONE, "", $"Heap[{posicionVariable}]", this.ultimoTemporal));
            return codigo;
        } else if (tipo == Tipo.ARRAY){
            codigo = codigo.Concat(this.precedente.GenerarC3D(tabla, ambito)).ToList(); //T0 = guarda la posicion de inicio del array
            string tipoPadre = tabla.VarType(precedente.PadreAmbito, precedente.identificador);
            string arrayType = tabla.ArrType(ambito, tipoPadre);
            this.identificador = tipoPadre;
            this.PadreAmbito = precedente.PadreAmbito;
            int sizeType = 1;
            if (arrayType.ToLower() == "integer" 
                || arrayType.ToLower() == "real" 
                || arrayType.ToLower() == "boolean" 
                || arrayType.ToLower() == "string"){
                    sizeType = 1;
            } else if (tabla.IsStruct(arrayType)){
                sizeType = tabla.GetObjectSize(arrayType);
            } else if (tabla.IsArray(arrayType)){
                sizeType = tabla.GetArraySizeType(arrayType, ambito);
            }

            //DIMENSIONES
            int[] dimensiones = tabla.GetDimensiones(tabla.VarType(ambito, precedente.identificador));
            int[] minimos = tabla.GetMinimos(tabla.VarType(ambito, precedente.identificador));
            if (dimensiones.Length == 1)
            {
                codigo = codigo.Concat(this.indices.First().GenerarC3D(tabla, ambito)).ToList();
                string indice = this.indices.First().ultimoTemporal;
                string indiceTemporal = Temporales.Correlativo;
                string posicion = Temporales.Correlativo;
                string apoyo = Temporales.Correlativo;
                codigo.Add(new C3D(C3D.Operador.SUSTRACCION, indice, $"{minimos[0]}", apoyo));
                codigo.Add(new C3D(C3D.Operador.MULTIPLICACION, apoyo, $"{sizeType}", indiceTemporal));
                codigo.Add(new C3D(C3D.Operador.ADICION, this.precedente.ultimoTemporal, indiceTemporal, posicion));
                codigo.Add(new C3D(C3D.Operador.NONE, "", $"Heap[{posicion}]", this.ultimoTemporal));
                return codigo;
            }else{
                //N dimensiones
                string carga = Temporales.Correlativo;
                codigo = codigo.Concat(this.indices.First().GenerarC3D(tabla, ambito)).ToList();
                codigo.Add(new C3D(C3D.Operador.SUSTRACCION, this.indices.First().ultimoTemporal, $"{minimos[0]}", carga));
                
                for (int i = 1; i < dimensiones.Length; i++)
                {
                    string cargatemp = Temporales.Correlativo;
                    codigo.Add(new C3D(C3D.Operador.MULTIPLICACION, carga, $"{dimensiones[i]}", cargatemp));
                    carga = cargatemp;
                    cargatemp = Temporales.Correlativo;
                    codigo = codigo.Concat(this.indices.ElementAt(i).GenerarC3D(tabla, ambito)).ToList();
                    string apoyo = Temporales.Correlativo;
                    codigo.Add(new C3D(C3D.Operador.SUSTRACCION, this.indices.ElementAt(i).ultimoTemporal, $"{minimos[i]}", apoyo));
                    codigo.Add(new C3D(C3D.Operador.ADICION, carga, apoyo, cargatemp));
                    carga = cargatemp;
                }
                string indiceTemporal = Temporales.Correlativo;
                string posicion = Temporales.Correlativo;
                codigo.Add(new C3D(C3D.Operador.MULTIPLICACION, carga, $"{sizeType}", indiceTemporal));
                codigo.Add(new C3D(C3D.Operador.ADICION, this.precedente.ultimoTemporal, indiceTemporal, posicion));
                codigo.Add(new C3D(C3D.Operador.NONE, "", $"Heap[{posicion}]", this.ultimoTemporal));
                return codigo;
            }
        }
        return codigo;
    }
}