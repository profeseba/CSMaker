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

        public Bloque Percepciones(SpriteComponent agente, SpriteComponent sprite)
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
            nuevoBloque.sector = ProfundidadNivel1(nPA, nPS, tPS);

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
            //if (sprite is Jugador)
            //{


            //    //Vector2 distancia = new Vector2(agente.Posicion.X - sprite.Posicion.X, agente.Posicion.Y - sprite.Posicion.Y);
            //    //// verifica si el jugador esta cerca o no.
            //    //if ((distancia.X > -64) && (distancia.X < 64)) nuevoEstado.estado.Add("player_is_near");
            //    //else nuevoEstado.estado.Add("player_no_near");
            //    //// verifica si el jugador esta en el aire o no.
            //    //if (sprite.isOnGround) nuevoEstado.estado.Add("player_no_onAir");
            //    //else nuevoEstado.estado.Add("player_onAir");
            //}
            //if (sprite is Muro)
            //{
            //    //// calcula la distancia entre el muro y agente.
            //    //// tamano del muro
            //    ////Vector2 distancia = new Vector2(agente.Posicion.X - (sprite.Posicion.X + sprite.Tamano.X), agente.Posicion.Y - (sprite.Posicion.Y ));
            //    //// calcular distancia entre objetos
            //    //float distanciaX;
            //    //if (Direccion.Equals("left"))
            //    //{
            //    //    distanciaX = agente.Posicion.X - (sprite.Posicion.X + sprite.Tamano.X);
            //    //} else distanciaX = agente.Posicion.X - sprite.Posicion.X;

            //    //// verifica si el jugador esta cerca o no.
            //    //if ((sprite.Posicion.Y >= agente.Posicion.Y) && (sprite.Posicion.Y < (agente.Posicion.Y + agente.Tamano.Y)))
            //    //{
            //    //    if ((distanciaX < agente.Tamano.X) && (distanciaX > -(agente.Tamano.X*2))) nuevoEstado.estado.Add("block_is_near");
            //    //    else nuevoEstado.estado.Add("block_no_near");
            //    //}
            //    ////if (((sprite.Posicion.Y + sprite.Tamano.Y ) > agente.Posicion.Y) && (sprite.Posicion.Y < (agente.Posicion.Y - agente.Tamano.Y)))
            //    ////{
            //    ////    if ((distanciaX < 32) && (distanciaX > 0)) nuevoEstado.estado.Add("muro_is_near");
            //    ////    else nuevoEstado.estado.Add("muro_no_near");
            //    ////}    
            //    //if ( ( sprite.Posicion.Y  < ( agente.Posicion.Y - agente.Tamano.Y ) ) && ( ( sprite.Posicion.Y + sprite.Tamano.Y ) > agente.Posicion.Y))
            //    //{
            //    //    if ((distanciaX < (agente.Tamano.X) + 1) && (distanciaX > -(agente.Tamano.X) - 1)) nuevoEstado.estado.Add("muro_is_near");
            //    //    else nuevoEstado.estado.Add("muro_no_near");
            //    //}
            //}

            // retorna los estados agregados.
            //
            //for (int i = 0; i < nuevoEstado.sector.Count; i++)  Console.Out.WriteLine(nuevoEstado.sector[i].name+" "+nuevoEstado.sector[i].value);   

            // filtra el bloque agregando los sectores que faltan como falsos.
            nuevoBloque = filtroNivel1(nuevoBloque);

            return nuevoBloque;
        }

        private List<Sector> ProfundidadNivel1(Vector2 nPA, Vector2 nPS, Vector2 tPS)
        {
            List<Sector> retorno = new List<Sector>();
            for (int j = 0; j < tPS.Y; j++)
            {
                for (int i = 0; i < tPS.X; i++)
                {
                    if ((nPA.Y - 1) == nPS.Y + j)
                    {
                        if ((nPA.X - 1) == nPS.X + i) { Sector aux = new Sector(); aux.name = "A"; aux.value = true; retorno.Add(aux); }
                        if ((nPA.X) == nPS.X + i) { Sector aux = new Sector(); aux.name = "B"; aux.value = true; retorno.Add(aux); }
                        if ((nPA.X + 1) == nPS.X + i) { Sector aux = new Sector(); aux.name = "C"; aux.value = true; retorno.Add(aux); }
                    }
                    if ((nPA.Y) == nPS.Y + j)
                    {
                        if ((nPA.X - 1) == nPS.X + i) { Sector aux = new Sector(); aux.name = "D"; aux.value = true; retorno.Add(aux); }
                        if ((nPA.X + 1) == nPS.X + i) { Sector aux = new Sector(); aux.name = "E"; aux.value = true; retorno.Add(aux); }
                    }
                    if ((nPA.Y + 1) == nPS.Y + j)
                    {
                        if ((nPA.X - 1) == nPS.X + i) { Sector aux = new Sector(); aux.name = "F"; aux.value = true; retorno.Add(aux); }
                        if ((nPA.X) == nPS.X + i) { Sector aux = new Sector(); aux.name = "G"; aux.value = true; retorno.Add(aux); }
                        if ((nPA.X + 1) == nPS.X + i) { Sector aux = new Sector(); aux.name = "H"; aux.value = true; retorno.Add(aux); }
                    }
                }
            }

            return retorno;
        }

        private Bloque filtroNivel1(Bloque input) 
        {
            Bloque output = input;
            //Bloque swap = new Bloque();
            List<Sector> swap = new List<Sector>();
            Sector sA;
            sA = new Sector();
            sA.name = "A";
            sA.value = false;
            swap.Add(sA);
            sA = new Sector();
            sA.name = "B";
            sA.value = false;
            swap.Add(sA);
            sA = new Sector();
            sA.name = "C";
            sA.value = false;
            swap.Add(sA);
            sA = new Sector();
            sA.name = "D";
            sA.value = false;
            swap.Add(sA);
            sA = new Sector();
            sA.name = "E";
            sA.value = false;
            swap.Add(sA);
            sA = new Sector();
            sA.name = "F";
            sA.value = false;
            swap.Add(sA);
            sA = new Sector();
            sA.name = "G";
            sA.value = false;
            swap.Add(sA);
            sA = new Sector();
            sA.name = "H";
            sA.value = false;
            swap.Add(sA);
            // comparacion
            foreach (var s in swap)
            {
                foreach (var o in output.sector)
                {
                    if (s.name == o.name)  s.value = true;
                }
            }

            output.sector = swap;

            return output;
        }
    }
}
