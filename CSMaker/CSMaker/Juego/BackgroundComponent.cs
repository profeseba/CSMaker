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
    public class BackgroundComponent : DrawableGameComponent
    {
        Texture2D fondo;
        SpriteBatch spriteBatch = null;
        Rectangle bgRect;
        public BackgroundComponent(Microsoft.Xna.Framework.Game game, Texture2D textura)
            : base(game)
        {
            this.fondo = textura;
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            bgRect = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(fondo, bgRect, Color.White);
            base.Draw(gameTime);
        }
    }
}
