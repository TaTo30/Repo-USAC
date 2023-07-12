
package Controlador;

import Modelos.*;
import Vista.*;
import java.io.*;
import static java.lang.Thread.sleep;
import java.net.*;
import java.math.BigInteger;
import javax.swing.JOptionPane;
import org.json.simple.*;
import org.json.simple.parser.JSONParser;
import org.json.simple.parser.ParseException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Date;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.xml.bind.DatatypeConverter;



public class Controlador {
    
    private Formulario formulario;
    private final AVLTree<Categoria> categorias;
    private final HashTable<Usuario> usuarios;
    private Usuario logeado;
    private List<NodoRed> nodos;
    private LinkedList<Block> blockchain;
    private JSONArray jsondata, jsonusersadd, jsonusersedit, jsonlibroadd, jsonlibrodelete;
    private JSONArray jsoncatadd, jsoncatdelete;

    
    
    public Controlador(){
        this.usuarios = new HashTable<>(45);
        this.categorias = new AVLTree<>();
        Usuario Predeterminado = new Usuario(100000000,"Predeterminado","Predeterminado","Predeterminado","Predeterminado");
        usuarios.Insertar(100000000, Predeterminado);
        //IniciarServidor();
        //this.host = "localhost";
        nodos = new List<>();
        jsondata = new JSONArray();
        jsonusersadd = new JSONArray();
        jsonusersedit = new JSONArray();
        jsonlibroadd = new JSONArray();
        jsonlibrodelete = new JSONArray();
        jsoncatadd = new JSONArray();
        jsoncatdelete = new JSONArray();
        blockchain = new LinkedList<>();
    }
   
    
    /**************************/
    /* METODOS SOBRE USUARIOS */
    /**************************/    
    //Obtiene informacion del usuario logeado actualmente
    public Usuario ObtenerLogeado(){
        return this.logeado;
    }
    //Agrega un usuario a la estructura
    private void AgregarUsuario(int Carnet, String Nombre, String Apellido, String Carrera, String Password){
        Usuario temp = new Usuario(Carnet, Nombre, Apellido, Carrera, MD5(Password));
        usuarios.Insertar(Carnet, temp);
        JSONObject objuser = new JSONObject();
        objuser.put("Carnet", Carnet);
        objuser.put("Nombre", Nombre);
        objuser.put("Apellido", Apellido);
        objuser.put("Carrera", Carrera);
        objuser.put("Password", Password);  
        jsonusersadd.add(objuser);
    }
    //Lee el archivo en formate JSON sobre los usuarios que se ingresen
    public boolean CargaUsuarios(String path){        
        try{
            JSONParser parser = new JSONParser();
            JSONObject json = (JSONObject) parser.parse(new FileReader(path));
            JSONArray array = (JSONArray) json.get("Usuarios");
            array.forEach(user -> ParseUsuario((JSONObject) user)); 
            return true;
        }catch(IOException | ParseException e){
            JOptionPane.showMessageDialog(null, e, "Ha ocurrido un error en la carga de datos", JOptionPane.ERROR_MESSAGE);
            return false;
        }
    }
    //Registra a un usuario nuevo desde el formulario del registro
    public boolean CargaUsuarios(int Carnet, String Nombre, String Apellido, String Carrera, String Password){
        try{
            AgregarUsuario(Carnet, Nombre, Apellido, Carrera, Password);
            return true;
        }catch(Exception e){
            JOptionPane.showMessageDialog(null, e, "Ha ocurrido un error en la carga de datos", JOptionPane.ERROR_MESSAGE);
            return false;
        }
    }
    //Parsea el archivo JSON y registra un usuarios
    private void ParseUsuario(JSONObject user){ 
        AgregarUsuario(Integer.parseInt(user.get("Carnet").toString()),user.get("Nombre").toString(), user.get("Apellido").toString(), user.get("Carrera").toString(), user.get("Password").toString());
        
              
    }
    //Logea un usuario verificando su carnter y contraseña
    public boolean Login(int carnet, String Password){
        Usuario temp = usuarios.Obtener(carnet);
        if (temp != null) {
            if (temp.getPassword().equals(MD5(Password))) { 
                logeado = temp;
                Formulario form = new Formulario(this);
                form.setVisible(true);
                formulario = form;                
                return true;
            }else{
                return false;
            }
        }else{
            return false;
        }
    }
    //Deslogea un usuario devolviendo el formulario de login
    public boolean Deslogin(){
        if (JOptionPane.showConfirmDialog(null, "¿Esta Seguro de salir?", "Terminar Sesion", JOptionPane.YES_NO_OPTION)==JOptionPane.YES_OPTION) {
            Login temp = new Login(this);
            temp.setVisible(true);
            return true;
        }else{
            return false;
        }
    }
    //Modifica los datos de un usuario dentro del la tabla hash
    public boolean ModificarUsuario(String nombre, String apell, String carrera){
        logeado.setApellido(apell);
        logeado.setNombre(nombre);
        logeado.setCarrera(carrera);
        JSONObject objuser = new JSONObject();
        objuser.put("Carnet", logeado.getCarnet());
        objuser.put("Nombre", logeado.getNombre());
        objuser.put("Apellido", logeado.getApellido());
        objuser.put("Carrera", logeado.getCarrera());
        objuser.put("Password", logeado.getPassword());  
        jsonusersedit.add(objuser);
        JOptionPane.showMessageDialog(null, "Datos Actualizados", "Actualizacion", JOptionPane.INFORMATION_MESSAGE);
        return true;
    }
    //Elimina un usuario de la tabla hash
    public boolean EliminarUsuario(){        
        if (JOptionPane.showConfirmDialog(null,"¿Estas Seguro?\nTodos los datos asociados a este usuario se eliminaran", "Eliminacion de usuario",JOptionPane.YES_NO_OPTION)==JOptionPane.YES_OPTION) {
            Categoria[] temp = categorias.InOrden(Categoria.class);
            for (Categoria cat : temp){
                if (cat != null) {
                    Libro[] libros = cat.getLibreria().RecorridoOrdenado();
                    for(Libro lib : libros){
                        if (lib != null) {
                            if (lib.getCarnet()==logeado.getCarnet())
                                lib.setCarnet(100000000);
                        }
                    }
                }                
            }
            usuarios.Eliminar(logeado.getCarnet());
            Login log = new Login(this);
            log.setVisible(true);
            return true;
        }else{
            return false;
        }
    }
    //Retorna un usuario que con el carnet especificado
    public Usuario ObtenerUser(int carnet){        
        Usuario temp = usuarios.Obtener(carnet);
        if (temp != null) {
            return temp;
        }else{
            return usuarios.Obtener(100000000);
        }
    }
    
    
    
    
    
    /************************/
    /* METODOS SOBRE LIBROS */
    /************************/ 
    //Carga un libro a travez de la carga masiva de libros    
    public boolean CargaLibro(String path){
        try{
            JSONParser parser = new JSONParser();
            JSONObject json = (JSONObject) parser.parse(new FileReader(path));
            JSONArray array = (JSONArray) json.get("libros");
            array.forEach(temp -> ParseLibro((JSONObject) temp));
            return true;
        }catch(IOException | ParseException e){
            JOptionPane.showMessageDialog(null, e, "Ha ocurrido un error en la carga de datos", JOptionPane.ERROR_MESSAGE);
            return false;
        }
    }
    //Agrega un libro a la estructura
    private void AgregarLibro(int ISBN, String titulo, String Autor, String Editorial, int Ano, int Edicion, String Categoria, String Idioma){
        Libro temp = new Libro(ISBN, titulo, Autor, Editorial, Ano, Edicion, Categoria, Idioma, logeado.getCarnet());
        categorias.Find(Categoria.charAt(0)).AgregarLibro(temp);
        JSONObject obj = new JSONObject();
        obj.put("ISBN", ISBN);
        obj.put("AÃ±o", Ano);
        obj.put("Idioma", Idioma);
        obj.put("Titulo", titulo);
        obj.put("Editorial", Editorial);
        obj.put("Autor", Autor);
        obj.put("Edicion", Edicion);
        obj.put("Categoria", Categoria);
        obj.put("Carnet", logeado.getCarnet());
        jsonlibroadd.add(obj);
    }
    //Carga un libro a travez del formulario de registro de libros
    public boolean CargaLibro(int ISBN, String titulo, String Autor, String Editorial, int Ano, int Edicion, String Categoria, String Idioma){
        if (categorias.Check(Categoria.charAt(0))) {
            //si existe la categoria añadimos
            if (ObtenerLibro(ISBN) == null) {
                //si no existe el ISBN, añadimos
                AgregarLibro(ISBN, titulo, Autor, Editorial, Ano, Edicion, Categoria, Idioma);
                return true;
            }else{
                //el libro ya existe
                JOptionPane.showMessageDialog(null, "El libro ya existe en la biblioteca", "Error de carga", JOptionPane.ERROR_MESSAGE);
                return false;
            }
        }else{
            if (JOptionPane.showConfirmDialog(null, "¿Desea añadir la categoria \""+Categoria+"\" a la biblioteca?", "Creacion de Categorias", JOptionPane.YES_NO_OPTION) == JOptionPane.YES_OPTION) {
                //si no existe, creamos la categoria                
                //añadimos el libro
                if (ObtenerLibro(ISBN) == null) {
                    //si no existe el ISBN, añadimos
                    CrearCategoria(Categoria);
                    AgregarLibro(ISBN, titulo, Autor, Editorial, Ano, Edicion, Categoria, Idioma);
                    /*Libro temp = new Libro(ISBN, titulo, Autor, Editorial, Ano, Edicion, Categoria, Idioma, logeado.getCarnet());
                    categorias.Find(Categoria.charAt(0)).AgregarLibro(temp);*/
                    JOptionPane.showMessageDialog(null, "Categoria "+Categoria+" creada", "Categoria Añadida", JOptionPane.INFORMATION_MESSAGE);
                    return true;
                }else{
                    //ya existe se cacela la operacion
                    JOptionPane.showMessageDialog(null, "El libro ya existe en la biblioteca", "Error de carga", JOptionPane.ERROR_MESSAGE);
                    return false;
                }                
            }else{
                //no se añade la categoria
                return false;
            }
        }
    }
    //Parsea un libro y lo carga a la estructura
    private void ParseLibro(JSONObject libro){
        String cat = libro.get("Categoria").toString();
        int ISBN = Integer.parseInt(libro.get("ISBN").toString());        
        if (categorias.Check(cat.charAt(0))) {
            //si la categoria existe verificamos el isbn
            if (ObtenerLibro(ISBN) == null) {
                //si el libro no existe lo agregamos
                AgregarLibro(ISBN, libro.get("Titulo").toString(), libro.get("Autor").toString(), libro.get("Editorial").toString(), Integer.parseInt(libro.get("AÃ±o").toString()), Integer.parseInt(libro.get("Edicion").toString()), cat, libro.get("Idioma").toString());
               /* Libro temp = new Libro(ISBN, , logeado.getCarnet());
                categorias.Find(cat.charAt(0)).AgregarLibro(temp);*/
            }else{
                //el libro ya existe
                JOptionPane.showMessageDialog(null, "El libro ya existe en la biblioteca", "Error de carga", JOptionPane.ERROR_MESSAGE);
            }
        }else{            
            if (JOptionPane.showConfirmDialog(null, "¿Desea añadir la categoria \""+cat+"\" a la biblioteca?", "Creacion de Categorias", JOptionPane.YES_NO_OPTION) == JOptionPane.YES_OPTION) {
                //si no existe, creamos la categoria
                //añadimos el libro
                if (ObtenerLibro(ISBN) == null) {
                    //si no existe el ISBN, añadimos
                    CrearCategoria(cat);
                    AgregarLibro(ISBN, libro.get("Titulo").toString(), libro.get("Autor").toString(), libro.get("Editorial").toString(), Integer.parseInt(libro.get("AÃ±o").toString()), Integer.parseInt(libro.get("Edicion").toString()), cat, libro.get("Idioma").toString());
                    /*Libro temp = new Libro(ISBN, libro.get("Titulo").toString(), libro.get("Autor").toString(), libro.get("Editorial").toString(), Integer.parseInt(libro.get("AÃ±o").toString()), Integer.parseInt(libro.get("Edicion").toString()), cat, libro.get("Idioma").toString(), logeado.getCarnet());
                    categorias.Find(cat.charAt(0)).AgregarLibro(temp);*/
                    JOptionPane.showMessageDialog(null, "Categoria "+cat+" creada", "Categoria Añadida", JOptionPane.INFORMATION_MESSAGE);                    
                }                
            }        
        }
    }
    //Obtiene un libro con el ISBN especificada
    public Libro ObtenerLibro(int ISBN){
        Categoria[] array = categorias.InOrden(Categoria.class);
        Libro toReturn = null;
        for (Categoria cat : array){
            if (cat != null) {
                if (cat.getLibreria().Count()!=0) {
                    if (cat.getLibreria().Check(ISBN)) {
                    toReturn = cat.getLibreria().Find(ISBN);
                    break;
                }
                }
                
            }
        }
        return toReturn;
    }
    //Elimina un libro de la estructura
    public boolean EliminarLibro(int carnet, int ISBN, String Categoria){
        if (carnet == logeado.getCarnet()) {
            //es dueño se puede eliminar
            Libro temp = categorias.Find(Categoria.charAt(0)).getLibreria().Find(ISBN);            
            categorias.Find(Categoria.charAt(0)).EliminarLibro(ISBN);
            JSONObject obj = new JSONObject();
            obj.put("ISBN", ISBN);
            obj.put("Titulo", temp.getTitulo());
            obj.put("Categoria", Categoria);
            jsonlibrodelete.add(obj);
            //
            int vacio = categorias.Find(Categoria.charAt(0)).getLibreria().Count();
            if (vacio == 0) {
                //categoria sin libros
                categorias.Delete(Categoria.charAt(0));
            }
            return true;
        }else{
            JOptionPane.showMessageDialog(null, "Tu no eres el dueño de este libro", "¡Error!", JOptionPane.ERROR_MESSAGE);
            return false;
        }        
    }
    
    
    
    
    
    
    
    /************************/
    /* METODOS SOBRE LIBROS */
    /************************/ 
    //crea una Nueva categoria
    public void CrearCategoria(String nombre){
        Categoria temp = new Categoria(nombre, logeado.getCarnet());
        categorias.Add(temp.getNombre().charAt(0), temp);
        JSONObject obj = new JSONObject();
        obj.put("NOMBRE", nombre);
        obj.put("Carnet", logeado.getCarnet());
        jsoncatadd.add(obj);
    }
    //returna una categoria
    public Categoria ObtenerCategoria(String nombre){
        return categorias.Find(nombre.charAt(0));
    }
    //elimina una categoria
    public void EliminarCategoria(String nombre){
        Categoria temp = categorias.Find(nombre.charAt(0));
        if (temp.getCarnet() == logeado.getCarnet()) {
            categorias.Delete(nombre.charAt(0));
            JSONObject obj = new JSONObject();
            obj.put("NOMBRE", nombre);
            jsoncatdelete.add(obj);
        }else{
            JOptionPane.showMessageDialog(null, "Esta Categoria no te pertenece", "Eliminacion de categorias", JOptionPane.ERROR_MESSAGE);
        }        
    }
    //Obtiene un array de categorias    
    public Categoria[] ObtenerCategorias(){
        return categorias.InOrden(Categoria.class);
    }
    
    
    
    
    
    
    /***********************************/
    /* METODOS SOBRE CONECCION SOCKETS */
    /***********************************/
    private int puerto;
    private String host;
    public void IniciarServidor(String host, int puerto){
        this.host = host;
        this.puerto = puerto;
        new Servidor(puerto,this).start();
    }
    //Ingresa a la lista de Nodos cada vez que la aplicacion se instancia
    public void IngresarRed(){
        nodos.AddLast(new NodoRed(puerto, host), 0);
        ImprimirNodos();
    }
    //Agrega nodos a la red de nodos o instancias de aplicacion
    public void AgregarNodo(NodoRed dato){ 
        boolean existe = false;
        for (int i = 0; i < nodos.Size(); i++) {
            NodoRed temp = nodos.ElementAt(i);
            if ((temp.getIP().equals(dato.getIP())) && (temp.getPuerto()==dato.getPuerto())) {
                existe = true;
            }
        }
        if (!existe)
            nodos.AddLast(dato, 0);        
        ImprimirNodos();
    }
    //Elimina un nodo especificado
    public void EliminarNodo(String ip, int puerto){
        for (int i = 1; i < nodos.Size(); i++) {
            NodoRed temp = nodos.ElementAt(i);
            if ((temp.getIP().equals(ip)) && (temp.getPuerto() == puerto)) {
                nodos.RemoveAt(i);
                break;
            }
        }
        ImprimirNodos();
    }
    //Añade nodos de una coneccion directa de red
    public String ProcesoAgregarNodo(String host, int puerto){
        JSONArray array = new JSONArray();
        for (int i = 0; i < nodos.Size(); i++) {
            JSONObject temp = new JSONObject();
            temp.put("IP", nodos.ElementAt(i).getIP());
            temp.put("Puerto", nodos.ElementAt(i).getPuerto());
            array.add(temp);
        }
        for (int i = 1; i < nodos.Size(); i++) {
            try {
                NodoRed temp = nodos.ElementAt(i);
                System.out.println("Al puerto "+temp.getPuerto()+" enviarle "+puerto);
                JSONObject obj = new JSONObject();
                obj.put("IP", host);
                obj.put("Puerto", puerto);
                JSONObject ins = new JSONObject();
                ins.put("Instruccion", 2);
                ins.put("Dato", obj);
                System.out.println("Envio: 0000"+ins.toJSONString());
                /*BufferedWriter writeFile = new BufferedWriter(new FileWriter("Instrucciones.json"));
                writeFile.write(ins.toJSONString());
                writeFile.close();*/
                System.out.println(temp.getIP() + temp.getPuerto());
                Socket cliente = new Socket(temp.getIP(), temp.getPuerto());
                PrintStream salida = new PrintStream(cliente.getOutputStream(),true);
                salida.println(ins.toJSONString()); 
                salida.close();
                System.out.println(cliente.isConnected());
                System.out.println(cliente.getPort());
                sleep(3000);
            } catch (IOException ex) { } catch (InterruptedException ex) { 
                Logger.getLogger(Controlador.class.getName()).log(Level.SEVERE, null, ex);
            } 
        }
        JSONObject ins = new JSONObject();
        formulario.ConnectMessage("¡Estas conectado en la Red!");
        ins.put("Instruccion", 1);
        ins.put("Dato", array);
        AgregarNodo(new NodoRed(puerto, host));
        return ins.toJSONString();
    }
    //Elimina un nodo cuando se cierra una aplicacion
    public void ProcesoEliminarNodoCliente(){
        try{
            //CREAMOS UN ARCHIVO JSON CON LAS INSTRUCCIONES
            JSONObject obj = new JSONObject();
            obj.put("IP",this.host);
            obj.put("Puerto", this.puerto);
            JSONObject ins = new JSONObject();
            ins.put("Instruccion", 3);
            ins.put("Dato", obj);
            //Por cada nodo de la red enviamos a eliminar
            for (int i = 1; i < nodos.Size(); i++) {
                Socket cliente = new Socket(nodos.ElementAt(i).getIP(),nodos.ElementAt(i).getPuerto());
                PrintStream printer = new PrintStream(cliente.getOutputStream(), true);
                printer.print(ins.toJSONString());
                cliente.close();
                printer.close();
                sleep(1000);
            }
                        
        }catch(IOException | InterruptedException e){}
    }
    //envia una solicitud de red
    public void RegistroNodoRedClienteSide(int puerto, String ipH){
        try{
            //CREAMOS UN ARCHIVO JSON CON LAS INSTRUCCIONES
            formulario.ConnectMessage("Conectandose...");
            JSONObject obj = new JSONObject();
            obj.put("IP", this.host);
            obj.put("Puerto", this.puerto);
            JSONObject ins = new JSONObject();
            ins.put("Instruccion", 0);
            ins.put("Dato", obj);
            Socket cliente = new Socket(ipH, puerto);
            PrintStream salida = new PrintStream(cliente.getOutputStream(),true);
            salida.println(ins.toJSONString()); 
            salida.close();
            System.out.println(cliente.getPort());
            System.out.println(cliente.isConnected());       
        }catch(IOException e){System.out.println(e.toString());}
    }
    //Registra todos los nodos que envia el nodo servidor
    public void RegistrarNodosMasivos(String json){
        try {
            JSONParser parser = new JSONParser();        
            JSONArray puertos = (JSONArray) parser.parse(json);
            puertos.forEach(port -> ParseNodos((JSONObject)port));
            formulario.ConnectMessage("¡Estas conectado en la Red!");
        } catch (ParseException ex) {
            Logger.getLogger(Controlador.class.getName()).log(Level.SEVERE, null, ex);
        }
    }
    //Parsea los nodos de un array json
    public void ParseNodos(JSONObject o){
        String ip = o.get("IP").toString();
        int puertoparse = Integer.parseInt(o.get("Puerto").toString());
        AgregarNodo(new NodoRed(puertoparse, ip));
    }
    //Imprime los nodos que hay en una red
    public void ImprimirNodos(){
        for (int i = 0; i < nodos.Size(); i++) {
            System.out.println(nodos.ElementAt(i).getIP()+" -> "+nodos.ElementAt(i).getPuerto());
        }
    }
    //verifica que haya nodos en la red
    public boolean PruebaRed(){
        return nodos.Size() > 1;
    }
    //returna los paramentros establecidos por el usuario
    public String Parametraje(){
        return "IP: "+this.host+"   Puerto: "+this.puerto;
    }
    
    
    
    /*******************************/
    /* METODOS SOBRE EL BLOCKCHAIN */
    /*******************************/  
    //calcula el nonce puente para el hash
    public int Nonce(String valor){
        int NONCE = 0;
        while(true){
            //System.out.println(NONCE);
            String temp = SHA256(valor+String.valueOf(NONCE));
            formulario.BlockMessage("Calculando NONCE....");
            if (temp.substring(0,4).equals("0000")) {                
                //System.out.println(NONCE);
                break;
            }
            NONCE++;
           // System.out.println(temp);
        }
        return NONCE;
    }
    //crea un bloque y lo añade al blockchain y genera un json con ello
    public void CrearBloque(){
        formulario.BlockMessage("Registrando procesos...");
        //INDEX
        int index = blockchain.Size();
        //TIMESTAMP
        String format = "dd-MM-yy HH:mm:ss";
        Date date = new Date();
        java.text.DateFormat dformat = new java.text.SimpleDateFormat(format);
        String fecha = dformat.format(date);        
        //PREV HASHT
        String prev;
        if (blockchain.Contain()) {
            prev = blockchain.GetLast().getHash();
        }else{
            prev = "0000";
        }
        //DATA
        JSONObject data0 = new JSONObject();
        data0.put("CREAR_USUARIO", jsonusersadd);
        JSONObject data1 = new JSONObject();
        data1.put("EDITAR_USUARIO", jsonusersedit);
        JSONObject data2 = new JSONObject();
        data2.put("​CREAR_LIBRO", jsonlibroadd);
        JSONObject data3 = new JSONObject();
        data3.put("ELIMINAR_LIBRO", jsonlibrodelete);
        JSONObject data4 = new JSONObject();
        data4.put("CREAR_CATEGORIA", jsoncatadd);
        JSONObject data5 = new JSONObject();
        data5.put("ELIMINAR_CATEGORIA", jsoncatdelete);
        jsondata.add(data0);
        jsondata.add(data1);
        jsondata.add(data2);
        jsondata.add(data3);
        jsondata.add(data4);
        jsondata.add(data5);        
        //HASH
        String hash;
        int nonce = Nonce(Integer.toString(index)+date.toString()+prev+jsondata.toJSONString());
        hash = SHA256(Integer.toString(index)+date.toString()+prev+jsondata.toJSONString()+Integer.toString(nonce));
        blockchain.AddLast(new Block(index,fecha,jsondata.toJSONString(),nonce,prev,hash));
        JSONObject obj = new JSONObject();
        obj.put("INDEX",index);
        obj.put("TIMESTAMP", date.toString());
        obj.put("NONCE", nonce);
        obj.put("DATA", jsondata);
        obj.put("PREVIUSHASTH", prev);
        obj.put("HASH", hash);
        //para cada nodo en la red enviamos los datos cargados
        formulario.BlockMessage("Enviando bloque a los nodos de la red....");
        for (int i = 1; i < nodos.Size(); i++) {
            JSONObject ins = new JSONObject();
            ins.put("Instruccion", 4);
            ins.put("Dato", obj);            
            try {
                Socket cs = new Socket(nodos.ElementAt(i).getIP(),nodos.ElementAt(i).getPuerto());
                PrintStream print = new PrintStream(cs.getOutputStream(),true);
                print.print(ins.toJSONString());
                print.close();
                cs.close();
            } catch (IOException ex) {}
        }
        formulario.BlockMessage("Limpiando buffer...");
        BufferedWriter writer;
        try {
            writer = new BufferedWriter(new FileWriter("block.json"));
            writer.write(obj.toJSONString());
            writer.close();
        } catch (IOException ex) {
            Logger.getLogger(Controlador.class.getName()).log(Level.SEVERE, null, ex);
        }
        formulario.BlockMessage("");
        jsondata.clear();
        jsoncatdelete.clear();
        jsoncatadd.clear();
        jsonusersadd.clear();
        jsonusersedit.clear();
        jsonlibroadd.clear();
        jsonlibrodelete.clear();       
        
    }
    //Agrega un bloque a la estructura
    public void AgregarBloque(JSONObject data){
        int nonce = Integer.parseInt(data.get("NONCE").toString());
        int index = Integer.parseInt(data.get("INDEX").toString());
        String Data = data.get("DATA").toString();
        String date = data.get("TIMESTAMP").toString();
        String hash = data.get("HASH").toString();
        String hashprev = data.get("PREVIUSHASTH").toString();
        Block temp = new Block(index,date,Data,nonce,hashprev,hash);
        blockchain.AddLast(temp);        
    }
    //Lado servidor que desenvuelve el bloque de cosas cargadas
    public void CargarBloque(JSONArray datos){
      //  System.out.println(datos.toJSONString());
        //CREAR USUARIOS
        JSONObject usersc = (JSONObject) datos.get(0);
        JSONArray arrayusersc = (JSONArray) usersc.get("CREAR_USUARIO");
        arrayusersc.forEach(uc -> ParseoBloqueUsersAdd((JSONObject)uc));
        
        //MODIFICAR USUARIOS
        JSONObject usered = (JSONObject) datos.get(1);
        JSONArray arrayusered = (JSONArray) usered.get("EDITAR_USUARIO");
        arrayusered.forEach(ue -> ParseoBloqueUsersEdit((JSONObject)ue));
        
        //CREAR CATEGORIA
        JSONObject cata = (JSONObject) datos.get(4);
        JSONArray arraycat = (JSONArray) cata.get("CREAR_CATEGORIA");
        arraycat.forEach(ca -> ParseoBloqueCatAdd((JSONObject)ca));
        
        //ELIMINAR CATEGORIA
        JSONObject catad = (JSONObject) datos.get(5);
        JSONArray arraycatad = (JSONArray) catad.get("ELIMINAR_CATEGORIA");
        arraycatad.forEach(ue -> ParseoBloqueCatDelete((JSONObject)ue));
        
        //CREAR LIBRO
        JSONObject libroa = (JSONObject) datos.get(2);
        //System.out.println(libroa.get("CREAR_LIBRO"));
        //String TEMP = libroa.get("CREAR_LIBRO");
        System.out.println(libroa.get("CREAR_LIBRO")); 
        JSONArray arraylibro = (JSONArray) libroa.get("​CREAR_LIBRO");
        arraylibro.forEach(ue -> ParseoBloqueLibroAdd((JSONObject)ue));
        
        //ELIMINAR LIBRO
        JSONObject librod = (JSONObject) datos.get(3);
        JSONArray arraylibroed = (JSONArray) librod.get("ELIMINAR_LIBRO");
        arraylibroed.forEach(ue -> ParseoBloqueLibroDelete((JSONObject)ue));
        
        formulario.ActualizarTabla();
        formulario.ComboCategorias();
    }
        //CREAR USUARIOS-----------------------------*
    public void ParseoBloqueUsersAdd(JSONObject obj){
        System.out.println(obj.toJSONString());
        int carnet = Integer.parseInt(obj.get("Carnet").toString());
        String Nombre = obj.get("Nombre").toString();
        String Apellido = obj.get("Apellido").toString();
        String Carrera = obj.get("Carrera").toString();
        String pass = obj.get("Password").toString();        
        if (usuarios.Obtener(carnet) == null) {
            Usuario user = new Usuario(carnet,Nombre,Apellido,Carrera,MD5(pass));
            usuarios.Insertar(carnet, user);
        }
    }
        //MODIFICAR USUARIOS-------------------------*
    public void ParseoBloqueUsersEdit(JSONObject obj){
        System.out.println(obj.toJSONString());
        int carnet = Integer.parseInt(obj.get("Carnet").toString());
        Usuario temp = usuarios.Obtener(carnet);
        temp.setApellido(obj.get("Apellido").toString());
        temp.setNombre(obj.get("Nombre").toString());
        temp.setCarrera(obj.get("Carrera").toString());
    }
        //CREAR LIBRO--------------------------------*
    public void ParseoBloqueLibroAdd(JSONObject obj){
        System.out.println(obj.toJSONString());
        int ISBN = Integer.parseInt(obj.get("ISBN").toString());
        String cate = obj.get("Categoria").toString();
        if (ObtenerLibro(ISBN) == null) {
            Categoria cat = categorias.Find(cate.charAt(0));
            Libro emp = new Libro(ISBN,obj.get("Titulo").toString(),obj.get("Autor").toString(),obj.get("Editorial").toString(),Integer.parseInt(obj.get("AÃ±o").toString()),Integer.parseInt(obj.get("Edicion").toString()),cate, obj.get("Idioma").toString(),Integer.parseInt(obj.get("Carnet").toString())); 
            cat.getLibreria().Add(ISBN, emp);
        }
    }
        //ELIMINAR LIBRO-----------------------------*
    public void ParseoBloqueLibroDelete(JSONObject obj){
        System.out.println(obj.toJSONString());
        int ISBN = Integer.parseInt(obj.get("ISBN").toString());
        String cat = obj.get("Categoria").toString();
        Categoria cate = categorias.Find(cat.charAt(0));
        cate.getLibreria().Remove(ISBN);
    }
        //CREAR CATEGORIA----------------------------*
    public void ParseoBloqueCatAdd(JSONObject obj){
        System.out.println(obj.toJSONString());
        String nombre = obj.get("NOMBRE").toString();
        int carnet = Integer.parseInt(obj.get("Carnet").toString());
        if (!categorias.Check(nombre.charAt(0))) {
            Categoria cate = new Categoria(nombre, carnet);
            categorias.Add(nombre.charAt(0), cate);  
        }
    }
        //ELIMINAR CATEGORIA-------------------------*
    public void ParseoBloqueCatDelete(JSONObject obj){
        System.out.println(obj.toJSONString());
        String nombre = obj.get("NOMBRE").toString();
        categorias.Delete(nombre.charAt(0));
    }
    //parsea la lista de blockchain en formato json
    public void ParseBlockChain(int puerto, String ip){
        JSONArray array = new JSONArray();
        for (int i = 0; i < blockchain.Size(); i++) {
            JSONObject objtemp = new JSONObject();
            Block temp = blockchain.ElementAt(i);
            objtemp.put("INDEX", temp.getIndex());
            objtemp.put("TIMESTAMP", temp.getDate());
            objtemp.put("NONCE", temp.getNonce());
            objtemp.put("DATA", temp.getData());
            objtemp.put("PREVIUSHASTH", temp.getPrevHash());
            objtemp.put("HASH", temp.getHash());
            array.add(objtemp);
        }
        JSONObject ins = new JSONObject();
        ins.put("Instruccion", 6);
        ins.put("Dato", array);
        try {
            Socket cliente = new Socket(ip, puerto);
            PrintStream printer = new PrintStream(cliente.getOutputStream(),true);
            printer.print(ins.toJSONString());
            printer.close();
            cliente.close();
        } catch (IOException ex) {System.out.println(ex.toString());}
    }
    //sincroniza el blockchain por parte del servidor
    public void BlockChainSync(JSONArray array){
        array.forEach(action -> BlockChainSyncParse((JSONObject)action));
    }
    //Parsea los datos del blockchain
    public void BlockChainSyncParse(JSONObject obj){
        AgregarBloque(obj);
        String arraystring = obj.get("DATA").toString();
        JSONParser parser = new JSONParser();
        try {
            JSONArray array = (JSONArray) parser.parse(arraystring);
            CargarBloque(array);
        } catch (ParseException ex) {System.out.println("PARSER SYNC"+ex.toString());}
    }
    //sincroniza el blockchain por parte del cliente
    public void BlockChainSyncCliente(){
        JSONObject info = new JSONObject();
        info.put("IP", this.host);
        info.put("Puerto", this.puerto);
        JSONObject ins = new JSONObject();
        ins.put("Instruccion", 5);
        ins.put("Dato",info);
        try {
            Socket cliente = new Socket(nodos.GetLast().getIP(),nodos.GetLast().getPuerto());
            PrintStream printer = new PrintStream(cliente.getOutputStream(), true);
            printer.print(ins.toJSONString());
            printer.close();
            cliente.close();
        } catch (IOException ex) {System.out.println("Socket 5"+ex.toString()); }
    }
    
    
    
    
    
    /***************************/
    /* METODOS DE ENCRIPTACION */
    /***************************/ 
    //Encriptacion MD5
    private String MD5(String password){
        try{
            MessageDigest md = MessageDigest.getInstance("MD5");
            
            byte[] mssDigest = md.digest(password.getBytes());
            BigInteger no = new BigInteger(1, mssDigest);
            
            String hashText = no.toString(16);
            while(hashText.length() < 32){
                hashText = "0" + hashText;
            }
            return hashText;
            
        }catch(NoSuchAlgorithmException e){
            System.out.println(e);
            
            return null;
        }
    } 
    //Encriptacion SHA-256
    public String SHA256(String data){
        String data_hast = "";
        byte[] data_tohast = data.getBytes();
        try{
            MessageDigest mss = MessageDigest.getInstance("SHA-256");
            mss.update(data_tohast);
            
            byte[] digested = mss.digest();
            data_hast = DatatypeConverter.printHexBinary(digested).toLowerCase();
            
        }catch(NoSuchAlgorithmException e){}
        return data_hast;
    }
    
    
    
    
    
    /********************************/
    /* METODOS SOBRE DE GRAFICACION */
    /********************************/     
    public void ImprimirAVL() throws IOException{
        categorias.Graficar();
    }
    public void ImprimirHash(){
        usuarios.Graficar();
    }
    public Categoria[] AVLInorden(){
        return categorias.InOrden(Categoria.class);
    }
    public Categoria[] AVLPreorden(){
        return categorias.PreOrden(Categoria.class);
    }
    public Categoria[] AVLPostorden(){
        return categorias.PostOrden(Categoria.class);
    }
    public LinkedList<Block> GetBlockchain(){
        return this.blockchain;
    }
    public List<NodoRed> GetNodos(){
        return this.nodos;
    }
    
  
  
 }
