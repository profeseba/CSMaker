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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Enemigo2 : AgenteAprende
    {
        public Enemigo2(Microsoft.Xna.Framework.Game game, Vector2 tamano, Vector2 posicion, string nombreImagen)
            : base(game, tamano, posicion, nombreImagen)
        {
            profundidad = 6;
            NombreImagen = nombreImagen;
            ColorImagen = Color.White;
            LoadContent();
        }

    }
}
