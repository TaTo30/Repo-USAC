public static class Saltos{
    static int correlativo;
    public static string Correlativo{
        get{return $"L{correlativo++}";}
    }
}