using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Game
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
                    Debug.WriteLine("fila encontrada --------------------------------");
                    return fila;
                }
            }
            Debug.WriteLine("fila no encontrada");
            return null;
        }
        // retorna la tupla con mayor valor
        public ColumnasAER getActionMaxQ(String accion,Bloque estado, float recompensa)
        {
            ColumnasAER tupla = new ColumnasAER(accion,estado, 0, recompensa);
            foreach (var fila in filas)
            {
                if (fila.estado.sector == estado.sector)
                {
                    if (fila.valor > tupla.valor)
                    {
                        tupla = fila;
                    }
                    else if (fila.valor == tupla.valor)
                    {
                        if (fila.frecuencia > tupla.frecuencia)
                        {
                            tupla = fila;
                        }
                    } 
                }
            }
            // ver si es bueno o malo
            if (tupla.valor == -1)
            {
                // malo
                foreach (var item in Movimientos)
                {
                    if (item!=tupla.accion)
                    {
                        tupla.accion = item;
                        break;
                    }
                }
            }

            return tupla;
        }
        // verifica que los elementos no esten en la tabla
        public void addTupla(ColumnasAER Input)
        {
            bool flag = false;
            foreach (var item in FilasAER)
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
