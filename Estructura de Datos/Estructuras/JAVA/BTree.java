

package Modelos;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import javax.swing.JFileChooser;
import java.lang.reflect.Array;
import javax.swing.filechooser.FileNameExtensionFilter;




public class BTree<T> {
    class Node<T>{
        int[] claves;//las claves
        T[] datos;//los datos
        Node[] Hijos;//apuntadores de hijos
        int min;//Minimo de claves
        int n;//Claves actuales
        int clave;//numero de nodo que es
        boolean hoja;//si es hoja o no
        Class<T> type;//tipo de objeto que se guarda en la estructura
        
        public Node(int min, Class<T> type, boolean hoja, int c){
            this.min = min;
            this.hoja = hoja;
            this.clave = c;
            claves = new int[2*min-1];
            datos = (T[]) Array.newInstance(type, 2*min-1);
            Hijos = new Node[2*min];
            
            n = 0;
            this.type = type;
        }
        

        //Funcion que cumple con la separacion de un nodo anidando claves al nodo padre
        public void Split(int i, Node y){
            //y el nodo que vamos a dividir en 2
            //i el indice de donde se empieza a anidar hijos
            
            //creamos un nuevo nodo z, con los mismos valores de y
            Node z = new Node(y.min,type, y.hoja, claveNodo); 
            claveNodo++;
            z.n = min-1;
            
            //le seteamos a z los valores de y desde la media hasta todos sus valores 
            for (int j = 0; j < min-1; j++) {
                z.claves[j] = y.claves[j+min];
                z.datos[j] = y.datos[j+min];
            
            }            
            //En caso de Y sea un nodo No Hoja la este le dara a Z la mitad de sus hijos
            if (y.hoja == false) {
                //copiamos los hijos de Y a Z desde el minimo de datos permitidos hasta todos los que tenga
                for (int j = 0; j < min; j++) {
                    z.Hijos[j] = y.Hijos[j+min];
                }
                //eliminamos de Y los hijos que le dio a Z
                for (int j = min; j < min*2; j++) {
                    y.Hijos[j] = null;
                }
            }            
            //reducir el numero de claves habidas en Y
            y.n = min-1;            
            //dado que el nodo padre tendra un nuevo hijo, crear el espacio donde estara el hijo
            for (int j = n; j >= i+1; j--) {
                Hijos[j+1] = Hijos[j];
            }            
            //enlazar en nuevo hijo al nodo padre en el espacio que creamos
            Hijos[i+1] = z;
            
            //dado que el nodo padre tendra una nueva clave, creamos el espacio
            //donde estara la clave corriendo los valores            
            for (int j = n-1; j >= i; j--) {
                claves[j+1] = claves[j];
                datos[j+1] = datos[j];
            }
            
            //Le ponemos al nodo padre el valor de la media que sera su nueva clave
            claves[i] = y.claves[min-1];
            datos[i] = (T)y.datos[min-1];
          
            //Eliminamos de Y los valores que son mayores que la media
            //Borramos tambien la media
            for (int j = min-1; j <= 2*min-2; j++) {
                y.claves[j]=0;
                y.datos[j]=null;
            }
            
            //Incrementamos el contador de claves para el nodo padre
            n = n+1;
        }
        //Funcion que añade a un nodo un valor en valores ascedentes
        public void AddNonFull(int k, T data){
            int i = n-1;
            if (hoja==true) {
                //si el nodo es una hoja entonces solo agregamos la clave de manera ascendente
                //este ciclo cumple con mover los valores y crear espacion para el nuevo valor
                while(i >= 0 && claves[i]>k){
                    claves[i+1] = claves[i];
                    datos[i+1] = datos[i];
                    i--;
                }
                //agregamos el nuevo valor al vector
                claves[i+1] = k;
                datos[i+1] = data;
                //incrementamos la cantidad de valores habidos
                n++;
            }else{
                //si el nodo es un No hoja entonces procedemos a buscar una hoja donde agregar la nueva llave
                //ciclo que encuentra la posicion donde debe ubicarse el valor
                while(i >= 0 && claves[i]>k){
                    i--;
                }                
                if (Hijos[i+1].n == 2*min-1) {
                    //si el hijo donde debe estar la llave tiene sus valores maximos
                    //hacemos un split para ese hijo
                    Split(i+1, Hijos[i+1]);
                    if (claves[i+1] < k) {
                        //una vez el hijo haya sido dividido buscamos en cual de los dos
                        //deberia agruparse i es el valor de la posicion donde debe 
                        //establecerse el nuevo hijo
                        i++;
                    }
                }
                //añadimos la llave al hijo encontrado correspondiente
                Hijos[i+1].AddNonFull(k, data);
            }
        }
        //Funcion que encuentra una llave mayor o igual a k
        public int BuscarKey(int k){
            int index = 0;
            //buscamos el indice del dato que sea menor o igual a la llave buscada
            while(index < n && claves[index] < k){
                index++;
            }
            return index;
        }
        //Funcion que toma el dato mas grade del hermano izquierdo de un nodo
        public void PrestarPredecesor(int index){
            //obtenemos el apuntador de los hijos que se prestaran datos
            Node hijo = Hijos[index];
            Node hermano = Hijos[index-1];
            
            //hacemos espacio en el array de claves moviendo a la derecha para dar espacio
            //al nuevo dato que se situara en la menor ponderacion del nodo
            for (int i = hijo.n-1; i >= 0; --i) {
                hijo.claves[i+1] = hijo.claves[i];
                hijo.datos[i+1] = hijo.datos[i];
            }    
            //si el nodo tiene hijos tambien dezplazamos sus hijos a la derecha para dar espacio al nuevo hijo
            if (!hijo.hoja) {
                for (int i = hijo.n; i >= 0; --i) {
                    hijo.Hijos[i+1] = hijo.Hijos[i];
                }
            }
            //seteamos el valor prestado en la posicion 0 del hijo de los mayores
            hijo.claves[0] = claves[index-1];
            hijo.datos[0] = datos[index-1];
            //si el nodo tiene hijos tambien seteamos el hijo del hermano 
            if (!hijo.hoja) {
                hijo.Hijos[0] = hermano.Hijos[hermano.n];
            }
            //seteamos en el espacio ocupado por la clave el dato mayor de su nodo de menores
            claves[index-1] = hermano.claves[hermano.n-1];
            datos[index-1] = (T)hermano.datos[hermano.n-1];
            //el hijo de de mayores aumenta sus datos en 1
            hijo.n += 1;
            //el hijo de los menores disminuye sus datos en 1
            hermano.n -= 1;
            //eliminamos los datos del hermano que presto sus datos
            for (int i = hermano.n; i < 2*min-1; i++) {
                hermano.claves[i] = 0;
                hermano.datos[i] = null;
            }
            //eliminaos los hijos del hermano que presto sus datos
            for (int i = hermano.n+1; i < 2*min; i++) {
                hermano.Hijos[i] = null;
            }
        }
        //Funcion que toma el dato mas pequeño del hermano derecho de un nodo
        public void PrestarSucesor(int index){
            //obtenemos los apuntadores que se prestaran datos
            Node hijo = Hijos[index];
            Node hermano = Hijos[index+1];
            //ponemos en la ultima posicion el dato del nodo padre
            hijo.claves[hijo.n] = claves[index];
            hijo.datos[hijo.n] = datos[index];
            //si el hijo tiene hijos seteamos el hijo del hermano que va a prestar
            if (!hijo.hoja) {
                hijo.Hijos[hijo.n+1] = hermano.Hijos[0];
            }
            //ponemos el dato a prestar en el hermano
            claves[index] = hermano.claves[0];
            datos[index] = (T)hermano.datos[0];
            //movemos los datos para reordenar el array            
            for (int i = 1; i < hermano.n; ++i) {
                hermano.claves[i-1] = hermano.claves[i];
                hermano.datos[i-1] = hermano.datos[i];
            }
            //movemos los hijos para reordenar el array de hijos
            if (!hermano.hoja) {
                for (int i = 1; i <= hermano.n; ++i) {
                    hermano.Hijos[i-1] = hermano.Hijos[i];
                }
            }
            //el hijo aumenta sus datos en 1
            hijo.n += 1;
            //el hermano disminuye sus datos en 1
            hermano.n -= 1;
            //eliminamos el dato que el hermano presto al hijo
            for (int i = hermano.n; i < 2*min-1; i++) {
                hermano.claves[i] = 0;
                hermano.datos[i] = null;
            }
            //eliminamos el hijo que el hermano presto al hijo
            for (int i = hermano.n+1; i < 2*min; i++) {
                hermano.Hijos[i] = null;
            }
        }
        //Funcion que junta dos nodos cuando estan en valores minimos
        public void Agrupar(int index){
            //apuntadores de los hijos que se agruparan
            Node hijo = Hijos[index];
            Node hermano = Hijos[index+1];
            //el hijo obtiene dato de la clave del padre
            hijo.claves[min-1] = claves[index];
            hijo.datos[min-1] = datos[index];
            //le ponemos al hijo los datos del hermano
            for (int i = 0; i < hermano.n; ++i) {
                hijo.claves[i+min] = hermano.claves[i];
                hijo.datos[i+min] = hermano.datos[i];
            }
            //si el hijo tiene hijos entonces obtiene los hijos del hermano
            if (!hijo.hoja) {
                for (int i = 0; i <= hermano.n; ++i) {
                   hijo.Hijos[i+min] = hermano.Hijos[i];
                } 
            }
            //movemos los datos del padre para reordenar el array
            for (int i = index+1; i < n; ++i) {
                claves[i-1] = claves[i];
                datos[i-1] = datos[i];
            }
            //movemos los hijos del padre para reordenar el array
            for (int i = index+2; i <= n; ++i) {
                Hijos[i-1] = Hijos[i];
            }
            //el hijo aumenta la cantidad de datos que obtuvo del hermano
            hijo.n += hermano.n+1;
            //el nodo padre disminuye sus datos
            n--;     
            //eliminas los apuntadores de los datos a donde ya no se puede llegar
            for (int i = n; i < 2*min-1; i++) {
                claves[i] = 0;
                datos[i] = null;
            }
            //eliminamos los apuntadores de los hijos a donde ya no se puede llegar
            for (int i = n+1; i < 2*min; i++) {
                Hijos[i] = null;
            }
        }
        //Elimina un dato de la estructura
        public void Remove(int k){
            //obtiene la posicion donde se situa el dato
            int index = BuscarKey(k);
            if (index < n && claves[index] == k) {
                //si el dato se encuentra en el nodo se elimina
                if (hoja) {
                    //eliminar en el caso de ser hoja
                    RemoveHoja(index);
                }else{
                    //eliminar en el caso de no ser hoja
                    RemoveNoHoja(index);
                }
            }else{
                //si el dato no se encuentra en el nodo
                if (hoja) {
                    //si es hoja, no existe el dato
                    System.out.print("no hay datos para borrar");
                    return;
                }
                
                //determinar si el dato es mayor que los datos del array
                boolean flag = ((index == n));
                
                if (Hijos[index].n < min) {
                    Fill(index);
                }
                
                if (flag && index > n) {
                    //si el dato es mayor que los datos del array retrocedemos en 1
                    Hijos[index-1].Remove(k);
                }else{
                    //si no, buscamos en el hijo correspondiente al indice encontrado
                    Hijos[index].Remove(k);
                }
            }
        }
        //devuelve el dato mas grande del hijo izquierdo de una clave
        public int Predecesor(int index){
            //devuelve el dato mayor entre los hijos de un padre
            Node cur = Hijos[index];
            while(!cur.hoja){
                cur = cur.Hijos[cur.n];
            }
            return cur.claves[cur.n-1];
        }
        //devuelve el dato de clase mas grande del hijo izquierdo de una clave
        public T PredecesorData(int index){
            //obtiene el dato mayor entre los hijos de un padre
            Node cur = Hijos[index];
            while(!cur.hoja){
                cur = cur.Hijos[cur.n];
            }
            return (T)cur.datos[cur.n-1];
        }
        //devuelve el dato mas pequeño del hijo derecho de una clave
        public int Sucesor(int index){
            //devuelve el dato menor entre los hijos del padre
            Node cur = Hijos[index+1];
            while(!cur.hoja){
                cur = cur.Hijos[0];
            }
            return cur.claves[0];
        }
        //devuelve el dato de clase mas pequeño del hijo derecho de una clave
        public T SucesorData(int index){
            //devuelve el dato mas pequeño entre los hijos de un padre
            Node cur = Hijos[index+1];
            while(!cur.hoja){
                cur = cur.Hijos[0];
            }
            return (T)cur.datos[0];
        }
        //llena un nodo padre con algun dato de sus nodos hijos
        public void Fill(int index){
            if (index !=0 && Hijos[index-1].n >= min){
                PrestarPredecesor(index);   
            }else if (index != n && Hijos[index+1].n >= min){
                PrestarSucesor(index); 
            }else{ 
                if (index != n){
                    Agrupar(index); 
                }else{
                    Agrupar(index-1); 
                }
            } 
        }
        //elimina un dato de un nodo hoja
        public void RemoveHoja(int index){
            //elimina un dato de l ahoja
            for (int i = index+1; i < n; ++i){
                claves[i-1] = claves[i]; 
                datos[i-1] = datos[i];
            }
            //los datos disminuyen en 1 para este nodo
            n--; 
            //eliminamos los datos a los que no se puede hacer referencia
            for (int i = n; i < 2*min-1; i++) {
                claves[i]=0;
                datos[i]=null;
            }
        }
        //elimina un dato de un nodo no hoja
        public void RemoveNoHoja(int index){
            int k = claves[index]; 
            //si el hijo menor tiene mas datos que el minimo entoncces se hace el intercambio y se elimina el dato intercambiado
            if (Hijos[index].n >= min){ 
                int pred = Predecesor(index); 
                T predData = PredecesorData(index);
                claves[index] = pred; 
                datos[index] = predData;
                Hijos[index].Remove(pred); 
            //si el hijo mayor tiene mas datos que el minimo entonces se hace el intercambio y se elimina el dato intercambiado
            }else if  (Hijos[index + 1].n >= min){ 
                int succ = Sucesor(index);
                T succData = SucesorData(index);
                claves[index] = succ; 
                datos[index] = succData;
                Hijos[index + 1].Remove(succ); 
            }else{ 
            //si los hijos tienen datos minimos entonces se agrupan los hijos y se elimina el dato en el nuevo nodo hijo
                Agrupar(index); 
                Hijos[index].Remove(k); 
            }
        }       
    }
    
    
    //Atributos de la estructura
    private Node<T> raiz;
    private T[] recorrido;
    private T[] recorridoNiveles;
    private int min;
    private Class<T> type;
    private int count;
    private Queue<Node> cola = new Queue<>();
    
    
    
    /**********************/
    /*  METODOS PRIVADOS  */
    /**********************/    
    
    //ordenes establecidos para la estructura
    public enum Order{
        TWO,
        FOUR,
        SIX,
        EIGHT,
        TEN
    }
    //ayudante del buscador de datos
    private T FindSupport(int k, Node<T> nodo){
        int i = 0;
        //buscamos la posicion donde deberia buscar el dato
        while(i < nodo.n && k > nodo.claves[i]){
            i++;
        }
        
        if (i == nodo.n) {
            //si el indice es igual al numero de datos buscamos en los datos mayores
            if (nodo.hoja) {
                return null;
            }else{
                return (T)FindSupport(k, nodo.Hijos[i]);
            }
            
        }else if (k == nodo.claves[i]){
            //retorna si se encuentra el dato
            return nodo.datos[i];
        }else{
            //retorna en sub arbol menor            
            if (nodo.hoja) {
                return null;
            }else{
                return (T)FindSupport(k, nodo.Hijos[i]);
            }
        }
    }
    
    private int posicion = 0;
    //ayudante para obtener datos ordenados
    private void RecorridoOrdenado(T[] array, Node<T> nodo){
        if (nodo != null) {
            for (int i = 0; i < nodo.n; i++) {
                //visitamos su lado mas pequeño
                RecorridoOrdenado(array, nodo.Hijos[i]);
                //imprimos el valor clave
                array[posicion] = nodo.datos[i];
                posicion++;
            }
            //visitamos el hijo mayor 
            RecorridoOrdenado(array, nodo.Hijos[nodo.n]);
        }
        
    }
    //ayudante pra obtener datos por niveles
    private void RecorridoNiveles(T[] array, Node<T> nodo){
        //rellenamos el array con todos los datos del nodo actual
        for (int i = 0; i < nodo.n; i++) {
            array[posicion] = nodo.datos[i];
            posicion++;
        }        
        //añadimos a la cola los nodos hijos siempre que no sea una hoja
        if (!nodo.hoja) {
            for (int i = 0; i <= nodo.n; i++) {
                cola.Enqueue(nodo.Hijos[i]);
            }
        }
    }
    
    
    
    
    /**********************/
    /*  METODOS PUBLICOS  */
    /**
     * @param min la cantidad minima de datos de un nodo
     * @param type el tipo de objeto que se guarda en la estrucutra */    
    
    //constructor de la estructura
    public BTree(Order min, Class<T> type){
        raiz = null;        
        this.type = type;
        switch(min){
            case TWO:
                this.min = 1;
                break;
            case FOUR:
                this.min = 2;
                break;
            case SIX:
                this.min = 3;
                break;
            case EIGHT:
                this.min = 4;
                break;
            case TEN:
                this.min = 5;
                break;
        }
    }
    //Añade un dato a la estructura
    public void Add(int k, T data){
        count++;
        if (raiz == null) {
            //si la raiz es nula creamos un nuevo nodo y ponemos el valor en la primera posicion
            raiz = new Node<>(min,type, true, claveNodo);
            claveNodo++;
            raiz.claves[0] = k;
            raiz.datos[0] = data;
            raiz.n = 1;            
        }else{
            if (raiz.n == 2*min-1) {
                //si la raiz esta llena procedemos a dividir en dos hijos
                //S sera nuestra nueva raiz
                Node<T> s = new Node<>(min, type, false,claveNodo);
                claveNodo++;
                //le ponemos en la primera posicion de S el nodo raiz
                s.Hijos[0] = raiz;
                //enviamos a dividir la raiz en la primera posicon (0)
                s.Split(0, raiz);
                
                int i = 0;
                if (s.claves[0] < k) {
                    //dado que la raiz tiene dos hijos, buscamos en que hijo debe ir la nueva llave
                    i++;
                }
                //enviamos a añadir la llave en el hijo encontrado
                s.Hijos[i].AddNonFull(k, data);
                //hacemos s la nueva raiz
                raiz = s;
            }else{
                //añadimos la llave
                raiz.AddNonFull(k, data);
            }
        }
    }
    //Elimina un dato de la estructura
    public void Remove(int k){
        if (raiz != null) {
            raiz.Remove(k);
            count--;
            if (raiz.n == 0) {
                if (raiz.hoja) {
                    raiz = null;
                }else{
                    raiz = raiz.Hijos[0];
                }
            }
            
        }
    }
    //Devuelve el dato con la llave especificada por el usuario
    public T Find(int k){
        return FindSupport(k, raiz);
    }
    //Devuelve true si el dato se encuentra en la estructura
    public boolean Check(int k){
        T temp = Find(k);
        return temp != null;
    }
    //Devuelve la cantidad de datos en la estructura
    public int Count(){
        return count;
    }
    //Devuelve un array de los datos recorridos de forma ascendente
    public T[] RecorridoOrdenado(){
        posicion = 0;
        recorrido = (T[]) Array.newInstance(type, count);
        RecorridoOrdenado(recorrido, raiz);
        return recorrido;
    }
    //Devuelve un array de los datos recorridos por nivel
    public T[] RecorridoNiveles(){
        posicion = 0;
        recorridoNiveles = (T[]) Array.newInstance(type, count);
        cola.Enqueue(raiz);
        //mientras la cola contiene nodos: recorrer sus datos
        while(cola.Contain()){
            RecorridoNiveles(recorridoNiveles, cola.Dequeue());
        }
        return recorridoNiveles;
    }
    
    String si = " ";
    String grafic;
    String auI=" ";
    String auD=" ";
    int cin = 0;
    public int claveNodo=0;
    
    public String Graficar(String nombre){
        if (raiz != null) {
            grafic = "digraph structs {\n"
                    +"node [shape = record];";
            GenerarDot(raiz);
            grafic = grafic + si;
            grafic = grafic + "}";
            try{
                
                BufferedWriter writer = new BufferedWriter(new FileWriter(nombre+".dot"));
                writer.write(String.valueOf(grafic));
                writer.close();
                File a = new File(nombre+".dot");
                //System.out.println(temp);
                String comando = "dot -Tpng "+a.getAbsolutePath()+" -o"+a.getAbsolutePath().replace(".dot", ".png");
                Runtime.getRuntime().exec(comando);
                return nombre+".png";
            }catch(IOException ex){}
            auI =" ";
            auD = " ";
            si = " ";
            grafic = " ";
        }
        return "";
    }
    
    public void GenerarDot(Node<T> n){
        if (n == raiz) {
           si = si+n.clave+"[label=\"<fo>";
           int r = 0;
            for (int i = 0; i < n.claves.length; i++) {
                if (n.claves[i] != 0) {
                    r = r+1;                    
                    if (n.datos[i] instanceof Libro) {
                        Libro temp = (Libro)n.datos[i];
                        si = si+"|<f22>"+temp.getTitulo()+"("+temp.getISBN()+")"+"|<f"+r+"> ";
                    }
                }
            }
            si = si +"\"];\n";
        }
        
        if (n.Hijos[0] != null) {
            for (int i = 0; i < n.Hijos.length; i++) {
                if (n.Hijos[i] != null) {
                    si = si+n.clave+":f"+i+" -> "+n.Hijos[i].clave+" :here;\n";
                    si = si+n.Hijos[i].clave +"[label = \"<fo>";
                    int r = 0;
                    for (int j = 0; j < n.Hijos[i].claves.length; j++) {
                        if (n.Hijos[i].claves[j] != 0) {
                            r = j+1;
                            if (n.Hijos[i].datos[j] instanceof Libro) {
                                Libro temp = (Libro)n.Hijos[i].datos[j];
                                si = si +" |<f22> "+temp.getTitulo()+"("+temp.getISBN()+")"+" |<f"+r+"> ";
                            }
                            
                        }
                    }
                    si = si+" \"];\n";
                }
            }
        }
        if (n.Hijos[0] != null) {
            for (int i = 0; i < n.Hijos.length; i++) {
                if (n.Hijos[i] != null) {
                    GenerarDot(n.Hijos[i]);
                }
            }
        }
    }
    
    
    
    
    
}
