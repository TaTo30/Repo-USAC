using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1
{
    class Token
    {
        private String ID;
        private String Contenido;
        private Tipo TokenTipo;

        public Token(String ID, String Contenido, Tipo TokenTipo)
        {
            this.ID = ID;
            this.Contenido = Contenido;
            this.TokenTipo = TokenTipo;
        }

        public enum Tipo
        {
            COMENTARIO,
            COMENTARIOML,
            CONJUNTO,
            EXPRESION,
            LEXEMA,
            CONCATENACION,
            DISYUNCION,
            CERRADURAPOSITIVA,
            CERRADURAKLEENE,
            INTERROGACION,
            HOJA
        }


        public String getID()
        {
            return ID;
        }

        public String getContenido()
        {
            return Contenido;
        }

        public Tipo getTokenTipo()
        {
            return TokenTipo;
        }

    }
}
