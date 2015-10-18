using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CSMaker
{
   // [XmlType("Framework.MuroXML")]
    public class MuroXML
    {
        
        public Vector2 posicion { get; set; }
        public Vector2 tam { get; set; }
        public String img { get; set; }
        public Vector2 posImg { get; set; }

        public MuroXML(Vector2 p, Vector2 t, String i, Vector2 pI)
        {
            this.posicion = p;
            this.tam = t;
            this.img = i;
            this.posImg = pI;
        }

        public MuroXML() { }
    }
}
