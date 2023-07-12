/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Controlador;
import Modelos.NodoRed;
import java.io.*;
import java.net.*;
import java.util.logging.Level;
import java.util.logging.Logger;
import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;
import org.json.simple.parser.ParseException;
/**
 *
 * @author aldo__nr420yj
 */
public class Conexion extends Thread {
    
    
    Socket cliente;
    Controlador ctrl;
    public Conexion(Socket cliente, Controlador ctrl){
        this.ctrl = ctrl;
        this.cliente = cliente;     
    }
    
    
    @Override
    public void run(){
        try{
            //OBTENEMOS EN LA ENTRADA EL NOMBRE DEL ARCHIVO
            BufferedReader entrada = new BufferedReader(new InputStreamReader(cliente.getInputStream()));
            System.out.println("Eperando datos");
            String archivo = (String) String.valueOf(entrada.readLine());
            System.out.println("Prueba JSON:"+archivo);
            //CREAMOS EL OBJETO JSON A PARTIR DE LA ENTRADA           
            JSONParser json = new JSONParser();
            JSONObject ins = (JSONObject) json.parse(archivo);
            int Instruccion = Integer.parseInt(ins.get("Instruccion").toString());
            switch(Instruccion){
                case 0:
                    System.out.println("---------se ejecuta el modulo de registro");
                    JSONObject data = (JSONObject) ins.get("Dato");
                    String retorno = ctrl.ProcesoAgregarNodo(data.get("IP").toString(), Integer.parseInt(data.get("Puerto").toString()));
                    Socket clienteTemporal = new Socket(data.get("IP").toString(),Integer.parseInt(data.get("Puerto").toString()));
                    PrintStream salida = new PrintStream(clienteTemporal.getOutputStream(),true);
                    salida.println(retorno);
                    salida.close();
                    //salida.flush();
                    break;
                case 1:
                    System.out.println("---------secuencia de carga masiva de nodos");
                    ctrl.RegistrarNodosMasivos(ins.get("Dato").toString());
                    break;
                case 2:
                    System.out.println("---------secuencia de carga auxiliar");
                    JSONObject data0 = (JSONObject) ins.get("Dato");
                    String ip3 = data0.get("IP").toString();
                    int port3 = Integer.parseInt(data0.get("Puerto").toString());
                    ctrl.AgregarNodo(new NodoRed(port3, ip3));
                    break;
                case 3:
                    System.out.println("---------secuencia de eliminar nodos");
                    JSONObject data1 = (JSONObject) ins.get("Dato");
                    String ip4 = data1.get("IP").toString();
                    int port4 = Integer.parseInt(data1.get("Puerto").toString());
                    ctrl.EliminarNodo(ip4, port4);
                    break;
                case 4:
                    System.out.println("---------secuencia de alimentar nodos");
                    JSONObject data2 = (JSONObject) ins.get("Dato");
                    ctrl.CargarBloque((JSONArray)data2.get("DATA"));
                    ctrl.AgregarBloque(data2);
                    break;
                case 5:
                    System.out.println("---------secuencia de envio de blockchain");
                    JSONObject data3 = (JSONObject) ins.get("Dato");
                    String ip5 = data3.get("IP").toString();
                    int port5 = Integer.parseInt(data3.get("Puerto").toString());
                    ctrl.ParseBlockChain(port5, ip5);
                    break;
                case 6:
                    System.out.println("---------secuencia de sincronizacion");
                    JSONArray data4 = (JSONArray) ins.get("Dato");
                    ctrl.BlockChainSync(data4);
                    break;
            }           
            cliente.close();
            System.out.println("Se ha perdido la conexion");
        }catch (IOException e){ System.out.println(e.toString());} catch (ParseException ex) {
            Logger.getLogger(Conexion.class.getName()).log(Level.SEVERE, null, ex);
        }
    }
    
}
