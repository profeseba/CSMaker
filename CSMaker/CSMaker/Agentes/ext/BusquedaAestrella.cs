using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSMaker
{
    class BusquedaAestrella
    {
        private const Int32 costoIrDerecho = 10;
        private const Int32 costoIrDiagonal = 15;
        private List<Nodo> listaAbierta = new List<Nodo>();
        private List<Vector2> listaCerrada = new List<Vector2>();
        private Bloque mapa;
        private int profundidad;
        private Vector2 posInicial;
        private Vector2 posFinal;
        public Vector2 getPosicionPlayer { get { return posFinal; } }
        public Vector2 getPosicionAgent { get { return posInicial; } }

        public BusquedaAestrella(Bloque mapa, int profundidad)
        {
            this.mapa = mapa;
            this.profundidad = (profundidad * 2) + 1;
            foreach (var item in mapa.sector)
            {
                if (item.name.Equals("player"))
                {
                    posFinal = item.posicion;
                }
            }
            posInicial = mapa.obtenerSector(new Vector2(profundidad,profundidad)).posicion;
        }

        /// <summary>
        /// Adiciona un Nodo a la lista abierta, ordenadamente
        /// </summary>
        /// <param name="nodo"></param>
        private void adicionarNodoAListaAbierta(Nodo nodo)
        {
            Int32 indice = 0;
            float costo = nodo.costoTotal;
            while ((listaAbierta.Count() > indice) && (costo < listaAbierta[indice].costoTotal))
            {
                indice++;
            }
            listaAbierta.Insert(indice, nodo);
        }

        public List<Vector2> encontrarCamino()
        {
            Sector sectorInicial = mapa.obtenerSector(posInicial);
            Sector sectorFinal = mapa.obtenerSector(posFinal);

            if (sectorInicial.name.Equals("wall") || sectorFinal.name.Equals("wall"))
            {
                return null;
            }

            listaAbierta.Clear();
            listaCerrada.Clear();

            Nodo nodoInicial;
            Nodo nodoFinal;

            nodoFinal = new Nodo(null, null, posFinal, 0);
            nodoInicial = new Nodo(null, nodoFinal, posInicial, 0);

            // se adiciona el nodo inicial
            adicionarNodoAListaAbierta(nodoInicial);
            while (listaAbierta.Count > 0)
            {
                Nodo nodoActual = listaAbierta[listaAbierta.Count - 1];
                // si es el nodo Final
                if (nodoActual.esIgual(nodoFinal))
                {
                    List<Vector2> mejorCamino = new List<Vector2>();
                    while (nodoActual != null)
                    {
                        mejorCamino.Insert(0, nodoActual.Posicion);
                        nodoActual = nodoActual.NodoPadre;
                    }
                    return mejorCamino;
                }
                listaAbierta.Remove(nodoActual);

                foreach (Nodo posibleNodo in encontrarNodosAdyacentes(nodoActual, nodoFinal))
                {
                    // si el nodo no se encuentra en la lista cerrada
                    if (!listaCerrada.Contains(posibleNodo.Posicion))
                    {
                        // si ya se encuentra en la lista abierta
                        if (listaAbierta.Contains(posibleNodo))
                        {
                            if (posibleNodo.costoG >= posibleNodo.costoTotal)
                            {
                                continue;
                            }
                        }
                        adicionarNodoAListaAbierta(posibleNodo);
                    }
                }
                // se cierra el nodo actual
                listaCerrada.Add(nodoActual.Posicion);
            }
            return null;
        }

        private List<Nodo> encontrarNodosAdyacentes(Nodo nodoActual, Nodo nodoFinal)
        {
            List<Nodo> nodosAdyacentes = new List<Nodo>();
            Int32 X = nodoActual.GrillaX;
            Int32 Y = nodoActual.GrillaY;
            Boolean arribaIzquierda = true;
            Boolean arribaDerecha = true;
            Boolean abajoIzquierda = true;
            Boolean abajoDerecha = true;

            //Izquierda
            if ((X > 0) && (!mapa.obtenerSector(new Vector2( X - 1, Y)).name.Equals("wall")))
            //if ((X > 0) && (!motor.Mapa.tileMapLayers[0].obtenerTile(X - 1, Y).Colision))
            {
                nodosAdyacentes.Add(new Nodo(nodoActual, nodoFinal, new Vector2(X - 1, Y), costoIrDerecho + nodoActual.costoG));
            }
            else
            {
                arribaIzquierda = false;
                abajoIzquierda = false;
            }

            //Derecha
            if ((X < profundidad - 1) && (!mapa.obtenerSector(new Vector2(X + 1, Y)).name.Equals("wall")))
            {
                nodosAdyacentes.Add(new Nodo(nodoActual, nodoFinal,
                new Vector2(X + 1, Y), costoIrDerecho + nodoActual.costoG));
            }
            else
            {
                arribaDerecha = false;
                abajoDerecha = false;
            }

            //Arriba
            //if ((Y > 0) && (!mapa.obtenerSector(new Vector2(X, Y - 1)).name.Equals("wall")))
            //{
            //    nodosAdyacentes.Add(new Nodo(nodoActual, nodoFinal,
            //    new Vector2(X, Y - 1), costoIrDerecho + nodoActual.costoG));
            //}
            //else
            //{
            //    arribaIzquierda = false;
            //    arribaDerecha = false;
            //}

            // Abajo
            if ((Y < profundidad - 1) && (!mapa.obtenerSector(new Vector2(X , Y + 1)).name.Equals("wall")))
            {
                nodosAdyacentes.Add(new Nodo(nodoActual, nodoFinal,
                new Vector2(X, Y + 1), costoIrDerecho + nodoActual.costoG));
            }
            else
            {
                abajoIzquierda = false;
                abajoDerecha = false;
            }

            // Dirección Diagonal
            if ((arribaIzquierda) && (!mapa.obtenerSector(new Vector2(X - 1, Y - 1)).name.Equals("wall")))
            {
                if (mapa.obtenerSector(new Vector2(X - 1, Y)).name.Equals("wall"))
                {
                    nodosAdyacentes.Add(new Nodo(nodoActual, nodoFinal,
                    new Vector2(X - 1, Y - 1), costoIrDiagonal + nodoActual.costoG));
                }
                else
                {
                    nodosAdyacentes.Add(new Nodo(nodoActual, nodoFinal,
                    new Vector2(X , Y), costoIrDiagonal + nodoActual.costoG));
                }
            }

            if ((arribaDerecha) && (!mapa.obtenerSector(new Vector2(X + 1, Y - 1)).name.Equals("wall")))
            {
                if (mapa.obtenerSector(new Vector2(X + 1, Y)).name.Equals("wall"))
                {
                    nodosAdyacentes.Add(new Nodo(nodoActual, nodoFinal,
                    new Vector2(X + 1, Y), costoIrDiagonal + nodoActual.costoG));
                }
                else
                {
                    nodosAdyacentes.Add(new Nodo(nodoActual, nodoFinal,
                    new Vector2(X, Y - 1), costoIrDiagonal + nodoActual.costoG));
                }
            }

            if ((abajoIzquierda) && (!mapa.obtenerSector(new Vector2(X - 1, Y + 1)).name.Equals("wall")))
            {
                nodosAdyacentes.Add(new Nodo(nodoActual, nodoFinal,
                new Vector2(X - 1, Y + 1), costoIrDiagonal + nodoActual.costoG));
            }

            if ((abajoDerecha) && (!mapa.obtenerSector(new Vector2(X + 1, Y + 1)).name.Equals("wall")))
            {
                nodosAdyacentes.Add(new Nodo(nodoActual, nodoFinal,
                new Vector2(X + 1, Y + 1), costoIrDiagonal + nodoActual.costoG));
            }

            return nodosAdyacentes;
        }
    }
}
