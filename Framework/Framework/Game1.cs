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
using CSMaker;

namespace Framework
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //MenuScene escenaInicio;
        //HelpScene escenaAyuda;
        GameScene escenaActiva;
        ActionScene escenaAccion;
        //SpriteFont fuenteNormal;
        Texture2D fondo;
        //KeyboardState newState;
        //KeyboardState oldState;
        Vector2 size;
        System.Windows.Forms.Control gameForm;
        private IntPtr drawSurface;
        private System.Windows.Forms.Form parentForm;
        private System.Windows.Forms.PictureBox pictureBox;

        public Game1(IntPtr drawingSurface, System.Windows.Forms.Form parentForm, System.Windows.Forms.PictureBox pictureBox)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "CSMakerTestv1.1";
            this.drawSurface = drawingSurface;
            this.parentForm = parentForm;
            this.pictureBox = pictureBox;

            size = new Vector2(960, 640); // x = 30*32px ; y = 20*32px
            this.pictureBox.Width = (int)size.X;
            this.pictureBox.Height = (int)size.Y;
            this.graphics.PreferredBackBufferWidth = (int)size.X;
            this.graphics.PreferredBackBufferHeight = (int)size.Y;

            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
            Mouse.WindowHandle = drawSurface;
            gameForm = System.Windows.Forms.Control.FromHandle(this.Window.Handle);
            gameForm.VisibleChanged += new EventHandler(gameForm_VisibleChanged);
            gameForm.SizeChanged += new EventHandler(gameForm_SizeChanged);

            //this.graphics.IsFullScreen = true;
        }
        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = drawSurface;
        }
        void gameForm_VisibleChanged(object sender, EventArgs e)
        {
            if (gameForm.Visible == true)
            {
                gameForm.Visible = false;
            }
        }
        void gameForm_SizeChanged(object sender, EventArgs e)
        {
            //graphics.PreferredBackBufferWidth = pictureBox.Height;
            //graphics.PreferredBackBufferHeight = pictureBox.Width;
            //graphics.ApplyChanges();
        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            Services.AddService(typeof(ContentManager), Content);
            //fuenteNormal = Content.Load<SpriteFont>("fuente");
            fondo = Content.Load<Texture2D>("background/mario");
            //escenaInicio = new MenuScene(this, fuenteNormal, fondo);
            //Components.Add(escenaInicio);
            //fondo = Content.Load<Texture2D>("fondo2");
            //escenaAyuda = new HelpScene(this, fondo);
            //Components.Add(escenaAyuda);
            escenaAccion = new ActionScene(this, fondo, new Vector2(70 * 32, size.Y));
            Components.Add(escenaAccion);
            escenaAccion.Show();
            //escenaInicio.Show();
            //escenaAyuda.Hide();
            //escenaActiva = escenaInicio;
            escenaActiva = escenaAccion;
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            // loop
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
