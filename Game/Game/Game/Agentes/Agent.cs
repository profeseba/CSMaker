using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Agentes;
using Game.Juego;
using Microsoft.Xna.Framework;
using Game.Agentes.ext;

namespace Game
{
    public abstract class Agent : SpriteComponent
    {
        public Agent(Microsoft.Xna.Framework.Game game, Vector2 tamano, Vector2 posicion, String nombreImagen)
            : base(game, tamano, posicion)
        {
            NombreImagen = nombreImagen;
            ColorImagen = Color.White;
            LoadContent();
        }

        public override void Colision(SpriteComponent otro, Vector2 desplazamiento) { }

        public estados Percepciones(SpriteComponent sprite, SpriteComponent agente)
        {
            // lee las entradas (percepciones) y las compara para luego retornar un estado
            // 'jugador' son las percepciones del objetivo: Jugador.
            // 'agente' son las percepciones del objetivo: Agente.
            estados nuevoEstado = new estados(); // inicializa los estados.
            nuevoEstado.estado = new List<string>(); // inicializa el conjunto de estados.
            // verifica si es un jugador u otro objeto
            if (sprite is Jugador)
            {
                // calcula la distancia entre jugador y agente.
                Vector2 distancia = new Vector2(agente.Posicion.X - sprite.Posicion.X, agente.Posicion.Y - sprite.Posicion.Y);
                // verifica si el jugador esta cerca o no.
                if (distancia.X < 64) nuevoEstado.estado.Add("player_near");
                else nuevoEstado.estado.Add("player_no_near");
                // verifica si el jugador esta en el aire o no.
                if (sprite.isOnGround) nuevoEstado.estado.Add("player_no_onAir");
                else nuevoEstado.estado.Add("player_onAir");
            }
            if (sprite is Muro)
            {
                // calcula la distancia entre el muro y agente.
                // tamano del muro
                //Vector2 distancia = new Vector2(agente.Posicion.X - (sprite.Posicion.X + sprite.Tamano.X), agente.Posicion.Y - (sprite.Posicion.Y ));
                // calcular distancia del salto.
                float distanciaX = agente.Posicion.X - (sprite.Posicion.X + sprite.Tamano.X);
                // verifica si el jugador esta cerca o no.
                if ((sprite.Posicion.Y >= agente.Posicion.Y) && (sprite.Posicion.Y < (agente.Posicion.Y + agente.Tamano.Y)) && (distanciaX < 32) && (distanciaX > 0)) nuevoEstado.estado.Add("block_is_near");
                else nuevoEstado.estado.Add("block_no_near");
            }
            
            // retorna los estados agregados.
            return nuevoEstado;
        }

        public acciones Regla(estados e, reglas r)
        {
            // deberia leer las reglas desde un archivo
            acciones nuevasAcciones = new acciones();
            nuevasAcciones.accion = new List<string>();
            foreach (var state in e.estado)
            {
                foreach (var rule in r.regla)
                {
                    if (rule.condicion == state)
                    {
                        nuevasAcciones.accion.Add(rule.accion);
                    }  
                }
            }
            return nuevasAcciones;
        }

        public abstract void Sensor(SpriteComponent sprite);

        public abstract void Comportamiento(acciones a);

        public void perseguir()
        {
            // calcula el mejor camino.. aun por definir.
            if (isOnGround)
            {
                velocidad.Y = -Salto;
                isOnGround = false;
            } 
        }

        public void saltar() 
        {
            if (isOnGround)
            {
                velocidad.Y = -Salto;

                isOnGround = false;
            }    
        }
        
        public void avanzar(String where)
        {
            if (where.Equals(Direccion.LEFT))
            {
                velocidad.X = -100;
            }
            else if (where.Equals(Direccion.RIGHT))
            {
                velocidad.X = 100;
            }
            
        }



    }
}
