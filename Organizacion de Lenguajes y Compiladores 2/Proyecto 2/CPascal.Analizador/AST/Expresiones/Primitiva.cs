using System.Collections.Generic;
using System.Linq;
using System;

public class Primitiva : Expresion
{
    public Posicion posicion {get; set;}
    public string ultimoTemporal {get; set;}
    public C3D.Print tipoPrint {get; set;}
    private object valor;
    public Primitiva(object valor, Posicion posicion){
        this.valor = valor;
        this.posicion = posicion;
    }

    public long ObtenerValorImplicito(){
        return Convert.ToInt64(this.valor);
    }

    public List<C3D> GenerarC3D(Tabla tabla, string ambito){
        List<C3D> codigo = new List<C3D>();
        double numeric = 0;
        this.tipoPrint = C3D.Print.DECIMAL;
        if (valor.ToString().ToLower() == "true")        
            this.ultimoTemporal = $"{1}";
        else if(valor.ToString().ToLower() == "false")
            this.ultimoTemporal = $"{0}";        
        else if (Double.TryParse(valor.ToString(), out numeric))        
            this.ultimoTemporal = $"{Double.Parse(valor.ToString())}";
        else {
            string heapBegin = Temporales.Correlativo;
            codigo.Add(new C3D(C3D.Operador.NONE, "", "HP", heapBegin));
            foreach (var c in valor.ToString() + "$")
            {
                codigo.Add(new C3D(C3D.Operador.NONE, "", $"{(int)c}", "Heap[HP]"));
                codigo.Add(new C3D(C3D.Operador.ADICION, "HP", "1", "HP"));
            }
            this.ultimoTemporal = heapBegin;
            this.tipoPrint = C3D.Print.CARACTER;
        }
        return codigo;
    }

    public List<C3D> GenerarC3D(Tabla tabla, string ambito, string verdadero, string falso){
        List<C3D> codigo = new List<C3D>();
        double numeric = 0;
        this.tipoPrint = C3D.Print.DECIMAL;
        if (valor.ToString().ToLower() == "true")        
            this.ultimoTemporal = $"{1}";
        else if(valor.ToString().ToLower() == "false")
            this.ultimoTemporal = $"{0}";        
        else if (Double.TryParse(valor.ToString(), out numeric))        
            this.ultimoTemporal = $"{Double.Parse(valor.ToString())}";
        else {
            string cadena = valor.ToString() + "$";
            string heapBegin = Temporales.Correlativo;
            codigo.Add(new C3D(C3D.Operador.NONE, "", "HP", heapBegin));
            foreach (var c in valor.ToString())
            {
                codigo.Add(new C3D(C3D.Operador.NONE, "", $"{(int)c}", "Heap[HP]"));
                codigo.Add(new C3D(C3D.Operador.ADICION, "HP", "1", "HP"));
            }
            this.ultimoTemporal = heapBegin;
            this.tipoPrint = C3D.Print.CARACTER;
        }
        if (valor.ToString().ToLower() == "false")
            codigo.Add(new C3D(C3D.Operador.DIFERENTE, this.ultimoTemporal, this.ultimoTemporal, verdadero, falso));
        else
            codigo.Add(new C3D(C3D.Operador.IGUAL, this.ultimoTemporal, this.ultimoTemporal, verdadero, falso));
         
        return codigo;
    }
}