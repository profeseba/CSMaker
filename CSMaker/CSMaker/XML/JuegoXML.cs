using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CSMaker
{
    //[XmlType("CSMaker.JuegoXML")]
    public class JuegoXML
    {
        
        public String nombre { get; set; }
        public Vector2 size { set; get; }
        public JugadorXML player { get; set; }
        public List<MuroXML> walls { get; set; }
        public List<AgentesXML> enemies { get; set; }

        public JuegoXML(String n,Vector2 s, JugadorXML j, List<MuroXML> m, List<AgentesXML> a)
        {
            this.nombre = n;
            this.size = s;
            this.player = j;
            this.walls = m;
            this.enemies = a;
        }

        public JuegoXML() { }
    }
}
