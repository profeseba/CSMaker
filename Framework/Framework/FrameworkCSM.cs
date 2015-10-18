using CSMaker;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Framework
{
    public partial class FrameworkCSM : Form
    {
        public Game1 game;
        private bool mapaCreado;
        Bitmap tileSheet, personaje, enemigo;
        private System.Drawing.Rectangle seleccion = new System.Drawing.Rectangle(0, 0, 0, 0);
        private System.Drawing.Rectangle pintarSeleccion = new System.Drawing.Rectangle(0, 0, 0, 0);
        PictureBox pb_tileset;
        private int altoMapa;
        private int anchoMapa;
        private int TamTile;
        System.Drawing.Point origen, destino;
        private bool imagen;

        public FrameworkCSM()
        {
            InitializeComponent();
            pb_editor.Visible = false;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        // boton crear mapa
        private void button1_Click(object sender, EventArgs e)
        {
            mapaCreado = true;
            String ancho = textBox1.Text;
            String alto = textBox2.Text;
            String tamCelda = textBox3.Text;
            try
            {
                altoMapa = Int32.Parse(alto);
                anchoMapa = Int32.Parse(ancho);
                TamTile = Int32.Parse(tamCelda);
                //int zoom = TamTile/32;
                cargarImagenPictureBox(@"img\background\default2.png");
                
                //Graphics g = Graphics.FromImage(tileSheet);
                //// dibuja la grilla
                //Pen miLapiz = new Pen(System.Drawing.Color.White);
                //for (int y = 0; y < tileSheet.Height; y += (TamTile + 1))
                //{
                //    g.DrawLine(miLapiz, 0, y, tileSheet.Width, y);
                //}
                //for (int x = 0; x < tileSheet.Width; x += (TamTile + 1))
                //{
                //    g.DrawLine(miLapiz, x, 0, x, tileSheet.Height);
                //}
                ////Dibujar los rectangulos de Selección
                //miLapiz = new Pen(System.Drawing.Color.Red, 2);
                //g.DrawRectangle(miLapiz, seleccion.X * (TamTile + 1) + 1,
                //seleccion.Y * (TamTile + 1) + 1, seleccion.Width * (TamTile + 1) - 1.5f,
                //seleccion.Height * (TamTile + 1) - 1.5f);

                imagen = true;
                pb_diseno.Visible = true;

                game.crearJuego(anchoMapa,altoMapa);
                // hace visible el picturebox del mapa
                pb_editor.Visible = true;                
                
            }
            catch (Exception)
            {
                //
                Debug.Print("paso algo en la creacion");
            }
            
        }
        // funcion para dibujado
        // captura la imagen a dibujar
        private void pb_diseno_MouseClick(object sender, MouseEventArgs e)
        {
            origen.X = (int)Math.Floor((double)e.X / TamTile);
            origen.Y = (int)Math.Floor((double)e.Y / TamTile);
            seleccion = new System.Drawing.Rectangle(origen.X, origen.Y, 1, 1);
            this.pb_diseno.Invalidate();

        }
        // dibuja la grilla y seleccion
        private void pb_diseno_Paint(object sender, PaintEventArgs e)
        {
            //Dibujar la Grilla
            if (imagen)
            {
                Pen miLapiz = new Pen(System.Drawing.Color.Black);
                for (int y = 0; y < this.pb_diseno.Height; y += (TamTile + 1))
                {
                    e.Graphics.DrawLine(miLapiz, 0, y, this.pb_diseno.Width, y);
                }
                for (int x = 0; x < this.pb_diseno.Width; x += (TamTile + 1))
                {
                    e.Graphics.DrawLine(miLapiz, x, 0, x, this.pb_diseno.Height);
                }
                //Dibujar los rectangulos de Selección
                miLapiz = new Pen(System.Drawing.Color.Red, 2);
                e.Graphics.DrawRectangle(miLapiz, seleccion.X * (TamTile + 1) + 1,
                seleccion.Y * (TamTile + 1) + 1, seleccion.Width * (TamTile + 1) - 1.5f,
                seleccion.Height * (TamTile + 1) - 1.5f);
            }
        }
        // dibujar en el mapa
        private void pb_editor_MouseClick(object sender, MouseEventArgs e)
        {
            int x1 = (int)Math.Floor((double)e.X / TamTile);
            int y1 = (int)Math.Floor((double)e.Y / TamTile);
            Debug.Print("nuevo muro");
            game.escenaAccion.nuevo_muro(new Muro(game, new Vector2(TamTile, TamTile), new Vector2(x1, y1)));
            this.pb_diseno.Invalidate();
        }
        // fin funcion para dibujado

        private void tabControlEventMapa(object sender, EventArgs e)
        {
            StringBuilder img = new StringBuilder();
            img.Append(@"img\");
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    pb_diseno.Visible = true;
                    //img.Append(@"background\default2.png");
                    //cargarImagenPictureBox(img.ToString());

                    break;
                case 1:
                    pb_diseno.Visible = false;
                    //img.Append(@"players\blue.png"); 
                    break;
                case 2:
                    pb_diseno.Visible = false;
                    //img.Append(@"players\red.png"); 
                    break;
                case 3:
                    pb_diseno.Visible = false;
                    //img.Append(@"background\black.jpg"); 
                    break;
                case 4:
                    pb_diseno.Visible = false;
                    //img.Append(@"background\black.jpg"); 
                    break;
                default:
                    pb_diseno.Visible = false;
                    //img.Append(@"background\default2.png"); 
                    break;
            }
            
        }

        private void cargarImagenPictureBox(String file)
        {
            Image img = Image.FromFile(file);
            //Size tamImg = new System.Drawing.Size(img.Width*zoom,img.Height*zoom);
            //tileSheet =  new Bitmap(img,tamImg);
            tileSheet = new Bitmap(img);
            pb_diseno.Image = tileSheet;
            pb_diseno.Size = tileSheet.Size;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void b_personaje_Click(object sender, EventArgs e)
        {

        }
    }
}
