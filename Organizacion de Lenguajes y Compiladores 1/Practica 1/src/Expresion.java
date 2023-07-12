/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package practica;

import javax.swing.table.DefaultTableModel;

/**
 *
 * @author aldo__nr420yj
 */
public class Expresion {
    private NodoArbol arbolExpresiones;
    private DefaultTableModel tablaSiguientes;
    private DefaultTableModel tablaTransiciones;
    private String DirArbolExpresiones;
    private String DirDFA;
    private String Nombre;

    public void setNombre(String Nombre) {
        this.Nombre = Nombre;
    }

    public String getNombre() {
        return Nombre;
    }
    

    public String getDirArbolExpresiones() {
        return DirArbolExpresiones;
    }

    public void setDirArbolExpresiones(String DirArbolExpresiones) {
        this.DirArbolExpresiones = DirArbolExpresiones;
    }

    public String getDirDFA() {
        return DirDFA;
    }

    public void setDirDFA(String DirDFA) {
        this.DirDFA = DirDFA;
    }
    
    

    public DefaultTableModel getTablaTransiciones() {
        return tablaTransiciones;
    }

    public void setTablaTransiciones(DefaultTableModel tablaTransiciones) {
        this.tablaTransiciones = tablaTransiciones;
    }

    public NodoArbol getArbolExpresiones() {
        return arbolExpresiones;
    }

    public void setArbolExpresiones(NodoArbol arbolExpresiones) {
        this.arbolExpresiones = arbolExpresiones;
    }

    public DefaultTableModel getTablaSiguientes() {
        return tablaSiguientes;
    }

    public void setTablaSiguientes(DefaultTableModel tablaSiguientes) {
        this.tablaSiguientes = tablaSiguientes;
    }
    
    
    
}
