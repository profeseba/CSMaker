using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSMaker
{
    public class Colision
    {
        public String colision(BoundingBox bound1, BoundingBox bound2)
        {
            // Esto es usado para calcular la diferencia entre las distancias de los lados.
            float diferencia = 0.0f;
            // colision entre el lado derecho del Jugador y el lado izquierdo del enemigo

            // calcula el tamaño de los bounding box
            Vector2 size1, size2;
            // tamano del objeto 1
            size1.X = bound1.Min.X + bound1.Max.X;
            size1.Y = bound1.Min.Y + bound1.Max.Y;
            // tamano del objeto 2
            size2.X = bound2.Min.X + bound2.Max.X;
            size2.Y = bound2.Min.Y + bound2.Max.Y;

            // Derecha
            diferencia = bound1.Max.X - bound2.Min.X;
            //Debug.Print("valor de la diferencia Derecha "+diferencia);
            if ((diferencia < 1.5f) && (diferencia > -1.5f))
            {
                diferencia = bound1.Max.Y - bound2.Min.Y;
                if ((diferencia <= size2.Y) && ((bound2.Max.Y - bound1.Min.Y) >= 0.0f))
                {
                    return "derecha";
                }
            }

            // Izquierda
            diferencia = bound2.Max.X - bound1.Min.X;
            //Debug.Print("valor de la diferencia Izquierda " + diferencia);
            if ((diferencia < 1.5f) && (diferencia > -1.5f))
            {
                diferencia = bound1.Max.Y - bound2.Min.Y;
                if ((diferencia <= size2.Y) && ((bound2.Max.Y - bound1.Min.Y) >= 0.0f))
                {
                    return "izquierda";
                }
            }

            // Abajo
            diferencia = bound1.Max.Y - bound2.Min.Y;
            //Debug.Print("valor de la diferencia Abajo " + diferencia);
            if ((diferencia < 1.5f) && (diferencia > -1.5f))
            {
                diferencia = bound1.Max.X - bound2.Min.X;
                if ((diferencia <= size2.X) && ((bound2.Max.X - bound1.Min.X) >= 0.0f))
                {
                    return "abajo";
                }
            }

            // Arriba
            diferencia = bound2.Max.Y - bound1.Min.Y;
            //Debug.Print("valor de la diferencia Arriba " + diferencia);
            if ((diferencia < 1.5f) && (diferencia > -1.5f))
            {
                diferencia = bound1.Max.X - bound2.Min.X;
                if ((diferencia <= size2.X) && ((bound2.Max.X - bound1.Min.X) >= 0.0f))
                {
                    return "arriba";
                }
            }
            return "nulo";
        }
    }
}
