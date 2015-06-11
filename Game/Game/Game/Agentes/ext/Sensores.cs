using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    class Sensores
    {
        //private List<estados> retorno = new List<estados>();

        public Bloque Percepciones(SpriteComponent agente, SpriteComponent sprite, int profundidad)
        {
            // lee las entradas (percepciones) y las compara para luego retornar un estado
            // 'jugador' son las percepciones del objetivo: Jugador.
            // 'agente' son las percepciones del objetivo: Agente.
            //estados nuevoEstado = new estados(); // inicializa los estados.
            //nuevoEstado.sector = new List<Sector>();
            Bloque nuevoBloque = new Bloque();
            //nuevoEstado.sector = new List<Sector>(); // inicializa el conjunto de estados.
            // verifica si es un jugador u otro objeto
            // calcula la distancia entre jugador y agente.
            /* nueva funcion de calculo
             * se crea una nueva posicion para el elemento verificando su posicion en celdas de 32*32px
             * Vector2 nPA = nueva Posicion Agente
             * Vector2 nPS = nueva Posicion Sprite
             * nivel de profundidad 1.
             * A, B, C, D, E, F, G y H son las posibles posiciones en las que se puede encontrar un elemento cercano al agente.
             * IF condiciones en que evalua si el sprite se encuentra en la matriz adyacente del agente.
             *      |A|B|C|
             *      |D|*|E|
             *      |F|G|H|
             * el agente "*" se encuentra en el centro.
             * A: X-1, Y-1     E: X+1, Y+0
             * B: X+0, Y-1     F: X-1, Y+1
             * C: X+1, Y-1     G: X+0, Y+1
             * D: X-1, Y+0     H: X+1, Y+1
             * nivel de profundidad 2.
             *      |A|B|C|D|E|
             *      |F|G|H|I|J|
             *      |K|L|*|M|N|
             *      |O|P|Q|R|S|
             *      |T|U|V|W|X|
             * nivel de profundidad 3...
             */
            Vector2 nPA = new Vector2(agente.Posicion.X / 32, agente.Posicion.Y / 32);
            Vector2 nPS = new Vector2(sprite.Posicion.X / 32, sprite.Posicion.Y / 32);
            Vector2 tPS = new Vector2(sprite.Tamano.X / 32, sprite.Tamano.Y / 32);
            // comprueba la posicion del sprite con profundidadnivel1
            //nuevoBloque.sector = ProfundidadNivel1(nPA, nPS, tPS);
            nuevoBloque.sector = Profundidad(nPA, nPS, tPS, profundidad);

            // asignamos el nombre del sprite
            if (sprite is Jugador)
            {
                nuevoBloque.element = "player";
            }
            else if (sprite is Muro)
            {
                nuevoBloque.element = "block";
            }
            else if (sprite is Agent)
            {
                nuevoBloque.element = "agent";
            }
            else if (sprite is Objeto)
            {
                nuevoBloque.element = "Objeto";
            }

            return nuevoBloque;
        }

        private List<Sector> Profundidad(Vector2 nPA, Vector2 nPS, Vector2 tPS, int profundidad)
        {
            int k = profundidad;
            int value = 0;
            List<Sector> retorno = new List<Sector>();
            for (int y = -k; y < k +1; y++)
            {
                for (int x = -k; x < k + 1; x++)
                {
                    value++;
                    for (int h = 0; h < tPS.Y; h++)
                    {
                        for (int w = 0; w < tPS.X; w++)
                        {
                            if ((nPA.X + x == nPS.X + w) && (nPA.Y + y == nPS.Y + h))
                            {
                                Sector aux = new Sector(); 
                                aux.name = ""+value; 
                                aux.value = true; 
                                retorno.Add(aux);
                            }
                        }
                    }
                }
            }

            return retorno;
        }

        public Bloque Suma(List<Bloque> input, int profundidad)
        {
            Bloque output = new Bloque();
            //Bloque swap = new Bloque();
            List<Sector> swap = new List<Sector>();
            int k;
            k = ( ((profundidad * 2) + 1) * ((profundidad * 2) + 1) );
            for (int i = 1; i < k+1; i++)
            {
                Sector aux = new Sector(); 
                aux.name = ""+i; 
                aux.value = false; 
                swap.Add(aux);
            }
            // comparacion
            foreach (var bloque in input)
            {
                foreach (var s in swap)
                {
                    foreach (var o in bloque.sector)
                    {
                        if ((s.name == o.name) && (o.value == true)) s.value = true;
                    }
                }
            }

            output.sector = swap;

            return output;
        }
    }
}
