
package Modelos;


public class Categoria {
    
    private String nombre;
    private BTree<Libro> libreria;
    private int carnet;

    public Categoria(String nombre, int carnet) {
        this.nombre = nombre;
        this.libreria = new BTree<>(BTree.Order.SIX, Libro.class);
        this.carnet = carnet;
    }
    
    public void AgregarLibro(Libro a){
        libreria.Add(a.getISBN(), a);
    }
    
    public void EliminarLibro(int ISBN){
        libreria.Remove(ISBN);
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    public BTree<Libro> getLibreria() {
        return libreria;
    }

    public void setLibreria(BTree<Libro> libreria) {
        this.libreria = libreria;
    }

    public int getCarnet() {
        return carnet;
    }

    public void setCarnet(int carnet) {
        this.carnet = carnet;
    }
    
    
    
}
