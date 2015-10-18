using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CSMaker
{
    //[XmlType("Framework.JugadorXML")]
    public class JugadorXML
    {
        public Vector2 posicion { get; set; }
        public Vector2 tam { get; set; }
        public String img { get; set; }

        public JugadorXML(Vector2 p, Vector2 t, String i)
        {
            this.posicion = p;
            this.tam = t;
            this.img = i;
        }

        public JugadorXML() { }
    }
}
