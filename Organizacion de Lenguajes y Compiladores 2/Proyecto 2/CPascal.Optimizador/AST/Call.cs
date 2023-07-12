namespace Optimizador
{
    public class Call : IC3D
    {
        public string Id {get; set;}
        public int Linea {get; set;}
        public Call(string id, int Linea){
            this.Id = id;
            this.Linea = Linea;
        }
        public string ParseString(){
            return $"{this.Id}();";
        }
    }
}