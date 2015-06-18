using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class Bloque
    {
        private List<Sector> eval { get { return sector; } }
        public List<Sector> sector { get; set; }

        public Sector obtenerSector(Vector2 posicion) 
        {
            foreach (var item in sector)
            {
                if (item.posicion == posicion)
                {
                    return item;
                }
            }
            return null;
        }

        public bool IsEquals(Bloque Input) 
        {
            int cont = 0;
            for (int i = 0; i < Input.sector.Count; i++)
            {
                if (Input.sector[i].Equals(eval[i]))
                {
                    cont++;
                }
                if (i == Input.sector.Count - 1)
                {
                    return true;
                }
            }
            return false;
        }

        public Bloque() { }

        public Bloque(int h)
        {
            for (int i = 0; i < ((h*2)+1); i++)
            {
                for (int j = 0; j < ((h*2)+1); j++)
                {
                    Sector aux = new Sector();
                    aux.name = "empty";
                    aux.posicion = new Vector2(j,i);
                    aux.value = false;
                    sector.Add(aux);
                }
            }
        }
    }
}
