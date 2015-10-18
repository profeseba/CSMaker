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
using System.Diagnostics;


namespace CSMaker
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ActionScene : GameScene
    {
        ContentManager Content;
        SpriteBatch spriteBatch;
        Mundo mundo;
        Jugador jugador;
        private int celda = 32;
        public int Celda { get { return celda; } }
        int Piso { get { return celda * 2; } }
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
            mundo.AdicionarSprite(new Muro(game, new Vector2(sizeWorld.X, 1), posicion(0,0)));   // muro de arriba     
            mundo.AdicionarSprite(new Muro(game, new Vector2(Celda, sizeWorld.Y), posicion(sizeWorld.X, 0))); // muro de derecha
            mundo.AdicionarSprite(new Muro(game, new Vector2(sizeWorld.X, Celda), posicion(0, sizeWorld.Y - Celda))); // muro de abajo
            mundo.AdicionarSprite(new Muro(game, new Vector2(Celda, sizeWorld.Y), posicion(0, 0))); // muro de izquierda

            ////crear algunas plataformas
            ////mundo.AdicionarSprite(new Muro(game, new Vector2(game.Window.ClientBounds.Width / 2, 24), new Vector2(0, 96)));
            ////mundo.AdicionarSprite(new Muro(game, new Vector2(game.Window.ClientBounds.Width / 2, 24), new Vector2(0, 500)));
            ////mundo.AdicionarSprite(new Muro(game, new Vector2(game.Window.ClientBounds.Width / 2, 24), new Vector2(game.Window.ClientBounds.Width - (game.Window.ClientBounds.Width / 2), 96 + 96 + 28)));
            ////mundo.AdicionarSprite(new Muro(game, new Vector2(256, 20), new Vector2(120, (int)(96 * 4.5f))));
            ////mundo.AdicionarSprite(new Muro(game, new Vector2(Celda, Celda), new Vector2(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height - Piso)));
            //mundo.AdicionarSprite(new Muro(game, new Vector2(Celda, Celda), posicion(Celda * 5, game.Window.ClientBounds.Height - Piso - Celda)));
            //mundo.AdicionarSprite(new Muro(game, new Vector2(Celda, Celda), posicion(Celda * 5, game.Window.ClientBounds.Height - Piso - Celda * 2)));
            //mundo.AdicionarSprite(new Muro(game, new Vector2(Celda * 2, Celda), posicion(Celda * 4, game.Window.ClientBounds.Height - Piso)));

            //mundo.AdicionarSprite(new Muro(game, new Vector2(3 * Celda, Celda), posicion(game.Window.ClientBounds.Width, game.Window.ClientBounds.Height - Piso)));

            ////crea al jugador
            //jugador = new Jugador(game, new Vector2(Celda, Celda), posicion(Celda, size.Y - 4 * Celda), "players/blue");
            //mundo.AdicionarSprite(jugador);

            ////crea al agente
            //Enemigo2 enemigo = new Enemigo2(game, new Vector2(Celda, Celda), posicion(20 * Celda, game.Window.ClientBounds.Height - Celda - Celda), "players/red");
            //mundo.AdicionarSprite(enemigo);
            //mundo.AdicionarAgente(enemigo);
        }

        public void nuevo_elemento(SpriteComponent obj)
        {
            mundo.AdicionarSprite(obj);
        }

        public void nuevo_muro(Muro obj)
        {
            mundo.AdicionarSprite(obj);
        }

        public void nuevo_enemigo(Agent obj) 
        {
            mundo.AdicionarAgente(obj);
        }

        private Vector2 posicion(float X, float Y) 
        {
            return new Vector2(X,Y);
        }
 
        protected override void LoadContent()
        {
            base.LoadContent();
        }
 
        public override void Update(GameTime gameTime)
        {
            if (!(jugador == null))
            {
                float deltaTime = (float)((double)gameTime.ElapsedGameTime.Milliseconds / 1000);
                float totalTime = (float)((double)gameTime.TotalGameTime.TotalMilliseconds / 1000);
                KeyboardState newState = Keyboard.GetState();
                Vector2 velocidad = jugador.Velocidad;
                Vector2 desplazamiento = Vector2.Zero;
                bool isOnGround = jugador.isOnGround;

                velocidad.X = 0;
                desplazamiento.X = 0;

                if (newState.IsKeyDown(Keys.Space))
                {
                    //Debug.Print("espacio presionado");
                    if (isOnGround)
                    {
                        velocidad.Y = -300;
                        isOnGround = false;
                    }
                }
                if (newState.IsKeyDown(Keys.A))
                {
                    velocidad.X = -200;
                }
                if (newState.IsKeyDown(Keys.D))
                {
                    if ((jugador.Posicion.X / 32) > (size.X / 64))
                    {
                        desplazamiento.X = -200;
                    }
                    else velocidad.X = 200;
                }

                mundo.Desplazamiento = desplazamiento;
                jugador.Velocidad = velocidad;
                jugador.isOnGround = isOnGround;
                mundo.Update(deltaTime, totalTime);
            }
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
