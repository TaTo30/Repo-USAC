
package Modelos;
/**
 *
 * @author aldo__nr420yj
 */
public class Queue<T> {
    class Nodo<T>{
        T dato;
        Nodo siguiente;
    }
    
    private Nodo<T> First;
    private Nodo<T> Last;
    
    public void Enqueue(T dato)
    {
        Nodo<T> aux = new Nodo<T>();
        aux.dato = dato;
        if (First == null)
        {
            //LISTA VACIA INSERTAR DATOS
            First = aux;
            Last = aux;
        }else{
            //ENCOLAR DATO SIEMPRE APUNTANTOD A FIRST
            aux.siguiente = First;
            First = aux;
        }       
    }
    //Retorna un valor de la cola
    T Dequeue(){
        Nodo<T> aux = First;
        Nodo<T> toReturn = null;
        if (First == Last)
        {
            toReturn = First;
            First = null;
            Last = null;
        }else{
            while (aux != null){
                if (aux.siguiente == Last)
                {
                    toReturn = Last;
                    Last = aux;
                    Last.siguiente = null;
                    aux = null;
                }
                else
                {
                    aux = aux.siguiente;
                }           
            }
        }    
        return toReturn.dato;
    }

    //Retorna un valor booleano, true si la cola tiene datos, false si no tiene datos
    boolean Contain(){
        boolean toReturn = true;
        if (First ==null)
        {
            toReturn = false;
        }
        return toReturn;
    }
    //Retorna el tamaño de la cola
    int Size(){
        Nodo<T> aux = First;
        int contador=0;
        while (aux != null)
        {
            contador++;
            aux=aux.siguiente;
        }
        return contador;
    }
    //Vacia toda la cola
    void Clear(){
        First = null;
        Last = null;
    }   
    
}
