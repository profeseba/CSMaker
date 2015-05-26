using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Game
{
    /// <summary>
    /// Funciones de serialización XML.
    /// </summary>
    public static class XML
    {
        /// <summary>
        /// Serializa un objeto a un archivo XML.
        /// </summary>
        /// <param name="data">Objeto a serializar a XML.</param>
        /// <param name="filename">Archivo XML a generar.</param>
        public static void Serialize(object data, string filename)
        {
            XmlSerializer mySerializer = new XmlSerializer(data.GetType());
            StreamWriter myWriter = new StreamWriter(filename);
            mySerializer.Serialize(myWriter, data);
            myWriter.Close();
        }

        /// <summary>
        /// Deserializa un archivo XML a objeto.
        /// </summary>
        /// <typeparam name="T">Tipo del objeto a deserializar.</typeparam>
        /// <param name="filename">Archivo XML a deserializar.</param>
        /// <returns>Devuelve una instancia del objeto serializado en el archivo XML.</returns>
        public static T Deserialize<T>(string filename)
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            FileStream myFileStream = new FileStream(filename, FileMode.Open);
            T ret = (T)mySerializer.Deserialize(myFileStream);
            myFileStream.Close();
            return ret;
        }
    }
}
