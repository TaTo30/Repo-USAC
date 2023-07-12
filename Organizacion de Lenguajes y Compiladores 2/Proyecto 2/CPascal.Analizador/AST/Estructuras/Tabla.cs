using System.Collections.Generic;
public class Tabla : List<Simbolo>{
    
    public int[] GetDimensiones(string id){
        foreach (var item in this)        
            if (item.Nombre.ToLower() == id.ToLower() && item.Rol.ToLower() == "arreglo")            
                return item.Dimensiones.ToArray();    
        return new int[0];
    }
    public int[] GetMinimos(string id){
        foreach (var item in this)        
            if (item.Nombre.ToLower() == id.ToLower() && item.Rol.ToLower() == "arreglo")            
                return item.Minimo.ToArray();    
        return new int[0];
    }
    public int? GetPointer(string varname, string ambito){
        foreach (var item in this)        
            if (item.Ambito.ToLower() == ambito.ToLower() && item.Nombre.ToLower() == varname.ToLower())            
                if (item.Rol.Equals("Variable"))
                    return item.Apuntador;
        foreach (var item in this)        
            if (item.Ambito.ToLower() == "global" && item.Nombre.ToLower() == varname.ToLower())            
                if (item.Rol.Equals("Variable"))
                    return item.Apuntador;
        throw new PascalExcepcion($"El nombre {varname} no existe en el contexto {ambito}", PascalExcepcion.ParseError.SEMANTICO, 0, 0);
    }
    public string GetPointerAmbito(string varname, string ambito){
        foreach (var item in this)        
            if (item.Ambito.ToLower() == ambito.ToLower() && item.Nombre.ToLower() == varname.ToLower())            
                if (item.Rol.Equals("Variable"))
                    return ambito;
        foreach (var item in this)        
            if (item.Ambito.ToLower() == "global" && item.Nombre.ToLower() == varname.ToLower())            
                if (item.Rol.Equals("Variable"))
                    return "Global";
        return "";
        //throw new PascalExcepcion($"El nombre {varname} no existe en el contexto {ambito}", PascalExcepcion.ParseError.SEMANTICO, 0, 0);
    }
    public string VarType(string ambito, string id){
        foreach (var item in this)
            if (Verify(item, ambito))
                if (item.Nombre.ToLower() == id.ToLower())
                    return item.Tipo;
        foreach (var item in this)
            if (Verify(item, "Global"))
                if (item.Nombre.ToLower() == id.ToLower())
                    return item.Tipo;
        return "";
    }
    public string ArrType(string ambito, string id){
        foreach (var item in this)
            if (VerifyArr(item, ambito))
                if (item.Nombre.ToLower() == id.ToLower())
                    return item.Tipo;
        foreach (var item in this)
            if (VerifyArr(item, "Global"))
                if (item.Nombre.ToLower() == id.ToLower())
                    return item.Tipo;
        return "";
    }
    public int GetAmbitoSize(string ambito){
        int r = 0;
        foreach (var item in this)
            if (Verify(item, ambito))
                r++;                
        return r;        
    }

    public List<Simbolo> GetAmbitoSymbols(string ambito){
        List<Simbolo> simbolos = new List<Simbolo>();
        foreach (var item in this)
            if (Verify(item, ambito))
                simbolos.Add(item);                
        return simbolos; 
    }
    public bool IsStruct(string tipo){
        foreach (var item in this)
            if (item.Rol.ToLower() == "struct" && tipo.ToLower() == item.Nombre.ToLower())
                return true;
        return false;
    }
    public bool IsArray(string tipo){
        foreach (var item in this)
            if (item.Rol.ToLower() == "arreglo" && tipo.ToLower() == item.Nombre.ToLower())
                return true;
        return false;
    }
    public int GetArraySize(string nombre){
        foreach (var item in this)
            if (item.Rol.ToLower() == "arreglo" && nombre.ToLower() == item.Nombre.ToLower())
                return (int)item.Apuntador;
        return 0;
    }
    public int GetArraySizeType(string nombre, string ambito){
        foreach (var item in this)
        {
            if (item.Rol.ToLower() == "arreglo" && nombre.ToLower() == item.Nombre.ToLower())
            {
                //MATCH para el arreglo
                if (item.Tipo.ToLower() == "integer" 
                || item.Tipo.ToLower() == "real" 
                || item.Tipo.ToLower() == "boolean" 
                || item.Tipo.ToLower() == "string")
                {
                    return 1 * GetArraySize(nombre);
                }else{
                    //UN STRUCT O ARREGLO
                    if (this.IsArray(item.Tipo))
                    {
                        return GetArraySizeType(item.Tipo, ambito) * GetArraySize(nombre); // * tamaño
                    }else{
                        return GetObjectSize(item.Tipo) * GetArraySize(nombre);
                    }
                }
            }
        }
        return 0;
    }

    public int GetObjectSize(string ambito){
        int size = 0;
        foreach (var item in this)
            if (Verify(item, ambito))
                {
                    if (item.Tipo.ToLower() == "integer" 
                    || item.Tipo.ToLower() == "real" 
                    || item.Tipo.ToLower() == "boolean" 
                    || item.Tipo.ToLower() == "string")
                    {
                        size += 1;
                    }else{
                        //UN STRUCT O ARREGLO
                        if (this.IsArray(item.Tipo))
                        {
                            size += GetArraySizeType(item.Tipo, ambito);
                        }else{
                            size += GetObjectSize(item.Tipo);
                        }
                    }
                }               
        return size;
    }
    private bool Verify(Simbolo item, string ambito){
        if (item.Ambito.ToLower() == ambito.ToLower() && item.Rol.ToLower() == "variable")
            return true;
        return false;
    }

    private bool VerifyArr(Simbolo item, string ambito){
        if (item.Ambito.ToLower() == ambito.ToLower() && item.Rol.ToLower() == "arreglo")
            return true;
        return false;
    }
}