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
                Image img = Image.FromFile(@"img\default2.png");
                //Size tamImg = new System.Drawing.Size(img.Width*zoom,img.Height*zoom);
                //tileSheet =  new Bitmap(img,tamImg);
                tileSheet = new Bitmap(img);
                pb_tileset = new PictureBox();
                pb_tileset.Image = tileSheet;
                pb_tileset.Size = tileSheet.Size;

                Graphics g = Graphics.FromImage(tileSheet);

                Pen miLapiz = new Pen(System.Drawing.Color.White);
                for (int y = 0; y < tileSheet.Height; y += (TamTile + 1))
                {
                    g.DrawLine(miLapiz, 0, y, tileSheet.Width, y);
                }
                for (int x = 0; x < tileSheet.Width; x += (TamTile + 1))
                {
                    g.DrawLine(miLapiz, x, 0, x, tileSheet.Height);
                }
                //Dibujar los rectangulos de Selección
                miLapiz = new Pen(System.Drawing.Color.Red, 2);
                g.DrawRectangle(miLapiz, seleccion.X * (TamTile + 1) + 1,
                seleccion.Y * (TamTile + 1) + 1, seleccion.Width * (TamTile + 1) - 1.5f,
                seleccion.Height * (TamTile + 1) - 1.5f);

                game.crearJuego(anchoMapa,altoMapa);

                pb_editor.Visible = true;

                panel_editor.Controls.Add(pb_tileset);

                
            }
            catch (Exception)
            {
                //
                Debug.Print("paso algo");
            }
            
        }
    }
}
