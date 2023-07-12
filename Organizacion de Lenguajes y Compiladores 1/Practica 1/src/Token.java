package practica;


public class Token {
    
    private String ID;
    private String Contenido;
    private Tipo TokenTipo;

    public Token(String ID, String Contenido, Tipo TokenTipo) {
        this.ID = ID;
        this.Contenido = Contenido;
        this.TokenTipo = TokenTipo;
    }
    
    public enum Tipo{
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
   

    public String getID() {
        return ID;
    }

    public String getContenido() {
        return Contenido;
    }
    
    public Tipo getTokenTipo(){
        return TokenTipo;
    }

    public String getTokenTipoString() {
        switch(TokenTipo){
            case COMENTARIO:
                return "COMENTARIO";
            case COMENTARIOML:
                return "COMENTARIOML";
            case CONJUNTO:
                return "CONJUNTO";
            case EXPRESION:
                return "EXPRESION";
            case LEXEMA:
                return "LEXEMA";
            default:
                return "NOIDENTIFICADO";                
        }
    }    
}
