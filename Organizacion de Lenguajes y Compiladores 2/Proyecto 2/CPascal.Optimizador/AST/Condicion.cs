using System;

namespace Optimizador
{
    public class Condicion
    {
        public enum Tipo
        {
            MAYOR,
            MENOR,
            MAYORQUE,
            MENORQUE,
            IGUAL,
            DIFERENTE
        }
        public Tipo TipoCondicion {get; set;}
        public Expresion OperadorIzq {get; set;}
        public Expresion OperadorDer{get; set;}
        public Condicion(Expresion izq, Expresion der, Tipo tipo){
            this.OperadorDer = der;
            this.OperadorIzq = izq;
            this.TipoCondicion = tipo;
        }
        public override string ToString()
        {
            switch (this.TipoCondicion)
            {
                case Tipo.MAYOR:
                    return $"{this.OperadorIzq.ToString()} > {this.OperadorDer.ToString()}";
                case Tipo.MENOR:
                    return $"{this.OperadorIzq.ToString()} < {this.OperadorDer.ToString()}";
                case Tipo.MAYORQUE:
                    return $"{this.OperadorIzq.ToString()} >= {this.OperadorDer.ToString()}";
                case Tipo.MENORQUE:
                    return $"{this.OperadorIzq.ToString()} <= {this.OperadorDer.ToString()}";
                case Tipo.IGUAL:
                    return $"{this.OperadorIzq.ToString()} == {this.OperadorDer.ToString()}";
                default:
                    return $"{this.OperadorIzq.ToString()} != {this.OperadorDer.ToString()}";
            }
        }
        public (bool isconstant, bool value) Evaluar(){
            if (this.OperadorDer.TipoValor == Expresion.Tipo.NUMERO && this.OperadorIzq.TipoValor == Expresion.Tipo.NUMERO)
            {
                //Valores son constantes
                bool valor;
                switch (this.TipoCondicion)
                {
                    case Tipo.MAYOR:
                        valor = Convert.ToDouble(this.OperadorIzq.Valor) > Convert.ToDouble(this.OperadorDer.Valor);
                        break;
                    case Tipo.MENOR:
                        valor = Convert.ToDouble(this.OperadorIzq.Valor) < Convert.ToDouble(this.OperadorDer.Valor);
                        break;
                    case Tipo.MAYORQUE:
                        valor = Convert.ToDouble(this.OperadorIzq.Valor) >= Convert.ToDouble(this.OperadorDer.Valor);
                        break;
                    case Tipo.MENORQUE:
                        valor = Convert.ToDouble(this.OperadorIzq.Valor) >= Convert.ToDouble(this.OperadorDer.Valor);
                        break;
                    case Tipo.IGUAL:
                        valor = Convert.ToDouble(this.OperadorIzq.Valor) == Convert.ToDouble(this.OperadorDer.Valor);
                        break;
                    default:
                        valor = Convert.ToDouble(this.OperadorIzq.Valor) != Convert.ToDouble(this.OperadorDer.Valor);
                        break;
                }
                return (true, valor);
            }
            return (false, false);
        }
        public void Negar(){
            switch (this.TipoCondicion)
            {
                case Tipo.MAYOR:
                    this.TipoCondicion = Tipo.MENORQUE;
                    break;
                case Tipo.MENOR:
                    this.TipoCondicion = Tipo.MAYORQUE;
                    break;
                case Tipo.MAYORQUE:
                    this.TipoCondicion = Tipo.MENOR;
                    break;
                case Tipo.MENORQUE:
                    this.TipoCondicion = Tipo.MAYOR;
                    break;
                case Tipo.IGUAL:
                    this.TipoCondicion = Tipo.DIFERENTE;
                    break;
                case Tipo.DIFERENTE:
                    this.TipoCondicion = Tipo.IGUAL;
                    break;
            }
        }
    }
}