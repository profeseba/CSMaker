using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSMaker
{
    public class ColumnasAER
    {
        public String accion { get; set; }
        public float valor { get; set; }
        public int frecuencia { get; set; }
        public Bloque estado { get; set; }
        
        

        public ColumnasAER(String accion, Bloque estado, int frecuencia, float valor) 
        {
            this.accion = accion;
            this.estado = estado;
            this.frecuencia = frecuencia;
            this.valor = valor;
        }
        public ColumnasAER() { }
    }
}
