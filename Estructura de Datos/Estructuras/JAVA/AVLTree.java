
package Modelos;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.lang.reflect.Array;



public class AVLTree<T> {
    class Node<T>{
        int key, height;
        T data;
        Node<T> izquierda, derecha;
    }
    
    //Atributos de la estructura
    private Node<T> raiz;
    private int count;
    private T[] PreOrden;
    private T[] InOrden;
    private T[] PostOrden;
    

    
    /**********************/
    /*  METODOS PRIVADOS  */
    /**********************/
    
    //Calcula la altura de un nodo
    private int Altura(Node<T> n){
        if (n == null) {
            return 0;
        }else{
            return n.height;
        }
    }
    //Calcula el valor maximo entre 2 alturas
    private int Max(int a, int b){
        return (a > b ) ? a : b;
    }
    //Calcula el balance de un nodo
    private int Balance(Node<T> n){
        if (n == null) {
            return 0;
        }else{
            return Altura(n.derecha) - Altura(n.izquierda);
        }
    }
    //Encuentra  el valor mas pequeño de un subarbol    
    private Node<T> MinValueNode(Node<T> nodo){
        Node<T> aux = nodo;
        while(aux.izquierda != null){
            aux = aux.izquierda;
        }
        return aux;
    }
    
    //Ayudante de recorrido para añadir datos a la estructura
    private Node<T> AddSupport(Node<T> nodo, int key, T data){
        if (nodo == null) {
            //si el nodo (puede ser la raiz) es nulo agragarle un nuevo nodo
            Node<T> aux = new Node<>();
            aux.data = data;
            aux.key = key;
            nodo = aux;
            count++;
        }else if (key < nodo.key) {
            //si la llave es mas pequeña se inserta en lado izquierdo
            nodo.izquierda = AddSupport(nodo.izquierda, key,data);
        }else if (key > nodo.key) {
            //si la llave es mas grande se inserta en el lado derecho
            nodo.derecha = AddSupport(nodo.derecha,key,data);
        }else{
            System.out.println("El dato es un duplicado");
        }
        //seteamos la altura del nuevo nodo
        nodo.height = 1 + Max(Altura(nodo.izquierda), Altura(nodo.derecha));
        //obtenemos el balance del nodo
        int balanceo = Balance(nodo);
       
        //evaluamos los 4 casos de balanceo
        if (balanceo > 1) {
            //Si el balanceo es +2 hacemos rotaciones RSI y RDD
            int balanceo2 = Balance(nodo.derecha);
            if (balanceo2 < 0) {
                //CASO 4 RDD
                Node<T> n = nodo;
                Node<T> n1 = n.derecha;
                Node<T> n2 = n1.izquierda;
                n1.izquierda = n2.derecha;
                n2.derecha = n1;
                n.derecha = n2.izquierda;
                n2.izquierda = n;
                n.height = 1 + Max(Altura(n.izquierda), Altura(n.derecha));
                n1.height = 1 + Max(Altura(n1.izquierda), Altura(n1.derecha));
                n2.height = 1 + Max(Altura(n2.izquierda), Altura(n2.derecha));
                nodo = n2;
            }
            if (balanceo2 > 0) {
                //CASO 1 RSI
                Node<T> n = nodo;
                Node<T> n1 = nodo.derecha;
                n.derecha = n1.izquierda;
                n1.izquierda = n;
                n.height = 1 + Max(Altura(n.izquierda), Altura(n.derecha));
                n1.height = 1 + Max(Altura(n1.izquierda), Altura(n1.derecha));
                nodo = n1;
            }            
        }else if (balanceo < -1) {
            //Si el balanceo es -2 hacemos rotaciones RSD y RDI
            int balanceo2 = Balance(nodo.izquierda);
            if (balanceo2 < 0) {
                //CASO 2 RSD
                Node<T> n = nodo;
                Node<T> n1 = nodo.izquierda;
                n.izquierda = n1.derecha;
                n1.derecha = n;
                n.height = 1 + Max(Altura(n.izquierda), Altura(n.derecha));
                n1.height = 1 + Max(Altura(n1.izquierda), Altura(n1.derecha));
                nodo = n1;
            }
            if (balanceo2 > 0) {
                //CASO 3 RDI
                Node<T> n = nodo;
                Node<T> n1 = nodo.izquierda;
                Node<T> n2 = n1.derecha;
                n1.derecha = n2.izquierda;
                n2.izquierda = n1;
                n.izquierda = n2.derecha;
                n2.derecha = n;
                n.height = 1 + Max(Altura(n.izquierda), Altura(n.derecha));
                n1.height = 1 + Max(Altura(n1.izquierda), Altura(n1.derecha));
                n2.height = 1 + Max(Altura(n2.izquierda), Altura(n2.derecha));
                nodo = n2;
            }
        }
        //Retornamos el nodo
        return nodo;        
    } 
    //Ayudante de recorrido para eliminar datos de la estructura
    private Node<T> DeleteSupport(Node<T> nodo, int key){
        if (nodo == null) {
            //si el nodo es nulo, entonces no existe el dato devolvemos nulo
            return nodo;
        }else if (key < nodo.key) {
            //si la llave es menor que el nodo actual buscamos en sus subarboles izquierdo
            nodo.izquierda = DeleteSupport(nodo.izquierda, key);
        }else if (key > nodo.key) {
            //si la llave es mayor que el nodo actual buscamos en sus subarboles derecho
            nodo.derecha = DeleteSupport(nodo.derecha, key);
        }else{
            
            //si la llave es igual al dato que buscamos, procedemos a eliminar
            if (nodo.izquierda == null || nodo.derecha == null) {
                //si tiene 0 o 1 hijos
                Node<T> temp = null;
                if (temp == nodo.izquierda) {
                    temp = nodo.derecha;
                }else{
                    temp = nodo.izquierda;
                }                
                //si no tiene hijos
                if (temp == null) {
                    //temp = nodo;
                    nodo = null;
                }else{
                    nodo = temp;
                }
            }else{
                //si tiene 2 hijos
                Node<T> temp = MinValueNode(nodo.derecha);
                nodo.key = temp.key;
                nodo.data = temp.data;
                nodo.derecha = DeleteSupport(nodo.derecha, temp.key);                
            }
        }        
        //en el caso de que el nodo no tuviera hijos, no sera necesario rebalancear, retornamos el nulo
        if (nodo == null) {
            return nodo;
        }
        //Actualizamos la altura del nodo actual
        nodo.height = 1 + Max(Altura(nodo.izquierda), Altura(nodo.derecha));
        //Obtenemos el balanceo del nodo actual
        int balanceo = Balance(nodo);        
        //evaluamos los 4 casos de balanceo
        if (balanceo > 1) {
            //Si el balanceo es +2 hacemos rotaciones RSI y RDD
            int balanceo2 = Balance(nodo.derecha);
            if (balanceo2 < 0) {
                //CASO 4 RDD
                Node<T> n = nodo;
                Node<T> n1 = n.derecha;
                Node<T> n2 = n1.izquierda;
                n1.izquierda = n2.derecha;
                n2.derecha = n1;
                n.derecha = n2.izquierda;
                n2.izquierda = n;
                n.height = 1 + Max(Altura(n.izquierda), Altura(n.derecha));
                n1.height = 1 + Max(Altura(n1.izquierda), Altura(n1.derecha));
                n2.height = 1 + Max(Altura(n2.izquierda), Altura(n2.derecha));
                nodo = n2;
            }
            if (balanceo2 > 0) {
                //CASO 1 RSI
                Node<T> n = nodo;
                Node<T> n1 = nodo.derecha;
                n.derecha = n1.izquierda;
                n1.izquierda = n;
                n.height = 1 + Max(Altura(n.izquierda), Altura(n.derecha));
                n1.height = 1 + Max(Altura(n1.izquierda), Altura(n1.derecha));
                nodo = n1;
            }            
        }else if (balanceo < -1) {
            //Si el balanceo es -2 hacemos rotaciones RSD y RDI
            int balanceo2 = Balance(nodo.izquierda);
            if (balanceo2 < 0) {
                //CASO 2 RSD
                Node<T> n = nodo;
                Node<T> n1 = nodo.izquierda;
                n.izquierda = n1.derecha;
                n1.derecha = n;
                n.height = 1 + Max(Altura(n.izquierda), Altura(n.derecha));
                n1.height = 1 + Max(Altura(n1.izquierda), Altura(n1.derecha));
                nodo = n1;
            }
            if (balanceo2 > 0) {
                //CASO 3 RDI
                Node<T> n = nodo;
                Node<T> n1 = nodo.izquierda;
                Node<T> n2 = n1.derecha;
                n1.derecha = n2.izquierda;
                n2.izquierda = n1;
                n.izquierda = n2.derecha;
                n2.derecha = n;
                n.height = 1 + Max(Altura(n.izquierda), Altura(n.derecha));
                n1.height = 1 + Max(Altura(n1.izquierda), Altura(n1.derecha));
                n2.height = 1 + Max(Altura(n2.izquierda), Altura(n2.derecha));
                nodo = n2;
            }
        }        
        //retornamos el nodo
        return nodo;
        
    }
    //Ayudante de recorrido para buscar datos de la estructura
    private Node<T> FindSupport(Node<T> nodo, int key){        
        if (nodo == null) {
            //si se llega a un nodo nulo, entonces el dato a buscar no existe
            return null;
        }else{
            if (key == nodo.key) {
                //si la llave se encuentra se retorna el dato;
                return nodo;
            }else if (key < nodo.key) {
                //si la llave es menor que el nodo que estamos evaluando, buscamos en el subarbol izquierdo
                return FindSupport(nodo.izquierda, key);
            }else{
                //si la llave es mayor que el nodo que estamos evaluando, buscamos en el subarbol derecho
                return FindSupport(nodo.derecha, key);
            }
        }
    }
    
    //variable entera que ayudara a posicionar los objetos en un array
    private int posicion = 0;
    //Ayudante de recorrido preOrden
    private void PreOrdenSupport(Node<T> nodo, T[] array){
        if (nodo != null) {
            //Visitamos la raiz
            array[posicion] = nodo.data;
            posicion++;
            //Visitamos su Izquierda;
            PreOrdenSupport(nodo.izquierda, array);
            //Visitamos su Derecha;
            PreOrdenSupport(nodo.derecha, array);            
        }
    }
    //Ayudante de recorrido inOrden
    private void InOrdenSupport(Node<T> nodo, T[] array){
        if (nodo != null) {
            //visitamos su nodo izquierdo
            InOrdenSupport(nodo.izquierda, array);
            //visitamos la raiz
            array[posicion] = nodo.data;
            posicion++;
            //visitamos su nodo derecho
            InOrdenSupport(nodo.derecha, array);
        }
    }
    //Ayudante de recorrido postOrden
    private void PostOrdenSupport(Node<T> nodo, T[] array){
        if (nodo != null) {
            //visitamos izquierda
            PostOrdenSupport(nodo.izquierda, array);
            //visitamos derecha
            PostOrdenSupport(nodo.derecha, array);
            //visitamos raiz
            array[posicion] = nodo.data;
            posicion++;
        }
    }
    
    
    
    
    /**********************/
    /*  METODOS PUBLICOS  */
    /**********************/
    
    //Constructor
    public AVLTree(){
        raiz = null;
        
        count = 0;
        PreOrden = null;
        /*InOrden = null;
        PostOrden = null;*/
    }
    //Añade un dato al arbol binario de busqueda con una llave de ordenamiento que especifique el usuario
    public void Add(int key, T data){
        raiz = AddSupport(raiz,key,data);
    }
    //Elimina un dato del arbol binario de busqueda con una llave que especifique el usuario
    public void Delete(int key){
        raiz = DeleteSupport(raiz, key);
        count--;
    }
    //Busca un dato del arbol binario de busqueda con una llave que especifique el usuario
    public T Find(int key){
        Node<T> toReturn = FindSupport(raiz, key);
        if (toReturn != null) {
            return toReturn.data;
        }else{
            return null;
        }
         
    }
    //Limpia la estructura
    public void Clear(){
        raiz = null;
    }
    //Retorna true si existe el dato con la llave, false cualquier otro caso
    public boolean Check(int key){
        if (Find(key) == null) {
            return false;
        }else{
            return true;
        }
    }
    //Retorna la cantidad de datos en la estructura
    public int Count(){
        return count;
    }
    //Returna un array de los elementos en PreOrden
    public T[] PreOrden(Class<T> type){        
        posicion = 0;
        PreOrden = (T[]) Array.newInstance(type, count);
        PreOrdenSupport(raiz, PreOrden);
        return PreOrden;        
    }
    //Retorna un array de los elementos en InOrden
    public T[] InOrden(Class<T> type){
        posicion = 0;
        InOrden = (T[]) Array.newInstance(type, count);
        InOrdenSupport(raiz, InOrden);
        return InOrden;
    }
    //Retorna un array de los elementos en postOrden
    public T[] PostOrden(Class<T> type){
        posicion = 0;
        PostOrden = (T[]) Array.newInstance(type, count);
        PostOrdenSupport(raiz, PostOrden);
        return PostOrden;
    }
    
    String grafo="";
    public void Graficar() throws IOException{
        if (raiz != null) {
            grafo ="";
            grafo += "digraph g{ node [shape = circle];";
            Recorrido(raiz);
            grafo += "\n}\n";
            try{
                BufferedWriter writer = new BufferedWriter(new FileWriter("Categorias.dot"));
                writer.write(grafo);
                writer.close();
                File a = new File("Categorias.dot");
                String comando = "dot -Tpng "+a.getAbsolutePath()+" -o"+a.getAbsolutePath().replace(".dot", ".png");
                Runtime.getRuntime().exec(comando);
            }catch(IOException ex){
                
            }
        }
    }
    
    public void Recorrido(Node<T> nodo){
        if (nodo != null) {
            Categoria temp = (Categoria)nodo.data;
            grafo += temp.getNombre()+";\n";
            if (nodo.izquierda != null) {
                Categoria tempI = (Categoria)nodo.izquierda.data;
                grafo += temp.getNombre()+"->"+tempI.getNombre()+";\n";
            }
            if (nodo.derecha != null) {
                Categoria tempI = (Categoria)nodo.derecha.data;
                grafo += temp.getNombre()+"->"+tempI.getNombre()+";\n";
            }
            Recorrido(nodo.izquierda);
            Recorrido(nodo.derecha);
        }
    }
}
