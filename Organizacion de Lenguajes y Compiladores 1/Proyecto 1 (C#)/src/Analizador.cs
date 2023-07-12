using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto1
{
    class Analizador
    {
        public LinkedList<Token> Tokens = new LinkedList<Token>();
        public LinkedList<string> Errores = new LinkedList<string>();
        public LinkedList<Expresion> Expresiones = new LinkedList<Expresion>();
        private LinkedList<AFN> AFNList = new LinkedList<AFN>();
        public List<Estado> listaEstados = new List<Estado>();
        private Form1 Formulario;
        public Analizador(Form1 a)
        {
            this.Formulario = a;
        }
        
        public void Scan(String entrada)
        {
            Tokens.Clear();
            Expresiones.Clear();
            AFNList.Clear();
            listaEstados.Clear();
            entrada += '#';
            int estadoActual = 0;
            string lexema = "", ID="";
            char c;
            for (int i = 0; i < entrada.Length; i++)
            {
                c = entrada.ElementAt(i);
                switch (estadoActual)
                {
                    //CASO DE ENTRADA, SALE CADA TIPO DE TEXTO A ANALIZAR
                    case 0:
                        switch (c)
                        {
                            //Caracteres para ignorar en el inicio del analisis
                            case ' ':
                            case '\n':
                            case '{':
                            case '}':
                            case '%':
                            case '\t':
                                estadoActual = 0;
                                break;
                            //Caso de salida para comentario una Linea
                            case '/':
                                lexema += c;
                                estadoActual = 1;
                                break;
                            //Caso de salida para comentario multilinea
                            case '<':
                                lexema += c;
                                estadoActual = 2;
                                break;
                            //Caso de salida para definicion de Conjuntos
                            case 'C':
                                lexema += c;
                                estadoActual = 4;
                                break;
                            //Salida por defecto para definir Expresiones o lexemas
                            default:
                                lexema += c;
                                estadoActual = 3;
                                break;
                        }
                        break;
                    /************************************
                     * ANALISIS COMENTARIO DE UNA LINEA *                   
                     ************************************/
                    case 1:
                        if (c == '/')
                        {
                            lexema += c;
                            estadoActual = 5;
                        }
                        else
                        {
                            lexema += c;
                            estadoActual = 3;
                        }
                        break;
                    case 5:
                        if (c == '\n')
                        {
                            logToken(lexema, "", Token.Tipo.COMENTARIO);
                            lexema = "";
                            estadoActual = 0;
                        }
                        else
                        {
                            lexema += c;
                        }
                        break;
                    /************************************
                     ** ANALISIS COMENTARIO MULTILINEA **                   
                     ************************************/                    
                    case 2:
                        if (c == '!')
                        {
                            lexema += c;
                            estadoActual = 6;
                        }
                        else
                        {
                            lexema += c;
                            estadoActual = 3;
                        }
                        break;
                    case 6:
                        if (c == '!')
                        {
                            lexema += c;
                            estadoActual = 11;
                        }
                        else
                        {
                            lexema += c;
                        }
                        break;
                    case 11:
                        if (c == '>')
                        {
                            lexema += c;
                            logToken(lexema, "", Token.Tipo.COMENTARIOML);
                            lexema = "";
                            estadoActual = 0;
                        }
                        else
                        {
                            lexema += c;
                            estadoActual = 6;
                        }
                        break;
                    /**************************
                    ** ANALISIS DE CONJUNTOS **                   
                    **************************/
                    case 4:
                        if (c == 'O')
                        {
                            lexema += c;
                            estadoActual = 9;
                        }
                        else
                        {
                            lexema += c;
                            estadoActual = 3;
                        }
                        break;
                    case 9:
                        if (c == 'N')
                        {
                            lexema += c;
                            estadoActual = 14;
                        }
                        else
                        {
                            lexema += c;
                            estadoActual = 3;
                        }
                        break;
                    case 14:
                        if (c == 'J')
                        {
                            lexema += c;
                            estadoActual = 18;
                        }
                        else
                        {
                            lexema += c;
                            estadoActual = 3;
                        }
                        break;
                    case 18:
                        if (c == ':')
                        {
                            lexema = "";
                            estadoActual = 21;
                        }
                        else
                        {
                            lexema += c;
                            estadoActual = 3;
                        }
                        break;
                    case 21:
                        if (c == '-')
                        {
                            //lexema += c;
                            ID = lexema;
                            lexema = "";
                            estadoActual = 23;
                        }
                        else if (c != ' ')
                        {
                            lexema += c;
                        }
                        break;
                    case 23:
                        if (c == '>')
                        {
                            //lexema += c;
                            estadoActual = 24;
                        }
                        else
                        {
                            //ESTADO DE ERROR
                        }
                        break;
                    case 24:
                        if ((int)c > 33 && (int)c < 254)
                        {
                            lexema += c;
                            estadoActual = 25;
                        }
                        break;
                    case 25:
                        if (c == '~')
                        {
                            lexema += c;
                            estadoActual = 26;
                        }
                        else if (c == ',')
                        {
                            lexema += c;
                            estadoActual = 27;
                        }
                        else if (c == ';')
                        {
                            logToken(lexema, ID, Token.Tipo.CONJUNTO);
                            lexema = "";
                            estadoActual = 0;
                            ID = "";
                        }
                        else
                        {
                            //ESTADO DE ERROR
                        }
                        break;
                    case 26:
                        if ((int)c > 33 && (int)c < 256)
                        {
                            lexema += c;
                            estadoActual = 28;
                        }
                        break;
                    case 27:
                        if ((int)c > 33 && (int)c < 256)
                        {
                            lexema += c;
                            estadoActual = 28;
                        }
                        break;
                    case 28:
                        if (c == ';')
                        {
                            logToken(lexema, ID, Token.Tipo.CONJUNTO);
                            lexema = "";
                            estadoActual = 0;
                            ID = "";
                        }
                        else if (c == ',')
                        {
                            lexema += c;
                            estadoActual = 27;
                        }
                        else
                        {
                            //ESTADO DE ERROR
                        }
                        break;
                    /**************************************
                    ** ANALISIS DE EXPRESIONES Y LEXEMAS **                   
                    **************************************/
                    case 3:
                        if (c == '-')
                        {
                            ID = lexema;
                            lexema = "";
                            estadoActual = 7;
                        }
                        else if (c == ':')
                        {
                            ID = lexema;
                            lexema += c;
                            estadoActual = 8;
                        }
                        else
                        {
                            if (c != ' ')
                            {
                                lexema += c;
                            }
                        }
                        break;
                    ////////// EXPRESIONES ////////////
                    case 7:
                        if (c == '>')
                        {
                            estadoActual = 12;
                        }
                        else
                        {
                            //ESTADO DE ERROR
                        }
                        break;
                    case 12:
                        if (c == ';')
                        {
                            logToken(lexema, ID, Token.Tipo.EXPRESION);
                            estadoActual = 0;
                            lexema = "";
                            ID = "";
                        }
                        else
                        {
                            lexema += c;
                        }
                        break;
                    ////////// LEXEMAS //////////
                    case 8:
                        if (c == '"')
                        {
                            estadoActual = 13;
                        }
                        else
                        {
                            //ESTADO DE ERROR
                        }
                        break;
                    case 13:
                        if (c == '"')
                        {
                            estadoActual = 17;
                        }
                        else
                        {
                            lexema += c;
                        }
                        break;
                    case 17:
                        if (c == ';')
                        {
                            logToken(lexema, ID, Token.Tipo.LEXEMA);
                            lexema = "";
                            ID = "";
                            estadoActual = 0;
                        }
                        else
                        {
                            //ESTADO DE ERROR;
                        }
                        break;
                        /* CASO QUE RECOLECTARA TODOS LOS ERRORES HASTA EL SALTO DE LINEA
                         * CORRESPONDIENTE*/
                    case 100:
                        if (c == '\n')
                        {
                            Errores.AddLast(lexema);
                            lexema = "";
                            ID = "";
                            estadoActual = 0;
                        }
                        else
                        {
                            lexema += c;
                        }
                        break;
                }
            }
        }

        private void logToken(String lexema, String ID, Token.Tipo tipo)
        {
            Formulario.SetLog(lexema + "<------>" + tipo.ToString());
            //Console.WriteLine(lexema);
            Tokens.AddLast(new Token(ID, lexema, tipo));
            if (tipo == Token.Tipo.EXPRESION)
            {
                Expresiones.AddLast(new Expresion(lexema,ID));                
            }            
        }

        public void GenerarDFA()
        {
            foreach (Expresion ex in Expresiones)
            {
                Console.WriteLine("ID: {0} Expresion: {1}", ex.getNombre(), ex.getExpresion());
                AFN(ex);
            }
        }

        private void AFN(Expresion ex)
        {
            string entrada = ex.getExpresion();
            AFNList.Clear();
            int contador = 0;
            char c;
            string hoja = "";
            for (int i = 0; i < entrada.Length; i++)
            {
                c = entrada.ElementAt(i);
                switch (c)
                {
                    case '.':
                        AFNList.AddLast(new AFN(Proyecto1.AFN.Tipo.OPERADOR, Proyecto1.AFN.Estructura.CONCATENACION));
                        
                        break;
                    case '|':
                        AFNList.AddLast(new AFN(Proyecto1.AFN.Tipo.OPERADOR, Proyecto1.AFN.Estructura.DISYUNCION));
                                               
                        break;
                    case '?':
                        AFNList.AddLast(new AFN(Proyecto1.AFN.Tipo.OPERADORUNITARIO, Proyecto1.AFN.Estructura.INTERROGATIVO));
                        
                        break;
                    case '*':
                        AFNList.AddLast(new AFN(Proyecto1.AFN.Tipo.OPERADORUNITARIO, Proyecto1.AFN.Estructura.KLEENE));
                        
                        break;
                    case '+':
                        AFNList.AddLast(new AFN(Proyecto1.AFN.Tipo.OPERADORUNITARIO, Proyecto1.AFN.Estructura.POSITIVO));
                        
                        break;
                    case '{':
                        i++;
                        c = entrada.ElementAt(i);
                        while (c != '}')
                        {
                            hoja += c;
                            i++;
                            c = entrada.ElementAt(i);
                        }
                        if (!hoja.Equals(""))
                        {
                            AFNList.AddLast(new AFN(hoja, contador, Proyecto1.AFN.Tipo.OPERANDO, Proyecto1.AFN.Estructura.HOJA, listaEstados));
                            
                            if (!ex.getList().Contains(hoja))
                            {
                                ex.getList().Add(hoja);
                            }
                            contador += 2;
                            hoja = "";
                        }
                        break;
                    case '\"':
                        i++;
                        c = entrada.ElementAt(i);
                        while (c != '\"')
                        {
                            hoja += c;
                            i++;
                            c = entrada.ElementAt(i);
                        }
                        if (!hoja.Equals(""))
                        {
                            AFNList.AddLast(new AFN(hoja, contador,Proyecto1.AFN.Tipo.OPERANDO, Proyecto1.AFN.Estructura.HOJA, listaEstados));
                            if (!ex.getList().Contains(hoja))
                            {
                                ex.getList().Add(hoja);
                            }
                            contador += 2;
                            hoja = "";
                        }
                        break;
                    case ' ':
                        break;
                    default:
                        hoja += c;
                        break;
                }
            }
            ///CREANDO EL AFN CON METODO DE THOMPSON
            while (AFNList.Count() > 1)
            {
                for (int i = 0; i < AFNList.Count(); i++)
                {
                    if ((AFNList.ElementAt(i).tipoNodo == Proyecto1.AFN.Tipo.OPERANDO) && (AFNList.ElementAt(i-1).tipoNodo == Proyecto1.AFN.Tipo.OPERANDO))
                    {
                        if (AFNList.ElementAt(i - 2).estructura == Proyecto1.AFN.Estructura.DISYUNCION)
                        {
                            AFNList.ElementAt(i - 2).Disyuncion(AFNList.ElementAt(i - 1), AFNList.ElementAt(i), contador, listaEstados);
                            contador += 2;
                        }
                        else if (AFNList.ElementAt(i - 2).estructura == Proyecto1.AFN.Estructura.CONCATENACION)
                        {
                            AFNList.ElementAt(i - 2).Concatenar(AFNList.ElementAt(i - 1), AFNList.ElementAt(i), contador);
                            contador += 2;
                        }
                        AFNList.ElementAt(i - 2).tipoNodo = Proyecto1.AFN.Tipo.OPERANDO;
                        AFNList.Remove(AFNList.ElementAt(i));
                        AFNList.Remove(AFNList.ElementAt(i - 1));
                        i = AFNList.Count() + 1;
                    }
                    else if ((AFNList.ElementAt(i).tipoNodo == Proyecto1.AFN.Tipo.OPERANDO) && (AFNList.ElementAt(i - 1).tipoNodo == Proyecto1.AFN.Tipo.OPERADORUNITARIO))
                    {
                        if (AFNList.ElementAt(i-1).estructura == Proyecto1.AFN.Estructura.INTERROGATIVO)
                        {
                            AFNList.ElementAt(i - 1).Indefinido(AFNList.ElementAt(i), contador, listaEstados);
                            contador += 2;
                        }
                        else if (AFNList.ElementAt(i - 1).estructura == Proyecto1.AFN.Estructura.KLEENE)
                        {
                            AFNList.ElementAt(i - 1).Kleene(AFNList.ElementAt(i), contador, listaEstados);
                            contador += 2;
                        }
                        else if (AFNList.ElementAt(i - 1).estructura == Proyecto1.AFN.Estructura.POSITIVO)
                        {
                            AFNList.ElementAt(i - 1).Positiva(AFNList.ElementAt(i), contador, listaEstados);
                            contador += 2;
                        }
                        AFNList.ElementAt(i - 1).tipoNodo = Proyecto1.AFN.Tipo.OPERANDO;
                        AFNList.Remove(AFNList.ElementAt(i));                        
                        i = AFNList.Count() + 1;
                    }
                }
            }
            ex.setAFN(AFNList.ElementAt(0));
            AFNList.Clear();            
            Console.WriteLine("Terminado el AFN");
            generarImagenAFN(ex);
            generarTransiciones(ex);
            generarTabla(ex);
            generaImagenDFA(ex);
            TerminadoElAnalisis();
        }

        List<Conjunto> Analizados = new List<Conjunto>();
        int nombrarConjuntos = 1;
        private void generarTransiciones(Expresion ex)
        {
            Analizados.Clear();
            nombrarConjuntos = 1;
            //MIENTRAS HAYA CONJUNTOS PARA ANALIZAR SE HARA LO DE ABAJO
            while (ex.GetConjuntos().Count > 0)
            {
                //CICLO POR CADA ELEMENTO DE LA LISTA
                for (int i = 0; i < ex.GetConjuntos().First().Movedura.Count; i++)
                {
                    //CICLO POR CADA TERMINAL QUE SE VA EVALUAR
                    for (int j = 0; j < ex.getList().Count; j++)
                    {
                        Conjunto temporal = new Conjunto();
                        temporal.nombre = "S" + nombrarConjuntos;
                        nombrarConjuntos++;
                        ayudanteRecorrido(EncontrarEstado(ex.GetConjuntos().First().Movedura.ElementAt(i)), ex.getList().ElementAt(j), ex.GetConjuntos().First().Movedura.ElementAt(i), ref temporal);  
                        if (temporal.Movedura.Count > 0)
                        {
                            if (!CompararConjuntos(temporal) && !CompararConjuntosActual(temporal, ex.GetConjuntos().First()))
                            {
                                //SI NO HAY SEMEJANTES EN LA LISTA SE AÑADE
                                Console.WriteLine("Para {0} se añade un nuevo conjunto", ex.getList().ElementAt(j));
                                
                                ex.GetConjuntos().Add(temporal);
                                //Console.WriteLine("TRANSICION DE {0} A {1} CON {2}", ex.GetConjuntos().First().nombre, temporal.nombre, ex.getList().ElementAt(j));

                            }
                            else
                            {
                                Console.WriteLine("{0} *************** es igual a ************* {1}", temporal.nombre, ex.GetConjuntos().First().nombre);
                                Console.WriteLine("Para {0} no se añade el conjunto", ex.getList().ElementAt(j));
                                nombrarConjuntos--;
                            }
                            ex.GetTransicions().Add(new Transicion(ex.GetConjuntos().First(),temporal, ex.getList().ElementAt(j)));
                            Console.WriteLine("TRANSICION DE {0} A {1} CON {2}", ex.GetConjuntos().First().nombre, temporal.nombre, ex.getList().ElementAt(j));

                        }
                        else
                        {
                            Console.WriteLine("Para {0} no se añade el conjunto", ex.getList().ElementAt(j));
                            nombrarConjuntos--;
                        }
                    }
                }
                Analizados.Add(ex.GetConjuntos().First());
                ex.GetConjuntos().RemoveAt(0);//eliminamos el conjunto primero (el analizado)
            }
            ex.SetConjuntos(Analizados);
            Console.WriteLine("Finalizacion de While");
        }
        private void ayudanteRecorrido(Estado a, string transicion, int init, ref Conjunto cn)
        {
            Console.WriteLine("Analizamos el estado {0} para la transicion {1}",a.nombre,transicion);           
                if (a.noEstado == init)
                {
                    //EVALUAR LA PRIMERA SALIDA
                    if (a.siguientePrimero != null)
                    {
                        if (a.transicionPrimero.Equals(""))
                        {
                        //VERIFICAR QUE LA CERRADURA EVALUADA NO EXISTE EN EL CONJUNTO DE CERRADURAS
                            if (!VerificarCerradura(a.siguientePrimero.noEstado,cn))
                            {
                                cn.Cerradura.Add(a.siguientePrimero.noEstado);
                                ayudanteRecorrido(a.siguientePrimero, transicion, a.siguientePrimero.noEstado, ref cn);
                            }
                        }
                        else if (a.transicionPrimero.Equals(transicion))
                        {
                            cn.Movedura.Add(a.siguientePrimero.noEstado);
                        }
                    }
                    //EVALUAR LA SEGUNDA SALIDA
                    if (a.siguienteSegundo != null)
                    {
                        if (a.transicionSegundo.Equals(""))
                        {
                        //VERIFICAR QUE LA CERRADURA EVALUADA NO EXISTE EN EL CONJUNTO DE CERRADURAS
                        if (!VerificarCerradura(a.siguienteSegundo.noEstado, cn))
                            {
                                cn.Cerradura.Add(a.siguienteSegundo.noEstado);
                                ayudanteRecorrido(a.siguienteSegundo, transicion, a.siguienteSegundo.noEstado, ref cn);
                            }
                        }
                        else if (a.transicionPrimero.Equals(transicion))
                        {
                            cn.Movedura.Add(a.siguienteSegundo.noEstado);
                        }
                    }
                }                          
        }
        private Estado EncontrarEstado(int init)
        {
            Estado retorno=listaEstados.First();
            foreach (Estado item in listaEstados)
            {
                if (item.noEstado == init)
                {
                    retorno = item;
                }
            }
            return retorno;
        }
        private bool VerificarCerradura(int ce, Conjunto cn)
        {
            bool existe = false;
            foreach (int item in cn.Cerradura)
            {
                if (item == ce)
                {
                    existe = true;
                }
            }
            return existe;
        }
        private bool CompararConjuntos(Conjunto a)
        {
            string save = a.nombre;
            bool iguales = false;  
            foreach (Conjunto item in Analizados)
            {
                Console.WriteLine(item.nombre+"--"+a.nombre);
                if (!iguales)
                {
                    if (item.Movedura.Count == a.Movedura.Count)
                    {
                        for (int i = 0; i < item.Movedura.Count(); i++)
                        {
                            if (item.Movedura.ElementAt(i) == a.Movedura.ElementAt(i))
                            {
                                iguales = true;
                                a.nombre = item.nombre;
                            }
                            else
                            {
                                iguales = false;
                                a.nombre = save;
                                break;
                            }
                        }
                    }
                }
            }            
            return iguales;
        }
        private bool CompararConjuntosActual(Conjunto a, Conjunto b)
        {
            //a temporal
            //b analizante
            string save = a.nombre;
            bool iguales = false;
            if (a.Movedura.Count == b.Movedura.Count)
            {
                for (int i = 0; i < a.Movedura.Count; i++)
                {
                    if (b.Movedura.ElementAt(i) == a.Movedura.ElementAt(i))
                    {
                        iguales = true;
                        a.nombre = b.nombre;
                    }
                    else
                    {
                        iguales = false;
                        a.nombre = save;
                        break;
                    }
                }
            }           
            return iguales;
        }
        private void generarTabla(Expresion ex)
        {
            DataGridView tabla = new DataGridView();
            tabla.ColumnCount = ex.getList().Count + 1;
            tabla.ColumnHeadersVisible = true;
            tabla.Dock = DockStyle.Fill;
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.BackColor = System.Drawing.Color.Beige;            
            tabla.ColumnHeadersDefaultCellStyle = columnHeaderStyle;            
            tabla.Columns[0].Name = "Cerradura";
            
            for (int i = 0; i < ex.getList().Count; i++)
            {                
                tabla.Columns[i + 1].Name = ex.getList().ElementAt(i);
            }
            
            //DECLARAMOS UN ARRAY DEL TAMAÑO DE LA LISTA DE TERMINALES MAS LA COLUMNA CERRADURA
            //ESTE ARRAY ALMACENA LAS TRANSICIONES POR CADA CONJUNTO
            string[] rowToAdd = new string[ex.getList().Count + 1];
            Console.WriteLine("Construir tabla");
            //PARA CADA CONJUNTO ANALIZADO BUSCAMOS SUS TRANSICIONES
            foreach (Conjunto item in ex.GetConjuntos())
            {
                rowToAdd[0] = item.nombre;
                //ITERAMOS LAS TRANSICIONES
                for (int i = 0; i < ex.GetTransicions().Count; i++)
                {
                    /*SI LA TRANSICION QUE ESTAMOS ANALIZANDO TIENE COMO INICIAL EL CONJUNTO QUE ESTAMOS ANALIZANDO
                     *SE PROCEDE A LLENAR EL ARRAY CON SU INFORMACION */
                    if (ex.GetTransicions().ElementAt(i).Inicial.nombre.Equals(item.nombre))
                    {
                        rowToAdd[ex.getList().IndexOf(ex.GetTransicions().ElementAt(i).transicion) + 1] = ex.GetTransicions().ElementAt(i).Final.nombre;
                    }
                }
                //TERMINADA LA BUSQUEDA DE TRANSICIONES SE PROCEDE APLICAR EL ARRAY EN LA TABLA y LIMPIAR SUS ELEMENTOS
                tabla.Rows.Add(rowToAdd);
                for (int i = 0; i < rowToAdd.Length; i++)
                {
                    rowToAdd[i] = null;
                }
            }
            //TERMINADA LA ITERACION DE CONJUNTOS APLICAMOS LA TABLA
            ex.SetTablaTransiciones(tabla);
            //Formulario.SetTable(tabla);
        }
        private void generaImagenDFA(Expresion ex)
        {
            string graph = "digraph g {	\nrankdir=LR;\n node[shape = doublecircle];\n";
            //IMPRIMIR LOS DE ACPETACION
            foreach (Conjunto item in ex.GetConjuntos())
            {
                for (int i = 0; i < item.Cerradura.Count; i++)
                {
                    if (item.Cerradura.ElementAt(i) == ex.GetAFN().final.noEstado)
                    {
                        graph += item.nombre + ";\n";
                        break;
                    }
                }
                for (int i = 0; i < item.Movedura.Count; i++)
                {
                    if (item.Movedura.ElementAt(i) == ex.GetAFN().final.noEstado)
                    {
                        graph += item.nombre + ";\n";
                        break;
                    }
                }
            }
            //AÑADIR LOS ESTADOS NORMALES
            graph += "node[shape = circle];\n";

            //IMPRIMIR LAS TRANSICIONES
            foreach (Transicion item in ex.GetTransicions())
            {
                graph += item.Inicial.nombre+ " -> "+item.Final.nombre+" [label =\""+item.transicion+"\"]; \n\r";
            }
            graph += "}";
            System.IO.StreamWriter redactor = new System.IO.StreamWriter(System.IO.Path.GetDirectoryName(Formulario.ObtenerPath())+"\\"+ex.getNombre()+"DFA.dot");
            ex.SetDot(System.IO.Path.GetDirectoryName(Formulario.ObtenerPath())+"\\" + ex.getNombre() + "DFA.dot");
            redactor.Write(graph);
            redactor.Close();
            System.Diagnostics.Process cmd = new System.Diagnostics.Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            cmd.StandardInput.WriteLine("dot -Tpng {0} -o {1}",ex.GetDot(),System.IO.Path.GetDirectoryName(ex.GetDot())+"\\"+ex.getNombre()+"DFA.png");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.Close();
            //ex.SetImagen(System.IO.Path.GetDirectoryName(ex.GetImagen()) + "\\" + ex.getNombre() + ".png");
            ex.SetImage(System.IO.Path.GetDirectoryName(ex.GetDot())+"\\"+ex.getNombre()+"DFA.png");
        }
        private void TerminadoElAnalisis()
        {
            listaEstados.Clear();
            Analizados.Clear();
        }
        private void generarImagenAFN(Expresion ex)
        {
            string graph = "digraph g {	\nrankdir=LR;\n node[shape = doublecircle];\n";
            //AÑADIR EL ESTADO DE ACEPTACION
            graph += ex.GetAFN().final.nombre+";\n";
            //AÑADIR ESTADOS NORMALES
            graph += "node[shape = circle];\n";
            //AÑADIR TRANSICIONES
            foreach (Estado item in listaEstados)
            {
                //PRIMERA SALIDA
                if (item.siguientePrimero != null)
                {
                    //SI LA TRANSICION ES VACIA SE IMPRIME EPSILON, SI NO PS NO ;V
                    if (item.transicionPrimero.Equals(""))
                    {
                        graph += item.nombre + " -> " + item.siguientePrimero.nombre + " [label =\" &#949; \"]; \n\r";
                    }
                    else
                    {
                        graph += item.nombre + " -> " + item.siguientePrimero.nombre + " [label =\" "+item.transicionPrimero+" \"]; \n\r";
                    }
                }
                //SEGUNDA SALIDA
                if (item.siguienteSegundo != null)
                {
                    //SI LA TRANSICION ES VACIA SE IMPRIME EPSILON, SI NO PS NO ;V
                    if (item.transicionSegundo.Equals(""))
                    {
                        graph += item.nombre + " -> " + item.siguienteSegundo.nombre + " [label =\" &#949; \"]; \n\r";
                    }
                    else
                    {
                        graph += item.nombre + " -> " + item.siguienteSegundo.nombre + " [label =\" " + item.transicionSegundo + " \"]; \n\r";
                    }
                }
            }
            graph += "}";
            System.IO.StreamWriter redactor = new System.IO.StreamWriter(System.IO.Path.GetDirectoryName(Formulario.ObtenerPath()) + "\\" + ex.getNombre() + "AFN.dot");
            ex.SetAFNDot(System.IO.Path.GetDirectoryName(Formulario.ObtenerPath()) + "\\" + ex.getNombre() + "AFN.dot");
            redactor.Write(graph);
            redactor.Close();
            System.Diagnostics.Process cmd = new System.Diagnostics.Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            cmd.StandardInput.WriteLine("dot -Tpng {0} -o {1}", ex.GetAFNADot(), System.IO.Path.GetDirectoryName(ex.GetAFNADot()) + "\\" + ex.getNombre() + "AFN.png");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.Close();
            ex.SetAFNimage(System.IO.Path.GetDirectoryName(ex.GetAFNADot()) + "\\" + ex.getNombre() + "AFN.png");

        }



    }
}
