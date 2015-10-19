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

namespace CSMaker
{
    public class Enemigo : AgenteReactivoSimple
    {
        public Enemigo(Microsoft.Xna.Framework.Game game, Vector2 tamano, Vector2 posicion, String nombreImagen)
            : base(game, tamano, posicion, nombreImagen)
        {
            profundidad = 1;
            NombreImagen = nombreImagen;
            ColorImagen = Color.White;
            LoadContent();
        }

        public override void Comportamiento(acciones a)
        {
            foreach (var accion in a.accion)
            {
                //Console.Out.WriteLine(accion);
                if (accion.Equals("cambiar_direccion_izq")) avanzarIzquierda();
                if (accion.Equals("cambiar_direccion_der")) avanzarDerecha();
                if (accion.Equals("avanzar")) avanzar();
                if (accion.Equals("saltarIzq")) saltar();
                if (accion.Equals("saltarDer")) saltar();
            }
        }
    }
}
