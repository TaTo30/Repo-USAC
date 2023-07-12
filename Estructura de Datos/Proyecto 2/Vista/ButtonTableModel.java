/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Vista;

import java.awt.Component;
import javax.swing.JButton;
import javax.swing.JTable;
import javax.swing.table.DefaultTableCellRenderer;


/**
 *
 * @author aldo__nr420yj
 */
public class ButtonTableModel extends DefaultTableCellRenderer{
    @Override
    public Component getTableCellRendererComponent(JTable table, Object o, boolean isSelected, boolean hasFocus, int row, int column){
        if (o instanceof JButton) {
            JButton btn = (JButton)o;
            return btn;
        }
        return super.getTableCellRendererComponent(table, o, isSelected, hasFocus, row, column);
    }
    
}
