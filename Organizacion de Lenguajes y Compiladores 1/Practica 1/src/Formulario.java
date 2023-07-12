
package practica;
import java.awt.Image;
import java.awt.event.ActionEvent;
import java.awt.event.ComponentEvent;
import java.awt.event.MouseEvent;
import javax.swing.*;
import java.io.*;
import java.util.LinkedList;
import javax.swing.table.DefaultTableModel;


public class Formulario extends javax.swing.JFrame {
    public String path;
    private String AllPath;
    private int indexLista = 0;
    Analizador scan;
    public LinkedList<Expresion> listaExpresiones = new LinkedList<Expresion>();
    public Formulario() {
        initComponents();
        initComponentsX2();
        scan = new Analizador(this);
        
    }

    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        jScrollPane1 = new javax.swing.JScrollPane();
        logTextArea = new javax.swing.JTextArea();
        jButton1 = new javax.swing.JButton();
        jLabel1 = new javax.swing.JLabel();
        jButton2 = new javax.swing.JButton();
        jButton3 = new javax.swing.JButton();
        jScrollPane3 = new javax.swing.JScrollPane();
        jTable2 = new javax.swing.JTable();
        jScrollPane4 = new javax.swing.JScrollPane();
        jTable3 = new javax.swing.JTable();
        jButton4 = new javax.swing.JButton();
        jButton5 = new javax.swing.JButton();
        label3 = new java.awt.Label();
        jLabel2 = new javax.swing.JLabel();
        jLabel3 = new javax.swing.JLabel();
        jButton6 = new javax.swing.JButton();

        setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);
        setMaximumSize(new java.awt.Dimension(1138, 484));
        setMinimumSize(new java.awt.Dimension(1138, 484));
        setPreferredSize(new java.awt.Dimension(1138, 484));
        getContentPane().setLayout(null);

        logTextArea.setColumns(20);
        logTextArea.setRows(5);
        jScrollPane1.setViewportView(logTextArea);

        getContentPane().add(jScrollPane1);
        jScrollPane1.setBounds(10, 65, 428, 335);

        jButton1.setText("Abri Archivo");
        jButton1.setActionCommand("Abrir Archivo");
        jButton1.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton1ActionPerformed(evt);
            }
        });
        getContentPane().add(jButton1);
        jButton1.setBounds(20, 10, 133, 23);

        jLabel1.setText("Archivo de Entrada:");
        getContentPane().add(jLabel1);
        jLabel1.setBounds(10, 45, 96, 14);

        jButton2.setText("Generar Automata");
        jButton2.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton2ActionPerformed(evt);
            }
        });
        getContentPane().add(jButton2);
        jButton2.setBounds(10, 410, 209, 23);

        jButton3.setText("Analizar Entrada");
        jButton3.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton3ActionPerformed(evt);
            }
        });
        getContentPane().add(jButton3);
        jButton3.setBounds(229, 410, 209, 23);

        jTable2.setModel(new javax.swing.table.DefaultTableModel(
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
        jScrollPane3.setViewportView(jTable2);

        getContentPane().add(jScrollPane3);
        jScrollPane3.setBounds(456, 11, 312, 154);

        jTable3.setModel(new javax.swing.table.DefaultTableModel(
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
        jScrollPane4.setViewportView(jTable3);

        getContentPane().add(jScrollPane4);
        jScrollPane4.setBounds(786, 11, 312, 154);

        jButton4.setText("Anterior");
        jButton4.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton4ActionPerformed(evt);
            }
        });
        getContentPane().add(jButton4);
        jButton4.setBounds(456, 410, 202, 23);

        jButton5.setText("Siguiente");
        jButton5.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton5ActionPerformed(evt);
            }
        });
        getContentPane().add(jButton5);
        jButton5.setBounds(896, 410, 202, 23);
        getContentPane().add(label3);
        label3.setBounds(680, 410, 190, 20);
        getContentPane().add(jLabel2);
        jLabel2.setBounds(460, 180, 310, 220);
        getContentPane().add(jLabel3);
        jLabel3.setBounds(790, 180, 310, 220);

        jButton6.setText("Guardar Archivo");
        jButton6.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton6ActionPerformed(evt);
            }
        });
        getContentPane().add(jButton6);
        jButton6.setBounds(290, 10, 133, 23);

        pack();
    }// </editor-fold>//GEN-END:initComponents
 
    private void initComponentsX2(){
        jLabel2.addMouseListener(new java.awt.event.MouseListener() {
            @Override
            public void mouseClicked(MouseEvent e) {
                ArbolClickEvent(e);
            } 

            @Override
            public void mousePressed(MouseEvent e) {
                
            }

            @Override
            public void mouseReleased(MouseEvent e) {
                
            }

            @Override
            public void mouseEntered(MouseEvent e) {
                
            }

            @Override
            public void mouseExited(MouseEvent e) {
                
            }
        });
        
        jLabel3.addMouseListener(new java.awt.event.MouseListener() {
            @Override
            public void mouseClicked(MouseEvent e) {
                DFAClickEvent(e);
                }

            @Override
            public void mousePressed(MouseEvent e) {
                }

            @Override
            public void mouseReleased(MouseEvent e) {
                }

            @Override
            public void mouseEntered(MouseEvent e) {
                }

            @Override
            public void mouseExited(MouseEvent e) {
                }
        });
    }
    private void jButton1ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton1ActionPerformed
        JFileChooser openFileDialogue = new JFileChooser();
        String texto="", temp="";
        try{
            if (openFileDialogue.showOpenDialog(this)== JFileChooser.APPROVE_OPTION) {
                File openFile = openFileDialogue.getSelectedFile();
                path=openFile.getParent();
                AllPath = openFile.getAbsolutePath();
                BufferedReader bf = new BufferedReader(new FileReader(openFile.getAbsolutePath()));
                temp = bf.readLine();
                while(temp != null){
                    texto += temp + '\n';
                    temp = bf.readLine();                
                }
                //System.out.println(texto);
                logTextArea.setText(texto);
               
                
            }else{
                System.out.println("Sin if");
            }
        }catch(Exception e){
            System.out.println(e.toString());
        }
    }//GEN-LAST:event_jButton1ActionPerformed

    private void jButton2ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton2ActionPerformed
        listaExpresiones.clear();
        scan.scan(logTextArea.getText());
        scan.generarArboles();
        if (!listaExpresiones.isEmpty()) {
            jTable2.setModel(listaExpresiones.getFirst().getTablaSiguientes());
            jTable3.setModel(listaExpresiones.getFirst().getTablaTransiciones());
            label3.setText(listaExpresiones.getFirst().getNombre());            
            ImageIcon fot = new ImageIcon(listaExpresiones.getFirst().getDirArbolExpresiones());
            Icon icono = new ImageIcon(fot.getImage().getScaledInstance(jLabel2.getWidth(), jLabel2.getHeight(), Image.SCALE_DEFAULT));
            jLabel2.setIcon(icono);
            ImageIcon fot1 = new ImageIcon(listaExpresiones.getFirst().getDirDFA());
            Icon icono1 = new ImageIcon(fot1.getImage().getScaledInstance(jLabel3.getWidth(), jLabel3.getHeight(), Image.SCALE_DEFAULT));
            jLabel3.setIcon(icono1);  
            
        }
    }//GEN-LAST:event_jButton2ActionPerformed

    private void jButton5ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton5ActionPerformed
        if (!listaExpresiones.isEmpty() && (indexLista+1 < listaExpresiones.size())) {
            jTable2.setModel(listaExpresiones.get(indexLista+1).getTablaSiguientes());
            jTable3.setModel(listaExpresiones.get(indexLista+1).getTablaTransiciones());
            label3.setText(listaExpresiones.get(indexLista+1).getNombre());
            ImageIcon fot = new ImageIcon(listaExpresiones.get(indexLista+1).getDirArbolExpresiones());
            Icon icono = new ImageIcon(fot.getImage().getScaledInstance(jLabel2.getWidth(), jLabel2.getHeight(), Image.SCALE_DEFAULT));
            jLabel2.setIcon(icono);
            ImageIcon fot1 = new ImageIcon(listaExpresiones.get(indexLista+1).getDirDFA());
            Icon icono1 = new ImageIcon(fot1.getImage().getScaledInstance(jLabel3.getWidth(), jLabel3.getHeight(), Image.SCALE_DEFAULT));
            jLabel3.setIcon(icono1);   
            indexLista++;
        }
    }//GEN-LAST:event_jButton5ActionPerformed

    private void jButton4ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton4ActionPerformed
        if (!listaExpresiones.isEmpty() && (indexLista-1 >= 0)) {
            jTable2.setModel(listaExpresiones.get(indexLista-1).getTablaSiguientes());
            jTable3.setModel(listaExpresiones.get(indexLista-1).getTablaTransiciones());
            label3.setText(listaExpresiones.get(indexLista-1).getNombre());            
            ImageIcon fot = new ImageIcon(listaExpresiones.get(indexLista-1).getDirArbolExpresiones());
            Icon icono = new ImageIcon(fot.getImage().getScaledInstance(jLabel2.getWidth(), jLabel2.getHeight(), Image.SCALE_DEFAULT));
            jLabel2.setIcon(icono);
            ImageIcon fot1 = new ImageIcon(listaExpresiones.get(indexLista-1).getDirDFA());
            Icon icono1 = new ImageIcon(fot1.getImage().getScaledInstance(jLabel3.getWidth(), jLabel3.getHeight(), Image.SCALE_DEFAULT));
            jLabel3.setIcon(icono1); 
            indexLista--;
        }
    }//GEN-LAST:event_jButton4ActionPerformed

    private void jButton3ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton3ActionPerformed
//        scan.ValidarLexemas(listaExpresiones);
    }//GEN-LAST:event_jButton3ActionPerformed

    private void jButton6ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton6ActionPerformed
        try{
            //java.io.BufferedWriter
            BufferedWriter bw = new BufferedWriter(new FileWriter(AllPath));
            bw.write(logTextArea.getText());
            bw.close();
        }catch(Exception e){
            
        }
    }//GEN-LAST:event_jButton6ActionPerformed

    private void ArbolClickEvent(java.awt.event.MouseEvent evt){
        try{
            //Runtime.getRuntime().exec(listaExpresiones.get(indexLista).getDirArbolExpresiones());
            Runtime.getRuntime().load(listaExpresiones.get(indexLista).getDirDFA());
        }catch(Exception e){
            
        }
    }
    private void DFAClickEvent(java.awt.event.MouseEvent evt){
        try{
            System.out.print("evento");
            //Runtime.getRuntime().exec(listaExpresiones.get(indexLista).getDirDFA());
            Runtime.getRuntime().load(listaExpresiones.get(indexLista).getDirDFA());
        }catch(Exception e){
            System.out.print(e);
        }
    }
    public void setTable(DefaultTableModel a){
        jTable2.setModel(a);
    }
    
    public void setTransitionTable(DefaultTableModel a){
        jTable3.setModel(a);
    }
    
    public void addLog(Token token){
        ///logTextArea.append(token.getID() +" - "+token.getContenido()+" - "+token.getTokenTipo()+"\n");
    }

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JButton jButton1;
    private javax.swing.JButton jButton2;
    private javax.swing.JButton jButton3;
    private javax.swing.JButton jButton4;
    private javax.swing.JButton jButton5;
    private javax.swing.JButton jButton6;
    private javax.swing.JLabel jLabel1;
    private javax.swing.JLabel jLabel2;
    private javax.swing.JLabel jLabel3;
    private javax.swing.JScrollPane jScrollPane1;
    private javax.swing.JScrollPane jScrollPane3;
    private javax.swing.JScrollPane jScrollPane4;
    private javax.swing.JTable jTable2;
    private javax.swing.JTable jTable3;
    private java.awt.Label label3;
    private javax.swing.JTextArea logTextArea;
    // End of variables declaration//GEN-END:variables

}
