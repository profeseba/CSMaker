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

        public String ColisionEntreObjetos(BoundingBox bound1, BoundingBox bound2) // retorna el lado de la colision
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

        public void Update(float deltaTime, float totalTime)
        {
            //List<estados> stat = new List<estados>();
            List<Bloque> stat = new List<Bloque>();
            Bloque outBloque = new Bloque();

            for (int i = 0; i < Sprites.Count; ++i)
            {
                Sprites[i].verificarMuerte();
                if (Sprites[i].died)
                {
                    RemoverSprite(Sprites[i]);
                    continue;
                    // game over
                }
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
                    //if (( (Sprites[i].nombreSprite.Equals("Agente")) || (Sprites[i].nombreSprite.Equals("Player")) ) && ( (Sprites[j].nombreSprite.Equals("Agente")) || (Sprites[j].nombreSprite.Equals("Player")) ))
                    //{
                    //    //Debug.Print("---------"+Sprites[i].nombreSprite+"----------");
                    //    Sprites[i].direccionColision = ColisionEntreObjetos(Sprites[i].Bound, Sprites[j].Bound);
                    //    //Debug.Print(""+Sprites[i].life+" "+Sprites[i].direccionColision);  
                    //}
                    //Sprites[i].Colision(Sprites[j], depth);
                    
                }

            }
            for (int k = 0; k < Agentes.Count; k++)
            {
                for (int i = 0; i < Sprites.Count; ++i)
                {
                    stat.Add(new Sensores().Percepciones(Agentes[k], Sprites[i], Agentes[k].profundidad));
                }
                outBloque = new Bloque();
                outBloque = new Sensores().Suma(stat, Agentes[k].profundidad);
                Agentes[k].Sensor(outBloque);
                stat = new List<Bloque>();
                if (Agentes[k].died)
                {
                    RemoverAgente(Agentes[k]);
                    continue;
                }
            }
            
        }
    }
}
