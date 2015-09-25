using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Agentes;
using Game.Juego;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Game
{
    public abstract class Agent : SpriteComponent
    {
        public int profundidad { get; set; } //profundidad del sensor
        protected bool contacto { get; set; }
        private int aceleracion;
        
        public Agent(Microsoft.Xna.Framework.Game game, Vector2 tamano, Vector2 posicion, string nombreImagen)
            : base(game, tamano, posicion)
        {
            NombreImagen = nombreImagen;
            ColorImagen = Color.White;
            Direccion = "left";
            aceleracion = 90;
            nombreSprite = "Agente";
            LoadContent();
        }
        
        public abstract void Sensor(Bloque area);

        public abstract void Comportamiento(acciones a);

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
                // causa daño
                //life--;
                //Debug.Print(direccionColision);
                if (desplazamiento.Y != 0)
                {
                    velocidad.Y = 0;
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
            //Debug.Print("Agente "+direccionColision);
            if (direccionColision.Equals("arriba"))
            {
                life--;
            }
        }
        
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
        
        public void avanzar()
        {
            if (Direccion.Equals("left"))
            {
                velocidad.X = -aceleracion;
            }
            else if (Direccion.Equals("right"))
            {
                velocidad.X = aceleracion;
            }
            
        }

        public void avanzarDireccion(String Direccion)
        {
            if (Direccion.Equals("left"))
            {
                velocidad.X = -aceleracion;
            }
            else if (Direccion.Equals("right"))
            {
                velocidad.X = aceleracion;
            }

        }

        public void cambiarDireccion() 
        {
            if (Direccion.Equals("left")) Direccion = "right";
            else Direccion = "left";
             // hace una llamada a la funcion avanzar para no entrar en un loop.
            avanzar();
        }

        public void avanzarIzquierda()
        {
            Direccion = "left";
            // hace una llamada a la funcion avanzar para no entrar en un loop.
            avanzar();
        }

        public void avanzarDerecha()
        {
            Direccion = "right";
            // hace una llamada a la funcion avanzar para no entrar en un loop.
            avanzar();
        }

       


    }
}
