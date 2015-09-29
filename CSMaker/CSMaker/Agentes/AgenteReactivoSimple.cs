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
    public abstract class AgenteReactivoSimple : Agent
    {
        public AgenteReactivoSimple(Microsoft.Xna.Framework.Game game, Vector2 tamano, Vector2 posicion, string nombreImagen)
            : base(game, tamano, posicion, nombreImagen)
        {
            profundidad = 1;
            NombreImagen = nombreImagen;
            ColorImagen = Color.White;
            LoadContent();
        }

        public override void Sensor(Bloque area)
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
            //sA.value = false;
            //sA.name = "wall";
            //sA.posicion = new Vector2(1,0);
            //sB.value = false;
            //sB.name = "wall";
            //sB.posicion = new Vector2(1,2);
            //bA.sector.Add(sA); // agrega sector 1,0
            //bA.sector.Add(sB); // agrega sector 1,2
            //cond.bloque.Add(bA); // agrega bloque a la lista de bloques.
            //r.regla.Add(cond); // agrega la condicion a la regla.
            //r.regla.Add(cond); // agrega la condicion a la regla.
            //// fin condiciones
            //  ---------------------------------------------------------------------
            //XML.Serialize(r, "reglas.dat");
            r = XML.Deserialize<reglas>("reglas.dat");
            acciones action = new acciones();
            action = Regla(area, r);
            //
            Comportamiento(action);
        }

        public acciones Regla(Bloque area, reglas r)
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
                        foreach (var e_sector in area.sector) // lista de sectores del sensor
                        {
                            //Debug.Write(e_sector.posicion + " ");
                            if ((r_sector.posicion == e_sector.posicion) && (r_sector.value == e_sector.value)) sectores++;
                        }
                        
                    }
                    if (sectores == r_bloq.sector.Count) cond++;
                    sectores = 0;
                }
                if (cond == r_cond.bloque.Count) { nuevasAcciones.accion.Add(r_cond.accion); cond = 0; }
            }
            return nuevasAcciones;
        }
    }
}
