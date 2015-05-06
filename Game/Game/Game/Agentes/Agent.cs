using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Agentes;
using Game.Juego;
using Microsoft.Xna.Framework;

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

        public estados Estado(percepciones jugador, percepciones agente)
        {
            // lee las entradas (percepciones) y las compara para luego retornar un estado
            // 'jugador' son las percepciones del objetivo: Jugador.
            // 'agente' son las percepciones del objetivo: Agente.
            estados nuevoEstado = new estados(); // inicializa los estados.
            nuevoEstado.estado = new List<string>(); // inicializa el conjunto de estados.
            // calcula la distancia entre jugador y agente.
            Vector2 distancia = new Vector2(jugador.Position.X - agente.Position.X, jugador.Position.Y - agente.Position.Y);
            // posibles estados
            // verifica si el jugador esta cerca o no.
            if (distancia.X < 2) nuevoEstado.estado.Add("near");
            else nuevoEstado.estado.Add("not_near");
            // verifica si el jugador esta en el aire o no.
            if (jugador.isOnGround)  nuevoEstado.estado.Add("not_onAir");
            else nuevoEstado.estado.Add("onAir");
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

    }
}
