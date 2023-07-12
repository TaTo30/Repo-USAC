package practica;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.util.LinkedList;
import java.util.Queue;
import javax.swing.JTable;
import javax.swing.table.DefaultTableModel;
import javax.swing.table.TableColumn;


public class Analizador {
    private String ID;
    Formulario form;
    LinkedList<Token> listaToken = new LinkedList<Token>();
    LinkedList<NodoArbol> listaExpresion = new LinkedList<NodoArbol>();
    //LinkedList<Expresion> Expresiones = new LinkedList<Expresion>();
    Analizador(Formulario a){
        this.form = a;
    }       
    public void scan(String entrada){
        entrada += '#';
        int estadoActual = 0;
        String lexema="";
        char c;
        //System.out.print(entrada);
        for (int i = 0; i < entrada.length(); i++) {
            c = entrada.charAt(i);
            switch(estadoActual){
                case 0:
                    switch(c){
                        case ' ':
                        case '\n':
                        case '{':
                        case '}':
                        case '%':
                            estadoActual = 0;
                            break;
                        case '/':
                            lexema += c;
                            estadoActual = 1;
                            break;
                        case '<':
                            lexema += c;
                            estadoActual = 3;
                            break;
                        case 'C':
                            lexema += c;
                            estadoActual = 4;
                            break;
                        default:
                            lexema += c;
                            estadoActual = 2;
                            break;
                    }
                    break;
                    
                case 1:
                    if (c == '/') {
                        lexema += c;
                        estadoActual  = 5;
                    }
                    break;
                case 2:
                    if (c == ' ') {
                        //No hacer nada :p
                    }else if (c == '-') {
                        ID = lexema;
                        lexema = "";
                        estadoActual = 6;
                    }else{
                        lexema += c;
                    }
                case 3:
                    if (c == '!') {
                        lexema += c;
                        estadoActual = 7;
                    }
                    break;
                case 4:
                    if (c == 'O') {
                        lexema += c;
                        estadoActual = 8;
                    }
                    break;
                case 5:
                    if (c == '\n') {
                        logToken(lexema, "", Token.Tipo.COMENTARIO);
                        lexema = "";
                        estadoActual = 0;
                    }else{
                        lexema += c;
                    }
                    break;
                case 6:
                    if (c == '>') {
                        estadoActual = 9;
                    }
                    break;
                case 7:
                    if (c == '!') {
                        lexema +=c;
                        estadoActual = 10;
                    }else{
                        lexema +=c;                        
                    }
                    break;
                case 8:
                    if (c == 'N') {
                        lexema += c;
                        estadoActual = 18;
                    }
                    break;
                case 9:
                    if (c == ' ') {
                        estadoActual = 9;
                    }else if (c == '"') {                        
                        estadoActual = 12;
                    }else{
                        lexema +=c;
                        estadoActual = 11;
                    }
                    break;
                case 10:
                    if (c == '>') {
                        lexema += c;
                        logToken(lexema, "", Token.Tipo.COMENTARIOML);
                        lexema ="";
                        estadoActual = 0;
                    }
                    break;
                case 11:
                    if (c == ';') {
                        logToken(lexema, ID, Token.Tipo.EXPRESION);
                        estadoActual = 0;
                        lexema = "";
                        ID = "";
                    }else{
                        lexema +=c;
                    }
                    break;
                case 12:                    
                    if (c == '"') {
                        estadoActual = 15;
                    }else{
                        lexema +=c;
                    }
                    break;
                case 15:
                    if (c == ';') {
                        logToken(lexema, ID, Token.Tipo.LEXEMA);
                        lexema ="";
                        ID = "";
                        estadoActual = 0;
                    }
                    break;
                case 18:
                    if (c == 'J') {
                        lexema += c;
                        estadoActual = 19;
                    }
                    break;
                case 19:
                    if (c == ':') {
                        lexema ="";
                        estadoActual = 21;
                    }
                    break;
                case 21:
                    if (c == '-') {
                        //lexema += c;
                        ID = lexema;
                        lexema = "";
                        estadoActual = 22;
                    }else if (c != ' ') {
                        lexema +=c;
                    }
                    break;
                case 22:
                    if (c == '>') {
                        //lexema += c;
                        estadoActual = 23;
                    }
                    break;
                case 23:
                    if ((int)c > 33 && (int)c < 126) {
                        lexema += c;
                        estadoActual = 24;
                    }
                    break;
                case 24:
                    if (c == '~') {
                        lexema += c;
                        estadoActual = 25;
                    }
                    break;
                case 25:
                    if ((int)c > 33 && (int)c < 126) {
                        lexema += c;
                        estadoActual = 26;
                    }
                    break;
                case 26:
                    if (c == ';') {
                        logToken(lexema, ID, Token.Tipo.CONJUNTO);
                        lexema ="";
                        estadoActual = 0;
                        ID = "";
                    }
                    break;                   
            }
        }
    }
    
    private void logToken(String lexema, String ID, Token.Tipo tipo){
        //System.out.println(lexema+" - "+ID +" - "+tipo);
        form.addLog(new Token(ID, lexema, tipo));
        if (tipo == Token.Tipo.EXPRESION || tipo == Token.Tipo.LEXEMA || tipo == Token.Tipo.CONJUNTO) {
            listaToken.add(new Token(ID,lexema, tipo));
        }
        
    }
    
    public void generarArboles(){
        for (Token elemento : listaToken){
            if (elemento.getTokenTipo() == Token.Tipo.EXPRESION) {
                generarArbol(elemento.getContenido(), elemento.getID());
            }
        }
    }
    
    
    public void generarArbol(String cadena, String nombre){
    listaExpresion.clear();
    int contadorNodos = 0;  
    int contadorHojas = 1;
    char c;
    String hoja=""; 
    String graph = "digraph G{\n   node [shape=circle fontname=Arial];\nedge [arrowhead=none,arrowtail=dot]; \n";
        for (int i = 0; i < cadena.length(); i++) {
            c = cadena.charAt(i);
            switch(c){
                case '.':                    
                    graph += contadorNodos +" [label=\".\"]\n"; 
                    listaExpresion.add(new NodoArbol(NodoArbol.TipoOperador.OPERADOR, Token.Tipo.CONCATENACION,".",null,null,"","",contadorNodos,false));
                    contadorNodos++;
                    break;
                case '|':                   
                    graph += contadorNodos +" [label=\"\\|\"]\n";
                    listaExpresion.add(new NodoArbol(NodoArbol.TipoOperador.OPERADOR, Token.Tipo.DISYUNCION,"|",null,null,"","",contadorNodos,false));
                    
                    contadorNodos++;
                    break;
                case '?':                    
                    graph += contadorNodos +" [label=\"?\"]\n";
                    listaExpresion.add(new NodoArbol(NodoArbol.TipoOperador.OPERADORUNITARIO, Token.Tipo.INTERROGACION,"?",null,null,"","",contadorNodos,true));
                    contadorNodos++;
                    break;
                case '*':                  
                    graph += contadorNodos +" [label=\"*\"]\n";
                    listaExpresion.add(new NodoArbol(NodoArbol.TipoOperador.OPERADORUNITARIO, Token.Tipo.CERRADURAKLEENE,"*",null,null,"","",contadorNodos, true));
                    contadorNodos++;
                    break;
                case '+':                    
                    graph += contadorNodos +" [label=\"+\"]\n";
                    listaExpresion.add(new NodoArbol(NodoArbol.TipoOperador.OPERADORUNITARIO, Token.Tipo.CERRADURAPOSITIVA,"+",null,null,"","",contadorNodos,false));
                    contadorNodos++;
                    break;                
                case '{':
                    i++;
                    c=cadena.charAt(i);
                    while(c!='}'){
                        hoja += c;
                        i++;
                        c = cadena.charAt(i);                        
                    }
                    if (!hoja.equals("")) {
                        graph += contadorNodos +" [label=\""+hoja+"\"]\n";
                        listaExpresion.add(new NodoArbol(NodoArbol.TipoOperador.OPERANDO, Token.Tipo.HOJA,hoja,null,null,String.valueOf(contadorHojas),String.valueOf(contadorHojas),contadorNodos,false));
                        contadorNodos++;
                        contadorHojas++;
                        hoja="";
                    }
                    break;
                case '\"':
                    i++;
                    c=cadena.charAt(i);
                    while(c!='\"'){
                        hoja += c;
                        i++;
                        c = cadena.charAt(i);                        
                    }
                    if (!hoja.equals("")) {
                        graph += contadorNodos +" [label=\""+hoja+"\"]\n";
                        listaExpresion.add(new NodoArbol(NodoArbol.TipoOperador.OPERANDO, Token.Tipo.HOJA,hoja,null,null,String.valueOf(contadorHojas),String.valueOf(contadorHojas),contadorNodos,false));
                        contadorNodos++;
                        contadorHojas++;
                        hoja="";
                    }
                    break;
                case ' ':
                    break;
                
                default:
                    hoja+=c;
                    break;
            }                    
        }
        /**FORMAR ARBOL**/
        System.out.println("a formar el arbol");
        while(listaExpresion.size() > 1){
            for (int i = 0; i < listaExpresion.size(); i++) {
                if ((listaExpresion.get(i).TipoTokenOperador == NodoArbol.TipoOperador.OPERANDO)&&(listaExpresion.get(i-1).TipoTokenOperador == NodoArbol.TipoOperador.OPERANDO)) {
                    listaExpresion.get(i-2).setHijoDerecho(listaExpresion.get(i));
                    listaExpresion.get(i-2).setHijoIzquierdo(listaExpresion.get(i-1));
                    if (listaExpresion.get(i-2).TipoExpresion == Token.Tipo.DISYUNCION) {
                        if (listaExpresion.get(i).Anulable || listaExpresion.get(i-1).Anulable) {
                            listaExpresion.get(i-2).setAnulable(true);
                        }
                        listaExpresion.get(i-2).setPrimero(listaExpresion.get(i-1).getPrimero());
                        listaExpresion.get(i-2).setPrimero(listaExpresion.get(i).getPrimero());
                        listaExpresion.get(i-2).setUltimo(listaExpresion.get(i-1).getUltimo());
                        listaExpresion.get(i-2).setUltimo(listaExpresion.get(i).getUltimo());
                        
                    }else if (listaExpresion.get(i-2).TipoExpresion == Token.Tipo.CONCATENACION) {
                        if (listaExpresion.get(i).Anulable && listaExpresion.get(i-1).Anulable) {
                            listaExpresion.get(i-2).setAnulable(true);
                        }
                        if (listaExpresion.get(i-1).Anulable) {
                           listaExpresion.get(i-2).setPrimero(listaExpresion.get(i-1).getPrimero()); 
                           listaExpresion.get(i-2).setPrimero(listaExpresion.get(i).getPrimero()); 
                        }else{
                           listaExpresion.get(i-2).setPrimero(listaExpresion.get(i-1).getPrimero()); 
                        }
                        if (listaExpresion.get(i).Anulable) {
                           listaExpresion.get(i-2).setUltimo(listaExpresion.get(i-1).getUltimo()); 
                           listaExpresion.get(i-2).setUltimo(listaExpresion.get(i).getUltimo()); 
                        }else{
                           listaExpresion.get(i-2).setUltimo(listaExpresion.get(i).getUltimo()); 
                        }
                    }
                    listaExpresion.get(i-2).setTipoTokenOperador(NodoArbol.TipoOperador.OPERANDO);
                    graph +=listaExpresion.get(i-2).Indice+" -> "+listaExpresion.get(i).Indice+"\n";
                    graph +=listaExpresion.get(i-2).Indice+" -> "+listaExpresion.get(i-1).Indice+"\n";
                    listaExpresion.remove(i);
                    listaExpresion.remove(i-1);
                    i = listaExpresion.size()+1;
                }else if ((listaExpresion.get(i).TipoTokenOperador == NodoArbol.TipoOperador.OPERANDO) && (listaExpresion.get(i-1).TipoTokenOperador)==NodoArbol.TipoOperador.OPERADORUNITARIO) {
                    listaExpresion.get(i-1).setHijoIzquierdo(listaExpresion.get(i));
                    
                    if ((listaExpresion.get(i-1).TipoExpresion == Token.Tipo.CERRADURAPOSITIVA)&&listaExpresion.get(i).Anulable) {
                        listaExpresion.get(i-1).setAnulable(true);
                    }
                    listaExpresion.get(i-1).setPrimero(listaExpresion.get(i).getPrimero());
                    listaExpresion.get(i-1).setUltimo(listaExpresion.get(i).getUltimo());
                    listaExpresion.get(i-1).setTipoTokenOperador(NodoArbol.TipoOperador.OPERANDO);
                    graph +=listaExpresion.get(i-1).Indice+" -> "+listaExpresion.get(i).Indice+"\n";
                    listaExpresion.remove(i);
                    i = listaExpresion.size()+1;
                }
            }
        }                   
        graph += "}";
        System.out.println(graph);       
        String DirectorioArchivo = form.path+"\\"+nombre+".dot";
        String DirectorioArchivoImagen = form.path+"\\"+nombre+".png";
        try{
            File destino = new File(DirectorioArchivo);
            if (!destino.exists()) {
                destino.createNewFile();
            }
            BufferedWriter bw = new BufferedWriter(new FileWriter(destino));
            bw.write(graph);
            bw.close();
            String Comando="dot -Tpng "+DirectorioArchivo+" -o "+form.path+"\\"+nombre+".png";
            System.out.print(Comando);
            Runtime.getRuntime().exec("dot -Tpng "+DirectorioArchivo+" -o "+DirectorioArchivoImagen);
        }catch(Exception e){
            e.printStackTrace();
        }
        Expresion ex = new Expresion();
        ex.setArbolExpresiones(listaExpresion.get(0));
        ex.setDirArbolExpresiones(DirectorioArchivoImagen);
        ex.setNombre(nombre);
        GenerarSiguientes(ex);
        GenerarTransiciones(ex);
        GenerarDFA(ex);
        form.listaExpresiones.add(ex);
    }
    
    private void GenerarSiguientes(Expresion a){
        DefaultTableModel tabla = new DefaultTableModel();
        NodoArbol raiz = a.getArbolExpresiones();
        //DefaultTableModel tabla = a.getTablaSiguientes();
        tabla.addColumn("No.");
        tabla.addColumn("Follow");
        tabla.addColumn("Asociacion");
        ayudanteRecorrido(raiz, tabla);
        a.setTablaSiguientes(tabla);
        form.setTable(a.getTablaSiguientes());
    }
    
    private void ayudanteRecorrido(NodoArbol nodo, DefaultTableModel $tabla){
        String[] rowForadd = new String[3];
        if (nodo != null){
            
            ayudanteRecorrido(nodo.HijoIzquierdo, $tabla);
            ayudanteRecorrido(nodo.HijoDerecho, $tabla);
            
            if (nodo.TipoExpresion == Token.Tipo.CERRADURAKLEENE || nodo.TipoExpresion == Token.Tipo.CERRADURAPOSITIVA) {
                String[] primeros = nodo.getUltimo().split(",");
                for (int i = 0; i < primeros.length; i++) {
                    // verficarSiExiste(primeros[i], $tabla);
                    int pos = verificarFollow(primeros[i], $tabla);
                    if (pos >= 0) {
                        RecorrerHastaHoja(nodo,primeros[i]);
                        rowForadd[0]=primeros[i]; rowForadd[1]=$tabla.getValueAt(pos,1)+","+nodo.getPrimero(); rowForadd[2]=CaracterAsociado;
                        $tabla.removeRow(pos);
                        $tabla.addRow(rowForadd);
                    }else{
                        RecorrerHastaHoja(nodo,primeros[i]);
                        rowForadd[0]=primeros[i];rowForadd[1]=nodo.getPrimero(); rowForadd[2]=CaracterAsociado;
                        $tabla.addRow(rowForadd);                
                        //System.out.println("Para cada: "+primeros[i]+" le sigue "+nodo.getUltimo());
                    }
                    
                }
            }else if (nodo.TipoExpresion == Token.Tipo.CONCATENACION) {
                String[] ultimosIzquierdo = nodo.HijoIzquierdo.getUltimo().split(",");
                for (int i = 0; i < ultimosIzquierdo.length; i++) {
                    int pos = verificarFollow(ultimosIzquierdo[i], $tabla);
                    if (pos >= 0) {
                        RecorrerHastaHoja(nodo,ultimosIzquierdo[i]);
                        rowForadd[0]=ultimosIzquierdo[i]; rowForadd[1]=$tabla.getValueAt(pos,1)+","+nodo.HijoDerecho.getPrimero(); rowForadd[2]=CaracterAsociado;
                        $tabla.removeRow(pos);
                        $tabla.addRow(rowForadd);
                    }else{
                        RecorrerHastaHoja(nodo,ultimosIzquierdo[i]);
                        rowForadd[0]=ultimosIzquierdo[i];rowForadd[1]=nodo.HijoDerecho.getPrimero(); rowForadd[2]=CaracterAsociado;
                        $tabla.addRow(rowForadd);                
                        //System.out.println("Para cada: "+primeros[i]+" le sigue "+nodo.getUltimo());
                    }
                }
            }
        }       
    }
    private String CaracterAsociado = " ";
    private void RecorrerHastaHoja(NodoArbol nodo, String first){
        
        if (nodo != null) {
            RecorrerHastaHoja(nodo.HijoIzquierdo, first);
            RecorrerHastaHoja(nodo.HijoDerecho, first);
            if ((nodo.TipoExpresion == Token.Tipo.HOJA)) {
                if (Integer.parseInt(nodo.getPrimero()) == Integer.parseInt(first)) {
                    CaracterAsociado = nodo.dato;
                }               
            }
        }
        
    }
    
    private int verificarFollow(String no, DefaultTableModel $tabla){        
        int posicion = -1;
        for (int i = 0; i < $tabla.getRowCount(); i++) {
            if ($tabla.getValueAt(i, 0).toString().equals(no)) {
                posicion = i;
                i = $tabla.getColumnCount()+1;
            }
        }        
        return posicion;
    }
    
    private boolean verificarAsociado(String aso, DefaultTableModel $tabla, int limite){
        boolean encontrado = false;
        for (int i = 0; i < limite; i++) {
            if ($tabla.getValueAt(i, 2).toString().equals(aso)) {
                encontrado = true;
                i = $tabla.getRowCount()+1;
            }  
        }
        return encontrado;
    }
    
    
    private void GenerarTransiciones(Expresion a){
        DefaultTableModel transicion = new DefaultTableModel();
        DefaultTableModel follow = a.getTablaSiguientes();
        /*SETEAR COLUMNAS*/
        transicion.addColumn("Estado");
        for (int i = 0; i < follow.getRowCount(); i++) {
            if (!verificarAsociado(follow.getValueAt(i, 2).toString(), follow, i)) {
                transicion.addColumn(follow.getValueAt(i,2).toString()); 
            }
        }
        /*SETEAR FILAS*/  
        
        String S0 = a.getArbolExpresiones().getPrimero();
        LinkedList<String> estados = new LinkedList<String>();
        LinkedList<String> estadosAnalizados = new LinkedList<String>();
        estados.addFirst(S0);
        String valorFollow="", valorAsociado="";
        String[] rowForadd = new String[transicion.getColumnCount()];
        while(!estados.isEmpty()){
            rowForadd[0] = estados.getLast();
            String[] estadoActual = estados.getLast().split(",");   
            for (int i = 0; i < estadoActual.length; i++) {
                valorFollow="";
                valorAsociado="";  
                //LOOP PARA OBTENER FOLLOW Y VALOR ASOCIADOS
                for (int j = 0; j < follow.getRowCount(); j++) {
                    if (follow.getValueAt(j, 0).toString().equals(estadoActual[i])) {
                        valorFollow = follow.getValueAt(j, 1).toString();
                        valorAsociado = follow.getValueAt(j, 2).toString();
                        j = follow.getRowCount()+1;
                    }
                }
                //LOOP PARA ENCONTRAR LA COLUMNA DEL VALOR ASOCIADO Y ASIGNAR EL FOLLOW EN ESA POSICION
                for (int j = 0; j < transicion.getColumnCount(); j++) {
                    if (transicion.getColumnName(j).equals(valorAsociado)) {
                        rowForadd[j]=valorFollow;
                        j = transicion.getColumnCount()+1;
                    }
                }
                //SI EL FOLLOW ENCONTRADO ES DISTINTO AL QUE SE ESTA ANALIZANDO SE AÑADE EN COLA PARA ANALIZAR
                if (!valorFollow.equals(estados.getLast()) && !estados.contains(valorFollow) && !estadosAnalizados.contains(valorFollow)) {
                    estados.addFirst(valorFollow);
                    
                }      
                
            }
            if (!valorFollow.equals("")) {
                transicion.addRow(rowForadd);
            }
            
            //ELIMINAR EL ESTADO QUE SE ESTABA ANALIZANDO
            estadosAnalizados.addFirst(estados.getLast());
            estados.removeLast();
            //REINICIAR VALORES            
            for (int i = 0; i < rowForadd.length; i++) {
                rowForadd[i] = null;
            }
                      
        }
        a.setTablaTransiciones(transicion);
        form.setTransitionTable(a.getTablaTransiciones());
    }
    
    private void GenerarDFA(Expresion a){
        int estados = 0;
        String graph = "digraph finite_state_machine {	\nrankdir=LR;\n node [shape = circle];\n";
        DefaultTableModel transicion = a.getTablaTransiciones();
        for (int i = 0; i < transicion.getRowCount(); i++) {
            for (int j = 1; j < transicion.getColumnCount(); j++) {
                //System.out.print(i +" "+j);
                if (transicion.getValueAt(i, j) != null){
                    graph += "\""+transicion.getValueAt(i, 0).toString()+"\"" +" -> "+"\""+transicion.getValueAt(i,j).toString()+"\""+" [ label = \""+transicion.getColumnName(j)+"\"]; \n";
                }
            }
        }
        graph +="}";
        System.out.println(graph);
        String DirectorioArchivo = form.path+"\\DFA"+a.getNombre()+".dot";
        String DirectorioArchivoImagen = form.path+"\\DFA"+a.getNombre()+".png";
        try{
            File destino = new File(DirectorioArchivo);
            if (!destino.exists()) {
                destino.createNewFile();
            }
            BufferedWriter bw = new BufferedWriter(new FileWriter(destino));
            bw.write(graph);
            bw.close();
            String Comando="dot -Tpng "+DirectorioArchivo+" -o "+form.path+"\\"+a.getNombre()+".png";
            System.out.print(Comando);
            Runtime.getRuntime().exec("dot -Tpng "+DirectorioArchivo+" -o "+DirectorioArchivoImagen);
        }catch(Exception e){
            e.printStackTrace();
        }
        a.setDirDFA(DirectorioArchivoImagen);
    }
    
    public void ValidarLexemas(LinkedList<Expresion> lista){
        for (Token elemento : listaToken) {
            if (elemento.getTokenTipo() == Token.Tipo.LEXEMA) {
                for (Expresion ex : lista) {
                    if (ex.getNombre().equals(elemento.getID())) {
                        ValidarEntrada(elemento.getContenido(), ex);
                        break;
                    }
                }
            }
        }
    }
    
    private void ValidarEntrada(String lexema, Expresion ex){
        int estadoActual=0;
        //int avance=0;
        boolean seguimiento = true;
        //BUSCAR COLUMNAS
        String Conjunto;
        if (seguimiento) {
            for (int i = 1; i < ex.getTablaTransiciones().getColumnCount(); i++) {
            Conjunto = DeterminarConjunt(ex.getTablaTransiciones().getColumnName(i));
           /* if (ex.getTablaTransiciones().getValueAt(estadoActual, i) !=null) {*/
                if (!Conjunto.equals("")) {
                    //SI LA COLUMNA PERTENECE A UN CONJUNTO
                    String[] rangos = Conjunto.split("~");
                    if (((int)lexema.charAt(estadoActual) >= (int)rangos[0].charAt(0)) && (int)lexema.charAt(estadoActual) <= (int)rangos[1].charAt(0)) {
                        //SI EL CARACTER ESTA DENTRO DEL VALOR ACEPTADO
                        seguimiento=true;
                        estadoActual=ObtenerRow(ex.getTablaTransiciones().getValueAt(estadoActual, i).toString(),i, ex.getTablaTransiciones());
                    }
                }else{
                    //SI LA COLUMNA ES UN CARACTER
                    String cadena = ex.getTablaTransiciones().getColumnName(i);                    
                        if (lexema.charAt(estadoActual) != cadena.charAt(0)) {
                            seguimiento=true;
                            estadoActual=ObtenerRow(ex.getTablaTransiciones().getValueAt(estadoActual, i).toString(),i, ex.getTablaTransiciones());
                        }                    
                }
            //}
        }
            System.out.println(lexema+" Es valido para la expresion");
        }else{
            System.out.println(lexema+" No es valido para la expresion");
        }
        
    }
    
    private String DeterminarConjunt(String nombre){
        String encontrado="";
        for (Token el : listaToken) {
            if (el.getTokenTipo() == Token.Tipo.CONJUNTO) {
                if (el.getID().equals(nombre)) {
                    encontrado = el.getContenido();
                }
            }
        }
        return encontrado;
    }
    
    private int ObtenerRow(String cell, int Column, DefaultTableModel $table){
        int a = 0;
        for (int i = 0; i < $table.getRowCount(); i++) {
            if ($table.getValueAt(i, Column).toString().equals(cell)) {
                a = i;
                i=$table.getRowCount()+1;
            }
        }
        return a;
    }
    
    
    
    
}