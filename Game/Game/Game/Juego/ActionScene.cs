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
        private int celda = 32;
        public int Celda { get { return celda; } }
        Vector2 size;
        Vector2 sizeWorld;
        //KeyboardState oldState;
 
        public ActionScene(Microsoft.Xna.Framework.Game game, Texture2D background, Vector2 sizeWorld)
            : base(game)
        {
            Componentes.Add(new BackgroundComponent(game, background));
            Content = (ContentManager)Game.Services.GetService(typeof(ContentManager));
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            this.size = new Vector2(game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
            this.sizeWorld = sizeWorld;
            mundo = new Mundo();
            //Crear los muros
            mundo.AdicionarSprite(new Muro(game, new Vector2(sizeWorld.X, Celda), new Vector2(0,0)));   // muro de arriba     
            mundo.AdicionarSprite(new Muro(game, new Vector2(Celda, sizeWorld.Y), new Vector2(sizeWorld.X, 0))); // muro de derecha
            mundo.AdicionarSprite(new Muro(game, new Vector2(sizeWorld.X, Celda), new Vector2(0, sizeWorld.Y))); // muro de abajo
            mundo.AdicionarSprite(new Muro(game, new Vector2(Celda, sizeWorld.Y), new Vector2(0, 0))); // muro de izquierda
 
            //crear algunas plataformas
            //mundo.AdicionarSprite(new Muro(game, new Vector2(game.Window.ClientBounds.Width / 2, 24), new Vector2(0, 96)));
            //mundo.AdicionarSprite(new Muro(game, new Vector2(game.Window.ClientBounds.Width / 2, 24), new Vector2(0, 500)));
            //mundo.AdicionarSprite(new Muro(game, new Vector2(game.Window.ClientBounds.Width / 2, 24), new Vector2(game.Window.ClientBounds.Width - (game.Window.ClientBounds.Width / 2), 96 + 96 + 28)));
            //mundo.AdicionarSprite(new Muro(game, new Vector2(256, 20), new Vector2(120, (int)(96 * 4.5f))));
            mundo.AdicionarSprite(new Muro(game, new Vector2(Celda, Celda), new Vector2(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height - Celda)));
            mundo.AdicionarSprite(new Muro(game, new Vector2(2 * Celda, Celda), new Vector2(game.Window.ClientBounds.Width, game.Window.ClientBounds.Height - Celda)));
 
            //crea al jugador
            jugador1 = new Jugador(game, new Vector2(Celda, Celda), new Vector2(Celda, size.Y - Celda - 1), "players/blue");
            mundo.AdicionarSprite(jugador1);

            //crea al agente
            reactivoSimple = new AgenteReactivoSimple(game, new Vector2(Celda, Celda), new Vector2(20*Celda, game.Window.ClientBounds.Height - Celda - Celda), "players/red");
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
            Vector2 desplazamiento = Vector2.Zero;
            bool isOnGround = jugador1.isOnGround;
 
            velocidad.X = 0;
            desplazamiento.X = 0;

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
                if ((jugador1.Posicion.X / 32) > (size.X / 64))
                {
                    desplazamiento.X = -200;
                }
                else velocidad.X = 200;
            }

            mundo.Desplazamiento = desplazamiento;
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
