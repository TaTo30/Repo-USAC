namespace Optimizador
{
    public class Return : IC3D
    {
        public int Linea {get; set;}
        public Return(int Linea){
            this.Linea = Linea;
        }

        public string ParseString(){
            return $"return;";
        }
    }
}