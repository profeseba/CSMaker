using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace CSMaker
{
    public class Jugador : SpriteComponent
    {

        public Jugador(Microsoft.Xna.Framework.Game game, Vector2 tamano, Vector2 posicion, String nombreImagen)
            : base(game, tamano, posicion)
        {
            NombreImagen = nombreImagen;
            ColorImagen = Color.White;
            direccionColision = "nulo";
            life = 1;
            nombreSprite = "Player";
            LoadContent();
        }

        public override void Colision(SpriteComponent otro, Vector2 desplazamiento)
        {
            //if (life <= 0)
            //{
            //    died = true;
            //}
            //Debug.Print("jugador " + direccionColision);
            //if ((!direccionColision.Equals("abajo")) && (!direccionColision.Equals("nulo")))
            //{
            //    life--;
            //}
            if (otro is Muro)
            {
                Mover(desplazamiento);
                if (desplazamiento.Y != 0)
                {
                    velocidad.Y = 0;
                    isOnGround = true;
                }
            }
            if (otro is Objeto)
            {
                Mover(desplazamiento);
                if (desplazamiento.Y != 0)
                {
                    velocidad.Y = 0;
                    isOnGround = true;
                }
            }
            if (otro is Agent)
            {
                direccionColision = cls.colision(this.Bound, otro.Bound);
                //Debug.Print("jugador " + direccionColision + " vida:"+life);
                if (direccionColision.Equals("abajo"))
                {
                    otro.life--;
                }
                Mover(desplazamiento);
                if (desplazamiento.Y != 0)
                {
                    velocidad.Y = 0;
                }
            }
            
        }

        
    }
}
