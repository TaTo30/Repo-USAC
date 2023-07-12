using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _LFP_Proyecto
{
    class Token
    {

        //TODOS LOS TIPO DE TOKENS A RECONOCER
        public enum Tipo
        {
            RESERVADA_CLASS,
            RESERVADA_STATIC,
            RESERVADA_VOID,
            RESERVADA_STRING,
            RESERVADA_INT,
            RESERVADA_FLOAT,
            RESERVADA_CHAR,
            RESERVADA_BOOL,
            RESERVADA_IF,
            RESERVADA_ELSE,
            RESERVADA_SWITCH,
            RESERVADA_CASE,
            RESERVADA_BREAK,
            RESERVADA_DEFAULT,
            RESERVADA_FOR,
            RESERVADA_CONSOLE,
            RESERVADA_WRITELINE,
            RESERVADA_WHILE,
            RESERVADA_MAIN,
            RESERVADA_ARGS,
            RESERVADA_NEW,
            SIGNO_LLAVEABRE,
            SIGNO_LLAVECIERRE,
            SIGNO_PARABRE,
            SIGNO_PARCIERRE,
            SIGNO_CORABRE,
            SIGNO_CORCIERRE,
            SIGNO_MENOR,
            SIGNO_MAYOR,
            SIGNO_IGUAL,
            SIGNO_MAS,
            SIGNO_MENOS,
            SIGNO_POR,
            SIGNO_EXC,
            SIGNO_DIV,
            SIGNO_PUNTOCOMA,
            SIGNO_COMA,
            SIGNO_PUNTO,
            SIGNO_DOSPUNTOS,
            COMENTARIO_LINEA,
            COMENTARIO_MULTI,
            CADENA,
            CARACTER,
            DIGITO_ENTERO,
            DIGITO_REAL,
            EXPRESION,
            VAR,
            BOOL_TRUE,
            BOOL_FALSE,
            ULTIMO,
            DESCONOCIDO

        }

        private Tipo tipoToken;
        private string valorToken;

        public Token(Tipo tipoDelToken, string val)
        {
            this.tipoToken = tipoDelToken;
            this.valorToken = val;
        }

        public string GetValor()
        {
            return valorToken;
        }

        public Tipo GetTipo()
        {
            return tipoToken;
        }

        public string GetTipoString()
        {
            switch (tipoToken)
            {
                case Tipo.CADENA:
                    return "CADENA";
                case Tipo.RESERVADA_NEW:
                    return "RESERVADA_NEW";
                case Tipo.RESERVADA_CLASS:
                    return "RESERVADA_CLASS";
                case Tipo.RESERVADA_STATIC:
                    return "RESERVADA_STATIC";
                case Tipo.RESERVADA_VOID:
                    return "RESERVADA_VOID";
                case Tipo.RESERVADA_STRING:
                    return "RESERVADA_STRING";
                case Tipo.RESERVADA_INT:
                    return "RESERVADA_INT";
                case Tipo.RESERVADA_FLOAT:
                    return "RESERVADA_FLOAT";
                case Tipo.RESERVADA_CHAR:
                    return "RESERVADA_CHAR";
                case Tipo.RESERVADA_BOOL:
                    return "RESERVADA_BOOL";
                case Tipo.RESERVADA_IF:
                    return "RESERVADA_IF";
                case Tipo.RESERVADA_ELSE:
                    return "RESERVADA_ELSE";
                case Tipo.RESERVADA_SWITCH:
                    return "RESERVADA_SWITCH";
                case Tipo.RESERVADA_CASE:
                    return "RESERVADA_CASE";
                case Tipo.RESERVADA_ARGS:
                    return "RESERVADA_ARGS";
                case Tipo.RESERVADA_BREAK:
                    return "RESERVADA_BREAK";
                case Tipo.RESERVADA_DEFAULT:
                    return "RESERVADA_DEFAULT";
                case Tipo.RESERVADA_FOR:
                    return "RESERVADA_FOR";
                case Tipo.RESERVADA_CONSOLE:
                    return "RESERVADA_CONSOLE";
                case Tipo.RESERVADA_WRITELINE:
                    return "RESERVADA_WRITELINE";
                case Tipo.RESERVADA_WHILE:
                    return "RESERVADA_WHILE";
                case Tipo.RESERVADA_MAIN:
                    return "RESERVADA_MAIN";
                case Tipo.SIGNO_CORABRE:
                    return "SIGNO_CORABRE";
                case Tipo.SIGNO_CORCIERRE:
                    return "SIGNO_CORCIERRE";
                case Tipo.SIGNO_PARABRE:
                    return "SIGNO_PARABRE";
                case Tipo.SIGNO_PARCIERRE:
                    return "SIGNO_PARCIERRE";
                case Tipo.SIGNO_LLAVEABRE:
                    return "SIGNO_LLAVEABRE";
                case Tipo.SIGNO_LLAVECIERRE:
                    return "SIGNO_LLAVECIERRE";
                case Tipo.SIGNO_MAYOR:
                    return "SIGNO_MAYOR";
                case Tipo.SIGNO_DOSPUNTOS:
                    return "SIGNO_DOSPUNTOS";
                case Tipo.SIGNO_MENOR:
                    return "SIGNO_MENOR";
                case Tipo.SIGNO_IGUAL:
                    return "SIGNO_IGUAL";
                case Tipo.SIGNO_MAS:
                    return "SIGNO_MAS";
                case Tipo.SIGNO_MENOS:
                    return "SIGNO_MENOS";
                case Tipo.SIGNO_EXC:
                    return "SIGNO_EXC";
                case Tipo.SIGNO_POR:
                    return "SIGNO_POR";
                case Tipo.SIGNO_DIV:
                    return "SIGNO_DIV";
                case Tipo.SIGNO_PUNTOCOMA:
                    return "SIGNO_PUNTOCOMA";
                case Tipo.SIGNO_COMA:
                    return "SIGNO_COMA";
                case Tipo.SIGNO_PUNTO:
                    return "SIGNO_PUNTO";
                case Tipo.COMENTARIO_LINEA:
                    return "COMENTARIO_LINEA";
                case Tipo.COMENTARIO_MULTI:
                    return "COMENTARIO_MULTI";
                case Tipo.DIGITO_ENTERO:
                    return "DIGITO_ENTERO";
                case Tipo.DIGITO_REAL:
                    return "DIGITO_REAL";
                case Tipo.EXPRESION:
                    return "EXPRESION";
                case Tipo.CARACTER:
                    return "CARACTER";
                case Tipo.VAR:
                    return "VAR";
                case Tipo.BOOL_FALSE:
                    return "BOOL_FALSE";
                case Tipo.BOOL_TRUE:
                    return "BOOL_TRUE";
                case Tipo.ULTIMO:
                    return "ULTIMO";
                default:
                    return "Desconocido";
            }
        }


    }
}
