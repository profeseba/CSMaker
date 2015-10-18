using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace CSMaker
{
    public abstract class AgenteUtilidad : Agent
    {
        private Vector2 posPlayer;
        private Vector2 posAgent;
        private Bloque area;
        private List<Vector2> camino;

        public AgenteUtilidad(Microsoft.Xna.Framework.Game game, Vector2 tamano, Vector2 posicion, String nombreImagen)
            : base(game, tamano, posicion, nombreImagen)
        {
            NombreImagen = nombreImagen;
            ColorImagen = Color.White;
            posPlayer = new Vector2(-1, -1);
            posAgent = new Vector2(-1, -1);
            area = new Bloque();
            life = 1;
            LoadContent();
        }

        public override void muerte() { }

        // --- Calcula el camino utilizando hilos
        private void CalcularCaminoThread()
        {
            foreach (var item in area.sector)
            {
                //Debug.Write(item.name+":"+item.value+" ");
                if (item.name.Equals("player"))
                {
                    acciones action = new acciones();
                    action = Camino(area);
                    Comportamiento(action);
                }
            }
            //Debug.WriteLine("");
            

        }
        // --- funcion sensor -- crea hilos
        public override void Sensor(Bloque area)
        {
            this.area = area;
            // crea un hilo para calcular el camino optimo
            //Thread cal = new Thread(new ThreadStart(CalcularCaminoThread));
            //Thread cal = new Thread(new ThreadStart(SensorAgente));
            //cal.Start();
            if (!died)
            {
                SensorAgente();
            }            
        }
        // --- funcion SensorAgente que aprende
        private void SensorAgente()
        {
            foreach (var item in area.sector)
            {
                //Debug.Write(item.name+":"+item.value+" ");
                if (item.name.Equals("player"))
                {
                    String accion = CaminoActualizado(area);
                    Bloque estado = disminuirArea(2);  // EJ: escala 1 : 1 un bloque por cada lado adyacente // matriz de 3x3                  
                    //accion = funcionQ(accion, estado); // algoritmo que aprende
                    // - Debug.WriteLine(accion);
                    Comportamiento(accion);
                }
            }
            if (life <= 0)
            {
                died = true;
            }
            //Debug.WriteLine("");
        }
        // --- funcion evaluarRecompensa
        private float evaluarRecompensa(TablaAER Q, String accion, Bloque estado)
        {
            // recompensa por morir.
            if (life <= 0)
            {
                return -1.0f;
            }
            ColumnasAER tupla = Q.getTupla(accion, estado);
            if (tupla != null)
            {
                return tupla.valor;
            }

            float Out = -0.1f;
            // recompensa para avanzarDer, avanzarIzq, saltar es de -0.03
            if ((accion.Equals("avanzarDer")) || (accion.Equals("avanzarIzq")))
            {
                Out = -0.3f;
            }
            // recompensa para saltarIzq, saltarDer es de -0.02
            if ((accion.Equals("saltarDer")) || (accion.Equals("saltarIzq")) || (accion.Equals("saltar")))
            {
                Out = -0.2f;
            }
            return Out;
        }
        // --- funcion evalua cada accion, estado y recompensa. Retorna la accion a tomar
        private String funcionQ(String aA, Bloque eA) // valores Actuales : accionActual, estadoActual, recompensaActual
        {
            TablaAER Q = new TablaAER();
            String accion = "nada";
            // guardar
            // XML.Serialize(Q, "memoria");
            // cargar
            // XML.Deserialize<TablaAER>("memoria");
            // verificamos si existe el archivo memoria
            //if (contacto)
            //{
            //    rA = 1;
            //    contacto = false;
            //}
            if (File.Exists("memoria"))
            {
                // cargar memoria
                Q = XML.Deserialize<TablaAER>("memoria");
                if (Q.getTupla(aA,eA) != null) // busca la tupla en la tabla
                {
                    float rA = evaluarRecompensa(Q,aA,eA);
                    ColumnasAER tupla = Q.getActionMaxQ(aA, eA, rA);
                    // agrega la tupla en la tabla
                    Q.addTupla(tupla);
                    accion = tupla.accion;
                     // - Debug.WriteLine("La tupla si esta");
                }
                else // si no la encuentra
                {
                    
                    float rA = evaluarRecompensa(Q,aA, eA);
                    ColumnasAER tupla = new ColumnasAER(aA, eA, 0, rA);
                    // agrega la tupla directo en la tabla
                    Q.addTupla(tupla);
                    accion = tupla.accion;
                    // - Debug.WriteLine("La tupla no esta");
                }
                XML.Serialize(Q, "memoria");
                return accion;
            }
            // algoritmo que se ejecuta por defecto cuando no existe memoria
            else
            {
                // crear informacion
                Q.FilasAER = new List<ColumnasAER>();
                float rA = evaluarRecompensa(Q,aA, eA);
                ColumnasAER tupla = new ColumnasAER(aA,eA,0,rA);
                Q.FilasAER.Add(tupla);
                // se agrega el moviemiento a su memoria
                Q.Movimientos = new List<String>();
                Q.Movimientos.Add(aA);
                // guardar informacion en memoria
                XML.Serialize(Q, "memoria");
                // cerrar archivo
                // asigna la accion actual como accion por defecto
                accion = aA;
                // retorna la accion
                // - Debug.WriteLine("No existe memoria");
                return accion;
            }
        }
        // --- disminuye el area de hxh a 3x3
        private Bloque disminuirArea(int escala)
        {
            //Vector2 centro = new Vector2(profundidad, profundidad); // centro del area
            //Bloque Out = new Bloque();
            Sector swap = new Sector();
            // crea un bloque de dimension hxh vacio
            Bloque In = new Bloque(escala);
            // asigna el area en un bloque para manipular
            Bloque Inter = this.area;
            // asigna los valores del Area actual al bloque 
            int h = (escala * 2) + 1;
            int cuadrarX = escala;
            int cuadrarY = escala;
            int progresion = 0;
            for (int j = 0; j < h; j++) // para Y
            {
                for (int k = 0; k < h; k++) // para X
                {
                    swap = Inter.obtenerSector(new Vector2(profundidad - cuadrarX, profundidad - cuadrarY));
                    In.sector[progresion].name = swap.name;
                    In.sector[progresion].value = swap.value;
                    In.sector[progresion].posicion = new Vector2(k,j);
                    progresion++;
                    cuadrarX--;
                    //Debug.Print();
                    //Debug.Write("[" + swap.name + "]");
                }
                cuadrarX = escala;
                cuadrarY--;
                //Debug.WriteLine("");
            }
            //Debug.WriteLine("");
            
            return In;
        }
        // --- funcion que calcula el camino que debe seguir el agente, retorna el camino completo
        public acciones Camino(Bloque area)
        {
            BusquedaAestrella cOp = new BusquedaAestrella(area, profundidad); // instancia el algoritmo A*
            Vector2 final = cOp.getPosicionPlayer; // obtiene la posicion del player
            acciones camino = new acciones();
            camino.accion = new List<string>();
            /* 
             *  verifica que el jugador haya cambiado de posicion.
            */
            if (final!=posPlayer)
            {
                Vector2 reff = cOp.getPosicionAgent; // obtiene la posicion del agente (posicion inicial)
                List<Vector2> posiciones = cOp.encontrarCamino();
                foreach (var item in posiciones)
                {
                    //Debug.WriteLine(item + " - " + reff);
                    //derecha
                    if ((item.X == reff.X + 1) && (item.Y == reff.Y))
                    {
                        camino.accion.Add("avanzarDer");
                    }
                    //izquierda
                    if ((item.X == reff.X - 1) && (item.Y == reff.Y))
                    {
                        camino.accion.Add("avanzarIzq");
                    }
                    //arriba
                    if ((item.X == reff.X) && (item.Y == reff.Y - 1))
                    {
                        camino.accion.Add("saltar");
                    }
                    //abajo
                    if ((item.X == reff.X) && (item.Y == reff.Y + 1))
                    {
                        // nose
                    }
                    //derecha-arriba
                    if ((item.X == reff.X + 1) && (item.Y == reff.Y - 1))
                    {
                        camino.accion.Add("saltarDer");
                    }
                    //derecha-abajo
                    if ((item.X == reff.X + 1) && (item.Y == reff.Y + 1))
                    {
                        camino.accion.Add("avanzarDer");
                    }
                    //izquierda-arriba
                    if ((item.X == reff.X - 1) && (item.Y == reff.Y - 1))
                    {
                        camino.accion.Add("saltarIzq");
                    }
                    //izquierda abajo
                    if ((item.X == reff.X - 1) && (item.Y == reff.Y + 1))
                    {
                        camino.accion.Add("avanzarIzq");
                    }
                    reff = item;
                }
                posPlayer = final;
                return camino;
            }
            
            return camino;
        }
        // --- funcion que calcula el camino que debe seguir el agente, retorna solo el siguiente movimiento.
        public String CaminoActualizado(Bloque area)
        {
            BusquedaAestrella cOp = new BusquedaAestrella(area, profundidad); // instancia el algoritmo A*
            Vector2 final = cOp.getPosicionPlayer; // obtiene la posicion del player
            Vector2 inicio = cOp.getPosicionAgent; // obtiene la posicion del agente (posicion inicial)
            String Accion = "nada";
            Vector2 sucesion; // item: posicion a alcansar

            if ((final != posPlayer) || (inicio != posAgent)) // si la posicion del jugador cambia
            {
                camino = cOp.encontrarCamino();
                sucesion = camino[1];
            }
            else
            {
                sucesion = camino[1];
            }

            //Debug.WriteLine(item + " - " + reff);
            //derecha
            if ((sucesion.X == inicio.X + 1) && (sucesion.Y == inicio.Y))
            {
                Accion = "avanzarDer";
            }
            //izquierda
            if ((sucesion.X == inicio.X - 1) && (sucesion.Y == inicio.Y))
            {
                Accion = "avanzarIzq";
            }
            //arriba
            if ((sucesion.X == inicio.X) && (sucesion.Y == inicio.Y - 1))
            {
                Accion = "saltar";
            }
            //abajo
            if ((sucesion.X == inicio.X) && (sucesion.Y == inicio.Y + 1))
            {
                // por definir
            }
            //derecha-arriba
            if ((sucesion.X == inicio.X + 1) && (sucesion.Y == inicio.Y - 1) )
            {
                if ((area.obtenerSector(new Vector2(inicio.X + 1, inicio.Y)).value == true))
                {
                    Accion = "saltarDer";
                }
                else
                {
                    Accion = "avanzarDer";
                }
                
            }
            //derecha-abajo
            if ((sucesion.X == inicio.X + 1) && (sucesion.Y == inicio.Y + 1))
            {
                Accion = "avanzarDer";
            }
            //izquierda-arriba
            if ((sucesion.X == inicio.X - 1) && (sucesion.Y == inicio.Y - 1) )
            {
                if ((area.obtenerSector(new Vector2(inicio.X - 1, inicio.Y)).value == true))
                {
                    Accion = "saltarIzq";
                }
                else
                {
                    Accion = "avanzarIzq";
                }                
            }
            //izquierda abajo
            if ((sucesion.X == inicio.X - 1) && (sucesion.Y == inicio.Y + 1))
            {
                Accion = "avanzarIzq";
            }
            //reff = item;
            // asigna los valores de las posiciones al agente y al player
            posPlayer = final;
            posAgent = inicio;

            return Accion;
        }
        // --- funcion que realiza la accion
        private void Comportamiento(String a)
        {
            //Debug.WriteLine(" -" +a);
            if (a.Equals("avanzarIzq")) avanzarIzquierda();
            else if (a.Equals("avanzarDer")) avanzarDerecha();
            else if (a.Equals("saltarIzq")) { avanzarIzquierda(); saltar(); }
            else if (a.Equals("saltarDer")) { avanzarDerecha(); saltar(); }
            else if (a.Equals("saltar")) saltar();

        }
        // --- funcion que realiza las acciones desde un conjunto de estas
        public override void Comportamiento(acciones a)
        {
            foreach (var accion in a.accion)
            {
                //Debug.WriteLine(accion);
                if (accion.Equals("avanzarIzq")) avanzarIzquierda();
                if (accion.Equals("avanzarDer")) avanzarDerecha();
                if (accion.Equals("saltarIzq")) { avanzarIzquierda(); saltar(); }
                if (accion.Equals("saltarDer")) { avanzarDerecha(); saltar(); }
                if (accion.Equals("saltar")) saltar();
            }
        }
        // --- fin ---
    }
}
