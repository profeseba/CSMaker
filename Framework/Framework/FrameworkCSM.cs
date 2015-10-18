using CSMaker;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

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
        System.Drawing.Point origen;
        private bool imagen;
        private bool player;
        private bool agente;
        private Jugador jugador;
        private bool flagJugador;
        private bool mapa;
        private List<int> listaAgente = new List<int>();
        List<MuroXML> murosXML;
        List<AgentesXML> agentesXML;
        JugadorXML jugadorXML;

        public FrameworkCSM()
        {
            InitializeComponent();
            pb_editor.Visible = false;
            murosXML = new List<MuroXML>();
            agentesXML = new List<AgentesXML>();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        // boton crear mapa
        [STAThread]
        private void button1_Click(object sender, EventArgs e)
        {
            mapaCreado = true;
            String ancho = textBox1.Text;
            String alto = textBox2.Text;
            String tamCelda = textBox3.Text;
            try
            {
                // 960,640 x = 30*32px ; y = 20*32px
                TamTile = Int32.Parse(tamCelda);
                altoMapa = Int32.Parse(alto) * TamTile;
                anchoMapa = Int32.Parse(ancho) * TamTile;
                
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
                mapa = true;
                pb_diseno.Visible = true;
                pb_editor.Width = anchoMapa;
                pb_editor.Height = altoMapa;
                game.crearJuego(anchoMapa,altoMapa);
                // redimensiona el pb_editor
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
        // dibujar en el mapa
        private void pb_editor_MouseClick(object sender, MouseEventArgs e)
        {
            int x1 = (int)Math.Floor((double)e.X / TamTile);
            int y1 = (int)Math.Floor((double)e.Y / TamTile);
            if (mapa)
            {
                game.escenaAccion.nuevo_muro(new Muro(game, new Vector2(TamTile, TamTile), new Vector2(x1 * TamTile, y1 * TamTile), "tileset/default2", new Vector2(origen.X * (TamTile + 1) + 1, origen.Y * (TamTile + 1) + 1)));
                // agrega el muro al XML para luego guardar
                murosXML.Add(new MuroXML(new Vector2(x1 * TamTile, y1 * TamTile), new Vector2(TamTile, TamTile), "tileset/default2", new Vector2(origen.X * (TamTile + 1) + 1, origen.Y * (TamTile + 1) + 1)));
            }
            if (player)
            {
                if (!flagJugador)
                {
                    textBox4.Text = "" + x1 * TamTile;
                    textBox5.Text = "" + y1 * TamTile;
                    jugador = new Jugador(game, new Vector2(32, 32), new Vector2(x1 * TamTile, y1 * TamTile), "players/blue");
                    jugador.Peso = 0.0f;
                    game.escenaAccion.nuevo_jugador(jugador);
                    flagJugador = true;
                    // agrega la info del personaje
                    jugadorXML = new JugadorXML(new Vector2(x1 * TamTile, y1 * TamTile), new Vector2(32, 32), "players/blue");
                }
                else
                {
                    textBox4.Text = ""+x1 * TamTile;
                    textBox5.Text = ""+y1 * TamTile;
                    game.escenaAccion.modificar_posicion_jugador(new Vector2(x1 * TamTile, y1 * TamTile));
                    // cambia la pos del jugador
                    jugadorXML.posicion = new Vector2(x1 * TamTile, y1 * TamTile);
                }
                
            }
            if (agente && (cb_enemigos.SelectedIndex!=-1))
            {
                label9.Text = "" + x1 * TamTile;
                label10.Text = "" + y1 * TamTile;
                game.escenaAccion.mover_obj(cb_enemigos.SelectedIndex, new Vector2(x1 * TamTile, y1 * TamTile));
                agentesXML[cb_enemigos.SelectedIndex].posicion = new Vector2(x1 * TamTile, y1 * TamTile);
            }

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
        
        // fin funcion para dibujado

        private void tabControlEventMapa(object sender, EventArgs e)
        {
            StringBuilder img = new StringBuilder();
            img.Append(@"img\");
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    mapa = true;
                    player = false;
                    agente = false;
                    if (imagen)
                    {
                        pb_diseno.Visible = true;
                        img.Append(@"background\default2.png");
                        cargarImagenPictureBox(img.ToString());
                    }
                    else pb_diseno.Visible = false;           
                    break;
                case 1:
                    mapa = false;
                    player = true;
                    agente = false;
                    pb_diseno.Visible = true;
                    img.Append(@"players\blue.png"); 
                    cargarImagenPictureBox(img.ToString());
                    break;
                case 2:
                    mapa = false;
                    player = false;
                    agente = true;
                    pb_diseno.Visible = false;
                    break;
                case 3:
                    
                    pb_diseno.Visible = false;
                    //img.Append(@"background\black.jpg"); 
                    //cargarImagenPictureBox(img.ToString());
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

        private void button2_Click(object sender, EventArgs e)
        {
            int index = -1;
            switch (cb_agentes.SelectedIndex)
            {
                case 0: // agente reactivo simple
                    //Enemigo ARS = new Enemigo(game, new Vector2(TamTile, TamTile), new Vector2(0, 0), "players/red");
                    Objeto ARS = new Objeto(game, new Vector2(TamTile, TamTile), new Vector2(-33, -33), "players/red");
                    // agrega el agente
                    agentesXML.Add(new AgentesXML(new Vector2(-33, -33), new Vector2(TamTile, TamTile), "players/red", "ARS"));
                    index = game.escenaAccion.nuevo_obj(ARS);
                    //listaAgente.Add(index);
                    cb_enemigos.Items.Add("Enemigo "+index);
                    break;
                case 1: // agente basado en utilidad
                    Objeto ABU = new Objeto(game, new Vector2(TamTile, TamTile), new Vector2(-33, -33), "players/orange");
                    agentesXML.Add(new AgentesXML(new Vector2(-33, -33), new Vector2(TamTile, TamTile), "players/orange", "ABU"));
                    index = game.escenaAccion.nuevo_obj(ABU);
                    //listaAgente.Add(index);
                    cb_enemigos.Items.Add("Enemigo "+index);
                    break;
                case 2: // agente basado en objetivo
                    Objeto ABO = new Objeto(game, new Vector2(TamTile, TamTile), new Vector2(-33, -33), "players/yellow");
                    agentesXML.Add(new AgentesXML(new Vector2(-33, -33), new Vector2(TamTile, TamTile), "players/yellow", "ABO"));
                    index = game.escenaAccion.nuevo_obj(ABO);
                    //listaAgente.Add(index);
                    cb_enemigos.Items.Add("Enemigo "+index);
                    break;
                case 3: // agente que aprende
                    Objeto AA = new Objeto(game, new Vector2(TamTile, TamTile), new Vector2(-33, -33), "players/green");
                    agentesXML.Add(new AgentesXML(new Vector2(-33, -33), new Vector2(TamTile, TamTile), "players/green", "AA"));
                    index = game.escenaAccion.nuevo_obj(AA);
                    //listaAgente.Add(index);
                    cb_enemigos.Items.Add("Enemigo "+index);
                    break;
                default:
                    break;
            }
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            JuegoXML juegoXML = new JuegoXML(textBox7.Text, new Vector2(anchoMapa, altoMapa), jugadorXML, murosXML, agentesXML);
            XML.Serialize(juegoXML, "game.dat");
            System.Diagnostics.Process.Start("copy", "CSMaker.exe " + textBox7.Text+".exe");
        }

        private void cerrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.Exit();
            Application.Exit();
        }
    }
}
