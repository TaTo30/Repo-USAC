
package Modelos;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.util.logging.Level;
import java.util.logging.Logger;


public class HashTable<T> {
    
    private final int Size;
    private int n;
    private final List<T>[] Elementos;
    private int[] claves;
    
    
    private int Funcion(int clave){
        return clave%Size;
    }
    
    public HashTable(int size){
        this.Size = size;
        this.n = 0;
        Elementos = new List[size];        
        for (int i = 0; i < Elementos.length; i++) {
            Elementos[i] = null;
        }
        claves = new int[size];
    }

    public int Size(){
        return n;
    }
    
    public void Insertar(int clave, T objeto){
        int indice = Funcion(clave);
        if (Elementos[indice] != null) {
            Elementos[indice].AddLast(objeto, clave);
            n++;
        }else{
            Elementos[indice] = new List<>();
            Elementos[indice].AddLast(objeto, clave);
            n++;
        }        
    }
    
    public void Eliminar(int clave){
        int indice = Funcion(clave);
        if (Elementos[indice] != null) {
            Elementos[indice].RemoveKey(clave);
            n--;
        }
        
        if (!Elementos[indice].Contain()) {
             Elementos[indice] = null;
        }
    }
    
    public T Obtener(int clave){
        int indice = Funcion(clave);
        if (Elementos[indice] != null) {
            return Elementos[indice].ElementKey(clave);
        }else{
            return null;
        }
    }
    
    int espacios=0;
    public void Graficar(){
        espacios = 0;
        String graph = "digraph structs {\n node [shape = record];";
        for (int i = 0; i < Elementos.length; i++) {
            if (Elementos[i] != null) {
                List temp = Elementos[i];
                graph += i+"[label=\"<f0>";
                //0[label="<f0>
                for (int j = 0; j < temp.Size(); j++) {
                    Usuario user = (Usuario)temp.ElementAt(j);
                    graph += "| <f22>"+user.getNombre()+" | <f"+(j+1)+">";
                    //0[label="<f0> | <f22> aldo | <f1>
                }
                graph+= "\"]; \n";
                //0[label="<f0> | <f22> aldo | <f1>"];  
                claves[espacios] = i;
                espacios++;
            }
        }
        
        for (int i = 1; i < espacios; i++) {
            graph += claves[i-1] +" -> "+claves[i]+";\n";
        }
        graph += "}";
        
        try{
            BufferedWriter writer = new BufferedWriter(new FileWriter("Usuarios.dot"));
            writer.write(graph);
            writer.close();
            File a = new File("Usuarios.dot");
            String comando = "dot -Tpng "+a.getAbsolutePath()+" -o"+a.getAbsolutePath().replace(".dot", ".png");
            Runtime.getRuntime().exec(comando);
        } catch (IOException ex) {
            Logger.getLogger(HashTable.class.getName()).log(Level.SEVERE, null, ex);
        }
        
    }
    
    
    
    
}
