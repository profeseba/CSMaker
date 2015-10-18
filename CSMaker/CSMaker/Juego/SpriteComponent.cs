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
    public abstract class SpriteComponent : DrawableGameComponent

    {
        // Propiedades
        ContentManager Content;
        SpriteBatch spriteBatch;
        public Rectangle textura_origen;
        protected Colision cls;
        protected Vector2 posicion { get; set; }
        public Vector2 Posicion { get { return posicion; } set { posicion = value; } }
        public Texture2D Textura { get; set; }
        protected Vector2 velocidad;
        public Vector2 Velocidad { get { return velocidad; } set { velocidad = value; } }
        public bool isOnGround { get; set; }
        public Vector2 Centro { get; set; }
        public Vector2 Tamano { get; set; }
        public float Peso { get; set; }
        public BoundingBox Bound { get { return bound; } set { bound = value; } }
        public Color ColorImagen { get; set; }
        public String NombreImagen { get; set; }
        public float Salto = 400;
        public String Direccion { get; set; }
        public int life { get; set; }
        public String direccionColision { get; set; } // lado en que se colisiono
        public String nombreSprite { get; set; }
        public bool died { get; set; }
        BoundingBox bound;

        public SpriteComponent(Microsoft.Xna.Framework.Game game, Vector2 tamano, Vector2 pos)
            : base(game)
        {
            // TODO: Construct any child components here
            Content = (ContentManager)Game.Services.GetService(typeof(ContentManager));
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            nombreSprite = "sprite";
            died = false;
            Tamano = tamano;
            textura_origen = new Rectangle(0, 0, (int)Tamano.X, (int)Tamano.Y);
            posicion = pos;
            life = 1;
            Peso = 1.0f;
            cls = new Colision();
            isOnGround = true;
            Vector3 min1 = new Vector3(posicion.X, posicion.Y, 0);
            Vector3 max1 = new Vector3((posicion.X + Tamano.X), (posicion.Y + Tamano.Y), 0);
            bound = new BoundingBox(min1, max1);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        protected override void LoadContent()
        {
            Textura = Content.Load<Texture2D>(NombreImagen);
            base.LoadContent();
        }
 
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        public void verificarMuerte()
        {
           // Debug.Print("vida :" + life);
            if (life <= 0)
            {
                died = true;
            }
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(Textura, posicion, textura_origen, ColorImagen);
            base.Draw(gameTime);
        }

        public virtual void Show()
        {
            Enabled = true;
            Visible = true;
        }

        public virtual void Hide()
        {
            Enabled = false;
            Visible = false;
        }

        public void Mover(Vector2 velocidad)
        {
            posicion += velocidad;
            bound.Max = new Vector3((posicion.X + Tamano.X), (posicion.Y + Tamano.Y), 0);
            bound.Min = new Vector3(posicion.X, posicion.Y, 0);
        }

        public abstract void Colision(SpriteComponent otro, Vector2 desplazamiento);

    }
}
