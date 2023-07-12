
package Modelos;

public class Libro {
    
    private int ISBN;
    private String Titulo;
    private String Autor;
    private String Editorial;
    private int Ano;
    private int Edicion;
    private String Categoria;
    private String Idioma;
    private int Carnet;

    public Libro(int ISBN, String Titulo, String Autor, String Editorial, int Ano, int Edicion, String Categoria, String Idioma, int Carnet) {
        this.ISBN = ISBN;
        this.Titulo = Titulo;
        this.Autor = Autor;
        this.Editorial = Editorial;
        this.Ano = Ano;
        this.Edicion = Edicion;
        this.Categoria = Categoria;
        this.Idioma = Idioma;
        this.Carnet = Carnet;
    }

    public int getISBN() {
        return ISBN;
    }

    public void setISBN(int ISBN) {
        this.ISBN = ISBN;
    }

    public String getTitulo() {
        return Titulo;
    }

    public void setTitulo(String Titulo) {
        this.Titulo = Titulo;
    }

    public String getAutor() {
        return Autor;
    }

    public void setAutor(String Autor) {
        this.Autor = Autor;
    }

    public String getEditorial() {
        return Editorial;
    }

    public void setEditorial(String Editorial) {
        this.Editorial = Editorial;
    }

    public int getAno() {
        return Ano;
    }

    public void setAno(int Ano) {
        this.Ano = Ano;
    }

    public int getEdicion() {
        return Edicion;
    }

    public void setEdicion(int Edicion) {
        this.Edicion = Edicion;
    }

    public String getCategoria() {
        return Categoria;
    }

    public void setCategoria(String Categoria) {
        this.Categoria = Categoria;
    }

    public String getIdioma() {
        return Idioma;
    }

    public void setIdioma(String Idioma) {
        this.Idioma = Idioma;
    }

    public int getCarnet() {
        return Carnet;
    }

    public void setCarnet(int Carnet) {
        this.Carnet = Carnet;
    }
    
    
    
    
}
