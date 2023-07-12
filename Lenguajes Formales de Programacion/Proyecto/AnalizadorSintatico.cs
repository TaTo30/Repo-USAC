using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _LFP_Proyecto
{
    class AnalizadorSintatico
    {
        int controlToken;
        Token tokenActual;
        LinkedList<Token> listaTok;
        LinkedList<Simbolo> listaSimbolos = new LinkedList<Simbolo>();
        string textoTraducido = "";
        int contadorSentencias = 0;
        string constanteSwitch = "";
        Simbolo.Tipo wardTipo;
        string wardValor, wardNombre;
        string printConsola = "";

        public LinkedList<Simbolo> ListaSimbolos()
        {
            return this.listaSimbolos;
        }

        public string printConsole()
        {
            return this.printConsola;
        }
        
        public string solocitaTrad()
        {
            return this.textoTraducido;
        }
        public string getTipoError(Token.Tipo tip)
        {
            switch (tip)
            {
                case Token.Tipo.CADENA:
                    return "Cadena";
                case Token.Tipo.RESERVADA_NEW:
                    return "Reservada new";
                case Token.Tipo.RESERVADA_CLASS:
                    return "Reservada class";
                case Token.Tipo.RESERVADA_STATIC:
                    return "Reservada static";
                case Token.Tipo.RESERVADA_VOID:
                    return "Reservada Void";
                case Token.Tipo.RESERVADA_STRING:
                    return "Reservada string";
                case Token.Tipo.RESERVADA_INT:
                    return "Reservada Int";
                case Token.Tipo.RESERVADA_FLOAT:
                    return "Reservada Float";
                case Token.Tipo.RESERVADA_CHAR:
                    return "Reservada Char";
                case Token.Tipo.RESERVADA_BOOL:
                    return "Reservada Bool";
                case Token.Tipo.RESERVADA_IF:
                    return "Reservada If";
                case Token.Tipo.RESERVADA_ELSE:
                    return "Reservada else";
                case Token.Tipo.RESERVADA_SWITCH:
                    return "Reservada switch";
                case Token.Tipo.RESERVADA_CASE:
                    return "Reservada Case";
                case Token.Tipo.RESERVADA_ARGS:
                    return "Reservada args";
                case Token.Tipo.RESERVADA_BREAK:
                    return "Reservada break";
                case Token.Tipo.RESERVADA_DEFAULT:
                    return "Reservada Default";
                case Token.Tipo.RESERVADA_FOR:
                    return "Reservada For";
                case Token.Tipo.RESERVADA_CONSOLE:
                    return "Reservada Console";
                case Token.Tipo.RESERVADA_WRITELINE:
                    return "Reservada Writeline";
                case Token.Tipo.RESERVADA_WHILE:
                    return "Reservada While";
                case Token.Tipo.RESERVADA_MAIN:
                    return "Reservada Main";
                case Token.Tipo.SIGNO_CORABRE:
                    return "Signo corchete izq";
                case Token.Tipo.SIGNO_CORCIERRE:
                    return "Signo corchete der";
                case Token.Tipo.SIGNO_PARABRE:
                    return "Signo parentesis izq";
                case Token.Tipo.SIGNO_PARCIERRE:
                    return "Signo parentesis der";
                case Token.Tipo.SIGNO_LLAVEABRE:
                    return "Signo llave izq";
                case Token.Tipo.SIGNO_LLAVECIERRE:
                    return "Signo llave der";
                case Token.Tipo.SIGNO_MAYOR:
                    return "Signo mayor que";
                case Token.Tipo.SIGNO_DOSPUNTOS:
                    return "Signo Dos Puntos";
                case Token.Tipo.SIGNO_MENOR:
                    return "Signo menor que";
                case Token.Tipo.SIGNO_IGUAL:
                    return "Signo igual";
                case Token.Tipo.SIGNO_MAS:
                    return "Signo mas";
                case Token.Tipo.SIGNO_MENOS:
                    return "Signo menos";
                case Token.Tipo.SIGNO_EXC:
                    return "Signo exclamacion";
                case Token.Tipo.SIGNO_POR:
                    return "Signo multiplicar";
                case Token.Tipo.SIGNO_DIV:
                    return "Signo dividir";
                case Token.Tipo.SIGNO_PUNTOCOMA:
                    return "Signo punto y coma";
                case Token.Tipo.SIGNO_COMA:
                    return "Signo coma";
                case Token.Tipo.SIGNO_PUNTO:
                    return "Signo Punto";
                case Token.Tipo.COMENTARIO_LINEA:
                    return "comentario una linea";
                case Token.Tipo.COMENTARIO_MULTI:
                    return "comentario multilinea";
                case Token.Tipo.DIGITO_ENTERO:
                    return "numero entero";
                case Token.Tipo.DIGITO_REAL:
                    return "numero real";
                case Token.Tipo.EXPRESION:
                    return "expression";
                case Token.Tipo.CARACTER:
                    return "Caracter";
                case Token.Tipo.VAR:
                    return "Variable";
                case Token.Tipo.BOOL_FALSE:
                    return "Valor Falso";
                case Token.Tipo.BOOL_TRUE:
                    return "Valor True";
                case Token.Tipo.ULTIMO:
                    return "Ultimo Token";
                default:
                    return "Desconocido";
            }

        }

        public void match(Token.Tipo tip) {
            Console.WriteLine("ENTRO: " + tokenActual.GetTipo() + " : " + tokenActual.GetValor());
            if (tokenActual.GetTipo() != tip)
            {
                Console.WriteLine("Se esperaba: " + getTipoError(tip) +"("+tokenActual.GetValor()+")") ;
            }
            if (tokenActual.GetTipo() != Token.Tipo.ULTIMO)
            {
                //Console.WriteLine("A VER OTRO TOKEN: "+tokenActual.GetValor());
                controlToken += 1;
                tokenActual = listaTok.ElementAt(controlToken);
            }
            else
            {
                Console.WriteLine(textoTraducido);
            }
        }


        public void parsear(LinkedList<Token> lista)
        {
            Console.WriteLine("METODO PARSER");
            this.listaTok = lista;
            controlToken = 0;
            tokenActual = listaTok.ElementAt(controlToken);
            CLASS();
        }

        public void busquedaSimbolo(string lexema)
        {
            LinkedList<Simbolo> refres2 = new LinkedList<Simbolo>(listaSimbolos);
            foreach (Simbolo item2 in refres2)
            {
                if (item2.getNombreVar() == lexema)
                {
                    printConsola += item2.getValorVar();
                }
            }


        }

        public void CLASS()
        {
            match(Token.Tipo.RESERVADA_CLASS);
            match(Token.Tipo.EXPRESION);
            match(Token.Tipo.SIGNO_LLAVEABRE);
            match(Token.Tipo.RESERVADA_STATIC);
            match(Token.Tipo.RESERVADA_VOID);
            match(Token.Tipo.RESERVADA_MAIN);
            match(Token.Tipo.SIGNO_PARABRE);
            match(Token.Tipo.RESERVADA_STRING);
            match(Token.Tipo.SIGNO_CORABRE);
            match(Token.Tipo.SIGNO_CORCIERRE);
            match(Token.Tipo.RESERVADA_ARGS);
            match(Token.Tipo.SIGNO_PARCIERRE);
            match(Token.Tipo.SIGNO_LLAVEABRE);
            BLOQUE();
            match(Token.Tipo.SIGNO_LLAVECIERRE);
            match(Token.Tipo.SIGNO_LLAVECIERRE);
        }

        public void BLOQUE()
        {
            DECASG();
            PRINT();
            IFS();
            SWITCHS();
            FORS();
            WHILE();
            
        }

        public void DECASG()
        {
            INSDEC();
            INSASG();
        }

        /*tokenActual.GetTipo() == Token.Tipo.RESERVADA_BOOL || tokenActual.GetTipo() == Token.Tipo.RESERVADA_INT ||
                tokenActual.GetTipo() == Token.Tipo.RESERVADA_STRING || tokenActual.GetTipo() == Token.Tipo.RESERVADA_FLOAT ||
                tokenActual.GetTipo() == Token.Tipo.RESERVADA_CHAR*/

        public void INSDEC()
        {
            if (listaTok.ElementAt(controlToken+1).GetTipo() == Token.Tipo.VAR)
            {
                TIPO();
                if (listaTok.ElementAt(controlToken+1).GetTipo() == Token.Tipo.SIGNO_IGUAL)
                {
                    textoTraducido += espaciador()+tokenActual.GetValor();
                }
                wardNombre = tokenActual.GetValor();
                match(Token.Tipo.VAR);
                ASIGNACION();
                MULTIDEC();
                textoTraducido += "\r\n";
                match(Token.Tipo.SIGNO_PUNTOCOMA);
                COMENT();
                INSDEC();
            }
            else if (listaTok.ElementAt(controlToken+1).GetTipo() == Token.Tipo.SIGNO_CORABRE)
            {
                TIPO();
                match(Token.Tipo.SIGNO_CORABRE);
                match(Token.Tipo.SIGNO_CORCIERRE);
                textoTraducido += espaciador() + tokenActual.GetValor();
                match(Token.Tipo.VAR);
                textoTraducido += tokenActual.GetValor();
                match(Token.Tipo.SIGNO_IGUAL);
                ASIGARRAY();
                textoTraducido += "\r\n";
                match(Token.Tipo.SIGNO_PUNTOCOMA);
                COMENT();
                BLOQUE();
            }
            else
            {
                //EPSILON
            }
        }

        public void ASIGARRAY()
        {
            if (tokenActual.GetTipo() == Token.Tipo.RESERVADA_NEW)
            {
                match(Token.Tipo.RESERVADA_NEW);
                TIPO();
                textoTraducido += tokenActual.GetValor();
                match(Token.Tipo.SIGNO_CORABRE);
                textoTraducido += tokenActual.GetValor();
                match(Token.Tipo.SIGNO_CORCIERRE);
            }
            else if (tokenActual.GetTipo() == Token.Tipo.SIGNO_LLAVEABRE)
            {
                textoTraducido += "[";
                match(Token.Tipo.SIGNO_LLAVEABRE);
                VALOR();
                MASVALARRAY();
                textoTraducido += "]";
                match(Token.Tipo.SIGNO_LLAVECIERRE);
            }
        }

        public void MASVALARRAY()
        {
            if (tokenActual.GetTipo() == Token.Tipo.SIGNO_COMA)
            {
                textoTraducido += tokenActual.GetValor();
                match(Token.Tipo.SIGNO_COMA);
                VALOR();
                MASVALARRAY();
            }
            else
            {
                //EPSILON
            }
        }
        public void INSASG()
        {
            if (tokenActual.GetTipo()==Token.Tipo.VAR)
            {
                textoTraducido += espaciador()+tokenActual.GetValor();
                match(Token.Tipo.VAR);
                textoTraducido += tokenActual.GetValor();
                match(Token.Tipo.SIGNO_IGUAL);
                OPERACION();
                textoTraducido += "\r\n";
                match(Token.Tipo.SIGNO_PUNTOCOMA);
                COMENT();
                DECASG();
            }
            else
            {
                //EPSILON
            }
        }

        public void TIPO()
        {

            if (tokenActual.GetTipo() == Token.Tipo.RESERVADA_INT)
            {
                match(Token.Tipo.RESERVADA_INT);
                wardTipo = Simbolo.Tipo.VAR_INT;
            }
            else if (tokenActual.GetTipo() == Token.Tipo.RESERVADA_FLOAT)
            {
                match(Token.Tipo.RESERVADA_FLOAT);
                wardTipo = Simbolo.Tipo.VAR_FLOAR;
            }
            else if (tokenActual.GetTipo() == Token.Tipo.RESERVADA_CHAR)
            {
                match(Token.Tipo.RESERVADA_CHAR);
                wardTipo = Simbolo.Tipo.VAR_CHAR;
            }
            else if (tokenActual.GetTipo() == Token.Tipo.RESERVADA_STRING)
            {
                match(Token.Tipo.RESERVADA_STRING);
                wardTipo = Simbolo.Tipo.VAR_STRING;
            }
            else if (tokenActual.GetTipo() == Token.Tipo.RESERVADA_BOOL)
            {
                match(Token.Tipo.RESERVADA_BOOL);
                wardTipo = Simbolo.Tipo.VAR_BOOL;
            }
            else
            {
                //EPSILON NO SE HACE NADA
            }
        }

        public void ASIGNACION()
        {
            if (tokenActual.GetTipo()==Token.Tipo.SIGNO_IGUAL)
            {
                textoTraducido += tokenActual.GetValor();
                wardValor = listaTok.ElementAt(controlToken + 1).GetValor();
                listaSimbolos.AddLast(new Simbolo(wardNombre, wardValor, wardTipo));


                match(Token.Tipo.SIGNO_IGUAL);
                OPERACION();
            }
            else
            {
                //EPSILON
                                  
                    listaSimbolos.AddLast(new Simbolo(wardNombre, null, wardTipo));

                
            }
        }

        

        public void MULTIDEC()
        {
            if (tokenActual.GetTipo() == Token.Tipo.SIGNO_COMA)
            {
                textoTraducido += "\r\n";
                match(Token.Tipo.SIGNO_COMA);
                if (listaTok.ElementAt(controlToken+1).GetTipo() == Token.Tipo.SIGNO_IGUAL)
                {
                    textoTraducido += espaciador()+tokenActual.GetValor();
                }
                wardNombre = tokenActual.GetValor();
                match(Token.Tipo.VAR);
                ASIGNACION();
                MULTIDEC();
            }
            else
            {
                //EPSILON   
            }
        }
        int detPrint = 0;
        public void VALOR()
        {
            if (tokenActual.GetTipo() == Token.Tipo.DIGITO_ENTERO)
            {
                textoTraducido += tokenActual.GetValor();
                if (detPrint == 1)
                {
                    printConsola += tokenActual.GetValor();
                }
                match(Token.Tipo.DIGITO_ENTERO);
            }
            else if (tokenActual.GetTipo() == Token.Tipo.DIGITO_REAL)
            {
                textoTraducido += tokenActual.GetValor();
                if (detPrint == 1)
                {
                    printConsola += tokenActual.GetValor();
                }
                match(Token.Tipo.DIGITO_REAL);
            }
            else if (tokenActual.GetTipo() == Token.Tipo.CARACTER)
            {
                textoTraducido += "\'"+tokenActual.GetValor()+"\'";
                if (detPrint == 1)
                {
                    printConsola += tokenActual.GetValor();
                }
                match(Token.Tipo.CARACTER);
            }
            else if (tokenActual.GetTipo() == Token.Tipo.CADENA)
            {
                textoTraducido += "\""+tokenActual.GetValor()+"\"";
                if (detPrint == 1)
                {
                    printConsola += tokenActual.GetValor();
                }
                match(Token.Tipo.CADENA);
            }
            else if (tokenActual.GetTipo() == Token.Tipo.BOOL_TRUE)
            {
                textoTraducido += "True";
                if (detPrint == 1)
                {
                    printConsola += tokenActual.GetValor();
                }
                match(Token.Tipo.BOOL_TRUE);
            }
            else if (tokenActual.GetTipo()==Token.Tipo.BOOL_FALSE)
            {
                textoTraducido += "False";
                if (detPrint == 1)
                {
                    printConsola += tokenActual.GetValor();
                }
                match(Token.Tipo.BOOL_FALSE);
            }
            else if (tokenActual.GetTipo()==Token.Tipo.VAR)
            {
                textoTraducido += tokenActual.GetValor();
                if (detPrint == 1)
                {
                    busquedaSimbolo(tokenActual.GetValor());
                }
                match(Token.Tipo.VAR);
            }
            else
            {
                //EPSILON
            }
        }

        public void OPERACION()
        {
            OP();
            OP2();
        }

        public void OP2()
        {
            if (tokenActual.GetTipo() == Token.Tipo.SIGNO_MAS)
            {
                textoTraducido += tokenActual.GetValor();
                match(Token.Tipo.SIGNO_MAS);
                OP();
                OP2();
            }
            else if (tokenActual.GetTipo() == Token.Tipo.SIGNO_MENOS)
            {
                textoTraducido += tokenActual.GetValor();
                match(Token.Tipo.SIGNO_MENOS);
                OP();
                OP2();
            }
            else
            {
                //EPSILON
            }
        }

        public void OP()
        {
            OP3();
            OP4();
        }

        public void OP4()
        {
            if (tokenActual.GetTipo() == Token.Tipo.SIGNO_POR)
            {
                textoTraducido += tokenActual.GetValor();
                match(Token.Tipo.SIGNO_POR);
                OP3();
                OP4();
            }
            else if (tokenActual.GetTipo() == Token.Tipo.SIGNO_DIV)
            {
                textoTraducido += tokenActual.GetValor();
                match(Token.Tipo.SIGNO_DIV);
                OP3();
                OP4();
            }
            else
            {
                //EPSILON
            }
        }

        public void OP3()
        {
            if (tokenActual.GetTipo() == Token.Tipo.SIGNO_PARABRE)
            {
                textoTraducido += tokenActual.GetValor();
                match(Token.Tipo.SIGNO_PARABRE);
                OPERACION();
                textoTraducido += tokenActual.GetValor();
                match(Token.Tipo.SIGNO_PARCIERRE);
            }
            else
            {
                VALOR();
            }
        }

        public void PRINT()
        {
            if (tokenActual.GetTipo() == Token.Tipo.RESERVADA_CONSOLE)
            {
                detPrint = 1;
                match(Token.Tipo.RESERVADA_CONSOLE);
                match(Token.Tipo.SIGNO_PUNTO);
                match(Token.Tipo.RESERVADA_WRITELINE);
                textoTraducido += espaciador()+"print" + tokenActual.GetValor();
                match(Token.Tipo.SIGNO_PARABRE);
                CONTPRINT();
                textoTraducido += tokenActual.GetValor()+"\r\n";
                match(Token.Tipo.SIGNO_PARCIERRE);
                Console.WriteLine("PRINT:" + printConsola);
                printConsola += "\r\n";
                detPrint = 0;
                match(Token.Tipo.SIGNO_PUNTOCOMA);
                
                COMENT();
                BLOQUE();

            }
        }

        public void CONTPRINT()

        {
            if (tokenActual.GetTipo()!=Token.Tipo.SIGNO_PARCIERRE)
            {
                
                VALOR();
                MASVALORES();
            }
            else
            {
                //EPSILON
            }
        }

        public void MASVALORES()
        {
            if (tokenActual.GetTipo() == Token.Tipo.SIGNO_MAS)
            {
                textoTraducido += tokenActual.GetValor();
                match(Token.Tipo.SIGNO_MAS);
                VALOR();
                MASVALORES();
            }
            else
            {
                //EPSILON
            }
        }

        public void IFS()
        {
            if (tokenActual.GetTipo()==Token.Tipo.RESERVADA_IF)
            {
                textoTraducido += espaciador() + tokenActual.GetValor()+" ";
                
                match(Token.Tipo.RESERVADA_IF);
                match(Token.Tipo.SIGNO_PARABRE);
                CONDICION();
                contadorSentencias += 1;
                Console.WriteLine("Aumenta Contador");
                textoTraducido += ": \r\n";
                match(Token.Tipo.SIGNO_PARCIERRE);
                match(Token.Tipo.SIGNO_LLAVEABRE);
                BLOQUE();
                contadorSentencias -= 1;
                Console.WriteLine("Disminuye Contador");
                match(Token.Tipo.SIGNO_LLAVECIERRE);
                
                ELSES();
                BLOQUE();
            }
        }

        public void CONDICION()
        {
            VALOR();
            OPLOGICO();
            VALOR();
        }
        
        public void OPLOGICO()
        {
            if (tokenActual.GetTipo() == Token.Tipo.SIGNO_IGUAL)
            {
                textoTraducido += tokenActual.GetValor();
                match(Token.Tipo.SIGNO_IGUAL);
                textoTraducido += tokenActual.GetValor();
                match(Token.Tipo.SIGNO_IGUAL);
            }
            else if (tokenActual.GetTipo() == Token.Tipo.SIGNO_MAYOR)
            {
                textoTraducido += tokenActual.GetValor();
                match(Token.Tipo.SIGNO_MAYOR);
                if (tokenActual.GetTipo() == Token.Tipo.SIGNO_IGUAL)
                {
                    textoTraducido += tokenActual.GetValor();
                    match(Token.Tipo.SIGNO_IGUAL);
                }
            }
            else if (tokenActual.GetTipo() == Token.Tipo.SIGNO_MENOR)
            {
                textoTraducido += tokenActual.GetValor();
                match(Token.Tipo.SIGNO_MENOR);
                if (tokenActual.GetTipo() == Token.Tipo.SIGNO_IGUAL)
                {
                    textoTraducido += tokenActual.GetValor();
                    match(Token.Tipo.SIGNO_IGUAL);
                }
            }
            else if (tokenActual.GetTipo() == Token.Tipo.SIGNO_EXC)
            {
                textoTraducido += tokenActual.GetValor();
                match(Token.Tipo.SIGNO_EXC);
                textoTraducido += tokenActual.GetValor();
                match(Token.Tipo.SIGNO_IGUAL);
            }
        }

        public void ELSES()
        {
            if (tokenActual.GetTipo()== Token.Tipo.RESERVADA_ELSE)
            {
                textoTraducido += espaciador()+tokenActual.GetValor() + ": \r\n";
                
                match(Token.Tipo.RESERVADA_ELSE);
                match(Token.Tipo.SIGNO_LLAVEABRE);
                contadorSentencias += 1;
                Console.WriteLine("Aumenta Contador");
                BLOQUE();
                contadorSentencias -= 1;
                Console.WriteLine("Disminuye Contador");
                match(Token.Tipo.SIGNO_LLAVECIERRE);
               
                BLOQUE();
            }
            else
            {
                //EPSILON
            }
        }

        public void SWITCHS()
        {
            if (tokenActual.GetTipo() == Token.Tipo.RESERVADA_SWITCH)
            {
                textoTraducido += espaciador() +"if " ;
                match(Token.Tipo.RESERVADA_SWITCH);
                match(Token.Tipo.SIGNO_PARABRE);
                constanteSwitch = tokenActual.GetValor();
                textoTraducido += constanteSwitch;
                match(Token.Tipo.VAR);
                match(Token.Tipo.SIGNO_PARCIERRE);
                match(Token.Tipo.SIGNO_LLAVEABRE);
                CASES();
                DEFAULT();
                match(Token.Tipo.SIGNO_LLAVECIERRE);
                BLOQUE();

            }
            else
            {
                //EPSILON
            }
        }

        int detSwitch = 0;
        public void CASES()
        {
            if (tokenActual.GetTipo() == Token.Tipo.RESERVADA_CASE)
            {
                if (detSwitch<1)
                {
                    textoTraducido += " == ";
                    detSwitch++;
                }
                else
                {
                    textoTraducido += espaciador() + "elif " + constanteSwitch + " == ";
                }
                match(Token.Tipo.RESERVADA_CASE);
                VALOR();
                textoTraducido += tokenActual.GetValor() + "\r\n";
                contadorSentencias++;
                match(Token.Tipo.SIGNO_DOSPUNTOS);
                BLOQUE();
                contadorSentencias--;
                match(Token.Tipo.RESERVADA_BREAK);
                match(Token.Tipo.SIGNO_PUNTOCOMA);
                COMENT();
                CASES();
            }
            else
            {
                //EPSILON
            }
        }

        public void DEFAULT()
        {
            if (tokenActual.GetTipo()==Token.Tipo.RESERVADA_DEFAULT)
            {
                textoTraducido += espaciador() + "else";
                match(Token.Tipo.RESERVADA_DEFAULT);
                VALOR();
                textoTraducido += tokenActual.GetValor() + "\r\n";
                contadorSentencias++;
                match(Token.Tipo.SIGNO_DOSPUNTOS);
                BLOQUE();
                contadorSentencias--;
                match(Token.Tipo.RESERVADA_BREAK);
                match(Token.Tipo.SIGNO_PUNTOCOMA);
                COMENT();

            }
            else
            {
                //EPSILON
            }
        }

        string constfor = "";

        public void FORS()
        {
            if (tokenActual.GetTipo() == Token.Tipo.RESERVADA_FOR)
            {
                match(Token.Tipo.RESERVADA_FOR);
                match(Token.Tipo.SIGNO_PARABRE);
                INSDEC();
                textoTraducido += espaciador() + "while ";
                CONDICION();
                textoTraducido += ":\r\n";
                match(Token.Tipo.SIGNO_PUNTOCOMA);
                DECINC();
                match(Token.Tipo.SIGNO_PARCIERRE);
                match(Token.Tipo.SIGNO_LLAVEABRE);
                contadorSentencias++;
                BLOQUE();
                textoTraducido += espaciador()+ constfor+"\r\n";
                constfor = "";
                contadorSentencias--;
                match(Token.Tipo.SIGNO_LLAVECIERRE);
                BLOQUE();
            }
            else
            {
                //EPSILON
            }
        }


        public void DECINC()
        {
            constfor += tokenActual.GetValor();
            match(Token.Tipo.VAR);
            if (tokenActual.GetTipo() == Token.Tipo.SIGNO_MAS)
            {
                constfor += " += 1";
                match(Token.Tipo.SIGNO_MAS);
                match(Token.Tipo.SIGNO_MAS);
            }
            else if (tokenActual.GetTipo() == Token.Tipo.SIGNO_MENOS)
            {
                constfor += " -= 1";
                match(Token.Tipo.SIGNO_MENOS);
                match(Token.Tipo.SIGNO_MENOS);
            }
        }

        public void WHILE()
        {
            if (tokenActual.GetTipo() == Token.Tipo.RESERVADA_WHILE)
            {
                textoTraducido += espaciador()+tokenActual.GetValor()+" ";
                match(Token.Tipo.RESERVADA_WHILE);
                match(Token.Tipo.SIGNO_PARABRE);
                CONDICION();
                textoTraducido += ": \r\n";
                
                match(Token.Tipo.SIGNO_PARCIERRE);
                match(Token.Tipo.SIGNO_LLAVEABRE);
                contadorSentencias++;
                BLOQUE();
                contadorSentencias--;
                match(Token.Tipo.SIGNO_LLAVECIERRE);
                BLOQUE();
            }
            else
            {
                //EPSILON
            }
        }

        public void COMENT()
        {
            if (tokenActual.GetTipo() == Token.Tipo.COMENTARIO_LINEA)
            {
                textoTraducido += "#" + tokenActual.GetValor()+"\r\n";
                match(Token.Tipo.COMENTARIO_LINEA);
                COMENT();
            }
            else if (tokenActual.GetTipo() == Token.Tipo.COMENTARIO_MULTI)
            {
                textoTraducido += "''' \r\b" + tokenActual.GetValor()+"\r\n ''' \r\n";
                match(Token.Tipo.COMENTARIO_MULTI);
                COMENT();
            }
            else
            {
                //EPSILON
            }
        }

        public string espaciador()
        {
            string a = "";
            for (int i = 0; i < contadorSentencias; i++)
            {
                a += "    ";
            }
            return a;
            
        }



    }
}
