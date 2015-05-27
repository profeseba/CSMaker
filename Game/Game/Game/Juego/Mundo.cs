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
using Game.Juego;


namespace Game
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Mundo
    {
        Vector2 gravedad = new Vector2(0, 16);
        public List<SpriteComponent> Sprites { get; set; }
        public List<Agent> Agentes { get; set; }
        public Vector2 Desplazamiento;

        public Mundo()
        {
            Sprites = new List<SpriteComponent>();
            Agentes = new List<Agent>();
            Desplazamiento = new Vector2(0,0);
        }

        public void AdicionarSprite(SpriteComponent sprite)
        {
            Sprites.Add(sprite);
        }

        public void RemoverSprite(SpriteComponent sprite)
        {
            Sprites.Remove(sprite);
        }

        public void AdicionarAgente(Agent agente)
        {
            Agentes.Add(agente);
        }

        public void RemoverAgente(Agent agente)
        {
            Agentes.Remove(agente);
        }

        public Vector2 CalcularMinimaDistanciaTraslacion(BoundingBox bound1, BoundingBox bound2)
        {
            // Nuestro vector resultado de despalzamineento contiene la información del movimiento
            // que hay en la intersección

            Vector2 resultado = Vector2.Zero;
            // Esto es usado para calcular la diferencia entre las distancias de los lados.
            float diferencia = 0.0f;
            // Esto guarda la minimna distancia que nosotros necesitamos para separar los obtejos que van a colisionar

            float distanciaTraslacionMinima = 0.0f;
            // ejes guarda el valor de X o Y.  X = 0, Y = 1.

            // lado guarda el valor de izquierda (-1) o derecha (+1).
            // Estos son usados despues para calcular el vector resultado
            int ejes = 0, lado = 0;

            // Izquierda
            diferencia = bound1.Max.X - bound2.Min.X;
            if (diferencia < 0.0f)
            {
                return Vector2.Zero;
            }
            {
                distanciaTraslacionMinima = diferencia;
                ejes = 0;
                lado = -1;
            }

            // Derecha
            diferencia = bound2.Max.X - bound1.Min.X;
            if (diferencia < 0.0f)
            {
                return Vector2.Zero;
            }
            if (diferencia < distanciaTraslacionMinima)
            {
                distanciaTraslacionMinima = diferencia;
                ejes = 0;
                lado = 1;
            }

            // Abajo
            diferencia = bound1.Max.Y - bound2.Min.Y;
            if (diferencia < 0.0f)
            {
                return Vector2.Zero;
            }
            if (diferencia < distanciaTraslacionMinima)
            {
                distanciaTraslacionMinima = diferencia;
                ejes = 1;
                lado = -1;
            }

            // Arriba
            diferencia = bound2.Max.Y - bound1.Min.Y;
            if (diferencia < 0.0f)
            {
                return Vector2.Zero;
            }
            if (diferencia < distanciaTraslacionMinima)
            {
                distanciaTraslacionMinima = diferencia;
                ejes = 1;
                lado = 1;
            }

            // Sí hay Colisión:
            if (ejes == 1) // Eje Y
                resultado.Y = (float)lado * distanciaTraslacionMinima;
            else // Ejec X
                resultado.X = (float)lado * distanciaTraslacionMinima;
            return resultado;
        }

        public void Update(float deltaTime, float totalTime)
        {
            for (int i = 0; i < Sprites.Count; ++i)
            {
                Sprites[i].Velocidad += gravedad * Sprites[i].Peso;
                Sprites[i].Mover((Sprites[i].Velocidad) * deltaTime);
                //redibuja las posiciones
                if (!(Sprites[i] is Jugador))
                {
                    Sprites[i].Mover(Desplazamiento * deltaTime);  
                }
                //verificar colisiones
                for (int j = 0; j < Sprites.Count; ++j)
                {
                    if (Sprites[i] == Sprites[j])
                        continue;
                    Vector2 depth = CalcularMinimaDistanciaTraslacion(Sprites[i].Bound, Sprites[j].Bound);
                    if (depth != Vector2.Zero)
                    {
                        Sprites[i].Colision(Sprites[j], depth);
                    }
                }
                //verificar interaccion con el agente
                for (int k = 0; k < Agentes.Count; k++)
                {
                    Agentes[k].Sensor(Sprites[i]);
                }               
            }
            
        }
    }
}
