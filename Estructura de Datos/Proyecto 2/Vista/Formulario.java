/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Vista;

import Controlador.Controlador;
import Modelos.Block;
import Modelos.Categoria;
import Modelos.Libro;
import Modelos.LinkedList;
import Modelos.List;
import Modelos.NodoRed;
import Modelos.Usuario;
import java.awt.Desktop;
import java.awt.Image;
import java.awt.event.WindowEvent;
import java.awt.image.BufferedImage;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.io.InputStream;
import static java.lang.Thread.sleep;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.swing.DefaultComboBoxModel;
import javax.swing.Icon;
import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JFileChooser;
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import javax.swing.JScrollPane;
import javax.swing.filechooser.FileNameExtensionFilter;
import javax.swing.table.DefaultTableModel;


/**
 *
 * @author aldo__nr420yj
 */
public class Formulario extends javax.swing.JFrame {

    
   private final Controlador ctrl;
   private DefaultTableModel model;
   private DefaultComboBoxModel cbModel;
   private String path;
   /* Creates new form Formulario
     * @param a
     */
    public Formulario(Controlador a) {
        this.ctrl = a;
        initComponents();
        ValoresIniciales();
             
    }
    
    private void ValoresIniciales(){
        this.addWindowListener(new java.awt.event.WindowAdapter() {
            @Override
            public void windowClosing(WindowEvent e) {
                ctrl.ProcesoEliminarNodoCliente();
                try {
                    sleep(2500);
                } catch (InterruptedException ex) {
                    Logger.getLogger(Formulario.class.getName()).log(Level.SEVERE, null, ex);
                }
                super.windowClosing(e); //To change body of generated methods, choose Tools | Templates.
            }
        
        });   
        
        
        this.jTable1.setDefaultRenderer(Object.class, new ButtonTableModel());
        String[] cabeceras = {"ISBN", "Titulo", "Categoria","Dueño", ""};
        this.model = new DefaultTableModel(null, cabeceras);
        this.jTable1.setModel(model);
        
        this.cbModel = new DefaultComboBoxModel(new String[]{"aldo", "hernandez"});
        this.jComboBox1.setModel(cbModel);
        this.jComboBox2.setModel(cbModel);       
        Eventos();
        ActualizarTabla();
        InfoLogeado();
        ComboCategorias();
    }
    
    private void InfoLogeado(){
        Usuario temp = ctrl.ObtenerLogeado();
        jLabelUserInfo.setText("Informacion de Usuario: "+temp.getCarnet()); 
        jTextNameEdit.setText(temp.getNombre());
        jTextLastNameEdit.setText(temp.getApellido());
        jTextCareerEdit.setText(temp.getCarrera());
    }


    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        buttonGroup1 = new javax.swing.ButtonGroup();
        buttonGroup2 = new javax.swing.ButtonGroup();
        buttonGroup3 = new javax.swing.ButtonGroup();
        buttonGroup4 = new javax.swing.ButtonGroup();
        jTabbedPane1 = new javax.swing.JTabbedPane();
        jPanel2 = new javax.swing.JPanel();
        jSeparator2 = new javax.swing.JSeparator();
        jButton5 = new javax.swing.JButton();
        jSeparator3 = new javax.swing.JSeparator();
        jSeparator4 = new javax.swing.JSeparator();
        jLabelUserInfo = new javax.swing.JLabel();
        jLabel21 = new javax.swing.JLabel();
        jTextNameEdit = new javax.swing.JTextField();
        jTextLastNameEdit = new javax.swing.JTextField();
        jLabel23 = new javax.swing.JLabel();
        jTextCareerEdit = new javax.swing.JTextField();
        jLabel24 = new javax.swing.JLabel();
        jButton6 = new javax.swing.JButton();
        jButton7 = new javax.swing.JButton();
        jLabelUserInfo1 = new javax.swing.JLabel();
        jTextAddCat = new javax.swing.JTextField();
        jLabel22 = new javax.swing.JLabel();
        jButton8 = new javax.swing.JButton();
        jComboBox1 = new javax.swing.JComboBox<>();
        jButton9 = new javax.swing.JButton();
        jLabel31 = new javax.swing.JLabel();
        jLabel32 = new javax.swing.JLabel();
        jLabel33 = new javax.swing.JLabel();
        jTextHost = new javax.swing.JTextField();
        jTextPuerto = new javax.swing.JTextField();
        jButton10 = new javax.swing.JButton();
        jLabelEstado = new javax.swing.JLabel();
        jLabelNet = new javax.swing.JLabel();
        jLabel34 = new javax.swing.JLabel();
        jTextField1 = new javax.swing.JTextField();
        jLabel35 = new javax.swing.JLabel();
        jTextField2 = new javax.swing.JTextField();
        jButton17 = new javax.swing.JButton();
        jButton19 = new javax.swing.JButton();
        jButton20 = new javax.swing.JButton();
        jButton21 = new javax.swing.JButton();
        jLabelBlock = new javax.swing.JLabel();
        jPanel1 = new javax.swing.JPanel();
        jSeparator1 = new javax.swing.JSeparator();
        jTextISBN = new javax.swing.JTextField();
        jLabel1 = new javax.swing.JLabel();
        jTextTitulo = new javax.swing.JTextField();
        jLabel2 = new javax.swing.JLabel();
        jTextAutor = new javax.swing.JTextField();
        jLabel3 = new javax.swing.JLabel();
        jTextEdicion = new javax.swing.JTextField();
        jLabel4 = new javax.swing.JLabel();
        jTextCategoria = new javax.swing.JTextField();
        jLabel5 = new javax.swing.JLabel();
        jTextIdioma = new javax.swing.JTextField();
        jLabel6 = new javax.swing.JLabel();
        jButton1 = new javax.swing.JButton();
        jButton2 = new javax.swing.JButton();
        jTextEditorial = new javax.swing.JTextField();
        jLabel7 = new javax.swing.JLabel();
        jTextAno = new javax.swing.JTextField();
        jLabel8 = new javax.swing.JLabel();
        jScrollPane1 = new javax.swing.JScrollPane();
        jTable1 = new javax.swing.JTable();
        jLabel9 = new javax.swing.JLabel();
        jTextISBN1 = new javax.swing.JTextField();
        jLabel10 = new javax.swing.JLabel();
        jTextTitulo1 = new javax.swing.JTextField();
        jLabel11 = new javax.swing.JLabel();
        jTextAutor1 = new javax.swing.JTextField();
        jLabel12 = new javax.swing.JLabel();
        jTextEdicion1 = new javax.swing.JTextField();
        jTextAno1 = new javax.swing.JTextField();
        jLabel13 = new javax.swing.JLabel();
        jTextCategoria1 = new javax.swing.JTextField();
        jLabel14 = new javax.swing.JLabel();
        jTextIdioma1 = new javax.swing.JTextField();
        jLabel15 = new javax.swing.JLabel();
        jTextEditorial1 = new javax.swing.JTextField();
        jLabel16 = new javax.swing.JLabel();
        jLabel17 = new javax.swing.JLabel();
        jTextDueno = new javax.swing.JTextField();
        jLabel18 = new javax.swing.JLabel();
        jButton3 = new javax.swing.JButton();
        jTextCarnet = new javax.swing.JTextField();
        jLabel19 = new javax.swing.JLabel();
        jButton4 = new javax.swing.JButton();
        jTextBuscador = new javax.swing.JTextField();
        jLabel20 = new javax.swing.JLabel();
        jPanel3 = new javax.swing.JPanel();
        jComboBox2 = new javax.swing.JComboBox<>();
        jButton11 = new javax.swing.JButton();
        jScrollPane2 = new javax.swing.JScrollPane();
        Report = new javax.swing.JLabel();
        jLabel25 = new javax.swing.JLabel();
        jLabel26 = new javax.swing.JLabel();
        jButton12 = new javax.swing.JButton();
        jButton13 = new javax.swing.JButton();
        jLabel27 = new javax.swing.JLabel();
        jButton14 = new javax.swing.JButton();
        jLabel28 = new javax.swing.JLabel();
        jButton15 = new javax.swing.JButton();
        jLabel29 = new javax.swing.JLabel();
        jButton16 = new javax.swing.JButton();
        jLabel30 = new javax.swing.JLabel();
        jLabel36 = new javax.swing.JLabel();
        jButton18 = new javax.swing.JButton();
        jLabel37 = new javax.swing.JLabel();
        jButton22 = new javax.swing.JButton();

        setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);

        jPanel2.setLayout(new org.netbeans.lib.awtextra.AbsoluteLayout());
        jPanel2.add(jSeparator2, new org.netbeans.lib.awtextra.AbsoluteConstraints(0, 46, 957, -1));

        jButton5.setText("Terminar Sesion");
        jButton5.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton5ActionPerformed(evt);
            }
        });
        jPanel2.add(jButton5, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 12, 130, -1));

        jSeparator3.setOrientation(javax.swing.SwingConstants.VERTICAL);
        jPanel2.add(jSeparator3, new org.netbeans.lib.awtextra.AbsoluteConstraints(304, 66, 4, 403));

        jSeparator4.setOrientation(javax.swing.SwingConstants.VERTICAL);
        jPanel2.add(jSeparator4, new org.netbeans.lib.awtextra.AbsoluteConstraints(620, 66, -1, 403));

        jLabelUserInfo.setFont(new java.awt.Font("Tahoma", 0, 18)); // NOI18N
        jLabelUserInfo.setText("Informacion de Usuario:");
        jPanel2.add(jLabelUserInfo, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 66, 288, 30));

        jLabel21.setText("Nombre:");
        jPanel2.add(jLabel21, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 107, -1, -1));
        jPanel2.add(jTextNameEdit, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 127, 188, -1));
        jPanel2.add(jTextLastNameEdit, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 178, 188, -1));

        jLabel23.setText("Apellido:");
        jPanel2.add(jLabel23, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 158, -1, -1));
        jPanel2.add(jTextCareerEdit, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 229, 288, -1));

        jLabel24.setText("Carrera:");
        jPanel2.add(jLabel24, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 209, -1, -1));

        jButton6.setText("Actualizar Datos");
        jButton6.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton6ActionPerformed(evt);
            }
        });
        jPanel2.add(jButton6, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 267, 133, -1));

        jButton7.setText("Eliminar Usuario");
        jButton7.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton7ActionPerformed(evt);
            }
        });
        jPanel2.add(jButton7, new org.netbeans.lib.awtextra.AbsoluteConstraints(165, 267, 133, -1));

        jLabelUserInfo1.setFont(new java.awt.Font("Tahoma", 0, 18)); // NOI18N
        jLabelUserInfo1.setText("Categorias:");
        jPanel2.add(jLabelUserInfo1, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 308, 288, 30));
        jPanel2.add(jTextAddCat, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 365, 188, -1));

        jLabel22.setText("Nombre:");
        jPanel2.add(jLabel22, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 344, -1, -1));

        jButton8.setText("Crear");
        jButton8.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton8ActionPerformed(evt);
            }
        });
        jPanel2.add(jButton8, new org.netbeans.lib.awtextra.AbsoluteConstraints(208, 364, 90, -1));

        jComboBox1.setModel(new javax.swing.DefaultComboBoxModel<>(new String[] { "Item 1", "Item 2", "Item 3", "Item 4" }));
        jPanel2.add(jComboBox1, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 399, 188, -1));

        jButton9.setText("Eliminar");
        jButton9.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton9ActionPerformed(evt);
            }
        });
        jPanel2.add(jButton9, new org.netbeans.lib.awtextra.AbsoluteConstraints(208, 398, 90, -1));

        jLabel31.setFont(new java.awt.Font("Tahoma", 0, 18)); // NOI18N
        jLabel31.setText("Configuracion de Red:");
        jPanel2.add(jLabel31, new org.netbeans.lib.awtextra.AbsoluteConstraints(326, 66, -1, -1));

        jLabel32.setText("Host:");
        jPanel2.add(jLabel32, new org.netbeans.lib.awtextra.AbsoluteConstraints(326, 99, -1, -1));

        jLabel33.setText("Puerto:");
        jPanel2.add(jLabel33, new org.netbeans.lib.awtextra.AbsoluteConstraints(470, 99, -1, -1));

        jTextHost.setText("localhost");
        jPanel2.add(jTextHost, new org.netbeans.lib.awtextra.AbsoluteConstraints(326, 118, 131, -1));
        jPanel2.add(jTextPuerto, new org.netbeans.lib.awtextra.AbsoluteConstraints(467, 118, 131, -1));

        jButton10.setText("Guardar Configuracion");
        jButton10.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton10ActionPerformed(evt);
            }
        });
        jPanel2.add(jButton10, new org.netbeans.lib.awtextra.AbsoluteConstraints(326, 149, 272, -1));

        jLabelEstado.setText("No has configurado tus parametros de red:");
        jPanel2.add(jLabelEstado, new org.netbeans.lib.awtextra.AbsoluteConstraints(326, 183, -1, -1));

        jLabelNet.setFont(new java.awt.Font("Tahoma", 0, 18)); // NOI18N
        jLabelNet.setText("¡No estas conectado en la Red!");
        jPanel2.add(jLabelNet, new org.netbeans.lib.awtextra.AbsoluteConstraints(326, 215, -1, -1));

        jLabel34.setText("Puerto:");
        jPanel2.add(jLabel34, new org.netbeans.lib.awtextra.AbsoluteConstraints(326, 268, -1, -1));
        jPanel2.add(jTextField1, new org.netbeans.lib.awtextra.AbsoluteConstraints(326, 288, 75, -1));

        jLabel35.setText("Host:");
        jPanel2.add(jLabel35, new org.netbeans.lib.awtextra.AbsoluteConstraints(427, 265, -1, -1));

        jTextField2.setText("localhost");
        jPanel2.add(jTextField2, new org.netbeans.lib.awtextra.AbsoluteConstraints(427, 288, 76, -1));

        jButton17.setText("Conectar");
        jButton17.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton17ActionPerformed(evt);
            }
        });
        jPanel2.add(jButton17, new org.netbeans.lib.awtextra.AbsoluteConstraints(521, 272, -1, 36));

        jButton19.setText("Update");
        jButton19.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton19ActionPerformed(evt);
            }
        });
        jPanel2.add(jButton19, new org.netbeans.lib.awtextra.AbsoluteConstraints(521, 243, 77, -1));

        jButton20.setText("BLOCK");
        jButton20.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton20ActionPerformed(evt);
            }
        });
        jPanel2.add(jButton20, new org.netbeans.lib.awtextra.AbsoluteConstraints(326, 346, 272, -1));

        jButton21.setText("SYNC");
        jButton21.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton21ActionPerformed(evt);
            }
        });
        jPanel2.add(jButton21, new org.netbeans.lib.awtextra.AbsoluteConstraints(326, 387, 272, -1));
        jPanel2.add(jLabelBlock, new org.netbeans.lib.awtextra.AbsoluteConstraints(326, 326, -1, -1));

        jTabbedPane1.addTab("Perfil", jPanel2);

        jPanel1.setLayout(new org.netbeans.lib.awtextra.AbsoluteLayout());

        jSeparator1.setOrientation(javax.swing.SwingConstants.VERTICAL);
        jPanel1.add(jSeparator1, new org.netbeans.lib.awtextra.AbsoluteConstraints(286, 0, -1, 449));

        jTextISBN.setNextFocusableComponent(jTextTitulo);
        jPanel1.add(jTextISBN, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 20, 258, -1));

        jLabel1.setText("ISBN:");
        jPanel1.add(jLabel1, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 0, -1, -1));

        jTextTitulo.setNextFocusableComponent(jTextAutor);
        jPanel1.add(jTextTitulo, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 71, 258, -1));

        jLabel2.setText("Titulo:");
        jPanel1.add(jLabel2, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 51, -1, -1));

        jTextAutor.setNextFocusableComponent(jTextEdicion);
        jPanel1.add(jTextAutor, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 122, 258, -1));

        jLabel3.setText("Autor:");
        jPanel1.add(jLabel3, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 102, -1, -1));

        jTextEdicion.setNextFocusableComponent(jTextAno);
        jPanel1.add(jTextEdicion, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 173, 112, -1));

        jLabel4.setText("Edicion");
        jPanel1.add(jLabel4, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 153, -1, -1));

        jTextCategoria.setNextFocusableComponent(jTextIdioma);
        jPanel1.add(jTextCategoria, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 224, 258, -1));

        jLabel5.setText("Categoria:");
        jPanel1.add(jLabel5, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 204, -1, -1));

        jTextIdioma.setNextFocusableComponent(jTextEditorial);
        jPanel1.add(jTextIdioma, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 275, 258, -1));

        jLabel6.setText("Idioma:");
        jPanel1.add(jLabel6, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 255, -1, -1));

        jButton1.setText("Cargar Libro");
        jButton1.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton1ActionPerformed(evt);
            }
        });
        jPanel1.add(jButton1, new org.netbeans.lib.awtextra.AbsoluteConstraints(60, 370, 151, -1));

        jButton2.setText("Cargar Archivo (JSON)");
        jButton2.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton2ActionPerformed(evt);
            }
        });
        jPanel1.add(jButton2, new org.netbeans.lib.awtextra.AbsoluteConstraints(19, 415, 223, -1));
        jPanel1.add(jTextEditorial, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 326, 258, -1));

        jLabel7.setText("Editorial:");
        jPanel1.add(jLabel7, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 306, -1, -1));

        jTextAno.setNextFocusableComponent(jTextCategoria);
        jPanel1.add(jTextAno, new org.netbeans.lib.awtextra.AbsoluteConstraints(156, 173, 112, -1));

        jLabel8.setText("Año:");
        jPanel1.add(jLabel8, new org.netbeans.lib.awtextra.AbsoluteConstraints(156, 153, -1, -1));

        jTable1.setModel(new javax.swing.table.DefaultTableModel(
            new Object [][] {
                {null, null, null, null},
                {null, null, null, null},
                {null, null, null, null},
                {null, null, null, null}
            },
            new String [] {
                "Title 1", "Title 2", "Title 3", "Title 4"
            }
        ));
        jScrollPane1.setViewportView(jTable1);

        jPanel1.add(jScrollPane1, new org.netbeans.lib.awtextra.AbsoluteConstraints(298, 11, 378, -1));

        jLabel9.setText("ISBN:");
        jPanel1.add(jLabel9, new org.netbeans.lib.awtextra.AbsoluteConstraints(688, 68, -1, -1));

        jTextISBN1.setEditable(false);
        jTextISBN1.setBackground(new java.awt.Color(255, 255, 255));
        jPanel1.add(jTextISBN1, new org.netbeans.lib.awtextra.AbsoluteConstraints(688, 88, 110, -1));

        jLabel10.setText("Titulo:");
        jPanel1.add(jLabel10, new org.netbeans.lib.awtextra.AbsoluteConstraints(688, 119, -1, -1));

        jTextTitulo1.setEditable(false);
        jTextTitulo1.setBackground(new java.awt.Color(255, 255, 255));
        jPanel1.add(jTextTitulo1, new org.netbeans.lib.awtextra.AbsoluteConstraints(688, 139, 258, -1));

        jLabel11.setText("Autor:");
        jPanel1.add(jLabel11, new org.netbeans.lib.awtextra.AbsoluteConstraints(688, 170, -1, -1));

        jTextAutor1.setEditable(false);
        jTextAutor1.setBackground(new java.awt.Color(255, 255, 255));
        jPanel1.add(jTextAutor1, new org.netbeans.lib.awtextra.AbsoluteConstraints(688, 190, 258, -1));

        jLabel12.setText("Edicion");
        jPanel1.add(jLabel12, new org.netbeans.lib.awtextra.AbsoluteConstraints(688, 221, -1, -1));

        jTextEdicion1.setEditable(false);
        jTextEdicion1.setBackground(new java.awt.Color(255, 255, 255));
        jPanel1.add(jTextEdicion1, new org.netbeans.lib.awtextra.AbsoluteConstraints(688, 241, 112, -1));

        jTextAno1.setEditable(false);
        jTextAno1.setBackground(new java.awt.Color(255, 255, 255));
        jPanel1.add(jTextAno1, new org.netbeans.lib.awtextra.AbsoluteConstraints(834, 241, 112, -1));

        jLabel13.setText("Año:");
        jPanel1.add(jLabel13, new org.netbeans.lib.awtextra.AbsoluteConstraints(834, 221, -1, -1));

        jTextCategoria1.setEditable(false);
        jTextCategoria1.setBackground(new java.awt.Color(255, 255, 255));
        jPanel1.add(jTextCategoria1, new org.netbeans.lib.awtextra.AbsoluteConstraints(839, 88, 107, -1));

        jLabel14.setText("Categoria:");
        jPanel1.add(jLabel14, new org.netbeans.lib.awtextra.AbsoluteConstraints(839, 68, -1, -1));

        jTextIdioma1.setEditable(false);
        jTextIdioma1.setBackground(new java.awt.Color(255, 255, 255));
        jPanel1.add(jTextIdioma1, new org.netbeans.lib.awtextra.AbsoluteConstraints(688, 287, 258, -1));

        jLabel15.setText("Idioma:");
        jPanel1.add(jLabel15, new org.netbeans.lib.awtextra.AbsoluteConstraints(688, 267, -1, -1));

        jTextEditorial1.setEditable(false);
        jTextEditorial1.setBackground(new java.awt.Color(255, 255, 255));
        jPanel1.add(jTextEditorial1, new org.netbeans.lib.awtextra.AbsoluteConstraints(688, 338, 258, -1));

        jLabel16.setText("Editorial:");
        jPanel1.add(jLabel16, new org.netbeans.lib.awtextra.AbsoluteConstraints(688, 318, -1, -1));

        jLabel17.setText("ISBN - TITULO - CATEGORIA");
        jPanel1.add(jLabel17, new org.netbeans.lib.awtextra.AbsoluteConstraints(700, 0, -1, -1));

        jTextDueno.setEditable(false);
        jTextDueno.setBackground(new java.awt.Color(255, 255, 255));
        jPanel1.add(jTextDueno, new org.netbeans.lib.awtextra.AbsoluteConstraints(772, 384, 174, -1));

        jLabel18.setText("Nombre:");
        jPanel1.add(jLabel18, new org.netbeans.lib.awtextra.AbsoluteConstraints(772, 364, -1, -1));

        jButton3.setText("Eliminar Libro");
        jButton3.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton3ActionPerformed(evt);
            }
        });
        jPanel1.add(jButton3, new org.netbeans.lib.awtextra.AbsoluteConstraints(736, 415, 151, -1));

        jTextCarnet.setEditable(false);
        jTextCarnet.setBackground(new java.awt.Color(255, 255, 255));
        jPanel1.add(jTextCarnet, new org.netbeans.lib.awtextra.AbsoluteConstraints(688, 384, 78, -1));

        jLabel19.setText("Carnet:");
        jPanel1.add(jLabel19, new org.netbeans.lib.awtextra.AbsoluteConstraints(694, 364, -1, -1));

        jButton4.setText("Buscar");
        jButton4.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton4ActionPerformed(evt);
            }
        });
        jPanel1.add(jButton4, new org.netbeans.lib.awtextra.AbsoluteConstraints(860, 20, 85, -1));

        jTextBuscador.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jTextBuscadorActionPerformed(evt);
            }
        });
        jPanel1.add(jTextBuscador, new org.netbeans.lib.awtextra.AbsoluteConstraints(690, 20, 165, -1));

        jLabel20.setText("INFORMACION DEL LIBRO:");
        jPanel1.add(jLabel20, new org.netbeans.lib.awtextra.AbsoluteConstraints(686, 48, -1, -1));

        jTabbedPane1.addTab("Biblioteca", jPanel1);

        jPanel3.setLayout(new org.netbeans.lib.awtextra.AbsoluteLayout());

        jComboBox2.setModel(new javax.swing.DefaultComboBoxModel<>(new String[] { "Item 1", "Item 2", "Item 3", "Item 4" }));
        jPanel3.add(jComboBox2, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 98, 212, -1));

        jButton11.setText("Generar Reporte");
        jButton11.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton11ActionPerformed(evt);
            }
        });
        jPanel3.add(jButton11, new org.netbeans.lib.awtextra.AbsoluteConstraints(228, 97, 37, -1));

        jScrollPane2.setViewportView(Report);

        jPanel3.add(jScrollPane2, new org.netbeans.lib.awtextra.AbsoluteConstraints(287, 11, 655, 425));

        jLabel25.setText("Libros:");
        jPanel3.add(jLabel25, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 77, -1, -1));

        jLabel26.setText("Categorias:");
        jPanel3.add(jLabel26, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 33, -1, -1));

        jButton12.setText("Generar Reporte");
        jButton12.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton12ActionPerformed(evt);
            }
        });
        jPanel3.add(jButton12, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 48, 255, -1));

        jButton13.setText("Generar Reporte");
        jButton13.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton13ActionPerformed(evt);
            }
        });
        jPanel3.add(jButton13, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 146, 253, -1));

        jLabel27.setText("Categorias (PREORDEN):");
        jPanel3.add(jLabel27, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 131, -1, -1));

        jButton14.setText("Generar Reporte");
        jButton14.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton14ActionPerformed(evt);
            }
        });
        jPanel3.add(jButton14, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 195, 253, -1));

        jLabel28.setText("Categorias (INORDEN):");
        jPanel3.add(jLabel28, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 180, -1, -1));

        jButton15.setText("Generar Reporte");
        jButton15.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton15ActionPerformed(evt);
            }
        });
        jPanel3.add(jButton15, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 244, 253, -1));

        jLabel29.setText("Categorias (POSTORDEN):");
        jPanel3.add(jLabel29, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 229, -1, -1));

        jButton16.setText("Generar Reporte");
        jButton16.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton16ActionPerformed(evt);
            }
        });
        jPanel3.add(jButton16, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 300, 253, -1));

        jLabel30.setText("Blockchain: ");
        jPanel3.add(jLabel30, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 380, -1, -1));

        jLabel36.setText("Usuarios:");
        jPanel3.add(jLabel36, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 278, -1, -1));

        jButton18.setText("Generar Reporte");
        jButton18.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton18ActionPerformed(evt);
            }
        });
        jPanel3.add(jButton18, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 400, 253, -1));

        jLabel37.setText("Nodos de la Red:");
        jPanel3.add(jLabel37, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 330, -1, -1));

        jButton22.setText("Generar Reporte");
        jButton22.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton22ActionPerformed(evt);
            }
        });
        jPanel3.add(jButton22, new org.netbeans.lib.awtextra.AbsoluteConstraints(10, 350, 253, -1));

        jTabbedPane1.addTab("Reportes", jPanel3);

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);
        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addComponent(jTabbedPane1, javax.swing.GroupLayout.PREFERRED_SIZE, 962, javax.swing.GroupLayout.PREFERRED_SIZE)
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addComponent(jTabbedPane1, javax.swing.GroupLayout.PREFERRED_SIZE, 475, javax.swing.GroupLayout.PREFERRED_SIZE)
        );

        pack();
    }// </editor-fold>//GEN-END:initComponents

    private void jButton3ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton3ActionPerformed
        if (JOptionPane.showConfirmDialog(null, "¿Estas seguro de eliminar este libro?", "Interfaz de eliminacion", JOptionPane.YES_NO_OPTION)==JOptionPane.YES_OPTION) {
            String m = JOptionPane.showInputDialog(null, "Dinos el motivo:", "Eliminacion de Libro", JOptionPane.QUESTION_MESSAGE);
            if (ctrl.EliminarLibro(Integer.parseInt(jTextCarnet.getText()), Integer.parseInt(jTextISBN1.getText()), jTextCategoria1.getText())) {
                JOptionPane.showMessageDialog(null, "Se ha eliminado el libro con el siguiente ISBN: "+jTextISBN1.getText(), "Libro Eliminado", JOptionPane.INFORMATION_MESSAGE);
                jTextISBN1.setText("");
                jTextTitulo1.setText("");
                jTextAutor1.setText("");
                jTextEditorial1.setText("");
                jTextAno1.setText("");
                jTextEdicion1.setText("");
                jTextCategoria1.setText("");
                jTextIdioma1.setText("");
                jTextDueno.setText("");
                jTextCarnet.setText("");
                if (jTextBuscador.getText().isEmpty()) {
                    ActualizarTabla();
                }else{
                    if (isNumeric(jTextBuscador.getText())) {
                        ActualizarTabla();
                        jTextBuscador.setText("");
                    }else{
                        BuscadorLibros();
                    }
                }
                ComboCategorias();
            }
        }
        
        
    }//GEN-LAST:event_jButton3ActionPerformed

    private void jButton2ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton2ActionPerformed
        JFileChooser chooser = new JFileChooser();
        chooser.setAcceptAllFileFilterUsed(false);
        chooser.setFileFilter(new FileNameExtensionFilter("Archivos Json (.json)", "json"));
        chooser.setDialogTitle("Selecciona un archivo de carga de Libros");

        if (chooser.showOpenDialog(null) == JFileChooser.APPROVE_OPTION) {
            File archivo = chooser.getSelectedFile();
            if (ctrl.CargaLibro(archivo.getAbsolutePath())) {
                JOptionPane.showMessageDialog(null, "Se han cargado los libros", "Carga Exitosa", JOptionPane.INFORMATION_MESSAGE);
            }
        }
        ActualizarTabla();
        ComboCategorias();
    }//GEN-LAST:event_jButton2ActionPerformed

    private void jButton1ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton1ActionPerformed
        if (ctrl.CargaLibro(Integer.parseInt(jTextISBN.getText()), jTextTitulo.getText(), jTextAutor.getText(), jTextEditorial.getText(), Integer.parseInt(jTextAno.getText()), Integer.parseInt(jTextEdicion.getText()), jTextCategoria.getText(), jTextIdioma.getText())) {
            JOptionPane.showMessageDialog(null, "Se ha registrado el libro", "Registro Exitoso", JOptionPane.INFORMATION_MESSAGE);
            jTextISBN.setText("");
            jTextTitulo.setText("");
            jTextAutor.setText("");
            jTextEditorial.setText("");
            jTextAno.setText("");
            jTextEdicion.setText("");
            jTextCategoria.setText("");
            jTextIdioma.setText("");
        }
        ActualizarTabla();
        ComboCategorias();
    }//GEN-LAST:event_jButton1ActionPerformed

    private void jButton4ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton4ActionPerformed
        BuscadorLibros();
    }//GEN-LAST:event_jButton4ActionPerformed

    private void jTextBuscadorActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jTextBuscadorActionPerformed
        System.out.println("teclado");
    }//GEN-LAST:event_jTextBuscadorActionPerformed

    private void jButton11ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton11ActionPerformed
        Categoria temp = ctrl.ObtenerCategoria(jComboBox2.getSelectedItem().toString());
        String repo = temp.getLibreria().Graficar(temp.getNombre());
        try {
            sleep(1000);
        } catch (InterruptedException ex) {
            Logger.getLogger(Formulario.class.getName()).log(Level.SEVERE, null, ex);
        }
        path = repo;
        Icon icono = new ImageIcon(repo);
        Report.setIcon(icono);

    }//GEN-LAST:event_jButton11ActionPerformed

    private void jButton12ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton12ActionPerformed
       try {
           ctrl.ImprimirAVL();
           sleep(1000);
       } catch (IOException ex) {
           Logger.getLogger(Formulario.class.getName()).log(Level.SEVERE, null, ex);
       } catch (InterruptedException ex) {
           Logger.getLogger(Formulario.class.getName()).log(Level.SEVERE, null, ex);
       }
       path = "Categorias.png";
       Icon icono = new ImageIcon("Categorias.png");
       Report.setIcon(icono);
    }//GEN-LAST:event_jButton12ActionPerformed

    private void jButton13ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton13ActionPerformed
        String graph = "digraph g{ rankdir = LR; node [shape = box];\n";
        Categoria[] array = ctrl.AVLPreorden();
        for(int i = 1; i < array.length; i++){
            graph += array[i-1].getNombre()+" -> "+array[i].getNombre()+";\n";
        }
        graph+="}";
        System.out.println(graph);
        try{
            BufferedWriter write = new BufferedWriter(new FileWriter("AVLPreorden.dot"));
            write.write(graph);
            write.close();
            path = "AVLPreorden.png";
            File a = new File("AVLPreorden.dot");
            String comando = "dot -Tpng "+a.getAbsolutePath()+" -o"+a.getAbsolutePath().replace(".dot", ".png");
            Runtime.getRuntime().exec(comando);
            sleep(1000);
        }catch(IOException ex){
            
        } catch (InterruptedException ex) {
           Logger.getLogger(Formulario.class.getName()).log(Level.SEVERE, null, ex);
        }
        Icon icono = new ImageIcon("AVLPreorden.png");
        Report.setIcon(icono);
    }//GEN-LAST:event_jButton13ActionPerformed

    private void jButton14ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton14ActionPerformed
        String graph = "digraph g{ rankdir = LR; node [shape = box];\n";
        Categoria[] array = ctrl.AVLInorden();
        for(int i = 1; i < array.length; i++){
            graph += array[i-1].getNombre()+" -> "+array[i].getNombre()+";\n";
        }
        graph+="}";
        System.out.println(graph);
        try{
            BufferedWriter write = new BufferedWriter(new FileWriter("AVLInorden.dot"));
            write.write(graph);
            write.close();
            path = "AVLInorden.png";
            File a = new File("AVLInorden.dot");
            String comando = "dot -Tpng "+a.getAbsolutePath()+" -o"+a.getAbsolutePath().replace(".dot", ".png");
            Runtime.getRuntime().exec(comando);
            sleep(1000);
        }catch(IOException ex){
            
        } catch (InterruptedException ex) {
           Logger.getLogger(Formulario.class.getName()).log(Level.SEVERE, null, ex);
        }
        Icon icono = new ImageIcon("AVLInorden.png");
        Report.setIcon(icono);
    }//GEN-LAST:event_jButton14ActionPerformed

    private void jButton15ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton15ActionPerformed
        String graph = "digraph g{ rankdir = LR; node [shape = box];\n";
        Categoria[] array = ctrl.AVLPostorden();
        for(int i = 1; i < array.length; i++){
            graph += array[i-1].getNombre()+" -> "+array[i].getNombre()+";\n";
        }
        graph+="}";
        System.out.println(graph);
        try{
            BufferedWriter write = new BufferedWriter(new FileWriter("AVLPostorden.dot"));
            write.write(graph);
            write.close();
            path = "AVLPostorden.png";
            File a = new File("AVLPostorden.dot");
            String comando = "dot -Tpng "+a.getAbsolutePath()+" -o"+a.getAbsolutePath().replace(".dot", ".png");
            Runtime.getRuntime().exec(comando);
            sleep(1000);
        }catch(IOException ex){
            
        } catch (InterruptedException ex) {
           Logger.getLogger(Formulario.class.getName()).log(Level.SEVERE, null, ex);
        }
        Icon icono = new ImageIcon("AVLPostorden.png");
        Report.setIcon(icono);
    }//GEN-LAST:event_jButton15ActionPerformed

    private void jButton16ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton16ActionPerformed
        try {
           ctrl.ImprimirHash();
           sleep(1000);
       } catch (InterruptedException ex) {
           Logger.getLogger(Formulario.class.getName()).log(Level.SEVERE, null, ex);
       }
       path = "Usuarios.png";
       Icon icono = new ImageIcon("Usuarios.png");
       Report.setIcon(icono);
    }//GEN-LAST:event_jButton16ActionPerformed

    private void jButton10ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton10ActionPerformed
        ctrl.IniciarServidor(jTextHost.getText(), Integer.parseInt(jTextPuerto.getText()));
        ctrl.IngresarRed();
        ActualizarParametros();
    }//GEN-LAST:event_jButton10ActionPerformed

    private void jButton9ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton9ActionPerformed
        if (JOptionPane.showConfirmDialog(null, "¿Desea eliminar la categoria \""+jComboBox1.getSelectedItem().toString()+"\" de la biblioteca?", "Eliminacion de Categorias", JOptionPane.YES_NO_OPTION)==JOptionPane.YES_OPTION) {
            ctrl.EliminarCategoria(jComboBox1.getSelectedItem().toString());
            ComboCategorias();
            ActualizarTabla();
        }
    }//GEN-LAST:event_jButton9ActionPerformed

    private void jButton8ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton8ActionPerformed
        if (JOptionPane.showConfirmDialog(null, "¿Desea añadir la categoria \""+jTextAddCat.getText()+"\" a la biblioteca?", "Creacion de Categorias", JOptionPane.YES_NO_OPTION) == JOptionPane.YES_OPTION) {
            ctrl.CrearCategoria(jTextAddCat.getText());
            ComboCategorias();
            jTextAddCat.setText("");
        }
    }//GEN-LAST:event_jButton8ActionPerformed

    private void jButton7ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton7ActionPerformed
        if (ctrl.EliminarUsuario()) {
            this.dispose();
        }
    }//GEN-LAST:event_jButton7ActionPerformed

    private void jButton6ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton6ActionPerformed
        if (ctrl.ModificarUsuario(jTextNameEdit.getText(), jTextLastNameEdit.getText(), jTextCareerEdit.getText())) {
            InfoLogeado();
        }
    }//GEN-LAST:event_jButton6ActionPerformed

    private void jButton5ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton5ActionPerformed
        if (ctrl.Deslogin()) {
            this.dispose();
        }
    }//GEN-LAST:event_jButton5ActionPerformed

    private void jButton17ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton17ActionPerformed
        ctrl.RegistroNodoRedClienteSide(Integer.parseInt(jTextField1.getText()), jTextField2.getText());
        ActualizarParametros();
    }//GEN-LAST:event_jButton17ActionPerformed

    private void jButton19ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton19ActionPerformed
        ActualizarParametros();
    }//GEN-LAST:event_jButton19ActionPerformed

    private void jButton20ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton20ActionPerformed
        ctrl.CrearBloque();
        ActualizarTabla();
        ComboCategorias();
    }//GEN-LAST:event_jButton20ActionPerformed

    private void jButton21ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton21ActionPerformed
        ctrl.BlockChainSyncCliente();
        ActualizarTabla();
        ComboCategorias();
    }//GEN-LAST:event_jButton21ActionPerformed

    private void jButton18ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton18ActionPerformed
        LinkedList<Block> blockchain = ctrl.GetBlockchain();
        String graph = "digraph g { rankdir=LR; node [shape = box];\n";
        for (int i = 0; i < blockchain.Size(); i++) {
            Block temp = blockchain.ElementAt(i);
            graph += "N"+i+" [ label = \""+temp.getIndex()+"\\n"+temp.getDate()+"\\n"+temp.getNonce()+"\\n"+temp.getPrevHash()+"\\n"+temp.getHash()+"\"];\n";
        }
        for (int i = 1; i < blockchain.Size(); i++) {
            graph += "N"+(i-1)+" -> N"+i+";\n";
        }
        graph +="}";
        System.out.println(graph);
        try{
            BufferedWriter write = new BufferedWriter(new FileWriter("Blockchain.dot"));
            write.write(graph);
            write.close();
            File a = new File("Blockchain.dot");
            String comando = "dot -Tpng "+a.getAbsolutePath()+" -o"+a.getAbsolutePath().replace(".dot", ".png");
            Runtime.getRuntime().exec(comando);
            sleep(1000);
        }catch(IOException | InterruptedException ex){}
        path = "Blockchain.png";
        Icon icono = new ImageIcon("Blockchain.png");
        Report.setIcon(icono);
    }//GEN-LAST:event_jButton18ActionPerformed

    private void jButton22ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton22ActionPerformed
        List<NodoRed> temp = ctrl.GetNodos();
        String graph = "digraph g { rankdir=LR; node [shape = box];\n";
        for (int i = 0; i < temp.Size(); i++) {
            NodoRed nodo = temp.ElementAt(i);
            graph += "N"+i+" [label = \""+nodo.getIP()+"\\n"+nodo.getPuerto()+"\"];\n";
        }
        for (int i = 1; i < temp.Size(); i++) {
            graph += "N"+(i-1)+" -> N"+i+";\n";
        }
        graph += "}";
        System.out.println(graph);
        try{
            BufferedWriter write = new BufferedWriter(new FileWriter("NodosRed.dot"));
            write.write(graph);
            write.close();
            File a = new File("NodosRed.dot");
            String comando = "dot -Tpng "+a.getAbsolutePath()+" -o"+a.getAbsolutePath().replace(".dot", ".png");
            Runtime.getRuntime().exec(comando);
            sleep(1000);
        }catch(IOException | InterruptedException ex){}
        path = "NodosRed.png";
        Icon icono = new ImageIcon("NodosRed.png");
        Report.setIcon(icono);
    }//GEN-LAST:event_jButton22ActionPerformed

    private void BuscadorLibros(){
        if (isNumeric(jTextBuscador.getText())) {
            Libro temp = ctrl.ObtenerLibro(Integer.parseInt(jTextBuscador.getText()));
            if (temp != null) {
                jTextISBN1.setText(String.valueOf(temp.getISBN()));
                jTextTitulo1.setText(temp.getTitulo());
                jTextAutor1.setText(temp.getAutor());
                jTextEditorial1.setText(temp.getEditorial());
                jTextAno1.setText(String.valueOf(temp.getAno()));
                jTextEdicion1.setText(Integer.toString(temp.getEdicion()));
                jTextCategoria1.setText(temp.getCategoria());
                jTextIdioma1.setText(temp.getIdioma());
                jTextDueno.setText(ctrl.ObtenerUser(temp.getCarnet()).getNombre()+" "+ctrl.ObtenerUser(temp.getCarnet()).getApellido());
                jTextCarnet.setText(Integer.toString(temp.getCarnet())); 
            }else{
                JOptionPane.showMessageDialog(null, "No existe el libro en la biblioteca", "Advertencia", JOptionPane.WARNING_MESSAGE);
                jTextISBN1.setText("");
                jTextTitulo1.setText("");
                jTextAutor1.setText("");
                jTextEditorial1.setText("");
                jTextAno1.setText("");
                jTextEdicion1.setText("");
                jTextCategoria1.setText("");
                jTextIdioma1.setText("");
                jTextDueno.setText("");
                jTextCarnet.setText("");
            }
        }else{
            this.model = null;
            String[] cabeceras = {"ISBN", "Titulo", "Categoria","Dueño", ""};
            this.model = new DefaultTableModel(null, cabeceras);
            Categoria[] array = ctrl.ObtenerCategorias();
            for(Categoria cat : array){
                if (cat != null) {
                    Libro[] libros = cat.getLibreria().RecorridoOrdenado();
                    if (libros != null) {
                        for (Libro lib : libros) {
                            if (lib.getTitulo().contains(jTextBuscador.getText())) {
                                this.model.addRow(new Object[]{lib.getISBN(),lib.getTitulo(),lib.getCategoria(),ctrl.ObtenerUser(lib.getCarnet()).getNombre(), new JButton("ver")});
                            }else{
                                if (lib.getCategoria().contains(jTextBuscador.getText())) {
                                    this.model.addRow(new Object[]{lib.getISBN(),lib.getTitulo(),lib.getCategoria(),ctrl.ObtenerUser(lib.getCarnet()).getNombre(), new JButton("ver")});    
                                }
                            }
                        }
                    }
                }
            }
            jTable1.setModel(model);
        }
    }
    
    
    private boolean isNumeric(String s){
        try{
            Integer.parseInt(s);
            return true;
        }catch(NumberFormatException e){
            return false;
        }
    }
    
    public void ConnectMessage(String msg){
        jLabelNet.setText(msg);
    }
    
    public void BlockMessage(String msg){
        jLabelBlock.setText(msg);
    }

    public void ActualizarTabla(){
        this.model = null;
        String[] cabeceras = {"ISBN", "Titulo", "Categoria","Dueño", ""};
        this.model = new DefaultTableModel(null, cabeceras);
        Categoria[] array = ctrl.ObtenerCategorias();
        for (Categoria cat : array) {
            if (cat != null) {
                Libro[] libros = cat.getLibreria().RecorridoOrdenado();
                for (Libro lib : libros) {
                    if (lib != null) {
                        Usuario temp = ctrl.ObtenerUser(lib.getCarnet());
                        model.addRow(new Object[]{lib.getISBN(), lib.getTitulo(), lib.getCategoria(), temp.getNombre(), new JButton("ver")});
                    }    
                }
            }
        }
        
        this.jTable1.setModel(model);
    }
    
    private void InfoLibro(Libro lib){
        jTextISBN1.setText(String.valueOf(lib.getISBN()));
        jTextTitulo1.setText(lib.getTitulo());
        jTextAutor1.setText(lib.getAutor());
        jTextEditorial1.setText(lib.getEditorial());
        jTextAno1.setText(String.valueOf(lib.getAno()));
        jTextEdicion1.setText(Integer.toString(lib.getEdicion()));
        jTextCategoria1.setText(lib.getCategoria());
        jTextIdioma1.setText(lib.getIdioma());
        jTextDueno.setText(ctrl.ObtenerUser(lib.getCarnet()).getNombre()+" "+ctrl.ObtenerUser(lib.getCarnet()).getApellido());
        jTextCarnet.setText(Integer.toString(lib.getCarnet())); 
        
    }
     
    public void ComboCategorias(){
        Categoria[] categorias = ctrl.ObtenerCategorias();
        this.cbModel.removeAllElements();
        for(Categoria cat : categorias){
            if (cat != null) {
                this.cbModel.addElement(cat.getNombre());
            }
        }
        this.jComboBox1.setModel(cbModel);
        this.jComboBox2.setModel(cbModel);
    } 
    //Metodo global que maneja eventos de cada uno de los componentes
    private void Eventos(){
        jTable1.addMouseListener(new java.awt.event.MouseAdapter() {
        @Override
        public void mouseClicked(java.awt.event.MouseEvent e){
            int fila = jTable1.rowAtPoint(e.getPoint());
            int columna = jTable1.columnAtPoint(e.getPoint());
            if (columna == 4) {
                Libro temp = ctrl.ObtenerLibro(Integer.parseInt(jTable1.getValueAt(fila, 0).toString()));
                InfoLibro(temp);
            }                
        }
        });
        
        this.jTextBuscador.addKeyListener(new java.awt.event.KeyAdapter() {
            @Override
            public void keyTyped(java.awt.event.KeyEvent e){
                if (!isNumeric(jTextBuscador.getText())) {
                    BuscadorLibros();
                    jTextISBN1.setText("");
                    jTextTitulo1.setText("");
                    jTextAutor1.setText("");
                    jTextEditorial1.setText("");
                    jTextAno1.setText("");
                    jTextEdicion1.setText("");
                    jTextCategoria1.setText("");
                    jTextIdioma1.setText("");
                    jTextDueno.setText("");
                    jTextCarnet.setText("");
                }
            }
        });
        
       this.Report.addMouseListener(new java.awt.event.MouseAdapter() {
            @Override
            public void mouseClicked(java.awt.event.MouseEvent e){
                try{
                    File a = new File(path);
                    Desktop.getDesktop().open(a);
                }catch (IOException ex){
                    System.out.println(ex.toString());
                }
            }
        });
        
    }
    
    private void ActualizarParametros(){
        if (ctrl.PruebaRed()) {
            jLabelNet.setText("¡Estas conectado en la Red!"); 
        }else{
            jLabelNet.setText("¡No estas conectado en la Red!"); 
        }
        jLabelEstado.setText(ctrl.Parametraje());
    }
    
    
    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JLabel Report;
    private javax.swing.ButtonGroup buttonGroup1;
    private javax.swing.ButtonGroup buttonGroup2;
    private javax.swing.ButtonGroup buttonGroup3;
    private javax.swing.ButtonGroup buttonGroup4;
    private javax.swing.JButton jButton1;
    private javax.swing.JButton jButton10;
    private javax.swing.JButton jButton11;
    private javax.swing.JButton jButton12;
    private javax.swing.JButton jButton13;
    private javax.swing.JButton jButton14;
    private javax.swing.JButton jButton15;
    private javax.swing.JButton jButton16;
    private javax.swing.JButton jButton17;
    private javax.swing.JButton jButton18;
    private javax.swing.JButton jButton19;
    private javax.swing.JButton jButton2;
    private javax.swing.JButton jButton20;
    private javax.swing.JButton jButton21;
    private javax.swing.JButton jButton22;
    private javax.swing.JButton jButton3;
    private javax.swing.JButton jButton4;
    private javax.swing.JButton jButton5;
    private javax.swing.JButton jButton6;
    private javax.swing.JButton jButton7;
    private javax.swing.JButton jButton8;
    private javax.swing.JButton jButton9;
    private javax.swing.JComboBox<String> jComboBox1;
    private javax.swing.JComboBox<String> jComboBox2;
    private javax.swing.JLabel jLabel1;
    private javax.swing.JLabel jLabel10;
    private javax.swing.JLabel jLabel11;
    private javax.swing.JLabel jLabel12;
    private javax.swing.JLabel jLabel13;
    private javax.swing.JLabel jLabel14;
    private javax.swing.JLabel jLabel15;
    private javax.swing.JLabel jLabel16;
    private javax.swing.JLabel jLabel17;
    private javax.swing.JLabel jLabel18;
    private javax.swing.JLabel jLabel19;
    private javax.swing.JLabel jLabel2;
    private javax.swing.JLabel jLabel20;
    private javax.swing.JLabel jLabel21;
    private javax.swing.JLabel jLabel22;
    private javax.swing.JLabel jLabel23;
    private javax.swing.JLabel jLabel24;
    private javax.swing.JLabel jLabel25;
    private javax.swing.JLabel jLabel26;
    private javax.swing.JLabel jLabel27;
    private javax.swing.JLabel jLabel28;
    private javax.swing.JLabel jLabel29;
    private javax.swing.JLabel jLabel3;
    private javax.swing.JLabel jLabel30;
    private javax.swing.JLabel jLabel31;
    private javax.swing.JLabel jLabel32;
    private javax.swing.JLabel jLabel33;
    private javax.swing.JLabel jLabel34;
    private javax.swing.JLabel jLabel35;
    private javax.swing.JLabel jLabel36;
    private javax.swing.JLabel jLabel37;
    private javax.swing.JLabel jLabel4;
    private javax.swing.JLabel jLabel5;
    private javax.swing.JLabel jLabel6;
    private javax.swing.JLabel jLabel7;
    private javax.swing.JLabel jLabel8;
    private javax.swing.JLabel jLabel9;
    private javax.swing.JLabel jLabelBlock;
    private javax.swing.JLabel jLabelEstado;
    private javax.swing.JLabel jLabelNet;
    private javax.swing.JLabel jLabelUserInfo;
    private javax.swing.JLabel jLabelUserInfo1;
    private javax.swing.JPanel jPanel1;
    private javax.swing.JPanel jPanel2;
    private javax.swing.JPanel jPanel3;
    private javax.swing.JScrollPane jScrollPane1;
    private javax.swing.JScrollPane jScrollPane2;
    private javax.swing.JSeparator jSeparator1;
    private javax.swing.JSeparator jSeparator2;
    private javax.swing.JSeparator jSeparator3;
    private javax.swing.JSeparator jSeparator4;
    private javax.swing.JTabbedPane jTabbedPane1;
    private javax.swing.JTable jTable1;
    private javax.swing.JTextField jTextAddCat;
    private javax.swing.JTextField jTextAno;
    private javax.swing.JTextField jTextAno1;
    private javax.swing.JTextField jTextAutor;
    private javax.swing.JTextField jTextAutor1;
    private javax.swing.JTextField jTextBuscador;
    private javax.swing.JTextField jTextCareerEdit;
    private javax.swing.JTextField jTextCarnet;
    private javax.swing.JTextField jTextCategoria;
    private javax.swing.JTextField jTextCategoria1;
    private javax.swing.JTextField jTextDueno;
    private javax.swing.JTextField jTextEdicion;
    private javax.swing.JTextField jTextEdicion1;
    private javax.swing.JTextField jTextEditorial;
    private javax.swing.JTextField jTextEditorial1;
    private javax.swing.JTextField jTextField1;
    private javax.swing.JTextField jTextField2;
    private javax.swing.JTextField jTextHost;
    private javax.swing.JTextField jTextISBN;
    private javax.swing.JTextField jTextISBN1;
    private javax.swing.JTextField jTextIdioma;
    private javax.swing.JTextField jTextIdioma1;
    private javax.swing.JTextField jTextLastNameEdit;
    private javax.swing.JTextField jTextNameEdit;
    private javax.swing.JTextField jTextPuerto;
    private javax.swing.JTextField jTextTitulo;
    private javax.swing.JTextField jTextTitulo1;
    // End of variables declaration//GEN-END:variables
}
