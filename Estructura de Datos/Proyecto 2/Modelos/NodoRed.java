
package Modelos;


public class NodoRed {
    
    private int puerto;
    private String IP;

    public NodoRed(int puerto, String IP) {
        this.puerto = puerto;
        this.IP = IP;
    }
    
    public NodoRed(String puerto, String IP){
        this.puerto = Integer.parseInt(puerto);
        this.IP = IP;
    }

    public int getPuerto() {
        return puerto;
    }

    public void setPuerto(int puerto) {
        this.puerto = puerto;
    }

    public String getIP() {
        return IP;
    }

    public void setIP(String IP) {
        this.IP = IP;
    }
    
    
    
}
