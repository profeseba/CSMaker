using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CSMaker
{
    public class TablaAER
    {
        private List<ColumnasAER> filas { get { return FilasAER; } }
        public List<ColumnasAER> FilasAER { get; set; }
        public List<String> Movimientos { get; set; }
        // retorna la tupla
        public ColumnasAER getTupla(String accion, Bloque estado)
        {
            if (filas == null)
            {
                return null; 
            }
            foreach (var fila in filas)
            {
                if ((accion.Equals(fila.accion)) && (estado.IsEquals(fila.estado)))
                {
                    // - Debug.WriteLine("fila encontrada --------------------------------");
                    return fila;
                }
            }
            // - Debug.WriteLine("fila no encontrada");
            return null;
        }
        // retorna la tupla con mayor valor
        public ColumnasAER getActionMaxQ(String accion,Bloque estado, float recompensa)
        {
            ColumnasAER tupla = new ColumnasAER(accion,estado, 0, recompensa);
            foreach (var fila in filas)
            {
                // verifica estados iguales
                if (fila.estado.IsEquals(tupla.estado))
                {
                    // verifica accion igual
                    if (fila.accion == tupla.accion)
                    {
                        if (tupla.valor <= -0.5)
                        {
                            foreach (var item in Movimientos)
                            {
                                if (item != tupla.accion)
                                {
                                    tupla.accion = item;
                                    return tupla;
                                }
                            }
                        }                    
                    }
                }
            }
            return tupla;
        }
        // verifica que los elementos no esten en la tabla
        public void addTupla(ColumnasAER Input)
        {
            bool flag = false;
            foreach (var item in filas)
            {
                if ((item.accion.Equals(Input.accion)) && (item.estado.IsEquals(Input.estado)))
                {
                    flag = true;
                    item.frecuencia++;
                    item.valor = Input.valor;
                }
            }
            if (flag == false)
            {
                FilasAER.Add(Input);
                Movimientos.Add(Input.accion);
            }
        }
    }
}
