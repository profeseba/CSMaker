using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Agentes;
using Game.Juego;
using Microsoft.Xna.Framework;

namespace Game
{
    public abstract class Agent : SpriteComponent
    {
        public Agent(Microsoft.Xna.Framework.Game game, Vector2 tamano, Vector2 posicion, String nombreImagen)
            : base(game, tamano, posicion)
        {
            NombreImagen = nombreImagen;
            ColorImagen = Color.White;
            Direccion = "left";
            LoadContent();
        }

        public override void Colision(SpriteComponent otro, Vector2 desplazamiento) 
        {
            if (otro is Muro)
            {
                Mover(desplazamiento);
                if (desplazamiento.Y != 0)
                {
                    velocidad.Y = 0;
                    isOnGround = true;
                }
            }
            if (otro is Jugador)
            {
                Mover(desplazamiento);
                if (desplazamiento.Y != 0)
                {
                    velocidad.Y = 0;
                }
            }
            if (otro is Objeto)
            {
                Mover(desplazamiento);
                if (desplazamiento.Y != 0)
                {
                    velocidad.Y = 0;
                    isOnGround = true;
                }
            }
        }

        

        public acciones Regla(estados e, reglas r)
        {
            acciones nuevasAcciones = new acciones();
            nuevasAcciones.accion = new List<string>();
            int cond = 0, sectores = 0;
            
            foreach (var r_cond in r.regla) // lista de condiciones
            {
                foreach (var r_bloq in r_cond.bloque) // lista de bloques regla
                {
                    foreach (var r_sector in r_bloq.sector) // sectores de cada bloque regla
                    {
                        foreach (var e_bloq in e.bloque) // lista de bloques del sensor
                        {
                            foreach (var e_sector in e_bloq.sector) // lista de sectores del sensor
                            {
                                if ((r_sector.name == e_sector.name) && (r_sector.value == e_sector.value)) sectores++;
                            }
                        }
                    }
                    if (sectores == r_bloq.sector.Count) cond++;
                    sectores = 0;
                }
                if (cond == r_cond.bloque.Count) { nuevasAcciones.accion.Add(r_cond.accion); cond = 0; }   
            }
            return nuevasAcciones;
        }

        public acciones Regla2(estados e, reglas r)
        {
            acciones nuevasAcciones = new acciones();
            nuevasAcciones.accion = new List<string>();
            int cond = 0, sectores = 0, aux = 0;

            foreach (var r_cond in r.regla) // lista de condiciones
            {
                foreach (var r_bloq in r_cond.bloque) // lista de bloques regla
                {
                    foreach (var r_sector in r_bloq.sector) // sectores de cada bloque regla
                    {
                        //Console.Out.WriteLine(r_bloq.element);
                        foreach (var e_bloq in e.bloque) // lista de bloques del sensor
                        {
                            if (aux == sectores)
                            {
                                foreach (var e_sector in e_bloq.sector) // lista de sectores del sensor
                                {
                                    //Console.Out.Write(" " + e_sector.name + ":" + e_sector.value);
                                    if ((r_sector.name == e_sector.name) && (r_sector.value == e_sector.value))
                                    {
                                        sectores++;
                                        //Console.Out.Write(" " + e_sector.name + ":" + e_sector.value);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                aux = sectores;
                                break;
                            }

                        }
                    }
                    if (sectores == r_bloq.sector.Count) { cond++; }
                    sectores = 0; aux = 0;
                }
                if (cond == r_cond.bloque.Count) { nuevasAcciones.accion.Add(r_cond.accion); Console.Out.WriteLine(r_cond.accion); cond = 0; }
            }

            return nuevasAcciones;
        }

        public void Sensor(estados st) 
        {
            //asigna los estados
            //estados e = Percepciones(sprite);
            // acciones necesitan reglas
            reglas r = new reglas();
            //// ---------------------------------------------------------------------
            //r.regla = new List<condiciones>();
            //// condiciones
            //// condicion 1
            //condiciones cond = new condiciones(); // tecnicamente esto es una regla
            //cond.bloque = new List<Bloque>();
            //Bloque bA = new Bloque();
            //bA.sector = new List<Sector>();
            //Sector sA = new Sector();
            //Sector sB = new Sector();
            //Sector sC = new Sector();
            //cond.accion = "avanzar";
            //sA.value = true;
            //sA.name = "F";
            //sB.value = true;
            //sB.name = "G";
            //sC.value = true;
            //sC.name = "H";
            //bA.sector.Add(sA); // agrega sector F
            //bA.sector.Add(sB); // agrega sector G
            //bA.sector.Add(sC); // agrega sector H
            //bA.element = "block"; // agrega el nombre del bloque
            //cond.bloque.Add(bA); // agrega bloque a la lista de bloques.
            //r.regla.Add(cond); // agrega la condicion a la regla.
            //// condicion 2
            //cond = new condiciones(); // tecnicamente esto es una regla
            //cond.bloque = new List<Bloque>();
            //bA = new Bloque();
            //bA.sector = new List<Sector>();
            //sA = new Sector();
            //cond.accion = "saltarIzq";
            //sA.value = true;
            //sA.name = "D";
            //bA.sector.Add(sA); // agrega sector D
            //bA.element = "block"; // agrega el nombre del bloque
            //cond.bloque.Add(bA); // agrega bloque a la lista de bloques.
            //r.regla.Add(cond); // agrega la condicion a la regla.
            //// fin condiciones
            ////  ---------------------------------------------------------------------
            //XML.Serialize(r, "reglas.dat");
            r = XML.Deserialize<reglas>("reglas.dat");
            acciones action = new acciones();
            action = Regla(st, r);
            //
            Comportamiento(action);
        }

        public abstract void Comportamiento(acciones a);

        public void perseguir()
        {
            // calcula el mejor camino.. aun por definir.
            if (isOnGround)
            {
                velocidad.Y = -Salto;
                isOnGround = false;
            } 
        }

        public void saltar() 
        {
            if (isOnGround)
            {
                velocidad.Y = -Salto;

                isOnGround = false;
            }    
        }
        
        public void avanzar()
        {
            if (Direccion.Equals("left"))
            {
                velocidad.X = -100;
            }
            else if (Direccion.Equals("right"))
            {
                velocidad.X = 100;
            }
            
        }

        public void avanzarDireccion(String Direccion)
        {
            if (Direccion.Equals("left"))
            {
                velocidad.X = -100;
            }
            else if (Direccion.Equals("right"))
            {
                velocidad.X = 100;
            }

        }

        public void cambiarDireccion() 
        {
            if (Direccion.Equals("left")) Direccion = "right";
            else Direccion = "left";
             // hace una llamada a la funcion avanzar para no entrar en un loop.
            avanzar();
        }

       


    }
}
