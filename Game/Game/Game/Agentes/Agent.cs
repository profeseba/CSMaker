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

        public estados Estado(percepciones jugador, percepciones agente)
        {
            // lee las entradas (percepciones) y las compara para luego retornar un estado
            // 'jugador' son las percepciones del objetivo: Jugador.
            // 'agente' son las percepciones del objetivo: Agente.
            estados nuevoEstado = new estados(); // inicializa los estados.
            nuevoEstado.estado = new List<string>(); // inicializa el conjunto de estados.
            // calcula la distancia entre jugador y agente.
            Vector2 distancia = new Vector2(agente.Position.X - jugador.Position.X, agente.Position.Y - jugador.Position.Y);
            // posibles estados
            // verifica si el jugador esta cerca o no.
            if (distancia.X < 64) nuevoEstado.estado.Add("near");
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

        public void perseguir()
        {
            // calcula el mejor camino.. aun por definir.
            if (isOnGround)
            {
                velocidad.Y = -400;
                isOnGround = false;
            } 
        }

        public void saltar() 
        {
            if (isOnGround)
            {
                velocidad.Y = -400;
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
