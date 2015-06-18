using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class Nodo
    {
        #region declaraciones
        public Nodo NodoPadre;
        public Nodo NodoFinal;
        private Vector2 posicion;
        public float costoTotal;
        public float costoG;
        public bool Cerrado = false;
        #endregion

        #region propiedades
        public Vector2 Posicion
        {
            get
            {
                return posicion;
            }
            set
            {
                posicion = new Vector2(value.X,value.Y);
            }
        }

        public Int32 GrillaX
        {
            get { return (Int32)posicion.X; }
        }

        public Int32 GrillaY
        {
            get { return (Int32)posicion.Y; }
        }
        #endregion

        public Nodo(Nodo nodoPadre, Nodo nodoFinal, Vector2 posicion, float costo)
        {
            NodoPadre = nodoPadre;
            NodoFinal = nodoFinal;
            Posicion = posicion;
            costoG = costo;
            if (nodoFinal != null)
            {
                costoTotal = costoG + Calcularcosto();
            }
        }

        public float Calcularcosto()
        {
            return Math.Abs(GrillaX - NodoFinal.GrillaX) + Math.Abs(GrillaY - NodoFinal.GrillaY);
        }

        public Boolean esIgual(Nodo nodo)
        {
            return (Posicion == nodo.Posicion);
        }
    }
}
