
package Modelos;


public class Usuario {
    
    private int Carnet;
    private String Nombre;
    private String Apellido;
    private String Carrera;
    private String Password;
    private AVLTree<Categoria> Biblioteca;

    public Usuario(int Carnet, String Nombre, String Apellido, String Carrera, String Password) {
        this.Carnet = Carnet;
        this.Nombre = Nombre;
        this.Apellido = Apellido;
        this.Carrera = Carrera;
        this.Password = Password;
        this.Biblioteca = new AVLTree<>();
    }

    public int getCarnet() {
        return Carnet;
    }

    public void setCarnet(int Carnet) {
        this.Carnet = Carnet;
    }

    public String getNombre() {
        return Nombre;
    }

    public void setNombre(String Nombre) {
        this.Nombre = Nombre;
    }

    public String getApellido() {
        return Apellido;
    }

    public void setApellido(String Apellido) {
        this.Apellido = Apellido;
    }

    public String getCarrera() {
        return Carrera;
    }

    public void setCarrera(String Carrera) {
        this.Carrera = Carrera;
    }

    public String getPassword() {
        return Password;
    }

    public void setPassword(String Password) {
        this.Password = Password;
    }

    public AVLTree<Categoria> getBiblioteca() {
        return Biblioteca;
    }

    public void setBiblioteca(AVLTree<Categoria> Biblioteca) {
        this.Biblioteca = Biblioteca;
    }
    
    
    
    
    
    
}
