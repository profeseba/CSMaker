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
using Game.Agentes.ext;


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
                //asigna los estados
                estados e = new estados();
                e = Percepciones(sprite, this);
                // acciones necesitan reglas
                reglas r = new reglas();
                r.regla = new List<condiciones>();
                // condiciones
                condiciones cond1 = new condiciones();
                cond1.accion = "perseguir";
                cond1.condicion = "player_near";
                condiciones cond2 = new condiciones();
                cond2.accion = "saltar";
                cond2.condicion = "block_is_near";
                condiciones cond3 = new condiciones();
                cond3.accion = "avanzar";
                cond3.condicion = "player_no_near";
                //condiciones cond4 = new condiciones();
                //cond4.accion = "avanzar";
                //cond4.condicion = "block_no_near";
                //
                r.regla.Add(cond1);
                r.regla.Add(cond2);
                r.regla.Add(cond3);
                //
                acciones action = new acciones();
                action = Regla(e, r);
                //
                Comportamiento(action);
        }

        public override void Comportamiento(acciones a)
        {
            foreach (var accion in a.accion)
            {
                if (accion.Equals("avanzar")) avanzar(Direccion.LEFT); 
                if (accion.Equals("saltar")) saltar();     
            }
        }

        



    }
}
