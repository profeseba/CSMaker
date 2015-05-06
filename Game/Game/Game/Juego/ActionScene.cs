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


namespace Game
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    class ActionScene : GameScene
    {
        ContentManager Content;
        SpriteBatch spriteBatch;
        Mundo mundo;
        Jugador jugador1;
        AgenteReactivoSimple reactivoSimple;
        //KeyboardState oldState;
 
        public ActionScene(Microsoft.Xna.Framework.Game game, Texture2D background)
            : base(game)
        {
            Componentes.Add(new BackgroundComponent(game, background));
            Content = (ContentManager)Game.Services.GetService(typeof(ContentManager));
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            mundo = new Mundo();
            //Crear los muros
            mundo.AdicionarSprite(new Muro(game, new Vector2(game.Window.ClientBounds.Width, 16), new Vector2(0,0)));           
            mundo.AdicionarSprite(new Muro(game, new Vector2(16, game.Window.ClientBounds.Width), new Vector2(game.Window.ClientBounds.Width - 16, 0)));
            mundo.AdicionarSprite(new Muro(game, new Vector2(game.Window.ClientBounds.Width, 16), new Vector2(0, game.Window.ClientBounds.Height - 16)));
            mundo.AdicionarSprite(new Muro(game, new Vector2(16, game.Window.ClientBounds.Width), new Vector2(0, 0)));
 
            //crear algunas plataformas
            //mundo.AdicionarSprite(new Muro(game, new Vector2(game.Window.ClientBounds.Width / 2, 24), new Vector2(0, 96)));
            //mundo.AdicionarSprite(new Muro(game, new Vector2(game.Window.ClientBounds.Width / 2, 24), new Vector2(0, 500)));
            //mundo.AdicionarSprite(new Muro(game, new Vector2(game.Window.ClientBounds.Width / 2, 24), new Vector2(game.Window.ClientBounds.Width - (game.Window.ClientBounds.Width / 2), 96 + 96 + 28)));
            //mundo.AdicionarSprite(new Muro(game, new Vector2(256, 16), new Vector2(64, (int)(96 * 3.5f))));
            //mundo.AdicionarSprite(new Muro(game, new Vector2(256, 8), new Vector2(game.Window.ClientBounds.Width / 2, (int)(96 * 4f))));
 
            //crea al jugador
            jugador1 = new Jugador(game, new Vector2(32, 32), new Vector2( 0, game.Window.ClientBounds.Height - 32), "players/blue");
            mundo.AdicionarSprite(jugador1);

            //crea al agente
            reactivoSimple = new AgenteReactivoSimple(game, new Vector2(32, 32), new Vector2(game.Window.ClientBounds.Width - 32, game.Window.ClientBounds.Height - 32), "players/red");
            mundo.AdicionarSprite(reactivoSimple);
            mundo.AdicionarAgente(reactivoSimple);
        }
 
        protected override void LoadContent()
        {
            base.LoadContent();
        }
 
        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)((double)gameTime.ElapsedGameTime.Milliseconds / 1000);
            float totalTime = (float)((double)gameTime.TotalGameTime.TotalMilliseconds / 1000);
            KeyboardState newState = Keyboard.GetState();
            Vector2 velocidad = jugador1.Velocidad;
            bool isOnGround = jugador1.isOnGround;
 
            velocidad.X = 0;
            if (newState.IsKeyDown(Keys.Space))
            {
                if (isOnGround)
                {
                    velocidad.Y = -400;
                    isOnGround = false;
                }
            }
            if (newState.IsKeyDown(Keys.A))
            {
                velocidad.X = -200;
            }
            if (newState.IsKeyDown(Keys.D))
            {
                velocidad.X = 200;
            }
            jugador1.Velocidad = velocidad;
            jugador1.isOnGround = isOnGround;
            mundo.Update(deltaTime, totalTime);
            base.Update(gameTime);
        }
 
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            foreach (SpriteComponent sp in mundo.Sprites)
            {
                sp.Draw(gameTime);
            }
        }
 
        public override void Show()
        {
            base.Show();
            Enabled = true;
            Visible = true;
        }
 
        public override void Hide()
        {
            base.Hide();
            Enabled = false;
            Visible = false;
        }
    }
}
