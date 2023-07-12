public static class Temporales{
    static int correlativo;
    public static string Correlativo{
        get{return $"T{correlativo++}";}
    }
    public static int Count {
        get{return correlativo;}
    }
}