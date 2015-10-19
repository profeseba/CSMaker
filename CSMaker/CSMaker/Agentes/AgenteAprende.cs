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
    public abstract class AgenteAprende : Agent
    {
        private Vector2 posPlayer;
        private Vector2 posAgent;
        private Bloque area;
        private List<Vector2> camino;
        private ColumnasAER accionAnterior;
        private TablaAER Q;
        private bool nomemory;

        public AgenteAprende(Microsoft.Xna.Framework.Game game, Vector2 tamano, Vector2 posicion, String nombreImagen)
            : base(game, tamano, posicion, nombreImagen)
        {
            NombreImagen = nombreImagen;
            ColorImagen = Color.White;
            posPlayer = new Vector2(-1, -1);
            posAgent = new Vector2(-1, -1);
            area = new Bloque();
            accionAnterior = new ColumnasAER();
            life = 1;
            try
            {
                Q = XML.Deserialize<TablaAER>("memoria");
            }
            catch (Exception)
            {
                nomemory = true;
                Q = new TablaAER();
            }
            
            LoadContent();
        }
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
            SensorAgente();
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
                    Bloque estado = disminuirArea(3);  // EJ: escala 1 : 1 un bloque por cada lado adyacente // matriz de 3x3                  
                    accion = funcionQ(accion, estado); // algoritmo que aprende
                    // - Debug.WriteLine(accion);
                    Comportamiento(accion);
                }
            }
            //Debug.WriteLine("");
        }
        // --- funcion evaluarRecompensa
        private float evaluarRecompensa(TablaAER Q, String accion, Bloque estado)
        {
            // recompensa por morir.
            if (life <= 0)
            {
                return -10f;
            }
            ColumnasAER tupla = Q.getTupla(accion, estado);
            if (tupla != null)
            {
                return tupla.valor;
            }

            float Out = -1f;
            // recompensa para avanzarDer, avanzarIzq, saltar es de -0.03
            if ((accion.Equals("avanzarDer")) || (accion.Equals("avanzarIzq")))
            {
                Out = -3f;
            }
            // recompensa para saltarIzq, saltarDer es de -0.02
            if ((accion.Equals("saltarDer")) || (accion.Equals("saltarIzq")) || (accion.Equals("saltar")))
            {
                Out = -2f;
            }
            return Out;
        }
        // --- funcion evaluarRecompensa2
        private float evaluarRecompensa(String accion, Bloque estado)
        {
            float Out = -1f;
            // recompensa para avanzarDer, avanzarIzq, saltar es de -0.03
            if ((accion.Equals("avanzarDer")) || (accion.Equals("avanzarIzq")))
            {
                Out = -3f;
            }
            // recompensa para saltarIzq, saltarDer es de -0.02
            if ((accion.Equals("saltarDer")) || (accion.Equals("saltarIzq")) || (accion.Equals("saltar")))
            {
                Out = -2f;
            }
            return Out;
        }
        // --- funcion evalua cada accion, estado y recompensa. Retorna la accion a tomar
        private String funcionQ(String aA, Bloque eA) // valores Actuales : accionActual, estadoActual, recompensaActual
        {
            String accion = "nada";
            if (nomemory)
            {
                Q.FilasAER = new List<ColumnasAER>();
                Q.Movimientos = new List<string>();
                float rA = evaluarRecompensa(aA, eA);
                ColumnasAER tupla = new ColumnasAER(aA, eA, 0, rA);
                // agrega la tupla directo en la tabla
                Q.addTupla(tupla);
                accion = tupla.accion;
                nomemory = false;
                return accion;
            }
            if (Q.getTupla(aA, eA) != null) // busca la tupla en la tabla
            {
                float rA = evaluarRecompensa(Q, aA, eA);
                ColumnasAER tupla = Q.getActionMaxQ(aA, eA, rA);
                // agrega la tupla en la tabla
                Q.addTupla(tupla);
                accionAnterior = tupla;
                accion = tupla.accion;
                // - Debug.WriteLine("La tupla si esta");
            }
            else
            {
                float rA = evaluarRecompensa(Q, aA, eA);
                ColumnasAER tupla = new ColumnasAER(aA, eA, 0, rA);
                // agrega la tupla directo en la tabla
                Q.addTupla(tupla);
                accion = tupla.accion;
            }
                return accion;
        }


        // guardar memoria
        public override void muerte() // guarda la tabla en memoria
        {
            // recompenza por morir
            accionAnterior.valor = -10f;
            Q.addTupla(accionAnterior);
            XML.Serialize(Q, "memoria");
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
