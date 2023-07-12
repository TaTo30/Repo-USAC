/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Controlador;
import java.io.IOException;
import java.net.*;

/**
 *
 * @author aldo__nr420yj
 */
public class Servidor extends Thread {
    private int puerto;
    private Controlador ctrl;
    
    public enum Proceso{
        REGISTRONODO
    }
    
    public Servidor(int puerto, Controlador ctrl){
        this.puerto = puerto;
        this.ctrl = ctrl;
    }
    
    
    @Override
    public void run(){
        try{  
            //System.out.println("servidor");
            ServerSocket server = new ServerSocket(puerto);
            System.out.println("Escuchando en el puerto: "+puerto);
            while(true){
                Socket cs = server.accept();
                new Conexion(cs, ctrl).start();
            }
        }catch (IOException ex){}
    }
}
