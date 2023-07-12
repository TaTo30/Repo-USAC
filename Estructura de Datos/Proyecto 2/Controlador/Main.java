/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Controlador;


import Modelos.*;
import Vista.Login;
import java.io.FileReader;
import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;


/**
 *
 * @author aldo__nr420yj
 */
public class Main {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        
        Controlador ctrl = new Controlador();
        Login login = new Login(ctrl);
        
        login.setVisible(true);

        
        
        /*HashTable<Usuario> users = new HashTable<>(45);
        users.Insertar(201800585, new Usuario(201800585, "Aldo", "Rigoberto", "Sistemas", "Rigoberto30" ));
        
        System.out.println(users.Obtener(201800585).getNombre());
        
        users.Eliminar(201800585);
        
        users.Insertar(201800585, new Usuario(201800585, "Aldo", "Rigoberto", "Sistemas", "Rigoberto30" ));
        users.Insertar(201800535, new Usuario(201800585, "Aldo", "Rigoberto", "Sistemas", "Rigoberto30" ));

        
        
        System.out.println("declaracion");*/
        
        
        /*JSONParser json = new JSONParser();

        try{
            FileReader reader = new FileReader("C:\\Users\\aldo__nr420yj\\Desktop\\Libros.json");
            JSONObject obj = (JSONObject) json.parse(reader);
            JSONArray objetolibros = (JSONArray) obj.get("libros");
            
            objetolibros.forEach(emp -> parseLibro((JSONObject) emp));
            reader.close();
            
            
            FileReader readerUser = new FileReader("C:\\Users\\aldo__nr420yj\\Desktop\\usuarios.json");
            JSONObject obj2 = (JSONObject) json.parse(readerUser);
            JSONArray objetoUsuarios = (JSONArray) obj2.get("Usuarios");
            
            objetoUsuarios.forEach(objj -> parseUsuario((JSONObject) objj));

            
        }catch(Exception e){
            System.out.println(e);
        }
        // TODO code application logic here*/
    }
    
    
    
    
    
    
    
    
    /*private static void parseLibro(JSONObject libro){
        System.out.println("");
        System.out.println(libro.get("ISBN"));
        System.out.println(libro.get("Año"));
        System.out.println(libro.get("Editorial"));
        System.out.println(libro.get("Autor"));
        System.out.println(libro.get("Titulo"));
        System.out.println(libro.get("Categoria"));
        System.out.println(libro.get("Edicion"));
        System.out.println(libro.get("Idioma"));
    }
    
    private static void parseUsuario(JSONObject user){
        System.out.println("");
        System.out.println(user.get("Carnet"));
        System.out.println(user.get("Nombre"));
        System.out.println(user.get("Apellido"));
        System.out.println(user.get("Carrera"));
        System.out.println(user.get("Password"));
    }*/
    
}
