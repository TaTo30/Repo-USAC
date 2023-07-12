
package practica;

public class NodoArbol {
    
    public enum TipoOperador{
        OPERADOR,
        OPERADORUNITARIO,
        OPERANDO
    }
    
    TipoOperador TipoTokenOperador;
    Token.Tipo TipoExpresion;
    String dato;
    NodoArbol HijoIzquierdo;
    NodoArbol HijoDerecho;
    String Primero;
    String Ultimo;
    int Indice;
    boolean Anulable;

    public NodoArbol(TipoOperador TipoToken, Token.Tipo TipoExpresion, String dato, NodoArbol HijoIzquierdo, NodoArbol HijoDerecho, String Primero, String Ultimo,int Indice,Boolean Anulable) {
        this.TipoTokenOperador = TipoToken;
        this.TipoExpresion = TipoExpresion;
        this.dato = dato;
        this.HijoIzquierdo = HijoIzquierdo;
        this.HijoDerecho = HijoDerecho;
        this.Primero = Primero;
        this.Ultimo = Ultimo;
        this.Indice = Indice;
        this.Anulable = Anulable;
    }

    public boolean isAnulable() {
        return Anulable;
    }

    public void setAnulable(boolean Anulable) {
        this.Anulable = Anulable;
    }
    

    public TipoOperador getTipoToken() {
        return TipoTokenOperador;
    }

    public Token.Tipo getTipoExpresion() {
        return TipoExpresion;
    }

    public String getDato() {
        return dato;
    }

    public NodoArbol getHijoIzquierdo() {
        return HijoIzquierdo;
    }

    public NodoArbol getHijoDerecho() {
        return HijoDerecho;
    }

    public String getPrimero() {
        return Primero;
    }

    public String getUltimo() {
        return Ultimo;
    }

    public void setTipoTokenOperador(TipoOperador TipoTokenOperador) {
        this.TipoTokenOperador = TipoTokenOperador;
    }

    public void setTipoExpresion(Token.Tipo TipoExpresion) {
        this.TipoExpresion = TipoExpresion;
    }

    public void setDato(String dato) {
        this.dato = dato;
    }

    public void setHijoIzquierdo(NodoArbol HijoIzquierdo) {
        this.HijoIzquierdo = HijoIzquierdo;
    }

    public void setHijoDerecho(NodoArbol HijoDerecho) {
        this.HijoDerecho = HijoDerecho;
    }

    public void setPrimero(String Primero) {
        if (this.Primero == "") {
            this.Primero = Primero;
        }else{
            this.Primero += ","+Primero;
        }
    }

    public void setUltimo(String Ultimo) {
        if (this.Ultimo == "") {
            this.Ultimo = Ultimo;
        }else{
            this.Ultimo += ","+Ultimo;
        }
    }

    public void setIndice(int Indice) {
        this.Indice = Indice;
    }
    
    
    
}
