using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Game.Agentes;
using Game.Juego;


namespace Game
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class AgenteReactivoSimple : Agent
    {
        public AgenteReactivoSimple(Microsoft.Xna.Framework.Game game, Vector2 tamano, Vector2 posicion, String nombreImagen)
            : base(game, tamano, posicion, nombreImagen)
        {
            NombreImagen = nombreImagen;
            ColorImagen = Color.White;
            LoadContent();
        }

        public override void Colision(SpriteComponent otro, Vector2 desplazamiento)
        {
            if (otro is Muro)
            {
                Mover(desplazamiento);
                if (desplazamiento.Y != 0)
                {
                    velocidad.Y = 0;
                    isOnGround = true;
                }
            }
            if (otro is Jugador) 
            {
                Mover(desplazamiento);
                if (desplazamiento.Y != 0)
                {
                    velocidad.Y = 0;
                }
            }

        }

        public override void Sensor(SpriteComponent sprite) 
        {
            if (sprite is Jugador)
            {
                percepciones jugador = new percepciones();
                percepciones agente = new percepciones();
                //Jugador
                jugador.Position = sprite.Posicion;
                jugador.isOnGround = sprite.isOnGround;
                jugador.isFire = false;
                //Agente
                agente.Position = Posicion;
                agente.isOnGround = isOnGround;
                agente.isFire = false;
                //inicia el comportamient
                //asigna los estados
                estados e = new estados();
                e = Estado(jugador, agente);
                // acciones necesitan reglas
                reglas r = new reglas();
                r.regla = new List<condiciones>();
                // condiciones
                condiciones cond1 = new condiciones();
                cond1.accion = "perseguir";
                cond1.condicion = "near";
                condiciones cond2 = new condiciones();
                cond2.accion = "saltar";
                cond2.condicion = "onAir";
                condiciones cond3 = new condiciones();
                cond3.accion = "perseguir";
                cond3.condicion = "near";
                //
                r.regla.Add(cond1);
                r.regla.Add(cond2);
                //r.regla.Add(cond3);
                //
                acciones action = new acciones();
                action = Regla(e, r);
                //
                Comportamiento(action);
            }
        }

        public override void Comportamiento(acciones a)
        {
            foreach (var accion in a.accion)
            {
                if (accion.Equals("perseguir"))
                {
                    // calcula el mejor camino.. aun por definir.

                }

                if (accion.Equals("saltar"))
                {
                    if (isOnGround)
                    {
                        velocidad.Y = -400;
                        isOnGround = false;
                    }                    
                }
            }
        }



    }
}
