using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _LFP_Proyecto
{
    class AnalizadorLexico
    {
        private LinkedList<Token> Salida;
        

        private int estadoActual;
        private string lexema;

        public void agregarToken(Token.Tipo tipo)
        {
            //Console.WriteLine("Llego: "+lexema+" TIPO: "+tipo);
            Salida.AddLast(new Token(tipo, lexema));
            lexema = "";
            estadoActual = 0;
        }

        public Boolean buscarToken()
        {
            bool finder = false;
            LinkedList<Token> refrres = new LinkedList<Token>(Salida);
            foreach (Token item in refrres)
            {
                if (item.GetValor() == lexema)
                {
                    agregarToken(item.GetTipo());
                    finder = true;
                }
            }
            return finder;
            /*bool finder = false;
            LinkedList<Token> Repositorio = Salida;
            foreach (Token item in Repositorio)
            {
                if (item.GetValor() == lexema)
                {
                    agregarToken(item.GetTipo());
                    finder = true;
                }
            }
            return finder;*/
        }


        public LinkedList<Token> Analizar(String entrada)
        {
            entrada = entrada + "#";
            Salida = new LinkedList<Token>();
            estadoActual = 0;
            lexema = "";
            int classname = 0;
            int VarInt = 0;
            int VarString = 0;
            int VarBool = 0;
            int VarFloat = 0;
            int varChar = 0;
            int estadoCaracter = 0;
            char c;

            for (int i = 0; i < entrada.Length-1; i++)
            {
                c = entrada.ElementAt(i);

                switch (estadoActual)
                {
                    case 0:
                        if (char.IsDigit(c) && entrada.ElementAt(i-1)== ' ')
                        {
                            estadoActual = 2;
                            lexema += c;
                        }
                        else if (c.CompareTo('/') == 0)
                        {
                            if (entrada.ElementAt(i+1)=='/' || entrada.ElementAt(i+1)=='*')
                            {
                                estadoActual = 5;
                            }
                            else
                            {
                                
                                lexema += c;
                                agregarToken(Token.Tipo.SIGNO_DIV);
                            }
                        }
                        else if (c.CompareTo('"')==0)
                        {
                            estadoActual = 3;
                            //lexema += c;
                        }
                        else if (c.CompareTo('\'')==0)
                        {
                            estadoActual = 13;
                        }
                        else if (c.CompareTo('{')==0)
                        {
                            lexema += c;
                            agregarToken(Token.Tipo.SIGNO_LLAVEABRE);
                        }
                        else if (c.CompareTo('}')==0)
                        {
                            lexema += c;
                            agregarToken(Token.Tipo.SIGNO_LLAVECIERRE);
                        }
                        else if (c.CompareTo('(') == 0)
                        {
                            lexema += c;
                            agregarToken(Token.Tipo.SIGNO_PARABRE);
                        }
                        else if (c.CompareTo(')') == 0)
                        {
                            lexema += c;
                            agregarToken(Token.Tipo.SIGNO_PARCIERRE);
                        }
                        else if (c.CompareTo('[') == 0)
                        {
                            lexema += c;
                            agregarToken(Token.Tipo.SIGNO_CORABRE);
                        }
                        else if (c.CompareTo(']') == 0)
                        {
                            lexema += c;
                            agregarToken(Token.Tipo.SIGNO_CORCIERRE);
                        }
                        else if (c.CompareTo('<') == 0)
                        {
                            lexema += c;
                            agregarToken(Token.Tipo.SIGNO_MENOR);
                        }
                        else if (c.CompareTo('>') == 0)
                        {
                            lexema += c;
                            agregarToken(Token.Tipo.SIGNO_MAYOR);
                        }
                        else if (c.CompareTo('=')==0)
                        {
                            lexema += c;
                            agregarToken(Token.Tipo.SIGNO_IGUAL);
                        }
                        else if (c.CompareTo('+') == 0)
                        {
                            lexema += c;
                            agregarToken(Token.Tipo.SIGNO_MAS);
                        }
                        else if (c.CompareTo('-') == 0)
                        {
                            lexema += c;
                            agregarToken(Token.Tipo.SIGNO_MENOS);

                        }
                        else if (c.CompareTo('!') == 0)
                        {
                            lexema += c;
                            agregarToken(Token.Tipo.SIGNO_EXC);
                        }
                        else if (c.CompareTo('*') == 0)
                        {
                            lexema += c;
                            agregarToken(Token.Tipo.SIGNO_POR);
                        }
                        else if (c.CompareTo(':')==0)
                        {
                            lexema += c;
                            agregarToken(Token.Tipo.SIGNO_DOSPUNTOS);

                        }
                        else if (c.CompareTo(';') == 0)
                        {
                            VarInt = 0;
                            VarString = 0;
                            VarBool = 0;
                            VarFloat = 0;
                            varChar = 0;
                            estadoCaracter = 0;
                            lexema += c;
                            agregarToken(Token.Tipo.SIGNO_PUNTOCOMA);
                        }
                        else if (c.CompareTo(',') == 0)
                        {
                            lexema += c;
                            agregarToken(Token.Tipo.SIGNO_COMA);
                        }
                        else if (c.CompareTo('.')==0)
                        {
                            lexema += c;
                            agregarToken(Token.Tipo.SIGNO_PUNTO);
                        }
                        else if (c.CompareTo(' ')==0 || c.CompareTo('\n') == 0)
                        {
                            lexema = "";
                        }
                        else
                        {
                            
                            if (entrada.ElementAt(i+1)==' ' || entrada.ElementAt(i+1) == ';' || entrada.ElementAt(i+1)==',' || entrada.ElementAt(i+1)=='\n' 
                                || entrada.ElementAt(i+1)=='{' || entrada.ElementAt(i + 1) == '}' || entrada.ElementAt(i + 1) == '(' || entrada.ElementAt(i + 1) == ')'
                                || entrada.ElementAt(i + 1) == '[' || entrada.ElementAt(i + 1) == ']' || entrada.ElementAt(i+1)=='.' || entrada.ElementAt(i+1)==':'
                                || entrada.ElementAt(i+1)=='+' || entrada.ElementAt(i+1)=='-')
                            {
                                lexema += c;
                               // Console.WriteLine("Dentro del Switch: "+lexema);
                                switch (lexema)
                                {
                                    case "class":
                                        classname = 1;
                                        agregarToken(Token.Tipo.RESERVADA_CLASS);
                                        break;
                                    case "static":
                                        agregarToken(Token.Tipo.RESERVADA_STATIC);
                                        break;
                                    case "void":
                                        agregarToken(Token.Tipo.RESERVADA_VOID);
                                        break;
                                    case "string":
                                        VarString = 1;
                                        agregarToken(Token.Tipo.RESERVADA_STRING);
                                        break;
                                    case "String":
                                        VarString = 1;
                                        agregarToken(Token.Tipo.RESERVADA_STRING);
                                        break;
                                    case "int":
                                        VarInt = 1;
                                        agregarToken(Token.Tipo.RESERVADA_INT);
                                        break;
                                    case "float":
                                        VarFloat = 1;
                                        agregarToken(Token.Tipo.RESERVADA_FLOAT);
                                        break;
                                    case "char":
                                        varChar = 1;
                                        agregarToken(Token.Tipo.RESERVADA_CHAR);
                                        break;
                                    case "bool":
                                        VarBool = 1;
                                        agregarToken(Token.Tipo.RESERVADA_BOOL);
                                        break;
                                    case "if":
                                        agregarToken(Token.Tipo.RESERVADA_IF);
                                        break;
                                    case "new":
                                        agregarToken(Token.Tipo.RESERVADA_NEW);
                                        break;
                                    case "else":
                                        agregarToken(Token.Tipo.RESERVADA_ELSE);
                                        break;
                                    case "switch":
                                        agregarToken(Token.Tipo.RESERVADA_SWITCH);
                                        break;
                                    case "case":
                                        agregarToken(Token.Tipo.RESERVADA_CASE);
                                        break;
                                    case "break":
                                        agregarToken(Token.Tipo.RESERVADA_BREAK);
                                        break;
                                    case "default":                             
                                        agregarToken(Token.Tipo.RESERVADA_DEFAULT);
                                        break;
                                    case "for":
                                        agregarToken(Token.Tipo.RESERVADA_FOR);
                                        break;
                                    case "Console":
                                        agregarToken(Token.Tipo.RESERVADA_CONSOLE);
                                        break;
                                    case "WriteLine":
                                        agregarToken(Token.Tipo.RESERVADA_WRITELINE);
                                        break;
                                    case "while":
                                        agregarToken(Token.Tipo.RESERVADA_WHILE);
                                        break;
                                    case "Main":
                                        agregarToken(Token.Tipo.RESERVADA_MAIN);
                                        break;
                                    case "args":
                                        agregarToken(Token.Tipo.RESERVADA_ARGS);
                                        break;
                                    case "false":
                                        agregarToken(Token.Tipo.BOOL_FALSE);
                                        break;
                                    case "true":
                                        agregarToken(Token.Tipo.BOOL_TRUE);
                                        break;
                                    default:
                                        if (VarInt == 1)
                                        {
                                            agregarToken(Token.Tipo.VAR);                                           
                                        }
                                        else if (VarBool==1)
                                        {
                                            agregarToken(Token.Tipo.VAR);                                           
                                        }
                                        else if (VarFloat==1)
                                        {
                                            agregarToken(Token.Tipo.VAR);                                            
                                        }
                                        else if (varChar == 1)
                                        {
                                            agregarToken(Token.Tipo.VAR);
                                        }
                                        else if (VarString == 1)
                                        {
                                            agregarToken(Token.Tipo.VAR);
                                        }
                                        else if (classname == 1)
                                        {
                                            agregarToken(Token.Tipo.EXPRESION);
                                            classname = 0;
                                        }
                                        else
                                        {
                                             //buscarToken();
                                             if (buscarToken())
                                             {
                                                 Console.WriteLine("token Encontrado");
                                             }
                                             else
                                             {
                                                 agregarToken(Token.Tipo.DESCONOCIDO);
                                             }
                                            
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                lexema += c;
                            }
                        }
                        break;

                    case 2:
                        if (char.IsDigit(c))
                        {
                            lexema += c;
                        }
                        else if (c.CompareTo('.') == 0)
                        {
                            estadoActual = 6;
                            lexema += c;
                        }
                        else
                        {
                            i -= 1;
                            agregarToken(Token.Tipo.DIGITO_ENTERO);
                        }
                        break;
                    case 3:
                        if (c.CompareTo('"')==0)
                        {
                            agregarToken(Token.Tipo.CADENA);
                        }
                        else
                        {
                            lexema += c;
                        }
                        break;
                    case 5:
                        if (c.CompareTo('*')==0)
                        {
                            estadoActual = 9;
                        }
                        else if (c.CompareTo('/')==0)
                        {
                            estadoActual = 8;
                        }
                        else
                        {
                            estadoActual = 0;
                            lexema += c;
                        }
                        break;
                    case 6:
                        if (char.IsDigit(c))
                        {
                            lexema += c;
                        }
                        else
                        {
                            i -= 1;
                            agregarToken(Token.Tipo.DIGITO_REAL);
                        }
                        break;
                    case 8:
                        if (entrada.ElementAt(i+1) == '\n')
                        {
                            lexema += c;
                            agregarToken(Token.Tipo.COMENTARIO_LINEA);
                        }
                        else
                        {
                            lexema += c;
                        }
                        break;
                    case 9:
                        if (c.CompareTo('*')==0)
                        {
                            estadoActual = 11;
                        }
                        else
                        {
                            lexema += c;
                        }
                        break;
                    case 11:
                        if (c.CompareTo('/')==0)
                        {
                            agregarToken(Token.Tipo.COMENTARIO_MULTI);
                        }
                        else
                        {
                            Console.WriteLine("Error en C");
                        }
                        break;
                    case 13:
                        if (c.CompareTo('\'')==0)
                        {
                            agregarToken(Token.Tipo.CARACTER);
                            estadoCaracter = 0;
                        }
                        else if (estadoCaracter == 0)
                        {
                            lexema += c;
                        }
                        else
                        {
                            Console.WriteLine("Error en C");
                        }
                        break;
                }
            }


            return Salida;

        }

        public void imprimirLista(LinkedList<Token> lista)
        {
            foreach (Token item in lista)
            {
                Console.WriteLine(item.GetTipoString() + "<--->" + item.GetValor());
            }

        }
    }
}
